using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.Visit;
using SF_Repositories.Common;
using SF_Repositories.VisitRepo;
using SF_Utils;

namespace SF_BusinessLogics.Visit
{
    public class VisitRealizationBLL : IVisitRealizationBLL
    {
        private readonly IBasGenericRepositories<v_visit_plan> _viewVisitPlanRepositories;
        private readonly IBasGenericRepositories<v_visit_product_topic> _viewVisitProductTopic;
        private ISqlSPRepository _spRepo;
        private IVisitRepo _visitRepo;
        private readonly IBasGenericRepositories<t_visit> _tVisit;

        public VisitRealizationBLL(IBasGenericRepositories<v_visit_plan> viewVisitPlanRepositories, ISqlSPRepository spRepo, IVisitRepo visitRepo
            , IBasGenericRepositories<v_visit_product_topic> viewVisitProductTopic, IBasGenericRepositories<t_visit> tVisit)
        {
            _viewVisitPlanRepositories = viewVisitPlanRepositories;
            _spRepo = spRepo;
            _visitRepo = visitRepo;
            _viewVisitProductTopic = viewVisitProductTopic;
            _tVisit = tVisit;
        }

        public List<v_visit_plan_DTO> GetVisitReal(VisitInputs inputs)
        {
            return GetDoctorAdditionalList(inputs);
        }

        public List<v_visit_plan_DTO> GetDoctorAdditionalList(VisitInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_visit_plan>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);    
            }
            if (inputs.Month != 0)
            {
                if (inputs.ActionSource == "GridDoctorAdditionalPartial")
                {
                    queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month || x.visit_date_plan.Value.Month == (inputs.Month - 1));
                }
                else if (inputs.ActionSource == "LoadDoctorPlaned")
                {
                    queryFilter = queryFilter.And(x => x.visit_date_plan.Value <= inputs.VisitDatePlan && x.visit_date_plan.Value >= inputs.VisitDatePlan);  
                }
                else
                {
                    queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month);    
                }
            }
            if (inputs.Year != 0)
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.Year);
            }
            if (inputs.ActionSource == "GridDoctorAdditionalPartial")
            {
                queryFilter = queryFilter.And(x => x.visit_code == "ADD");
            }
            if (inputs.ActionSource == "DataViewPartial" || inputs.ActionSource == "DataViewPartialCustomCallback" || inputs.ActionSource == "LoadDoctorPlaned")
            {
                queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1);
            }
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_visit_plan>();
            var dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
        }

        public List<v_visit_product_topic_DTO> GetProductTopic(VisitInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_visit_product_topic>();
            if (inputs.VdId != 0)
            {
                queryFilter = queryFilter.And(x => x.vd_id == inputs.VdId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_visit_product_topic>();
            var dbResult = _viewVisitProductTopic.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_product_topic_DTO>>(dbResult);
        }

        public List<t_visit_DTO> GetVisitData(VisitInputs inputs)
        {
            var queryFilter = PredicateHelper.True<t_visit>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            queryFilter = queryFilter.And(x => x.visit_date_plan.Value <= inputs.VisitDatePlan && x.visit_date_plan.Value >= inputs.VisitDatePlan);
            queryFilter = queryFilter.And(x => x.dr_code <= 100005);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<t_visit>();
            var dbResult = _tVisit.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<t_visit_DTO>>(dbResult);
        }

        public List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> GetProductTopicList(int vdid)
        {
            return _spRepo.GetProductTopicList(vdid);
        }

        public List<SP_SELECT_DOCTOR_LIST_DTO> GetDataDoctorList(VisitInputs inputs)
        {
            return _spRepo.GetDataDoctorList(inputs);
        }

        public List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductList()
        {
            return _spRepo.GetDataProductList();
        }

        public bool InsertVisitProduct(VisitInputs inputs)
        {
            return _spRepo.InsertVisitProduct(inputs);
        }

        public bool isValidDay(VisitInputs inputs)
        {
            var dbResult = GetVisitData(inputs);
            if (dbResult.Count <= 0)
            {
                return true;
            }
            return false;
        }

        #region sql repo
        public void SaveAdditionalVisit(VisitInputs inputs)
        {
            _spRepo.SaveAdditionalVisit(inputs);
        }

        public List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSP(VisitInputs inputs)
        {
            return _spRepo.GetSP(inputs.VisitId);
        }

        public IEnumerable<int> GetTopicFeedback(int vptId)
        {
            return _visitRepo.GetTopicFeedback(vptId);
        }

        public List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid)
        {
            return _spRepo.GetSPAttachment(visitid);
        }

        public int CheckMaxVisit(VisitInputs inputs)
        {
            return _spRepo.CheckMaxVisit(inputs);
        }

        

        public void DeleteVisitPlan(VisitInputs inputs)
        {
            _spRepo.DeleteVisitPlan(inputs);
        }

        public void DeleteVisitProduct(VisitInputs inputs)
        {
            _spRepo.DeleteVisitProduct(inputs);
        }

        public void DeleteProductTopic(VisitInputs inputs)
        {
            _visitRepo.DeleteProductTopic(inputs);
        }
        #endregion

        

        
    }
}

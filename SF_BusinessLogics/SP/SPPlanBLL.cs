using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.SP;
using SF_Domain.Inputs.Visit;
using SF_Repositories.Common;
using SF_Utils;

namespace SF_BusinessLogics.SP
{
    public class SPPlanBLL : ISPPlanBLL
    {
        private ISqlSPRepository _spRepo;
        private readonly IBasGenericRepositories<t_sp_approval> _tSpAppRepo;
        private readonly IBasGenericRepositories<v_sp_product> _vSpProdRepo;
        private readonly IBasGenericRepositories<v_doctor_sponsor> _vDoctorSponsorRepo;
        private readonly IBasGenericRepositories<m_sponsor> _mSponsorRepo;
        private readonly IBasGenericRepositories<v_auth_sp> _vAuthSpRepo;
        private readonly IBasGenericRepositories<m_event> _mEventRepo;
        private readonly IBasGenericRepositories<m_topic> _mTopicRepo;
        private readonly IBasGenericRepositories<v_spr> _vSprRepo;

        public SPPlanBLL(ISqlSPRepository spRepo, IBasGenericRepositories<t_sp_approval> tSpAppRepo,
            IBasGenericRepositories<v_sp_product> vSpProdRepo, IBasGenericRepositories<v_doctor_sponsor> vDoctorSponsorRepo,
            IBasGenericRepositories<m_sponsor> mSponsorRepo, IBasGenericRepositories<v_auth_sp> vAuthSpRepo,
            IBasGenericRepositories<m_event> mEventRepo, IBasGenericRepositories<m_topic> mTopicRepo,
            IBasGenericRepositories<v_spr> vSprRepo)
        {
            _spRepo = spRepo;
            _tSpAppRepo = tSpAppRepo;
            _vSpProdRepo = vSpProdRepo;
            _vDoctorSponsorRepo = vDoctorSponsorRepo;
            _mSponsorRepo = mSponsorRepo;
            _vAuthSpRepo = vAuthSpRepo;
            _mEventRepo = mEventRepo;
            _mTopicRepo = mTopicRepo;
            _vSprRepo = vSprRepo;
        }
        public List<SP_SELECT_SP_PLAN_DTO> GetSpPlan(SPInputs inputs)
        {
            return _spRepo.GetSpPlan(inputs);
        }

        public List<t_sp_approval_DTO> GetSPApproval(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<t_sp_approval>();
            if (!String.IsNullOrEmpty(inputs.SprId))
            {
                queryFilter = queryFilter.And(x => x.spr_id == inputs.SprId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<t_sp_approval>();
            var dbResult = _tSpAppRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<t_sp_approval_DTO>>(dbResult);
        }

        public List<SP_PRODUCT_SP_PLAN_MOBILE_DTO> GetProductDropdown()
        {
            return _spRepo.GetProductDropdown();
        }

        public void AddDetailProduct(string sprid, string productid)
        {
            _spRepo.AddDetailProduct(sprid, productid);
        }

        public void DeleteProduct(int sppid)
        {
            _spRepo.DeleteProduct(sppid);
        }

        public List<v_doctor_sponsor_DTO> GetSPParticipant(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_doctor_sponsor>();
            if (inputs.SpId != 0)
            {
                queryFilter = queryFilter.And(x => x.sp_id == inputs.SpId);
            }
            if (!String.IsNullOrEmpty(inputs.SpType))
            {
                queryFilter = queryFilter.And(x => x.sp_type == inputs.SpType);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_doctor_sponsor>();
            var dbResult = _vDoctorSponsorRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_doctor_sponsor_DTO>>(dbResult);
        }

        public List<m_sponsor_DTO> GetSponsor(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<m_sponsor>();
            queryFilter = queryFilter.And(x => x.sponsor_id == 9);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<m_sponsor>();
            var dbResult = _mSponsorRepo.Get(queryFilter, orderByFilter).ToList();
            
            return Mapper.Map<List<m_sponsor_DTO>>(dbResult);
        }

        public List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO> GetDoctor(SPInputs inputs)
        {
            return _spRepo.GetDoctor(inputs);
        }

        public List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorListFiltered(SPInputs inputs)
        {
            return _spRepo.GetDoctorListFiltered(inputs);
        }

        public void AddDetailParticipant(SPInputs inputs)
        {
            _spRepo.AddDetailParticipant(inputs);
        }

        public void DeleteSpeaker(SPInputs inputs)
        {
            _spRepo.DeleteSpeaker(inputs);
        }

        public void UpdateDetailSpeaker(SPInputs inputs)
        {
            _spRepo.UpdateDetailSpeaker(inputs);
        }

        public List<v_auth_sp_DTO> GetSPList(SPInputs inputs)
        {
            //var queryFilter = PredicateHelper.True<v_auth_sp>();
            //if (inputs.RoleId != 0)
            //{
            //    queryFilter = queryFilter.And(x => x.role_id == inputs.RoleId);
            //}
            //queryFilter = queryFilter.And(x => x.auth_view.Value == 1);
            //var dbResult = _vAuthSpRepo.Get(queryFilter).ToList();
            ////var order = dbResult.OrderBy(x => x.gl_sub_code.Substring(3, 2)).ToList();

            //return Mapper.Map<List<v_auth_sp_DTO>>(dbResult);

            bas_trialEntities bas = new bas_trialEntities();
            List<v_auth_sp_DTO> dbResult = bas.Database.SqlQuery<v_auth_sp_DTO>("SELECT * FROM v_auth_sp " +
                                                   "WHERE role_id = '" + inputs.RoleId + "' " +
                                                   "and auth_view = 1 ").ToList();
            return dbResult;

        }

        public List<SP_PRODUCT_VISIT_MOBILE_DTO> GetDataProductSPList()
        {
            return _spRepo.GetDataProductSPList();
        }

        public List<m_event_DTO> GetEventNameList(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<m_event>();
            if (!String.IsNullOrEmpty(inputs.SpType))
            {
                queryFilter = queryFilter.And(x => x.event_sp == inputs.SpType);
            }
            if (!String.IsNullOrEmpty(inputs.EventBudget))
            {
                queryFilter = queryFilter.And(x => x.event_budget == inputs.EventBudget);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<m_event>();
            var dbResult = _mEventRepo.Get(queryFilter, orderByFilter).Distinct().ToList();

            return Mapper.Map<List<m_event_DTO>>(dbResult);
        }

        public List<m_topic_DTO> GetProductTopicList()
        {
            
            var dbResult = _mTopicRepo.Get().Distinct().ToList();

            return Mapper.Map<List<m_topic_DTO>>(dbResult);
        }

        public void InsertSP1(SPInputs inputs)
        {
            _spRepo.InsertSP1(inputs);
        }

        public void InsertSP2(SPInputs inputs)
        {
            _spRepo.InsertSP2(inputs);
        }

        public string GetSprId()
        {
            return _spRepo.GetSprId();
        }

        public void DeleteSPPlan(SPInputs inputs)
        {
            _spRepo.DeleteSPPlan(inputs);
        }

        public bool UpdateEventDetail(SPInputs inputs)
        {
            return _spRepo.UpdateEventDetail(inputs);
        }

        public List<v_spr_DTO> GetSPRInfo(string sprid)
        {
            var queryFilter = PredicateHelper.True<v_spr>();
            if (!String.IsNullOrEmpty(sprid))
            {
                queryFilter = queryFilter.And(x => x.spr_id == sprid);
            }
            var dbResult = _vSprRepo.Get(queryFilter).ToList();

            return Mapper.Map<List<v_spr_DTO>>(dbResult);
        }

        public bool UpdateBudgetAllocationOnPlan(SPInputs inputs)
        {
            return _spRepo.UpdateBudgetAllocationOnPlan(inputs);
        }

        public string GetOwnBudgetRemaining(SPInputs inputs)
        {
            return _spRepo.GetOwnBudgetRemaining(inputs);
        }

        public List<v_sp_product_DTO> GetSPProduct(SPInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_sp_product>();
            if (!String.IsNullOrEmpty(inputs.SprId))
            {
                queryFilter = queryFilter.And(x => x.spr_id == inputs.SprId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_sp_product>();
            var dbResult = _vSpProdRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_sp_product_DTO>>(dbResult);
        }
    }
}

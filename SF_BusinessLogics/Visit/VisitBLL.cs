using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AutoMapper;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.Offline;
using SF_DAL.BAS;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.DTOs.BAS.OfflineInputs;
using SF_Domain.DTOs.HRD;
using SF_Domain.Inputs.SP;
using SF_Domain.Inputs.Visit;
using SF_Repositories.Common;
using SF_Repositories.VisitRepo;
using SF_Utils;

namespace SF_BusinessLogics.Visit
{
    public class VisitBLL : IVisitBLL
    {
        private readonly IJsonFilesGenerator _jsonFilesGenerator;
        private readonly IBasGenericRepositories<m_event> _mEvent;
        private readonly ISqlSPRepository _spRepo;
        private readonly IBasGenericRepositories<t_sales> _tSalesRepo;
        private readonly IBasGenericRepositories<t_visit> _tVisit;
        private readonly IBasGenericRepositories<v_m_doctor> _vMDoctor;
        private readonly IBasGenericRepositories<v_rep_admin> _vRepadmin;
        private readonly IBasGenericRepositories<v_visit_plan_new> _vVisitPlanNew;
        private readonly IBasGenericRepositories<v_visit_associate> _vVisitAssociated;
        private readonly IBasGenericRepositories<v_visit_plan_mobile> _vVisitPlanRepo;
        private readonly IBasGenericRepositories<v_visit_product> _vVisitProductRepo;
        private readonly IBasGenericRepositories<C_parameter> _mParameter;
        private readonly IVTLogger _vtLogger;

        private readonly IBasGenericRepositories<v_visit_plan> _viewVisitPlanRepositories;
        private readonly IBasGenericRepositories<v_visit_product_topic> _viewVisitProductTopic;
        private readonly IVisitRepo _visitRepo;

        public VisitBLL(IBasGenericRepositories<v_visit_plan_mobile> vVisitPlanRepo,
            IBasGenericRepositories<t_sales> tSalesRepo,
            IBasGenericRepositories<v_m_doctor> vMDoctor,
            ISqlSPRepository spRepo, IBasGenericRepositories<v_visit_product> vVisitProductRepo,
            IBasGenericRepositories<v_visit_plan> viewVisitPlanRepositories, IVisitRepo visitRepo,
            IBasGenericRepositories<v_visit_product_topic> viewVisitProductTopic,
            IBasGenericRepositories<t_visit> tVisit,
            IBasGenericRepositories<m_product> mProduct,
            IBasGenericRepositories<v_rep_admin> vRepadmin, IBasGenericRepositories<m_event> mEvent,
            IBasGenericRepositories<v_visit_plan_new> vVisitPlanNew,
            IBasGenericRepositories<v_visit_associate> vVisitAssociated,
            IBasGenericRepositories<C_parameter> mParameter,
            IJsonFilesGenerator jsonFilesGenerator,
            IVTLogger vtLogger
            )
        {
            _tSalesRepo = tSalesRepo;
            _vVisitPlanRepo = vVisitPlanRepo;
            _vMDoctor = vMDoctor;
            _spRepo = spRepo;
            _vVisitProductRepo = vVisitProductRepo;
            _viewVisitPlanRepositories = viewVisitPlanRepositories;
            _visitRepo = visitRepo;
            _viewVisitProductTopic = viewVisitProductTopic;
            _tVisit = tVisit;
            _vRepadmin = vRepadmin;
            _mEvent = mEvent;
            _vVisitPlanNew = vVisitPlanNew;
            _mParameter = mParameter;
            _jsonFilesGenerator = jsonFilesGenerator;
            _vtLogger = vtLogger;
            _vVisitAssociated = vVisitAssociated;
        }

        public List<SP_SelectVisitPlanDTO> GetVisitPlan(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                List<SP_SELECT_VISIT_PLAN_Result> dbResult =
                    context.SP_SELECT_VISIT_PLAN(inputs.RepId, inputs.Day, inputs.Month, inputs.Year).ToList();
                if (!String.IsNullOrEmpty(inputs.SearchColumn))
                {
                    string[] arrSearch = inputs.SearchValue.Split(',');
                    string[] arrColumn = inputs.SearchColumn.Split(',');
                    for (int i = 0; i < arrColumn.Length; i++)
                        dbResult =
                            dbResult.Where(
                                r =>
                                    r.GetType()
                                        .GetProperty(arrColumn[i])
                                        .GetValue(r, null)
                                        .ToString()
                                        .Contains(arrSearch[i])).ToList();
                }
                var res = new List<SP_SelectVisitPlanDTO>();
                foreach (SP_SELECT_VISIT_PLAN_Result data in dbResult)
                {
                    var x = new SP_SelectVisitPlanDTO
                    {
                        visit_id = data.visit_id,
                        rep_id = data.rep_id,
                        visit_date_plan = data.visit_date_plan,
                        visit_plan = data.visit_plan,
                        visit_realization = data.visit_realization,
                        dr_code = data.dr_code,
                        dr_name = data.dr_name,
                        dr_spec = data.dr_spec,
                        dr_sub_spec = data.dr_sub_spec,
                        dr_quadrant = data.dr_quadrant,
                        dr_monitoring = data.dr_monitoring,
                        dr_dk_lk = data.dr_dk_lk,
                        dr_area_mis = data.dr_area_mis,
                        dr_category = data.dr_category,
                        dr_chanel = data.dr_chanel,
                        visit_date_realization_saved = data.visit_date_realization_saved,
                        visit_date_plan_saved = data.visit_date_plan_saved,
                        visit_date_plan_updated = data.visit_date_plan_updated,
                        visit_info = data.visit_info,
                        visit_sp = data.visit_sp,
                        visit_sp_value = data.visit_sp_value,
                        visit_plan_verification_status = data.visit_plan_verification_status,
                        visit_plan_verification_by = data.visit_plan_verification_by,
                        visit_plan_verification_date = data.visit_plan_verification_date,
                        visit_real_verification_status = data.visit_real_verification_status,
                        visit_real_verification_by = data.visit_real_verification_by,
                        visit_real_verification_date = data.visit_real_verification_date,
                        dr_address = data.dr_address,
                        visit_code = data.visit_code,
                        prd_visit = data.prd_visit == 1 ? 0 : 1
                    };
                    res.Add(x);
                }


                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanDTO>>(res);
                return viewMapper;
            }
        }

        public List<v_visit_plan_DTO> GetVisitPlanExport(SP_SELECT_VISIT_PLAN_INPUTS inputs)
        {
            Expression<Func<v_visit_plan, bool>> queryFilter = PredicateHelper.True<v_visit_plan>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            if (!String.IsNullOrEmpty(inputs.DrName))
            {
                queryFilter = queryFilter.And(x => x.dr_name == inputs.DrName);
            }
            if (!String.IsNullOrEmpty(inputs.DrSpec))
            {
                queryFilter = queryFilter.And(x => x.dr_spec == inputs.DrSpec);
            }
            if (!String.IsNullOrEmpty(inputs.DrSubSpec))
            {
                queryFilter = queryFilter.And(x => x.dr_sub_spec == inputs.DrSubSpec);
            }
            if (!String.IsNullOrEmpty(inputs.DrQuadrant))
            {
                queryFilter = queryFilter.And(x => x.dr_quadrant == inputs.DrQuadrant);
            }
            if (!String.IsNullOrEmpty(inputs.DrMonitoring))
            {
                queryFilter = queryFilter.And(x => x.dr_monitoring == inputs.DrMonitoring);
            }
            if (!String.IsNullOrEmpty(inputs.DrDkLk))
            {
                queryFilter = queryFilter.And(x => x.dr_dk_lk == inputs.DrDkLk);
            }
            if (!String.IsNullOrEmpty(inputs.DrAreaMis))
            {
                queryFilter = queryFilter.And(x => x.dr_area_mis == inputs.DrMonitoring);
            }
            if (!String.IsNullOrEmpty(inputs.DrCategory))
            {
                queryFilter = queryFilter.And(x => x.dr_category == inputs.DrCategory);
            }
            if (!String.IsNullOrEmpty(inputs.DrChannel))
            {
                queryFilter = queryFilter.And(x => x.dr_chanel == inputs.DrChannel);
            }

            queryFilter =
                queryFilter.And(
                    x => x.visit_plan_verification_status == (inputs.ActionSource == "visitrealization" ? 1 : 0));
            //if (inputs.ActionSource != "visitrealization")
            //{
            //    queryFilter = queryFilter.And(x => x.visit_plan_verification_status ==  0);
            //}
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);
            if (inputs.Prm == "filter")
            {
                queryFilter = queryFilter.And(x =>
                    x.visit_date_plan.Value <= inputs.VisitDatePlan.Value &&
                    x.visit_date_plan.Value >= inputs.VisitDatePlan.Value);
            }
            else
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.VisitDatePlan.Value.Month);
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.VisitDatePlan.Value.Year);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_plan>, IOrderedQueryable<v_visit_plan>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan>();
            List<v_visit_plan> dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    dbResult =
                        dbResult.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString()
                                    .Contains(arrSearch[i])).ToList();
            }
            var viewMapper = Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
            return viewMapper;
        }


        public List<SP_SELECT_VISIT_USER_PRODUCT_DTO> GetUserProductPartial(string id, string visitid, int month,
            int year)
        {
            using (var context = new bas_trialEntities())
            {
                List<SP_SELECT_VISIT_USER_PRODUCT_Result> dbResult =
                    context.SP_SELECT_VISIT_USER_PRODUCT(id, visitid, month, year).ToList();
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_DTO>>(dbResult);
                return viewMapper;
            }
        }

        public bool isAnyPlannedSalesInSelectedMonth(string id, int month, int year)
        {
            Expression<Func<t_sales, bool>> queryFilter = PredicateHelper.True<t_sales>();
            if (!String.IsNullOrEmpty(id))
            {
                queryFilter = queryFilter.And(x => x.rep_id == id);
            }
            if (month != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_date_plan == month);
            }
            if (year != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_year_plan == year);
            }

            List<t_sales> dbResult = _tSalesRepo.Get(queryFilter).ToList();
            if (dbResult.Any())
            {
                return true;
            }
            return false;
        }

        public bool isAnyPlannedSalesInCurrMonth(string id, string visitid)
        {
            //throw new NotImplementedException();
            using (var context = new bas_trialEntities())
            {
                int drCode = 0;
                if (!String.IsNullOrEmpty(visitid))
                {
                    drCode =
                        context.t_visit.Where(x => x.visit_id == visitid).Select(x => x.dr_code.Value).FirstOrDefault();
                }
                if (drCode != 0)
                {
                    DateTime currentDate = DateTime.Now;
                    int month = currentDate.Month;
                    int year = currentDate.Year;
                    List<t_sales> dbResult =
                        context.t_sales.Where(
                            x =>
                                x.sales_date_plan.Value == month && x.sales_year_plan.Value == year && x.rep_id == id &&
                                x.dr_code == drCode).ToList();
                    if (dbResult.Any())
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public bool CopySalesProductPlan(string id, string visitid, int frommonth, int fromyear)
        {
            DateTime currentDate = DateTime.Now;
            int month = currentDate.Month;
            int year = currentDate.Year;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    context.SP_COPY_SALES_PRODUCT_PLAN(id, visitid, frommonth, fromyear, month, year);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string InsertVisitPlan(string id, string visitDatePlan, List<VisitModel> drCollection,string rep_position, string rep_region)
        {
            DateTime visitDatePlanParam = Convert.ToDateTime(visitDatePlan);
            string result = "";
            bool state = false;

            try
            {
                if (isMaxLimitedDoctorinCurrDayOnInit(visitDatePlanParam, id, rep_position))
                {
                    result = EnumHelper.GetDescription(Enums.ResponseType.ReachMaximumDoctor);
                    return result;
                }

                int doctors = drCollection.Count;
                if (doctors > 1)
                {
                    foreach (VisitModel drCode in drCollection)
                    {
                        if (Convert.ToInt32(drCode.Code) < 100005)
                        {
                            result = EnumHelper.GetDescription(Enums.ResponseType.PairedCode);
                            break;
                        }
                    }
                }

                foreach (VisitModel drCode in drCollection)
                {
                    if (Convert.ToInt32(drCode.Code) > 100005)
                    {
                        if (isAlreadyPlannedLeaveInCurrDay(visitDatePlanParam, id))
                        {
                            result = EnumHelper.GetDescription(Enums.ResponseType.PairedCode);
                            break;
                        }
                    }

                    if (isMaxLimitedDoctorinCurrDayOnRunning(visitDatePlanParam, id, rep_position)) //TRANSFERED
                    {
                        result = EnumHelper.GetDescription(Enums.ResponseType.ReachMaximumDoctor);
                        break;
                    }

                    if (isAlreadyPlannedDoctorInCurrDay(visitDatePlanParam, drCode.Code, id)) //TRANSFERED
                    {
                        result = EnumHelper.GetDescription(Enums.ResponseType.AlreadyPlannedDoctor);
                        break;
                    }

                    if (rep_region.Replace(" ", String.Empty) != "BIOD")
                    {
                        //'TRANSFERED
                        if (isAlreadyPlannedDoctorInCurrWeek(visitDatePlanParam, drCode.Code, id))
                        {
                            result = EnumHelper.GetDescription(Enums.ResponseType.AlreadyPlannedDoctorWeek);
                            break;
                        }
                    }

                    if (drCode.Code == "")
                    {
                        break;
                    }

                    string newvisitid = GetVisitId();
                    using (var context = new bas_trialEntities())
                    {
                        context.SP_INSERT_VISIT_PLAN(newvisitid, id, visitDatePlanParam, Convert.ToInt32(drCode.Code),rep_position);
                        //var sql = "EXEC SP_INSERT_VISIT_PLAN_MOBILE '" + newvisitid + "','" + id + "','" + visitDatePlanParam + "'," + Convert.ToInt32(drCode.Code) + ",'" + rep_position + "' ";
                        //context.Database.SqlQuery<int>(sql);
                        result = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = EnumHelper.GetDescription(Enums.ResponseType.SaveError);
            }

            return result;
        }

        public bool isAnyDoctorUnverificatedRealInPrevMonth(int prevMonth, string id)
        {
            Expression<Func<v_m_doctor, bool>> queryFilter = PredicateHelper.True<v_m_doctor>();
            queryFilter = queryFilter.And(x => x.dr_used_month_session == prevMonth);
            queryFilter = queryFilter.And(x => x.dr_status == 1);
            queryFilter = queryFilter.And(x => x.dr_rep == id);
            List<v_m_doctor> dbResult = _vMDoctor.Get(queryFilter).ToList();
            if (dbResult.Any())
            {
                return true;
            }
            return false;
        }

        public bool isAnyDoctorUnplaned(int prevMonth, string id)
        {
            DateTime currentDate = DateTime.Now;
            int currYear = currentDate.Year;
            int currMonthRequest = prevMonth;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int? dbResult =
                        context.SP_SELECT_IS_ANY_DOCTOR_UNPLANED(id, currMonthRequest, currYear).FirstOrDefault();
                    return Convert.ToBoolean(dbResult.Value);
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool isAnyVisitUnplanedProduct(int month, string id)
        {
            DateTime currentDate = DateTime.Now;
            int currYear = currentDate.Year;
            int currMonthRequest = currentDate.Month;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int? dbResult = context.SP_SELECT_IS_ANY_VISIT_UNPLANED_PRODUCT(id, currMonthRequest, currYear)
                        .FirstOrDefault();
                    return Convert.ToBoolean(dbResult.Value);
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool isAnyDoctorUnplanedSales(string id, int month, int year)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int? dbResult = context.SP_SELECT_IS_ANY_DOCTOR_UNPLANED_USER(id, month, year).FirstOrDefault();
                    return Convert.ToBoolean(dbResult.Value);
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool isAnyDayLessThenMinimumDoctor(string rep_postion, string id)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    //int? dbResult = context.f_isAnyDayLessThenMinimumDoctor(id).FirstOrDefault();
                    int? dbResult = context.Database.SqlQuery<Nullable<int>>("_isAnyDayLessThenMinimumDoctor '"+id+"'").FirstOrDefault();
                    return dbResult != null && Convert.ToBoolean(dbResult.Value);
                    //var bas = new bas_trialEntities();
                    //var dbResult = new List<SP_SELECT_DOCTOR_LIST_NEW_DTO>();
                    //dbResult =
                    //    bas.Database.SqlQuery<SP_SELECT_DOCTOR_LIST_NEW_DTO>("SP_SELECT_DOCTOR_LIST_NEW '" + id + "', '" +
                    //                                                         position + "'").ToList();

                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        //public bool isAnyDayLessThenMinimumDoctor(string rep_postion, string id)
        //{
        //    DateTime currentDate = DateTime.Now;
        //    int currMonth = currentDate.Month;
        //    int currYear = currentDate.Year;
        //    try
        //    {
        //        using (var context = new bas_trialEntities())
        //        {
        //            var dbResult = context.t_visit.Where(x =>
        //                x.rep_id == id && x.visit_date_plan.Value.Month == currMonth &&
        //                x.visit_date_plan.Value.Year == currYear
        //                && x.visit_plan_verification_status.Value == 0 && x.dr_code > 1005)
        //                .GroupBy(x => x.visit_date_plan)
        //                .Select(x => new {visitdateplan = x.Key, visitcount = x.Count()}).ToList();
        //            int recordSet = dbResult.Count;
        //            for (int i = 0; i < recordSet; i++)
        //            {
        //                int visitCount = dbResult[i].visitcount;
        //                if (visitCount == 0)
        //                {
        //                    return true;
        //                }

        //                if (rep_postion == "MR")
        //                {
        //                    if (visitCount < 11)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                else if (rep_postion == "PS")
        //                {
        //                    if (visitCount < 6)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                else if (rep_postion == "MS")
        //                {
        //                    if (visitCount < 9)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return true;
        //    }
        //}

        public bool isHaveRemainingToSendMail(string emailType, string id)
        {
            DateTime currentDate = DateTime.Now;
            int currMonth = currentDate.Month;
            int currYear = currentDate.Year;
            DateTime dateSent = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd"));
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int? dbResult =
                        context.SP_SELECT_TRANSACT_EMAIL(currMonth, currYear, emailType, id, dateSent).FirstOrDefault();
                    return Convert.ToBoolean(dbResult.Value);
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public List<FullVisitDateDTO> getFullVisitDate(string id)
        {
            using (var context = new bas_trialEntities())
            {
                DateTime currentDate = DateTime.Now;
                var dbResult = context.t_visit
                    .Where(x => x.rep_id == id && x.visit_date_plan.Value.Month == currentDate.Month &&
                                x.visit_date_plan.Value.Year == currentDate.Year)
                    .GroupBy(x => x.visit_date_plan)
                    .Select(x => new {DayList = x.Key.Value.Day, Counter = x.Count()})
                    .ToList();

                var viewMapper = Mapper.Map<List<FullVisitDateDTO>>(dbResult);
                return viewMapper;
            }
        }


        public bool InsertSalesProduct(string id, string visitid, int month, int year, string prdcode, int qty,
            string note, int sp, int percentage)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int dbResult = context.SP_INSERT_SALES_PRODUCT_VISIT(id, visitid, month, year, prdcode, qty, note,
                        sp, percentage);
                    if (dbResult == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertVisitProduct(string id, string visitid, string visitcode, int sp, int percentage)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    int dbResult = context.SP_INSERT_PRODUCT_VISIT(id, visitid, visitcode, sp, percentage);
                    if (dbResult == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //_vVisitProductRepo
        public List<v_visit_product_DTO> GetVisitProduct(VisitInputs inputs)
        {
            Expression<Func<v_visit_product, bool>> queryFilter = PredicateHelper.True<v_visit_product>();
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_product>, IOrderedQueryable<v_visit_product>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_product>();
            List<v_visit_product> dbResult = _vVisitProductRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_product_DTO>>(dbResult);
        }

        public List<v_visit_product_DTO> GetVisitProduct(string visitid)
        {
            using (var context = new bas_trialEntities())
            {
                List<v_visit_product> dbResult =
                    context.v_visit_product.Where(x => x.visit_id == visitid).OrderBy(x => x.vd_id).ToList();
                var viewMapper = Mapper.Map<List<v_visit_product_DTO>>(dbResult);
                return viewMapper;
            }
        }

        public void InsertVisitSP(string id, string eventname, string bAllocation, int bAmount)
        {
            using (var context = new bas_trialEntities())
            {
                context.SP_INSERT_SP_2_ON_VISIT_PLAN(id, eventname, bAllocation, bAmount);
            }
        }

        public void DeleteVisitPlan(string visitid, string rep_position)
        {
            using (var contex = new bas_trialEntities())
            {
                contex.SP_DELETE_VISIT_PLAN(visitid, rep_position);
            }
        }

        public void DeleteSalesProduct(int sp_id)
        {
            using (var contex = new bas_trialEntities())
            {
                contex.SP_DELETE_PRODUCT_SALES(sp_id);
            }
        }

        public void DeleteVisitSP(string spdsId)
        {
            using (var contex = new bas_trialEntities())
            {
                int id = Convert.ToInt32(spdsId);
                contex.SP_DELETE_VISIT_SP(id);
            }
        }

        public void DeleteVisitProduct(int vdid)
        {
            using (var contex = new bas_trialEntities())
            {
                contex.SP_DELETE_PRODUCT_VISIT(vdid);
            }
        }

        public void UpdatePlan(UpdatePlanModel coll, string position)
        {
            using (var context = new bas_trialEntities())
            {
                if (coll.DrCode == coll.OldDrCode)
                {
                    t_visit dbResult = context.t_visit.Find(coll.VisitId);
                    var newInput = Mapper.Map<t_visit>(dbResult);
                    newInput.visit_date_plan = coll.VisitDatePlan;
                    context.t_visit.Add(newInput);
                    context.Entry(newInput).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    int newdrCode = Convert.ToInt32(coll.DrCode);
                    context.SP_UPDATE_VISIT_PLAN(coll.VisitId, coll.VisitDatePlan, newdrCode, coll.OldDrCode, position);
                }
            }
        }

        public void ShiftRealization(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                t_visit dbResult = context.t_visit.Find(inputs.VisitId);
                var newInput = Mapper.Map<t_visit>(dbResult);
                newInput.visit_date_plan = inputs.VisitDatePlan;
                context.t_visit.Add(newInput);
                context.Entry(newInput).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public int ValidateShiftCounter(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                var startDate = inputs.PrevVisitDate.StartOfWeek(DayOfWeek.Monday);
                var endDate = inputs.PrevVisitDate.EndOfWeek(DayOfWeek.Sunday);

                if (inputs.VisitDatePlan < startDate || inputs.VisitDatePlan > endDate)
                {
                    return 2; // tanggal lintas week
                }

                var dbReturn =
                    context.t_shift_log.Count(x => x.visit_date_plan >= startDate && x.visit_date_plan <= endDate
                                                   && x.rep_id == inputs.RepId);

                if (dbReturn < 10)
                {
                    return 0; // boleh shift atau masih kurang dari liimit = 10x
                }
             
                return 1; // berarti shift lebih dari 5x seminggu
             
            }
        }

        public void InsertShiftLog(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                var newInput = new t_shift_log
                {
                    visit_id = inputs.VisitId,
                    rep_id = inputs.RepId,
                    prev_visit_date = inputs.PrevVisitDate,
                    visit_date_plan = inputs.VisitDatePlan,
                    created_date = DateTime.Now,
                    updated_date = DateTime.Now,
                    is_active = 1
                };
                context.t_shift_log.Add(newInput);
                context.SaveChanges();
            }
        }

        public string UpdateSalesProduct(int spid, int qty, int sp, int percentage, string repid, int drcode,
            DateTime dateplan)
        {
            using (var context = new bas_trialEntities())
            {
                #region validation

                DateTime date = dateplan.Date;
                List<v_doctor_sponsor> validation =
                    context.v_doctor_sponsor.Where(x => x.spr_initiator == repid && x.dr_code == drcode &&
                                                        x.sp_type == "SP2" && x.sp_ba == "PMD" &&
                                                        x.e_dt_start == date).ToList();

                #endregion

                if (!validation.Any())
                {
                    context.SP_UPDATE_SALES_PRODUCT_MOBILE(spid, qty, sp, percentage);

                    return "Success";
                }
                return "sp_already_planned";
            }
        }

        //public void UpdateVisitProduct(int vdid, string visitcode, int sp, int percentage)
        //{
        //    using (var context = new bas_trialEntities())
        //    {
        //        var id = Convert.ToInt32(vdid);
        //        context.SP_UPDATE_VISIT_PRODUCT(id,visitcode,sp,percentage);
        //    }
        //}
        public int UpdateVisitProduct(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                int code = Convert.ToInt32(inputs.DrCode);
                List<v_doctor_sponsor> validation = context.v_doctor_sponsor.Where(x => x.spr_initiator == inputs.RepId
                                                                                        && x.dr_code == code &&
                                                                                        x.sp_type == "SP2" &&
                                                                                        x.sp_ba == "PMD").ToList();
                if (validation.Any())
                {
                    return 0;
                }

                t_visit_product dbResult = context.t_visit_product.Find(inputs.VdId);
                dbResult.visit_code = inputs.VisitCode;
                dbResult.sp_sp = (byte?) inputs.Sp;
                dbResult.sp_percentage = inputs.Percentage;
                context.t_visit_product.Add(dbResult);
                context.Entry(dbResult).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public List<v_visit_plan_mobileDTO> GetDoctorListPerDay(string id, DateTime? visitdateplan, string SearchColumn,
            string SearchValue)
        {
            Expression<Func<v_visit_plan_mobile, bool>> queryFilter = PredicateHelper.True<v_visit_plan_mobile>();
            queryFilter = queryFilter.And(x => x.rep_id == id);
            queryFilter =
                queryFilter.And(
                    x =>
                        x.visit_date_plan.Value <= visitdateplan.Value && x.visit_date_plan.Value >= visitdateplan.Value);
            List<v_visit_plan_mobile> dbResult = _vVisitPlanRepo.Get(queryFilter).ToList();
            var viewMapper = Mapper.Map<List<v_visit_plan_mobileDTO>>(dbResult);
            if (!String.IsNullOrEmpty(SearchColumn))
            {
                string[] arrSearch = SearchValue.Split(',');
                string[] arrColumn = SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    viewMapper =
                        viewMapper.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString().ToLower()
                                    .Contains(arrSearch[i].ToLower())).ToList();
            }

            return viewMapper;
        }

        public List<SP_SELECT_DOCTOR_LIST_NEW_DTO> GetDoctorList(string id, string position, string SearchColumn,
            string SearchValue)
        {
            var bas = new bas_trialEntities();
            var dbResult = new List<SP_SELECT_DOCTOR_LIST_NEW_DTO>();
            dbResult =
                bas.Database.SqlQuery<SP_SELECT_DOCTOR_LIST_NEW_DTO>("SP_SELECT_DOCTOR_LIST_NEW '" + id + "', '" +
                                                                     position + "'").ToList();
            var res = new List<SP_SELECT_DOCTOR_LIST_NEW_DTO>();
            foreach (SP_SELECT_DOCTOR_LIST_NEW_DTO viewMapper in dbResult)
            {
                var data = new SP_SELECT_DOCTOR_LIST_NEW_DTO
                {
                    sbo_id = viewMapper.sbo_id,
                    dr_code = viewMapper.dr_code,
                    dr_name = viewMapper.dr_name,
                    dr_spec = viewMapper.dr_spec,
                    dr_sub_spec = viewMapper.dr_sub_spec,
                    dr_quadrant = viewMapper.dr_quadrant,
                    dr_monitoring = viewMapper.dr_monitoring ?? "",
                    dr_status = viewMapper.dr_status,
                    is_used = viewMapper.is_used,
                    dr_used_remaining = viewMapper.dr_used_remaining.HasValue ? viewMapper.dr_used_remaining.Value : 0,
                    is_used_on_sales = viewMapper.prd_plan
                };
                res.Add(data);
            }
            if (!String.IsNullOrEmpty(SearchColumn))
            {
                string[] arrSearch = SearchValue.Split(',');
                string[] arrColumn = SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    res =
                        res.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString().ToLower()
                                    .Contains(arrSearch[i].ToLower())).ToList();
            }
            return res;
        }

        public void SaveReportPlan(string id)
        {
            _spRepo.SaveReportPlan(id);
        }

        public bool SaveRealizationVisitWithAdditionalSP(VisitInputs inputs)
        {
            return _spRepo.SaveRealizationVisitWithAdditionalSP(inputs);
        }

        public bool SubmitAdditionalRealizationVisit(VisitInputs inputs)
        {
            return _spRepo.SubmitAdditionalRealizationVisit(inputs);
        }

        public bool SaveRealizationVisit(VisitInputs inputs)
        {
            return _spRepo.SaveRealizationVisit(inputs);
        }

        public void InsertSPAttachment(string visitid, string filename, string filepath)
        {
            _spRepo.InsertSPAttachment(visitid, filename, filepath);
        }

        public void InsertSPRealizationAttachment(string sprid, string filename, string filepath)
        {
            using (var context = new bas_trialEntities())
            {
                var newInput = new t_sp_attachment
                {
                    spr_id = sprid,
                    spf_file_name = filename,
                    spf_file_path = filepath,
                    spf_date_uploaded = DateTime.Now
                };
                context.t_sp_attachment.Add(newInput);
                context.SaveChanges();
            }
        }

        public List<SP_SELECT_SPR_INFO_DTO> GetSprInformation(string visitid)
        {
            return _spRepo.GetSprInformation(visitid);
        }

        //=========================================================================================================================
        public IList<v_visit_plan_new_DTO> GetVisitRealOptimize(VisitInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_visit_plan_new>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }

            if (inputs.Month != 0)
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month);
            }
            if (inputs.Year != 0)
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.Year);
            }
            
            queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1);
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_visit_plan_new>();
            //var dbResult1 = _vVisitPlanNew.Get(queryFilter, orderByFilter).ToList();
            //var count = dbResult1.Count();
            var dbResult = _vVisitPlanNew.Get(queryFilter, orderByFilter).ToList();
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    dbResult =
                        dbResult.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString()
                                    .Contains(arrSearch[i])).ToList();
            }
            return Mapper.Map<List<v_visit_plan_new_DTO>>(dbResult);
        }

        public List<v_visit_plan_new_DTO> GetVisitReal(VisitInputs inputs)
        {
            return GetVisitRealizationDatas(inputs);
        }

        public List<v_visit_plan_new_DTO> GetVisitRealizationDatas(VisitInputs inputs)
        {
            Expression<Func<v_visit_plan_new, bool>> queryFilter = PredicateHelper.True<v_visit_plan_new>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Month != 0)
            {
                if (inputs.ActionSource == "GridDoctorAdditionalPartial")
                {
                    queryFilter =
                        queryFilter.And(
                            x =>
                                x.visit_date_plan.Value.Month == inputs.Month ||
                                x.visit_date_plan.Value.Month == (inputs.Month - 1));
                }
                else if (inputs.ActionSource == "LoadDoctorPlaned" || inputs.ActionSource == "validatecancellation")
                {
                    queryFilter =
                        queryFilter.And(
                            x =>
                                x.visit_date_plan.Value <= inputs.VisitDatePlan &&
                                x.visit_date_plan.Value >= inputs.VisitDatePlan);
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
            if (inputs.ActionSource == "DataViewPartial" || inputs.ActionSource == "DataViewPartialCustomCallback" ||
                inputs.ActionSource == "LoadDoctorPlaned" || inputs.ActionSource == "validatecancellation")
            {
                queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1);
            }
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_plan_new>, IOrderedQueryable<v_visit_plan_new>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan_new>();
            List<v_visit_plan_new> dbResult = _vVisitPlanNew.Get(queryFilter, orderByFilter).ToList();
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    dbResult =
                        dbResult.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString()
                                    .Contains(arrSearch[i])).ToList();
            }
            return Mapper.Map<List<v_visit_plan_new_DTO>>(dbResult);
        }

        public List<v_visit_plan_DTO> GetDoctorAdditionalList(VisitInputs inputs)
        {
            Expression<Func<v_visit_plan, bool>> queryFilter = PredicateHelper.True<v_visit_plan>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Month != 0)
            {
                if (inputs.ActionSource == "GridDoctorAdditionalPartial")
                {
                    queryFilter =
                        queryFilter.And(
                            x =>
                                x.visit_date_plan.Value.Month == inputs.Month ||
                                x.visit_date_plan.Value.Month == (inputs.Month - 1));
                }
                else if (inputs.ActionSource == "LoadDoctorPlaned" || inputs.ActionSource == "validatecancellation")
                {
                    queryFilter =
                        queryFilter.And(
                            x =>
                                x.visit_date_plan.Value <= inputs.VisitDatePlan &&
                                x.visit_date_plan.Value >= inputs.VisitDatePlan);
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
            if (inputs.ActionSource == "DataViewPartial" || inputs.ActionSource == "DataViewPartialCustomCallback" ||
                inputs.ActionSource == "LoadDoctorPlaned" || inputs.ActionSource == "validatecancellation")
            {
                //queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1);
            }
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_plan>, IOrderedQueryable<v_visit_plan>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan>();
            List<v_visit_plan> dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
        }
        //Her Inj 1 = %20
        //liv 1 = liv1
        public List<v_visit_product_topic_DTO> GetProductTopic(VisitInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_visit_product_topic>();
            if (inputs.VptId != 0)
            {
                queryFilter = queryFilter.And(x => x.vpt_id == inputs.VptId);
            }
            if (inputs.VdId != 0)
            {
                queryFilter = queryFilter.And(x => x.vd_id == inputs.VdId);
            }
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_visit_product_topic>();
            var dbResult = _viewVisitProductTopic.Get(queryFilter, orderByFilter).ToList();

            var settingsReader = new AppSettingsReader();
            var filePath = (string)settingsReader.GetValue("TopicFiles", typeof(String));
            ////~/Asset/Files/Downloads/Pdf/
            var newReturn = new List<v_visit_product_topic_DTO>();
            if (dbResult.Any())
            {
                newReturn = dbResult.Select(x => new v_visit_product_topic_DTO
                {
                    vpt_id = x.vpt_id,
                    vd_id = x.vd_id,
                    topic_id = x.topic_id,
                    topic_title = x.topic_title,
                    topic_group_product = x.topic_group_product,
                    topic_filepath = inputs.Host.Replace("bas_api_mobile", "bas") +  (String.IsNullOrEmpty(x.topic_filepath) ? (GetStringPattern().Replace(filePath, "") + "no_available_image.jpg") : GetStringPattern().Replace(x.topic_filepath.Substring(1), "%20")),
                    topic_file_name = x.topic_file_name,
                    vpt_feedback = x.vpt_feedback.HasValue ? x.vpt_feedback.Value:0,
                    vpt_feedback_date = x.vpt_feedback_date.HasValue ? x.vpt_feedback_date.Value:DateTime.Now,
                    visit_id = x.visit_id
                }).ToList();
            }

            //var mapper = Mapper.Map<List<v_visit_product_topic_DTO>>(dbResult);
            return newReturn;
        }

        public bool isAlreadyPlannedVisitInCurrDay(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                List<t_visit> dbResult = context.t_visit.Where(x => x.rep_id == inputs.RepId &&
                                                                    x.visit_date_plan.Value <= inputs.VisitDatePlan &&
                                                                    x.visit_date_plan.Value >= inputs.VisitDatePlan &&
                                                                    x.visit_plan_verification_status == 1 )
                    .ToList();
                int visitCount = dbResult.Count;
                if (visitCount > 0)
                {
                    return true;
                }
                //if (inputs.RepPosition == "MR")
                //{
                //    if (visitCount >= 11)
                //    {
                //        return true;
                //    }
                //}
                //else if (inputs.RepPosition == "AM")
                //{
                //    if (visitCount >= 5)
                //    {
                //        return true;
                //    }
                //}
                //else if (inputs.RepPosition == "PPM")
                //{
                //    if (visitCount >= 4)
                //    {
                //        return true;
                //    }
                //}
                //else if (inputs.RepPosition == "PS")
                //{
                //    if (visitCount >= 6)
                //    {
                //        return true;
                //    }
                //}
                //else if (inputs.RepPosition == "MS")
                //{
                //    if (visitCount >= 9)
                //    {
                //        return true;
                //    }
                //}
                return false;
            }
        }

        public bool isAlreadyPlannedDoctorInCurrDay(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                int drcode = Convert.ToInt32(inputs.DrCode);
                List<v_visit_plan> dbResult = context.v_visit_plan.Where(x => x.rep_id == inputs.RepId &&
                                                                              x.visit_date_plan.Value <=
                                                                              inputs.VisitDatePlan &&
                                                                              x.visit_date_plan.Value >=
                                                                              inputs.VisitDatePlan &&
                                                                              x.dr_code == drcode && x.visit_plan == 1)
                    .ToList();
                if (dbResult.Any())
                {
                    return true;
                }
                return false;
            }
        }
        public bool isMaxLimitedDoctorinCurrDay(VisitInputs inputs){
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var date = inputs.VisitDatePlan.ToString("yyyy-MM-dd");
                    int? dbResult = context.f_isMaxLimitedDoctorinCurrDayOnAdditional(inputs.RepId, date).FirstOrDefault();
                    return Convert.ToBoolean(dbResult.Value);
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        //public bool isMaxLimitedDoctorinCurrDay(VisitInputs inputs)
        //{
        //    using (var context = new bas_trialEntities())
        //    {
        //        List<t_visit> dbResult = context.t_visit.Where(x => x.rep_id == inputs.RepId &&
        //                                                            x.visit_date_plan.Value <= inputs.VisitDatePlan &&
        //                                                            x.visit_date_plan.Value >= inputs.VisitDatePlan)
        //            .ToList();
        //        int visitCount = dbResult.Count;
        //        if (inputs.RepPosition == "MR")
        //        {
        //            if (visitCount >= 22)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (inputs.RepPosition == "AM")
        //        {
        //            if (visitCount >= 10)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (inputs.RepPosition == "PPM")
        //        {
        //            if (visitCount >= 8)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (inputs.RepPosition == "PS")
        //        {
        //            if (visitCount >= 12)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (inputs.RepPosition == "MS")
        //        {
        //            if (visitCount >= 18)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //}

        public string GetDeviation(VisitInputs inputs)
        {
            using (var contex = new bas_trialEntities())
            {
                int code = Convert.ToInt32(inputs.DrCode);
                string dbResult =
                    contex.t_signature_mobile.Where(x => x.visit_id == inputs.VisitId && x.rep_id == inputs.RepId
                                                         && x.dr_code == code).Select(x => x.reason).FirstOrDefault();
                return dbResult;
            }
        }

        public string GetSign(VisitInputs inputs)
        {
            using (var contex = new bas_trialEntities())
            {
                int code = Convert.ToInt32(inputs.DrCode);
                string dbResult =
                    contex.t_signature_mobile.Where(x => x.visit_id == inputs.VisitId && x.rep_id == inputs.RepId
                                                         && x.dr_code == code)
                        .Select(x => x.file_upload)
                        .FirstOrDefault();
                return dbResult;
            }
        }

        public string VerifySign(VisitInputs inputs)
        {
            using (var contex = new bas_trialEntities())
            {
                int code = Convert.ToInt32(inputs.DrCode);
                string dbResult =
                    contex.t_signature_mobile.Where(x => x.visit_id == inputs.VisitId && x.rep_id == inputs.RepId
                                                         && x.dr_code == code).Select(x => x.visit_id).FirstOrDefault();
                return dbResult;
            }
        }

        public void DeleteGps(string visitId)
        {
            using (var context = new bas_trialEntities())
            {
                t_gps_mobile result = context.t_gps_mobile.FirstOrDefault(x => x.visit_id == visitId);
                if(result != null)
                {
                    context.t_gps_mobile.Remove(result);
                    context.SaveChanges();    
                }
            }
        }


        public void AddDeviation(VisitInputs inputs, bool statgps)
        {
            using (var context = new bas_trialEntities())
            {
                //cek data sebelumnya
                int code = Convert.ToInt32(inputs.DrCode);
                t_signature_mobile dbResult =
                    context.t_signature_mobile.FirstOrDefault(
                        x => x.visit_id == inputs.VisitId && x.rep_id == inputs.RepId
                             && x.dr_code == code);
                //jika ada maka di update
                if (dbResult != null)
                {
                    dbResult.reason = (statgps) ? inputs.Reason : ("Offline : " + inputs.Reason);
                    dbResult.sign = false;
                    dbResult.file_upload = null;
                    dbResult.updated_at = DateTime.Now;
                    context.t_signature_mobile.Add(dbResult);
                    context.Entry(dbResult).State = EntityState.Modified;
                }
                else
                {
                    var newInput = new t_signature_mobile
                    {
                        reason = (statgps) ? inputs.Reason : ("Offline : " + inputs.Reason),
                        visit_id = inputs.VisitId,
                        rep_id = inputs.RepId,
                        dr_code = code,
                        file_upload = null,
                        sign = false,
                        created_at = DateTime.Now
                    };
                    context.t_signature_mobile.Add(newInput);
                }
                //jika tidak ada maka insert baru

                context.SaveChanges();

                if (!statgps)
                {
                    DeleteGps(inputs.VisitId);
                }
            }
        }

        public void AddSign(string repid, string visitid, int drcode, string filename, bool statgps)
        {
            using (var context = new bas_trialEntities())
            {
                //cek data sebelumnya
                t_signature_mobile dbResult =
                    context.t_signature_mobile.FirstOrDefault(
                        x => x.visit_id == visitid && x.rep_id == repid && x.dr_code == drcode);
                //jika ada maka di update
                if (dbResult != null)
                {
                    dbResult.reason = (statgps) ? null : "Offline";
                    dbResult.sign = true;
                    dbResult.updated_at = DateTime.Now;
                    dbResult.file_upload = filename;
                    context.t_signature_mobile.Add(dbResult);
                    context.Entry(dbResult).State = EntityState.Modified;
                }
                else
                {
                    var newInput = new t_signature_mobile
                    {
                        reason = (statgps) ? null : "Offline",
                        visit_id = visitid,
                        rep_id = repid,
                        dr_code = drcode,
                        sign = true,
                        created_at = DateTime.Now,
                        file_upload = filename
                    };
                    context.t_signature_mobile.Add(newInput);
                }
                //jika tidak ada maka insert baru

                context.SaveChanges();

                if (!statgps)
                {
                    DeleteGps(visitid);
                }
            }
        }

        public void AddShowTopicDuration(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                //cek data sebelumnya
                t_visit_product_topic dbResult =
                context.t_visit_product_topic.FirstOrDefault(x => x.vd_id == inputs.VdId && x.topic_id == inputs.TopicId);
                //jika ada maka di update
                if (dbResult != null)
                {
                    dbResult.show_duration = inputs.Duration;
                    context.t_visit_product_topic.Add(dbResult);
                    context.Entry(dbResult).State = EntityState.Modified;
                }
                else
                {
                    var newInput = new t_visit_product_topic
                    {
                        show_duration = inputs.Duration,
                        vd_id = inputs.VdId,
                        vpt_feedback = inputs.VptFeedBack
                    };
                    context.t_visit_product_topic.Add(newInput);
                }
                //jika tidak ada maka insert baru
                context.SaveChanges();
            }
        }

        public List<v_rep_admin_DTO> GetDataAdmin(string repid)
        {
            Expression<Func<v_rep_admin, bool>> queryFilter = PredicateHelper.True<v_rep_admin>();
            if (!String.IsNullOrEmpty(repid))
            {
                queryFilter = queryFilter.And(x => x.rep_id == repid);
            }
            queryFilter = queryFilter.And(x => x.admin_email != null);
            List<v_rep_admin> dbResult = _vRepadmin.Get(queryFilter).ToList();

            return Mapper.Map<List<v_rep_admin_DTO>>(dbResult);
        }

        public List<m_event_DTO> GetEventNameList(string budget)
        {
            Expression<Func<m_event, bool>> queryFilter = PredicateHelper.True<m_event>();

            queryFilter = queryFilter.And(x => x.event_budget == budget);

            queryFilter = queryFilter.And(x => x.event_sp == "SP2");
            List<m_event> dbResult = _mEvent.Get(queryFilter).ToList();

            List<m_event> res = dbResult.GroupBy(x => new m_event
            {
                event_description = x.event_description,
                event_detail_description = x.event_detail_description
            })
                .Select(x => new m_event
                {
                    event_description = x.Key.event_description,
                    event_detail_description = x.Key.event_detail_description
                }).OrderBy(x => x.event_description).ToList();
            return Mapper.Map<List<m_event_DTO>>(res);
        }

        public bool SaveCancellationVisit(VisitInputs inputs)
        {
            return inputs.ConditionValue == "half"
                ? _spRepo.UpdateVisitPlanByCancellation(inputs)
                : _spRepo.UpdateVisitPlanByDate(inputs);
        }

        public bool isAnyPlanOnChoosenDay(VisitInputs inputs)
        {
            Expression<Func<t_visit, bool>> queryFilter = PredicateHelper.True<t_visit>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            queryFilter =
                queryFilter.And(
                    x =>
                        x.visit_date_plan.Value <= inputs.VisitDateTime &&
                        x.visit_date_plan.Value >= inputs.VisitDateTime);
            //queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1); //tutup dl untuk tes
            List<t_visit> dbResult = _tVisit.Get(queryFilter).ToList();
            if (dbResult.Any())
            {
                return true;
            }
            return false;
        }

        public string GetFilePath(int spfid)
        {
            using (var context = new bas_trialEntities())
            {
                t_sp_attachment result = context.t_sp_attachment.Find(spfid);
                return result.spf_file_path;
            }
        }

        public void DeleteSPAttachment(int spfid)
        {
            using (var context = new bas_trialEntities())
            {
                t_sp_attachment result = context.t_sp_attachment.Find(spfid);
                context.t_sp_attachment.Remove(result);
                context.SaveChanges();
            }
        }

        public List<SummaryDoctor_DTO> GetQuadrantSummaryPlan(VisitInputs inputs)
        {
            List<SP_SelectVisitPlanDTO> dbResult = GetVisitPlan(inputs);
            var dbResult1 =
                dbResult.Select(x => new {dr_quadrant = x.dr_quadrant.Replace(" ", String.Empty), x.dr_code}).ToList();
            var res = new List<SummaryDoctor_DTO>();

            for (int i = 1; i <= 3; i++)
            {
                var data = new SummaryDoctor_DTO
                {
                    DrQuadrant = "Q" + i,
                    DrTotal = dbResult1.Where(x => x.dr_quadrant == "Q" + i).Select(x => x.dr_code).Count()
                };
                res.Add(data);
            }
            return res;
        }

        public List<SummaryDoctor_DTO> GetQuadrantSummaryRealization(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                //List<v_visit_plan_new_DTO> dbResult = GetVisitReal(inputs);
                var dbResult1 = context.v_visit_plan_new.Where(x => x.rep_id == inputs.RepId
                                                    && x.visit_date_plan.Value.Month == inputs.Month
                                                    && x.visit_date_plan.Value.Year == inputs.Year
                                                    && x.visit_plan_verification_status == 1
                                                    && x.visit_date_realization_saved == null)
                    .Select(x => new {dr_quadrant = x.dr_quadrant.Replace(" ", String.Empty), x.dr_code}).AsNoTracking()
                    .ToList();
                var res = new List<SummaryDoctor_DTO>();

                for (int i = 1; i <= 3; i++)
                {
                    var data = new SummaryDoctor_DTO
                    {
                        DrQuadrant = "Q" + i,
                        DrTotal = dbResult1.Where(x => x.dr_quadrant == "Q" + i).Select(x => x.dr_code).Count()
                    };
                    res.Add(data);
                }
                return res;
            }
        }

        public void SaveGpsLocation(VisitInputs inputs)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    t_gps_mobile dbResult = context.t_gps_mobile.FirstOrDefault(x => x.visit_id == inputs.VisitId);
                    if (dbResult != null)
                    {
                        dbResult.latitude = inputs.Latitude;
                        dbResult.longitude = inputs.Longitude;
                        dbResult.rep_id = inputs.RepId;
                        dbResult.address = inputs.Address;
                        dbResult.updatedDate = DateTime.Now;
                        context.t_gps_mobile.Add(dbResult);
                        context.Entry(dbResult).State = EntityState.Modified;
                    }
                    else
                    {
                        var newInput = new t_gps_mobile
                        {
                            visit_id = inputs.VisitId,
                            rep_id = inputs.RepId,
                            dr_code = inputs.DrCode,
                            latitude = inputs.Latitude,
                            longitude = inputs.Longitude,
                            address = inputs.Address
                        };
                        context.t_gps_mobile.Add(newInput);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<v_visit_plan_DTO> GetDataDoctorPlaned(VisitInputs inputs)
        {
            Expression<Func<v_visit_plan, bool>> queryFilter = PredicateHelper.True<v_visit_plan>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            queryFilter =
                queryFilter.And(
                    x =>
                        x.visit_date_plan.Value <= inputs.VisitDateTime &&
                        x.visit_date_plan.Value >= inputs.VisitDateTime);
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved == null);
            //queryFilter = queryFilter.And(x => x.visit_plan_verification_status == 1); // tutup dl untuk tes
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_plan>, IOrderedQueryable<v_visit_plan>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan>();
            List<v_visit_plan> dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
        }

        public List<v_info_feedback_DTO> GetTopicInfo(int topicid, int topicfeedback, int vptid, string host)
        {
            using (var context = new bas_trialEntities())
            {
                t_visit_product_topic validate = context.t_visit_product_topic.Find(vptid);
                List<v_info_feedback> dbResult =
                    context.v_info_feedback.Where(x => x.info_feedback_type == topicfeedback && x.topic_id == topicid)
                        .ToList();
                var result = new List<v_info_feedback_DTO>();
                var settingsReader = new AppSettingsReader();
                var filePath = (string) settingsReader.GetValue("TopicFiles", typeof (String));
                //~/Asset/Files/Downloads/Pdf/
                if (topicfeedback == 4)
                {
                    if (validate.note_feedback != null)
                    {
                        m_topic topicDatas = context.m_topic.Find(topicid);
                        var res = new v_info_feedback_DTO
                        {
                            info_feedback_id = 0,
                            info_feedback_type = topicfeedback,
                            info_description = validate.note_feedback,
                            topic_id = topicDatas.topic_id,
                            topic_title = topicDatas.topic_title,
                            topic_group_product = topicDatas.topic_group_product,
                            topic_filepath =
                                host.Replace("bas_api_mobile", "bas") +
                                ((topicDatas.topic_filepath == null)
                                    ? (GetStringPattern().Replace(filePath, "") + "no_available_image.jpg")
                                    : GetStringPattern().Replace(topicDatas.topic_filepath, "")),
                            topic_file_name = topicDatas.topic_file_name,
                            is_selected = 0
                        };
                        result.Add(res);
                    }
                }
                else
                {
                    foreach (v_info_feedback data in dbResult)
                    {
                        var res = new v_info_feedback_DTO
                        {
                            info_feedback_id = data.info_feedback_id,
                            info_feedback_type = data.info_feedback_type,
                            info_description = data.info_description,
                            topic_id = data.topic_id,
                            topic_group_product = data.topic_group_product,
                            topic_title = data.topic_title,
                            topic_filepath =
                                host.Replace("bas_api_mobile", "bas") +
                                ((data.topic_filepath == null)
                                    ? (GetStringPattern().Replace(filePath, "") + "no_available_image.jpg")
                                    : GetStringPattern().Replace(data.topic_filepath, "")),
                            topic_file_name = data.topic_file_name,
                            is_selected = (validate.info_feedback_id == data.info_feedback_id) ? 1 : 0
                        };
                        result.Add(res);
                    }
                }


                var viewMapper = Mapper.Map<List<v_info_feedback_DTO>>(result);
                return viewMapper;
            }
        }

        public string GetTopicPath(int topicid)
        {
            using (var context = new bas_trialEntities())
            {
                m_topic topicpath = context.m_topic.Find(topicid);
                return topicpath.topic_filepath;
            }
        }


        public List<t_visit_DTO> GetVisitData(VisitInputs inputs)
        {
            Expression<Func<t_visit, bool>> queryFilter = PredicateHelper.True<t_visit>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            queryFilter =
                queryFilter.And(
                    x =>
                        x.visit_date_plan.Value <= inputs.VisitDatePlan &&
                        x.visit_date_plan.Value >= inputs.VisitDatePlan);
            queryFilter = queryFilter.And(x => x.dr_code <= 100005);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<t_visit>, IOrderedQueryable<t_visit>> orderByFilter = sortCriteria.GetOrderByFunc<t_visit>();
            List<t_visit> dbResult = _tVisit.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<t_visit_DTO>>(dbResult);
        }

        //public List<m_product_custom_DTO> GetDataProductListPlan()
        //{
        //    using (var context = new bas_trialEntities())
        //    {
        //        var dbResult = from a in context.m_product
        //            where a.prd_status == 1 
        //            && a.visit_code != "T0" 
        //            && a.visit_code != "T00"
        //                       group a by new m_product()
        //            {
        //                visit_code=a.visit_code,
        //                visit_team=a.visit_team,
        //                visit_product=a.visit_product,
        //                visit_category=a.visit_category
        //            }
        //            into grp
        //            select new m_product()
        //            {
        //                visit_code = grp.Key.visit_code.TrimEnd(),
        //                visit_team = grp.Key.visit_team.TrimEnd(),
        //                visit_product = grp.Key.visit_product.TrimEnd(),
        //                visit_category = grp.Key.visit_category.TrimEnd()
        //            };
        //        var viewMapper = Mapper.Map<List<m_product_custom_DTO>>(dbResult);
        //        return viewMapper;
        //    }
        //}

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

        public List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductListPlan()
        {
            return _spRepo.GetDataProductList().Where(x => x.visit_code != "T00").ToList();
        }

        public bool InsertVisitProduct(VisitInputs inputs)
        {
            return _spRepo.InsertVisitProduct(inputs);
        }

        public List<SP_SELECT_PRODUCT_USER_DTO> dsProductLookup()
        {
            return _spRepo.dsProductLookup();
        }

        public void InsertProductTopic(VisitInputs inputs)
        {
            _spRepo.InsertProductTopic(inputs.VdId, inputs.TopicId);
        }

        public bool UpdateFeedbackTopic(VisitInputs inputs)
        {
            return _visitRepo.UpdateFeedbackTopic(inputs);
        }

        public bool isValidDay(VisitInputs inputs)
        {
            List<t_visit_DTO> dbResult = GetVisitData(inputs);
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

        public List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSpRealization(string visitid)
        {
            return _spRepo.GetSpRealization(visitid);
        }

        public List<t_sp_attachment_DTO> GetVRealAttachment(string visitid, string host)
        {
            using (var context = new bas_trialEntities())
            {
                var res = context.t_sp_attachment.Where(x => x.spf_file_name.Contains(visitid)).ToList();
                var newReturn = res.Select(x => new t_sp_attachment()
                {
                    spf_id = x.spf_id,
                    spr_id = x.spr_id,
                    spf_file_name = x.spf_file_name,
                    spf_file_path = (host.Replace("bas_api_mobile", "bas").Contains("vodjo") ? (host.Replace("bas_api_mobile", "bas") + ":8088/") : host.Replace("bas_api_mobile", "bas")) + GetStringPattern().Replace(x.spf_file_path, ""),
                    spf_date_uploaded = x.spf_date_uploaded
                }).ToList();
                return Mapper.Map<List<t_sp_attachment_DTO>>(newReturn);
            }
        }

        public List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSpPlan(string visitid)
        {
            return _spRepo.GetSpPlan(visitid);
            //using (var context = new bas_trialEntities())
            //{
            //    var dbResult = context.SP_SELECT_VISIT_SP_PLAN(inputs.VisitId);
            //    var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_DTO>>(dbResult);
            //    return viewMapper;
            //}
        }

        public IEnumerable<int> GetTopicFeedback(int vptId)
        {
            return _visitRepo.GetTopicFeedback(vptId);
        }

        public List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid)
        {
            return _spRepo.GetSPAttachment(visitid);
        }

        public int GetParamMaxVisit()
        {
            using (var context = new bas_trialEntities())
            {
                decimal dbResult =
                    context.C_parameter.Where(x => x.transaction_id == "AV")
                        .Select(x => x.max_value.Value)
                        .FirstOrDefault();
                int amount = Convert.ToInt32(dbResult);

                return amount;
            }
        }

        public int CheckMaxVisit(VisitInputs inputs)
        {
            return _spRepo.CheckMaxVisit(inputs);
        }

        public bool CheckNewMaxVisit(VisitInputs inputs)
        {
            return _spRepo.CheckNewMaxVisit(inputs);
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

        #region visit actual

        public List<v_visit_plan_new_DTO> GetVisitActual(VisitInputs inputs)
        {
            Expression<Func<v_visit_plan_new, bool>> queryFilter = PredicateHelper.True<v_visit_plan_new>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Day > 0)
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Day == inputs.Day);
            }
            queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month);
            queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.Year);
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved.Value != null);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            Func<IQueryable<v_visit_plan_new>, IOrderedQueryable<v_visit_plan_new>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan_new>();
            List<v_visit_plan_new> dbResult = _vVisitPlanNew.Get(queryFilter, orderByFilter).ToList();
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    dbResult =
                        dbResult.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString()
                                    .Contains(arrSearch[i])).ToList();
            }
            return Mapper.Map<List<v_visit_plan_new_DTO>>(dbResult);
        }


        //public List<v_visit_plan_DTO> GetVisitActual(VisitInputs inputs)
        //{
        //    Expression<Func<v_visit_plan, bool>> queryFilter = PredicateHelper.True<v_visit_plan>();
        //    if (!String.IsNullOrEmpty(inputs.RepId))
        //    {
        //        queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
        //    }
        //    if (inputs.Day > 0)
        //    {
        //        queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Day == inputs.Day);
        //    }
        //    queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month);
        //    queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.Year);
        //    queryFilter = queryFilter.And(x => x.visit_date_realization_saved.Value != null);
        //    var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
        //    Func<IQueryable<v_visit_plan>, IOrderedQueryable<v_visit_plan>> orderByFilter =
        //        sortCriteria.GetOrderByFunc<v_visit_plan>();
        //    List<v_visit_plan> dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();
        //    if (!String.IsNullOrEmpty(inputs.SearchColumn))
        //    {
        //        string[] arrSearch = inputs.SearchValue.Split(',');
        //        string[] arrColumn = inputs.SearchColumn.Split(',');
        //        for (int i = 0; i < arrColumn.Length; i++)
        //            dbResult =
        //                dbResult.Where(
        //                    r =>
        //                        r.GetType()
        //                            .GetProperty(arrColumn[i])
        //                            .GetValue(r, null)
        //                            .ToString()
        //                            .Contains(arrSearch[i])).ToList();
        //    }
        //    return Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
        //}


        public List<v_visit_product_DTO> GetDataDetail(VisitInputs inputs)
        {
            Expression<Func<v_visit_product, bool>> queryFilter = PredicateHelper.True<v_visit_product>();
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            List<v_visit_product> dbResult = _vVisitProductRepo.Get(queryFilter).ToList();
            var viewMapper = Mapper.Map<List<v_visit_product_DTO>>(dbResult);
            return viewMapper;
        }

        public List<v_visit_product_DTO> GetDataDetailAll(List<string> visitid)
        {
            Expression<Func<v_visit_product, bool>> queryFilter = PredicateHelper.True<v_visit_product>();
            queryFilter = queryFilter.And(x => visitid.Contains(x.visit_id));
            List<v_visit_product> dbResult = _vVisitProductRepo.Get(queryFilter).ToList();
            var viewMapper = Mapper.Map<List<v_visit_product_DTO>>(dbResult);
            return viewMapper;
        }

        public List<m_product_custom_DTO> GetDataProductListActual()
        {
            using (var context = new bas_trialEntities())
            {
                List<m_product_custom_DTO> dbResult =
                    context.m_product.Where(x => x.prd_status == 1 && x.visit_code != "t0")
                        .GroupBy(x => new m_product_custom_DTO
                        {
                            visit_category = x.visit_category,
                            visit_code = x.visit_code,
                            visit_product = x.visit_product,
                            visit_team = x.visit_team
                        })
                        .Select(x => new m_product_custom_DTO
                        {
                            visit_category = x.Key.visit_category.Replace(" ", String.Empty),
                            visit_code = x.Key.visit_code.Replace(" ", String.Empty),
                            visit_product = x.Key.visit_product.Replace(" ", String.Empty),
                            visit_team = x.Key.visit_team.Replace(" ", String.Empty)
                        })
                        .OrderBy(x => x.visit_team)
                        .ToList();
                return dbResult;
            }
        }

        public void DeleteDetailProduct(int vdid)
        {
            _visitRepo.DeleteDetailProduct(vdid);
        }

        public void AddDetailProduct(VisitInputs inputs)
        {
            _spRepo.AddDetailProduct(inputs);
        }

        public string editValidation(string visitid)
        {
            using (var context = new bas_trialEntities())
            {
                try
                {
                    List<t_visit_product> dbResult = context.t_visit_product.Where(x => x.visit_id == visitid).ToList();
                    if (dbResult.Any())
                    {
                        if (dbResult.Select(x => x.visit_code.Replace(" ", String.Empty)).Contains("T0"))
                        {
                            return "T0";
                        }
                        return "pass";
                    }
                    return "null_product";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void InsertNotVisitedByDefault(string visitid)
        {
            _spRepo.InsertNotVisitedByDefault(visitid);
        }

        public bool ExecUpdate(VisitInputs inputs)
        {
            return _spRepo.ExecUpdate(inputs);
        }

        public List<v_visit_plan_DTO> GetVisitActualExport(SP_SELECT_VISIT_PLAN_INPUTS inputs)
        {
            Expression<Func<v_visit_plan, bool>> queryFilter = PredicateHelper.True<v_visit_plan>();
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            if (!String.IsNullOrEmpty(inputs.DrName))
            {
                queryFilter = queryFilter.And(x => x.dr_name == inputs.DrName);
            }
            if (!String.IsNullOrEmpty(inputs.DrSpec))
            {
                queryFilter = queryFilter.And(x => x.dr_spec == inputs.DrSpec);
            }
            if (!String.IsNullOrEmpty(inputs.DrSubSpec))
            {
                queryFilter = queryFilter.And(x => x.dr_sub_spec == inputs.DrSubSpec);
            }
            if (!String.IsNullOrEmpty(inputs.DrQuadrant))
            {
                queryFilter = queryFilter.And(x => x.dr_quadrant == inputs.DrQuadrant);
            }
            if (!String.IsNullOrEmpty(inputs.DrMonitoring))
            {
                queryFilter = queryFilter.And(x => x.dr_monitoring == inputs.DrMonitoring);
            }
            if (!String.IsNullOrEmpty(inputs.DrDkLk))
            {
                queryFilter = queryFilter.And(x => x.dr_dk_lk == inputs.DrDkLk);
            }
            if (!String.IsNullOrEmpty(inputs.DrAreaMis))
            {
                queryFilter = queryFilter.And(x => x.dr_area_mis == inputs.DrMonitoring);
            }
            if (!String.IsNullOrEmpty(inputs.DrCategory))
            {
                queryFilter = queryFilter.And(x => x.dr_category == inputs.DrCategory);
            }
            if (!String.IsNullOrEmpty(inputs.DrChannel))
            {
                queryFilter = queryFilter.And(x => x.dr_chanel == inputs.DrChannel);
            }
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Day > 0)
            {
                queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Day == inputs.Day);
            }
            queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Month == inputs.Month);
            queryFilter = queryFilter.And(x => x.visit_date_plan.Value.Year == inputs.Year);
            queryFilter = queryFilter.And(x => x.visit_date_realization_saved.Value != null);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] {inputs.SortExpression}, inputs.SortOrder);
            Func<IQueryable<v_visit_plan>, IOrderedQueryable<v_visit_plan>> orderByFilter =
                sortCriteria.GetOrderByFunc<v_visit_plan>();
            List<v_visit_plan> dbResult = _viewVisitPlanRepositories.Get(queryFilter, orderByFilter).ToList();
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    dbResult =
                        dbResult.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString()
                                    .Contains(arrSearch[i])).ToList();
            }
            return Mapper.Map<List<v_visit_plan_DTO>>(dbResult);
        }

        public List<SummaryDoctor_DTO> GetQuadrantSummaryActual(VisitInputs inputs)
        {
            List<v_visit_plan_new_DTO> dbResult1 = GetVisitActual(inputs);
            var dbResult = dbResult1.Where(x => x.rep_id == inputs.RepId
                                                && x.visit_date_plan.Value.Month == inputs.Month
                                                && x.visit_date_plan.Value.Year == inputs.Year
                                                && x.visit_date_realization_saved != null)
                .Select(x => new {dr_quadrant = x.dr_quadrant.Replace(" ", String.Empty), x.dr_code})
                .ToList();
            var res = new List<SummaryDoctor_DTO>();

            for (int i = 1; i <= 3; i++)
            {
                var data = new SummaryDoctor_DTO
                {
                    DrQuadrant = "Q" + i,
                    DrTotal = dbResult.Where(x => x.dr_quadrant == "Q" + i).Select(x => x.dr_code).Count()
                };
                res.Add(data);
            }
            return res;
        }

        #endregion

        #region visithistory

        public List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistory(VisitInputs inputs)
        {
            return _spRepo.GetVisitHistory(inputs);
        }

        public List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistorySearch(VisitInputs inputs)
        {
            return _spRepo.GetVisitHistorySearch(inputs);
        }

        public List<SummaryDoctor_DTO> GetQuadrantSummaryHistory(VisitInputs inputs)
        {
            List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> dbResult = _spRepo.GetVisitHistory(inputs);
            var dbResult2 = dbResult
                .Select(x => new {dr_quadrant = x.dr_quadrant.Replace(" ", String.Empty), x.dr_code})
                .ToList();
            var res = new List<SummaryDoctor_DTO>();

            for (int i = 1; i <= 3; i++)
            {
                var data = new SummaryDoctor_DTO
                {
                    DrQuadrant = "Q" + i,
                    DrTotal = dbResult2.Where(x => x.dr_quadrant == "Q" + i).Select(x => x.dr_code).Count()
                };
                res.Add(data);
            }
            return res;
        }

        #endregion

        public string GetVisitId()
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    string dbResult = context.SP_GET_NEW_VISIT_NUMBER().FirstOrDefault();
                    return dbResult;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool isAlreadyPlannedDoctorInCurrWeek(DateTime datePlan, string drCode, string id)
        {
            
            //try
            //{
            //    using (var context = new bas_trialEntities())
            //    {
                    
            //        var date = string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-ENG"), "yyyy-MM-dd",datePlan);
                    
                    
            //        if (dbResult.Any())
            //        {
            //            var err = new Exception();
            //            _vtLogger.Err(err, new List<object> { dbResult });
            //            return true;
            //        }
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    _vtLogger.Err(ex, new List<object> { dbResult });
            //    return true;
            //}
            int code = Convert.ToInt32(drCode);
            var context = new bas_trialEntities();
            try
            {
                var dbResult = context.SP_SELECT_IS_ALREADY_PLANNED_DOCTOR_IN_CURR_WEEK_MOBILE(id, datePlan, code).FirstOrDefault();
                if (dbResult == 0)
                {
                    return false;
                }
                return true;
            }catch(Exception ex)
            {
                //_vtLogger.Err(ex, new List<object> { dbResult });
                return true;
            }
        }

        public bool isMaxLimitedDoctorinCurrDayOnRunning(DateTime datePlan, string id, string position)
        {
            return isMaxLimitedDoctorinCurrDayOnInit(datePlan, id, position);
        }

        public bool isAlreadyPlannedDoctorInCurrDay(DateTime datePlan, string drCode, string id)
        {
            try
            {
                int code = Convert.ToInt32(drCode);
                using (var context = new bas_trialEntities())
                {
                    List<v_visit_plan> dbResult = context.v_visit_plan.Where(x =>
                        x.visit_date_plan.Value <= datePlan && x.visit_date_plan.Value >= datePlan
                        && x.rep_id == id &&
                        x.dr_code.Value == code).ToList();
                    if (dbResult.Any())
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool isAlreadyPlannedLeaveInCurrDay(DateTime datePlan, string id)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    List<t_visit> dbResult = context.t_visit.Where(x =>
                        x.visit_date_plan.Value <= datePlan && x.visit_date_plan.Value >= datePlan
                        && x.rep_id == id
                        && x.dr_code < 100005).ToList();

                    if (dbResult.Any())
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                var inputs = new
                {
                    dateplan = datePlan,
                    rep_id = id
                };
                _vtLogger.Err(ex, new List<object> { inputs });
                return true;
            }
        }

        public bool isMaxLimitedDoctorinCurrDayOnInit(DateTime datePlan, string id, string position)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    //var date = string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-ENG"), "yyyy-MM-dd", datePlan); //datePlan.ToString("yyyy-MM-dd");
                    //int? dbResult = context.f_isMaxLimitedDoctorinCurrDay(id, date).FirstOrDefault();
                    var sql = "EXEC _isMaxLimitedDoctorinCurrDayMobile '" + id + "','" + datePlan + "' ";
                    var dbResult = context.Database.SqlQuery<int>(sql).FirstOrDefault();
                    return Convert.ToBoolean(dbResult);
                }
            }
            catch (Exception ex)
            {
                _vtLogger.Err(ex, new List<object> { datePlan, id });
                return true;
            }
        }
        //public bool isMaxLimitedDoctorinCurrDayOnInit(DateTime datePlan, string id, string position)
        //{
        //    try
        //    {
        //        using (var context = new bas_trialEntities())
        //        {
        //            List<t_visit> dbResult = context.t_visit
        //                .Where(x =>
        //                    x.visit_date_plan.Value <= datePlan && x.visit_date_plan.Value >= datePlan
        //                    && x.rep_id == id).ToList();
        //            if (position == "MR")
        //            {
        //                if (dbResult.Count > 10)
        //                    return true;
        //            }
        //            else if (position == "PS")
        //            {
        //                if (dbResult.Count > 5)
        //                    return true;
        //            }
        //            else if (position == "MS")
        //            {
        //                if (dbResult.Count > 8)
        //                    return true;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return true;
        //    }
        //}

        public Regex GetStringPattern()
        {
            var pattern = new Regex("[~;,\t\r ]|[\n]{2}");
            return pattern;
        }   

        #region Visit Associate
        public List<SP_SELECT_VISIT_SP_ASSOCIATED_DTO> GetSPVisitAssociated(VisitAssociatedRequestModel inputs)
        {
            using (var context = new bas_trialEntities())
            {
                IEnumerable<SP_SELECT_VISIT_ASSOCIATED_Result> dbResult =
                    context.SP_SELECT_VISIT_ASSOCIATED(inputs.RepId);

                if (inputs.VisitDate.HasValue)
                {
                    dbResult = dbResult.Where(x => x.visit_date_plan_invited == inputs.VisitDate.Value);
                }

                if (!String.IsNullOrEmpty(inputs.AssociatedBy))
                {
                    dbResult = dbResult.Where(x => x.initiator_name.IndexOf(inputs.AssociatedBy, StringComparison.InvariantCultureIgnoreCase) >= 0);
                }

                if (!String.IsNullOrEmpty(inputs.DrName))
                {
                    dbResult = dbResult.Where(x => x.dr_name.IndexOf(inputs.DrName, StringComparison.InvariantCultureIgnoreCase) >= 0);
                }

                if (!String.IsNullOrEmpty(inputs.SearchValue))
                {
                    dbResult = dbResult.Where(
                        x => x.dr_name.IndexOf(inputs.SearchValue, StringComparison.InvariantCultureIgnoreCase) >= 0
                        || x.initiator_name.IndexOf(inputs.SearchValue, StringComparison.InvariantCultureIgnoreCase) >= 0
                        || x.initiator_email.IndexOf(inputs.SearchValue, StringComparison.InvariantCultureIgnoreCase) >= 0
                        || x.visit_id_initiator.IndexOf(inputs.SearchValue, StringComparison.InvariantCultureIgnoreCase) >= 0
                        || x.visit_id_invited.IndexOf(inputs.SearchValue, StringComparison.InvariantCultureIgnoreCase) >= 0
                        );
                }

                var res = dbResult.Select(x => new SP_SELECT_VISIT_SP_ASSOCIATED_DTO
                {
                    associate_id = x.associate_id,
                    associate_status = x.associate_status,
                    dr_code_initiator = x.dr_code_initiator,
                    dr_name = x.dr_name,
                    initiator_email = x.initiator_email,
                    initiator_name = x.initiator_name,
                    invited_email = x.invited_email,
                    invited_name = x.invited_name,
                    rep_id_initiator = x.rep_id_initiator,
                    rep_id_invited = x.rep_id_invited,
                    va_notif = x.va_notif,
                    visit_date_plan_initiator = x.visit_date_plan_initiator,
                    visit_date_plan_invited = x.visit_date_plan_invited,
                    visit_date_realization_saved = x.visit_date_realization_saved,
                    visit_id_initiator = x.visit_id_initiator,
                    visit_id_invited = x.visit_id_invited
                }).ToList();

                return res;
            }
        }

        public List<SP_VISIT_ASSOCIATE_NOTIFICATION_DTO> GetSPVisitAssociatedNotification(VisitInputs inputs)
        {
            using (var context = new bas_trialEntities())
            {
                IEnumerable<SP_SELECT_VISIT_ASSOCIATED_Result> dbResult =
                    context.SP_SELECT_VISIT_ASSOCIATED(inputs.RepId);

                var res = dbResult.Select(x => new SP_VISIT_ASSOCIATE_NOTIFICATION_DTO
                {
                    associate_id = x.associate_id,
                    va_notif = x.va_notif,
                    visit_date_plan_invited = x.visit_date_plan_invited
                }).ToList();

                return res;
            }
        }

        public void SPConfirmVisitAssoicated(VisitAssociatedConfirmRequestModel inputs)
        {
            using (var context = new bas_trialEntities())
            {
             context.SP_CONFIRM_ASSOCIATE_VISIT(inputs.AssociateId);
            }
        }

        public void SPRejectVisitAssoicated(VisitAssociatedConfirmRequestModel inputs)
        {
            using (var context = new bas_trialEntities())
            {
                context.SP_REJECT_ASSOCIATE_VISIT(inputs.AssociateId);
            }
        }

        public SP_SELECT_VISIT_SP_ASSOCIATED_DTO GetSPVisitAssociatedByVisitId(string VisitId)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.SP_SELECT_VISIT_ASSOCIATED_BY_VISIT_ID(VisitId);

                var res = dbResult.Select(x => new SP_SELECT_VISIT_SP_ASSOCIATED_DTO
                {
                    associate_id = x.associate_id,
                    associate_status = x.associate_status,
                    dr_code_initiator = x.dr_code_initiator,
                    dr_name = x.dr_name,
                    initiator_email = x.initiator_email,
                    initiator_name = x.initiator_name,
                    invited_email = x.invited_email,
                    invited_name = x.invited_name,
                    rep_id_initiator = x.rep_id_initiator,
                    rep_id_invited = x.rep_id_invited,
                    va_notif = x.va_notif,
                    visit_date_plan_initiator = x.visit_date_plan_initiator,
                    visit_date_plan_invited = x.visit_date_plan_invited,
                    visit_date_realization_saved = x.visit_date_realization_saved,
                    visit_id_initiator = x.visit_id_initiator,
                    visit_id_invited = x.visit_id_invited
                }).FirstOrDefault();

                return res;
            }
        }

        public v_visit_associated_DTO GetVisitAssociatedById(int associate_id)
        {
            var dbResult = _vVisitAssociated.GetAll().Where(x => x.associate_id == associate_id).FirstOrDefault();
            if (dbResult!= null)
                return new v_visit_associated_DTO
                {
                    associate_id = dbResult.associate_id,
                    associate_status = dbResult.associate_status,
                    dr_code_initiator = dbResult.associate_status,
                    dr_code_invited = dbResult.dr_code_invited,
                    rep_id_initiator = dbResult.rep_id_initiator,
                    rep_id_invited = dbResult.rep_id_invited,
                    visit_code = dbResult.visit_code,
                    visit_id_initiator = dbResult.visit_id_initiator,
                    visit_id_invited = dbResult.visit_id_invited
                };

            return null;
        }

        #endregion

        #region download data offline

        //public string GenerateJsonFiles(VisitInputs inputs)
        //{
        //    //var res = new List<OfflineResponseModel>();
        //    var res = new List<string>();
        //    string linkStr = "";
        //    inputs.TableName = "1.t_visit";
        //    List<t_visit_DTO> tVisit = _spRepo.OfflineTVisit(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {tVisit});
        //    linkStr = _jsonFilesGenerator.GenFileLink(inputs, new List<object> {tVisit});
        //    res.Add(linkStr);

        //    inputs.TableName = "2.m_customer_aso";
        //    List<m_customer_aso_DTO> mCustomerAso = _spRepo.OfflineMCustomerAso(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {mCustomerAso});

        //    inputs.TableName = "3.m_doctor";
        //    List<m_doctor_DTO> mDoctor = _spRepo.OfflineMDoctor(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {mDoctor});

        //    inputs.TableName = "4.t_visit_product";
        //    var tVisitProduct = _spRepo.OfflineTVisitProduct(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {tVisitProduct});

        //    inputs.TableName = "5.t_visit_product_topic";
        //    var tVisitProductTopic = _spRepo.OfflineTVisitProductTopic(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {tVisitProductTopic});

        //    inputs.TableName = "6.m_bo";
        //    List<m_bo_DTO> mBo = _spRepo.OfflineMBo(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {mBo});

        //    inputs.TableName = "7.m_regional";
        //    List<m_regional_DTO> mRegional = _spRepo.OfflineMRegional(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {mRegional});

        //    inputs.TableName = "8.Karyawan";
        //    List<Karyawan_DTO> karyawan = _spRepo.OfflineKaryawan(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {karyawan});

        //    inputs.TableName = "9.Bagian";
        //    List<Bagian_DTO> bagian = _spRepo.OfflineBagian(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {bagian});

        //    inputs.TableName = "10.Jabatan";
        //    List<Jabatan_DTO> jabatan = _spRepo.OfflineJabatan(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {jabatan});

        //    inputs.TableName = "11.HeadQuarter";
        //    List<HeadQuarter_DTO> headQuarter = _spRepo.OfflineHeadQuarter(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {headQuarter});

        //    inputs.TableName = "12.Departemen";
        //    List<Departemen_DTO> departmen = _spRepo.OfflineDepartemen(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {departmen});

        //    inputs.TableName = "13.m_rep";
        //    List<m_rep_DTO> OfflineMRep = _spRepo.OfflineMRep(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {OfflineMRep});

        //    inputs.TableName = "14.m_sbo";
        //    List<m_sbo_DTO> OfflineMSbo = _spRepo.OfflineMSbo(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {OfflineMSbo});

        //    inputs.TableName = "15.m_topic";
        //    List<m_topic_DTO> OfflineMTopic = _spRepo.OfflineMTopic(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {OfflineMTopic});

        //    inputs.TableName = "16.m_gl_cs";
        //    List<m_gl_cs_DTO> OfflineMGlCs = _spRepo.OfflineMGlCs(inputs);
        //    _jsonFilesGenerator.GenFile(inputs, new List<object> {OfflineMGlCs});

        //    string resultLink = _jsonFilesGenerator.ZipFiles(inputs);
        //    // zip folder

        //    return resultLink;
        //}

        public List<string> GenerateLinkJsonFiles(VisitInputs inputs)
        {
            var res = new List<string>();

            inputs.TableName = "1.t_visit";
            List<t_visit_DTO> tVisit = _spRepo.OfflineTVisit(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {tVisit}));

            inputs.TableName = "2.m_customer_aso";
            List<m_customer_aso_DTO> mCustomerAso = _spRepo.OfflineMCustomerAso(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {mCustomerAso}));

            inputs.TableName = "3.m_doctor";
            List<m_doctor_DTO> mDoctor = _spRepo.OfflineMDoctor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {mDoctor}));

            inputs.TableName = "4.t_visit_product";
            var tVisitProduct = _spRepo.OfflineTVisitProduct(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {tVisitProduct}));

            inputs.TableName = "5.t_visit_product_topic";
            var tVisitProductTopic = _spRepo.OfflineTVisitProductTopic(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {tVisitProductTopic}));

            inputs.TableName = "6.m_bo";
            List<m_bo_DTO> mBo = _spRepo.OfflineMBo(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {mBo}));

            inputs.TableName = "7.m_regional";
            List<m_regional_DTO> mRegional = _spRepo.OfflineMRegional(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {mRegional}));

            inputs.TableName = "8.Karyawan";
            List<Karyawan_DTO> karyawan = _spRepo.OfflineKaryawan(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {karyawan}));

            inputs.TableName = "9.Bagian";
            List<Bagian_DTO> bagian = _spRepo.OfflineBagian(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {bagian}));

            inputs.TableName = "10.Jabatan";
            List<Jabatan_DTO> jabatan = _spRepo.OfflineJabatan(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {jabatan}));

            inputs.TableName = "11.HeadQuarter";
            List<HeadQuarter_DTO> headQuarter = _spRepo.OfflineHeadQuarter(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {headQuarter}));

            inputs.TableName = "12.Departemen";
            List<Departemen_DTO> departmen = _spRepo.OfflineDepartemen(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {departmen}));

            inputs.TableName = "13.m_rep";
            List<m_rep_DTO> OfflineMRep = _spRepo.OfflineMRep(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {OfflineMRep}));

            inputs.TableName = "14.m_sbo";
            List<m_sbo_DTO> OfflineMSbo = _spRepo.OfflineMSbo(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {OfflineMSbo}));

            inputs.TableName = "15.m_topic";
            List<m_topic_DTO> OfflineMTopic = _spRepo.OfflineMTopic(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {OfflineMTopic}));

            inputs.TableName = "16.m_gl_cs";
            List<m_gl_cs_DTO> OfflineMGlCs = _spRepo.OfflineMGlCs(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> {OfflineMGlCs}));

            inputs.TableName = "17.m_product";
            List<m_product_DTO> OfflineMProduct = _spRepo.OfflineMProduct(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMProduct }));

            inputs.TableName = "18.t_signature_mobile";
            var OfflineTSignature = _spRepo.OfflineTSignature(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSignature }));

            inputs.TableName = "19.t_gps_mobile";
            var OfflineTGps = _spRepo.OfflineTGps(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTGps }));

            inputs.TableName = "20.m_event";
            List<m_event_DTO> OfflineMEvent = _spRepo.OfflineMEvent(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMEvent }));

            inputs.TableName = "21.m_sponsor";
            List<m_sponsor_DTO> OfflineMSponsor = _spRepo.OfflineMSponsor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMSponsor }));

            inputs.TableName = "22.m_status";
            List<m_status_DTO> OfflineMStatus = _spRepo.OfflineMStatus(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMStatus }));

            inputs.TableName = "23.t_spr";
            var OfflineTspOfflineTSpr = _spRepo.OfflineTSpr(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTspOfflineTSpr }));

            inputs.TableName = "24.t_sp";
            var OfflineTSp = _spRepo.OfflineTSp(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSp }));
            
            inputs.TableName = "25.t_sp_doctor";
            var OfflineTSpDoctor = _spRepo.OfflineTSpDoctor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSpDoctor }));
            
            inputs.TableName = "26.t_sp_sponsor";
            var OfflineTSpSponsor = _spRepo.OfflineTSpSponsor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSpSponsor }));

            inputs.TableName = "27.t_info_feedback_topic_mapping_mobile";
            var OfflineInfoFeedbackMapping = _spRepo.OfflineInfoFeedbackMapping(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineInfoFeedbackMapping }));

            inputs.TableName = "28.m_info_feedback_mobile";
            var OfflineInfoFeedback = _spRepo.OfflineInfoFeedback(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineInfoFeedback }));

            return res;
        }

        public List<string> GetAllDataMaster(VisitInputs inputs)
        {
            var res = new List<string>();

            inputs.TableName = "2.m_customer_aso";
            List<m_customer_aso_DTO> mCustomerAso = _spRepo.OfflineMCustomerAso(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { mCustomerAso }));

            inputs.TableName = "3.m_doctor";
            List<m_doctor_DTO> mDoctor = _spRepo.OfflineMDoctor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { mDoctor }));

            inputs.TableName = "6.m_bo";
            List<m_bo_DTO> mBo = _spRepo.OfflineMBo(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { mBo }));

            inputs.TableName = "7.m_regional";
            List<m_regional_DTO> mRegional = _spRepo.OfflineMRegional(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { mRegional }));

            inputs.TableName = "8.Karyawan";
            List<Karyawan_DTO> karyawan = _spRepo.OfflineKaryawan(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { karyawan }));

            inputs.TableName = "9.Bagian";
            List<Bagian_DTO> bagian = _spRepo.OfflineBagian(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { bagian }));

            inputs.TableName = "10.Jabatan";
            List<Jabatan_DTO> jabatan = _spRepo.OfflineJabatan(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { jabatan }));

            inputs.TableName = "11.HeadQuarter";
            List<HeadQuarter_DTO> headQuarter = _spRepo.OfflineHeadQuarter(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { headQuarter }));

            inputs.TableName = "12.Departemen";
            List<Departemen_DTO> departmen = _spRepo.OfflineDepartemen(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { departmen }));

            inputs.TableName = "13.m_rep";
            List<m_rep_DTO> OfflineMRep = _spRepo.OfflineMRep(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMRep }));

            inputs.TableName = "14.m_sbo";
            List<m_sbo_DTO> OfflineMSbo = _spRepo.OfflineMSbo(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMSbo }));

            inputs.TableName = "15.m_topic";
            List<m_topic_DTO> OfflineMTopic = _spRepo.OfflineMTopic(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMTopic }));

            inputs.TableName = "16.m_gl_cs";
            List<m_gl_cs_DTO> OfflineMGlCs = _spRepo.OfflineMGlCs(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMGlCs }));

            inputs.TableName = "17.m_product";
            List<m_product_DTO> OfflineMProduct = _spRepo.OfflineMProduct(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMProduct }));

            inputs.TableName = "20.m_event";
            List<m_event_DTO> OfflineMEvent = _spRepo.OfflineMEvent(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMEvent }));

            inputs.TableName = "21.m_sponsor";
            List<m_sponsor_DTO> OfflineMSponsor = _spRepo.OfflineMSponsor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMSponsor }));

            inputs.TableName = "22.m_status";
            List<m_status_DTO> OfflineMStatus = _spRepo.OfflineMStatus(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineMStatus }));

            inputs.TableName = "27.t_info_feedback_topic_mapping_mobile";
            var OfflineInfoFeedbackMapping = _spRepo.OfflineInfoFeedbackMapping(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineInfoFeedbackMapping }));

            inputs.TableName = "28.m_info_feedback_mobile";
            var OfflineInfoFeedback = _spRepo.OfflineInfoFeedback(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineInfoFeedback }));
            return res;
        }

        public List<string> GetAllDataTrans(VisitInputs inputs)
        {
            var res = new List<string>();

            inputs.TableName = "1.t_visit";
            List<t_visit_DTO> tVisit = _spRepo.OfflineTVisit(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { tVisit }));

            inputs.TableName = "4.t_visit_product";
            var tVisitProduct = _spRepo.OfflineTVisitProduct(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { tVisitProduct }));

            inputs.TableName = "5.t_visit_product_topic";
            var tVisitProductTopic = _spRepo.OfflineTVisitProductTopic(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { tVisitProductTopic }));

            inputs.TableName = "18.t_signature_mobile";
            var OfflineTSignature = _spRepo.OfflineTSignature(inputs);
            var resTSignature = new List<sp_offline_t_signature_mobile_DTO>();
            foreach (var data in OfflineTSignature)
            {
                var arrTSign = new sp_offline_t_signature_mobile_DTO()
                {
                    signature_id = data.signature_id,
                    m_signature_id = data.m_signature_id,
                    visit_id = data.visit_id,
                    rep_id = data.rep_id,
                    dr_code = data.dr_code,
                    sign = data.sign,
                    reason = (String.IsNullOrEmpty(data.reason) ? String.Empty : data.reason),
                    created_at = data.created_at,
                    updated_at = data.updated_at,
                    file_upload = (String.IsNullOrEmpty(data.file_upload) ? String.Empty : inputs.Host.Replace("bas_api_mobile", "bas") + data.file_upload.Substring(1)) 
                };
                resTSignature.Add(arrTSign);
            }
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { resTSignature }));

            inputs.TableName = "19.t_gps_mobile";
            var OfflineTGps = _spRepo.OfflineTGps(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTGps }));

            inputs.TableName = "23.t_spr";
            var OfflineTspOfflineTSpr = _spRepo.OfflineTSpr(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTspOfflineTSpr }));

            inputs.TableName = "24.t_sp";
            var OfflineTSp = _spRepo.OfflineTSp(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSp }));

            inputs.TableName = "25.t_sp_doctor";
            var OfflineTSpDoctor = _spRepo.OfflineTSpDoctor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSpDoctor }));

            inputs.TableName = "26.t_sp_sponsor";
            var OfflineTSpSponsor = _spRepo.OfflineTSpSponsor(inputs);
            res.Add(_jsonFilesGenerator.GenFileLink(inputs, new List<object> { OfflineTSpSponsor }));
            return res;
        }

        public List<m_topic_DTO> GetAllDataImages(VisitInputs inputs)
        {
            inputs.TableName = "15.m_topic";
            //List<m_topic_DTO> OfflineMTopic = _spRepo.OfflineMTopic(inputs).Where(x=>x.topic_file_name != null && x.topic_filepath != null).ToList();
            List<m_topic_DTO> OfflineMTopic = _spRepo.OfflineMTopic(inputs).ToList();
            var resTopic = new List<m_topic_DTO>();
            foreach (var data in OfflineMTopic)
            {
                var arrTopic = new m_topic_DTO()
                {
                    topic_id = data.topic_id,
                    topic_title = data.topic_title,
                    topic_group_product = data.topic_group_product,
                    topic_file_name = (String.IsNullOrEmpty(data.topic_file_name) ? String.Empty : data.topic_file_name),
                    topic_status = data.topic_status,
                    //topic_filepath = (String.IsNullOrEmpty(data.topic_filepath) ? inputs.Host.Replace("bas_api_mobile", "bas") + "/Files/SP-Topics/no_available_image.jpg" : inputs.Host.Replace("bas_api_mobile", "bas") + data.topic_filepath.Replace(" ", "%20").Substring(1))
                    topic_filepath = inputs.Host.Replace("bas_api_mobile", "bas") + data.topic_filepath.Substring(1)
                };
                resTopic.Add(arrTopic);
            }
            return resTopic;
            //return OfflineMTopic;
        }

        #endregion

        #region SyncTrans

        public string SyncTrans(RealizationOfflineInputs inputs)
        {
            //_spRepo.SyncTrans(inputs);
            var dbRes = "";
            bas_trialEntities bas = new bas_trialEntities();
            foreach (var listObj in inputs.ListObject)
            {
                var listProd = BulkDataProduct(listObj.ListProduct).ToString();
                var listTopic = BulkDataTopic(listObj.ListTopic).ToString();
                var listSign = BulkDataSign(listObj.ListSign).ToString();
                try
                {
                    //dbRes =
                    //    bas.Database.SqlQuery<string>("SyncTrans @listProduct = '" + listProd + "',@listTopic = '" +
                    //                                  listTopic + "',@listSign = '" + listSign + "'").ToString();
                    dbRes = bas.SyncTrans(listProd, listTopic, listSign).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return dbRes;
        }

        public int CheckSignatureMessage(VisitInputs inputs)
        {
            var result = _spRepo.CheckSignatureMessage(inputs);
            return result;
        }

        private XDocument BulkDataProduct(List<t_visit_product_offline> list)
        {
            var xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(
                    "Products", from cls in list
                              select new XElement("ListProduct",
                                  new XElement("vd_id", cls.vd_id),
                                  new XElement("m_vd_id", cls.m_vd_id),
                                  new XElement("visit_id", cls.visit_id),
                                  new XElement("visit_code", cls.visit_code),
                                  new XElement("vd_value", cls.vd_value.HasValue ? cls.vd_value.Value : 0),
                                  new XElement("vd_date_saved", cls.vd_date_saved.HasValue ? cls.vd_date_saved.Value.ToString("yyyy-MM-dd HH:mm:ss"):""),
                                  new XElement("sp_sp", cls.sp_sp),
                                  new XElement("sp_percentage", cls.sp_percentage),
                                  new XElement("is_inserted", cls.is_inserted),
                                  new XElement("is_updated", cls.is_updated),
                                  new XElement("is_deleted", cls.is_deleted))
                    )
                );
            return xmlDocument;
        }

        private XDocument BulkDataTopic(List<t_visit_product_topic_offline> list)
        {
            var xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(
                    "Topics", from cls in list
                              select new XElement("ListTopic",
                                  new XElement("vpt_id", cls.vpt_id),
                                  new XElement("m_vpt_id", cls.m_vpt_id),
                                  new XElement("vd_id", cls.vd_id),
                                  new XElement("m_vd_id", cls.m_vd_id),
                                  new XElement("topic_id", cls.topic_id),
                                  new XElement("vpt_feedback", cls.vpt_feedback),
                                  new XElement("vpt_feedback_date", cls.vpt_feedback_date.HasValue ? cls.vpt_feedback_date.Value.ToString("yyyy-MM-dd HH:mm:ss"):""),
                                  new XElement("note_feedback", cls.note_feedback),
                                  new XElement("info_feedback_id", cls.info_feedback_id),
                                  new XElement("is_inserted", cls.is_inserted),
                                  new XElement("is_updated", cls.is_updated),
                                  new XElement("is_deleted", cls.is_deleted))
                    )
                );
            return xmlDocument;
        }

        private XDocument BulkDataSign(List<sp_offline_t_signature_mobile_DTO> list)
        {
            var xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(
                    "Sign", from cls in list
                              select new XElement("ListSign",
                                  new XElement("signature_id", cls.signature_id),
                                  new XElement("m_signature_id", cls.m_signature_id),
                                  new XElement("visit_id", cls.visit_id),
                                  new XElement("rep_id", cls.rep_id),
                                  new XElement("dr_code", cls.dr_code),
                                  new XElement("sign", cls.sign),
                                  new XElement("file_upload", cls.file_upload),
                                  new XElement("reason", cls.reason),
                                  new XElement("created_at", cls.created_at.HasValue ? cls.created_at.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""),
                                  new XElement("updated_at", cls.updated_at.HasValue ? cls.updated_at.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""))
                    )
                );
            return xmlDocument;
        }

        public int GetMaxAdditionalVisit()
        {
            var res = 0;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    C_parameter dbResult = context.C_parameter.Where(x => x.transaction_id == "AV").FirstOrDefault();

                    //var dbResult = _mParameter.Get().Where(x => x.transaction_id == "AV").Select(x => x.max_value).ToString(); //context.C_parameter.Where(x => x.transaction_id == "AV").Select(x => x.max_value).ToString();
                    if (dbResult != null)
                    {
                        res = Convert.ToInt32(dbResult.max_value);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return res;
        }

        //private DataTable GenerateBulkData(List<t_visit_product_DTO> inputs)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("vd_id");
        //    dt.Columns.Add("m_vd_id");
        //    dt.Columns.Add("visit_id");
        //    dt.Columns.Add("visit_code");
        //    dt.Columns.Add("vd_value");
        //    dt.Columns.Add("vd_date_saved");
        //    dt.Columns.Add("sp_sp");
        //    dt.Columns.Add("sp_percentage");

        //    foreach (var param in inputs)
        //    {
        //        dt.Rows.Add(param.vd_id, param.m_vd_id, param.visit_id, param.visit_code, param.vd_value, param.vd_date_saved, param.sp_sp, param.sp_percentage);
        //    }
        //    return dt;
        //}

        #endregion
    }
}
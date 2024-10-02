using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using SF_Domain.Inputs.Visit;
using SF_Repositories.Common;
using SF_Utils;

namespace SF_BusinessLogics.Visit
{
    public class VisitPlanBLL : IVisitPlanBLL
    {
        //private readonly IGenericRepository<SP_SELECT_VISIT_PLAN_Result> _spvisitplanrepo;
        private readonly IBasGenericRepositories<v_visit_plan_mobile> _vVisitPlanRepo;
        private readonly IBasGenericRepositories<t_sales> _tSalesRepo;
        private readonly IBasGenericRepositories<v_m_doctor> _vMDoctor;
        private ISqlSPRepository _sqlRepo;
        private readonly IBasGenericRepositories<v_visit_product> _vVisitProductRepo;

        public VisitPlanBLL(IBasGenericRepositories<v_visit_plan_mobile> vVisitPlanRepo, IBasGenericRepositories<t_sales> tSalesRepo, IBasGenericRepositories<v_m_doctor> vMDoctor, ISqlSPRepository sqlRepo, IBasGenericRepositories<v_visit_product> vVisitProductRepo)
        {
            _tSalesRepo = tSalesRepo;
            _vVisitPlanRepo = vVisitPlanRepo;
            _vMDoctor = vMDoctor;
            _sqlRepo = sqlRepo;
            _vVisitProductRepo = vVisitProductRepo;
        }

        public List<SP_SelectVisitPlanDTO> GetVisitPlan(string rep_id, int day, int month, int year)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.SP_SELECT_VISIT_PLAN(rep_id, day, month, year).ToList();
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanDTO>>(dbResult);
                return viewMapper;
            }
        }

        public List<v_visit_plan_mobileDTO> GetDoctorListPerDay(string id, DateTime? visitdateplan)
        {
            var queryFiler = PredicateHelper.True<v_visit_plan_mobile>();
            queryFiler = queryFiler.And(x => x.rep_id == id);
            queryFiler = queryFiler.And(x => x.visit_date_plan.Value <= visitdateplan.Value && x.visit_date_plan.Value >= visitdateplan.Value);
            var dbResult = _vVisitPlanRepo.Get(queryFiler).ToList();
            var viewMapper = Mapper.Map<List<v_visit_plan_mobileDTO>>(dbResult);
            return viewMapper;
        }


        public List<SP_SELECT_VISIT_USER_PRODUCT_DTO> GetUserProductPartial(string id, string visitid, int month, int year)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.SP_SELECT_VISIT_USER_PRODUCT(id, visitid, month, year).ToList();
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_DTO>>(dbResult);
                return viewMapper;
            }
        }


        public bool isAnyPlannedSalesInSelectedMonth(string id, int month, int year)
        {
            var queryFiler = PredicateHelper.True<t_sales>();
            if (!String.IsNullOrEmpty(id))
            {
                queryFiler = queryFiler.And(x => x.rep_id == id);
            }
            if (month != 0)
            {
                queryFiler = queryFiler.And(x => x.sales_date_plan == month);
            }
            if (year != 0)
            {
                queryFiler = queryFiler.And(x => x.sales_date_plan == month);
            }
            
            var dbResult = _tSalesRepo.Get(queryFiler).ToList();
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
                var drCode = 0;
                if (!String.IsNullOrEmpty(visitid))
                {
                    drCode = context.t_visit.Where(x => x.visit_id == visitid).Select(x => x.dr_code.Value).FirstOrDefault();
                }
                if (drCode != 0)
                {
                    var currentDate = DateTime.Now;
                    var month = currentDate.Month;
                    var year = currentDate.Year;
                    var dbResult = context.t_sales.Where(x => x.sales_date_plan.Value == month && x.sales_year_plan.Value == year && x.rep_id == id && x.dr_code == drCode).ToList();
                    if (dbResult.Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                return false;
            }
        }

        public bool CopySalesProductPlan(string id, string visitid, int frommonth, int fromyear)
        {
            var currentDate = DateTime.Now;
            var month = currentDate.Month;
            var year = currentDate.Year;
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

        public List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSP(string visitid)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.SP_SELECT_VISIT_SP_PLAN(visitid);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_DTO>>(dbResult);
                return viewMapper;
            }
        }

        public string InsertVisitPlan(string id, string visitDatePlan, List<VisitModel> drCollection, string rep_position, string rep_region)
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            var currentDatePlan = Convert.ToDateTime(visitDatePlan);
            var currentMonthPlan = currentDatePlan.Month;
            var currentYearPlan = currentDatePlan.Year;
            var visitDatePlanParam = Convert.ToDateTime(visitDatePlan);
            var result = "";
            var state = false;

            try
            {
                if (isMaxLimitedDoctorinCurrDayOnInit(visitDatePlanParam, id, rep_position))
                {
                    result = EnumHelper.GetDescription(Enums.ResponseType.ReachMaximumDoctor);
                    return result;
                }

                var doctors = drCollection.Count;
                if (doctors > 1)
                {
                    foreach (var drCode in drCollection)
                    {
                        if (Convert.ToInt32(drCode.Code) < 100005)
                        {
                            result = EnumHelper.GetDescription(Enums.ResponseType.PairedCode);
                            break;
                        }

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

                        if (isAlreadyPlannedDoctorInCurrDay(visitDatePlanParam, drCode.Code,id)) //TRANSFERED
                        {
                            result = EnumHelper.GetDescription(Enums.ResponseType.ReachMaximumDoctor);
                            break;
                        }

                        if (rep_region != "BIOD")
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

                        var newvisitid = GetVisitId();
                        using (var context = new bas_trialEntities())
                        {
                            var dbResult = context.SP_INSERT_VISIT_PLAN(newvisitid, id, visitDatePlanParam,
                                Convert.ToInt32(drCode.Code), rep_position);
                            result = EnumHelper.GetDescription(Enums.ResponseType.Success);
                        }

                    }
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = EnumHelper.GetDescription(Enums.ResponseType.SaveError);
            }

            return result;
        }

        public string GetVisitId()
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_GET_NEW_VISIT_NUMBER().FirstOrDefault();
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
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var code = Convert.ToInt32(drCode);
                    var dbResult = context.SP_SELECT_IS_ALREADY_PLANNED_DOCTOR_IN_CURR_WEEK(id, datePlan, code)
                        .ToList();
                    if (dbResult.Any())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
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
                var code = Convert.ToInt32(drCode);
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.v_visit_plan.Where(x =>
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
                    var dbResult = context.t_visit.Where(x =>
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
                return true;
            }
        }

        public bool isMaxLimitedDoctorinCurrDayOnInit(DateTime datePlan, string id, string position)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.t_visit
                        .Where(x => 
                            x.visit_date_plan.Value <= datePlan && x.visit_date_plan.Value >= datePlan 
                                                                && x.rep_id == id).ToList();
                    if (position == "MR")
                    {
                        if (dbResult.Count > 10)
                            return true;
                    }
                    else if (position == "PS")
                    {
                        if (dbResult.Count > 5)
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

        public bool isAnyDoctorUnverificatedRealInPrevMonth(int prevMonth, string id)
        {
            var queryFilter = PredicateHelper.True<v_m_doctor>();
            queryFilter = queryFilter.And(x => x.dr_used_month_session == prevMonth);
            queryFilter = queryFilter.And(x => x.dr_status == 1);
            queryFilter = queryFilter.And(x => x.dr_rep == id);
            var dbResult = _vMDoctor.Get(queryFilter).ToList();
            if (dbResult.Any())
            {
                return true;
            }
            return false;
        }

        public bool isAnyDoctorUnplaned(int prevMonth, string id)
        {
            var currentDate = DateTime.Now;
            var currYear = currentDate.Year;
            var currMonthRequest = prevMonth;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_SELECT_IS_ANY_DOCTOR_UNPLANED(id, currMonthRequest, currYear).FirstOrDefault();
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
            var currentDate = DateTime.Now;
            var currYear = currentDate.Year;
            var currMonthRequest = month;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_SELECT_IS_ANY_VISIT_UNPLANED_PRODUCT(id, currMonthRequest, currYear)
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
                    var dbResult = context.SP_SELECT_IS_ANY_DOCTOR_UNPLANED_USER(id, month, year).FirstOrDefault();
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
            var currentDate = DateTime.Now;
            var currMonth = currentDate.Month;
            var currYear = currentDate.Year;
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.t_visit.Where(x =>
                            x.rep_id == id && x.visit_date_plan.Value.Month == currMonth &&
                            x.visit_date_plan.Value.Year == currYear
                            && x.visit_plan_verification_status.Value == 0 && x.dr_code > 1005)
                        .GroupBy(x => x.visit_date_plan)
                        .Select(x => new {visitdateplan = x.Key, visitcount = x.Count()}).ToList();
                    var recordSet = dbResult.Count;
                    for (var i = 0; i < recordSet; i++)
                    {
                        var visitCount = dbResult[i].visitcount;
                        if (visitCount == 0)
                        {
                            return true;
                        }

                        if (rep_postion == "MR")
                        {
                            if (visitCount < 11)
                            {
                                return true;
                            }
                        }
                        else if (rep_postion == "PS")
                        {
                            if (visitCount < 6)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool isHaveRemainingToSendMail(string emailType, string id)
        {
            var currentDate = DateTime.Now;
            var currMonth = currentDate.Month;
            var currYear = currentDate.Year;
            var dateSent = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd"));
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_SELECT_TRANSACT_EMAIL(currMonth, currYear, emailType, id, dateSent).FirstOrDefault();
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
                var currentDate = DateTime.Now;
                var dbResult = context.t_visit
                    .Where(x => x.rep_id == id && x.visit_date_plan.Value.Month == currentDate.Month &&
                                x.visit_date_plan.Value.Year == currentDate.Year)
                    .GroupBy(x => x.visit_date_plan)
                    .Select(x => new { DayList = x.Key.Value.Day, Counter = x.Count() })
                    .ToList();

                var viewMapper = Mapper.Map<List<FullVisitDateDTO>>(dbResult);
                return viewMapper;
            }
        }


        public bool InsertSalesProduct(string id, string visitid, int month, int year, string prdcode, int qty, string note, int sp, int percentage)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_INSERT_SALES_PRODUCT_VISIT(id, visitid, month, year, prdcode, qty, note,
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
                    var dbResult = context.SP_INSERT_PRODUCT_VISIT(id, visitid, visitcode, sp, percentage);
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
            var queryFilter = PredicateHelper.True<v_visit_product>();
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                queryFilter = queryFilter.And(x => x.visit_id == inputs.VisitId);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_visit_product>();
            var dbResult = _vVisitProductRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_visit_product_DTO>>(dbResult);
        }
        public List<v_visit_product_DTO> GetVisitProduct(string visitid)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.v_visit_product.Where(x => x.visit_id == visitid).OrderBy(x=>x.vd_id).ToList();
                var viewMapper = Mapper.Map<List<v_visit_product_DTO>>(dbResult);
                return viewMapper;
            }
        }

        public bool InsertVisitSP(string id, string eventname, string bAllocation, int bAmount)
        {
            try
            {
                using (var context = new bas_trialEntities())
                {
                    var dbResult = context.SP_INSERT_SP_2_ON_VISIT_PLAN(id, eventname, bAllocation, bAmount);
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

        public void DeleteVisitPlan(string visitid, string rep_position)
        {
            using (var contex = new bas_trialEntities())
            {
                contex.SP_DELETE_VISIT_PLAN(visitid, rep_position);
            }
        }

        public void DeleteSalesProduct(string sp_id)
        {
            using (var contex = new bas_trialEntities())
            {
                var id = Convert.ToInt32(sp_id);
                contex.SP_DELETE_PRODUCT_SALES(id);
            }
        }

        public void DeleteVisitSP(string spdsId)
        {
            using (var contex = new bas_trialEntities())
            {
                var id = Convert.ToInt32(spdsId);
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
                    var dbResult = context.t_visit.Find(coll.VisitId);
                    var newInput = Mapper.Map<t_visit>(dbResult);
                    newInput.visit_date_plan = coll.VisitDatePlan;
                    context.t_visit.Add(newInput);
                    context.Entry(newInput).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    var newdrCode = Convert.ToInt32(coll.DrCode);
                    context.SP_UPDATE_VISIT_PLAN(coll.VisitId, coll.VisitDatePlan, newdrCode, coll.OldDrCode, position);
                }
            }
        }

        public void UpdateSalesProduct(string id, int qty, int sp,int percentage)
        {
            using (var context = new bas_trialEntities())
            {
                var sp_id = Convert.ToInt32(id);
                context.SP_UPDATE_SALES_PRODUCT(sp_id, qty, sp, percentage);
            }
        }

        public void UpdateVisitProduct(int vdid, string visitcode, int sp, int percentage)
        {
            using (var context = new bas_trialEntities())
            {
                var id = Convert.ToInt32(vdid);
                context.SP_UPDATE_VISIT_PRODUCT(id,visitcode,sp,percentage);
            }
        }

        public List<SP_SELECT_DOCTOR_LIST_NEW_DTO> GetDoctorList(string id, string position)
        {
            using (var context = new bas_trialEntities())
            {
                var dbResult = context.SP_SELECT_DOCTOR_LIST_NEW(id, position).ToList();
                var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_NEW_DTO>>(dbResult);
                return viewMapper;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using SF_Domain.Inputs.Visit;

namespace SF_BusinessLogics.Visit
{
    public interface IVisitPlanBLL
    {
        //IEnumerable<SP_SelectVisitPlanDTO> DataViewPartial(VisitInputs inputs);
        List<SP_SelectVisitPlanDTO> GetVisitPlan(string rep_id, int day, int month, int year);
        List<v_visit_plan_mobileDTO> GetDoctorListPerDay(string id, DateTime? visitdateplan);
        List<SP_SELECT_VISIT_USER_PRODUCT_DTO> GetUserProductPartial(string id, string visitid, int month, int year);
        bool isAnyPlannedSalesInSelectedMonth(string id, int month, int year);
        bool isAnyPlannedSalesInCurrMonth(string id, string visitid);
        bool CopySalesProductPlan(string id, string visitid, int frommonth, int fromyear);
        List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSP(string visitid);
        string InsertVisitPlan(string id, string visitDatePlan, List<VisitModel> drCollection, string rep_position, string rep_region);
        bool isAnyDoctorUnverificatedRealInPrevMonth(int prevMonth, string id);
        bool isAnyDoctorUnplaned(int prevMonth, string id);
        bool isAnyVisitUnplanedProduct(int month, string id);
        bool isAnyDoctorUnplanedSales(string id, int month, int year);
        bool isAnyDayLessThenMinimumDoctor(string rep_postion, string id);
        bool isHaveRemainingToSendMail(string emailType, string id);
        List<FullVisitDateDTO> getFullVisitDate(string id);
        bool InsertSalesProduct(string id, string visitid, int month, int year, string prdcode, int qty, string note, int sp, int percentage);
        bool InsertVisitProduct(string id, string visitid, string visitcode, int sp, int percentage);
        List<v_visit_product_DTO> GetVisitProduct(string visitid);
        List<v_visit_product_DTO> GetVisitProduct(VisitInputs inputs);
        bool InsertVisitSP(string id, string eventname, string bAllocation, int bAmount);
        void DeleteVisitPlan(string visitid, string rep_position);
        void DeleteSalesProduct(string sp_id);
        void DeleteVisitSP(string spdsId);
        void DeleteVisitProduct(int vdid);
        void UpdatePlan(UpdatePlanModel coll, string position);
        void UpdateSalesProduct(string id, int qty, int sp, int percentage);
        void UpdateVisitProduct(int vdid,string visitcode, int sp, int percentage);
        List<SP_SELECT_DOCTOR_LIST_NEW_DTO> GetDoctorList(string id, string position);
    }
}

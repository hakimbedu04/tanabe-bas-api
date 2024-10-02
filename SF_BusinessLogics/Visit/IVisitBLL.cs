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
    public interface IVisitBLL
    {

        List<SP_SelectVisitPlanDTO> GetVisitPlan(VisitInputs inputs);
        List<v_visit_plan_DTO> GetVisitPlanExport(SP_SELECT_VISIT_PLAN_INPUTS inputs);
        List<v_visit_plan_mobileDTO> GetDoctorListPerDay(string id, DateTime? visitdateplan, string SearchColumn, string SearchValue);
        List<SP_SELECT_VISIT_USER_PRODUCT_DTO> GetUserProductPartial(string id, string visitid, int month, int year);
        bool isAnyPlannedSalesInSelectedMonth(string id, int month, int year);
        bool isAnyPlannedSalesInCurrMonth(string id, string visitid);
        bool CopySalesProductPlan(string id, string visitid, int frommonth, int fromyear);
        
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
        void InsertVisitSP(string id, string eventname, string bAllocation, int bAmount);
        void DeleteVisitPlan(string visitid, string rep_position);
        void DeleteSalesProduct(int sp_id);
        void DeleteVisitSP(string spdsId);
        void DeleteVisitProduct(int vdid);
        void UpdatePlan(UpdatePlanModel coll, string position);
        void ShiftRealization(VisitInputs inputs);
        int ValidateShiftCounter(VisitInputs inputs);
        void InsertShiftLog(VisitInputs inputs);
        string UpdateSalesProduct(int spid, int qty, int sp, int percentage, string repid, int drcode,DateTime dateplan);
        int UpdateVisitProduct(VisitInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_NEW_DTO> GetDoctorList(string id, string position, string SearchColumn, string SearchValue);

        void SaveReportPlan(string id);
        bool SaveRealizationVisitWithAdditionalSP(VisitInputs inputs);
        bool SubmitAdditionalRealizationVisit(VisitInputs inputs);
        bool SaveRealizationVisit(VisitInputs inputs);

        void InsertSPAttachment(string visitid, string filename, string filepath);
        void InsertSPRealizationAttachment(string sprid, string filename, string filepath);
        List<SP_SELECT_SPR_INFO_DTO> GetSprInformation(string visitid);
        //==================================================================================================================================
        IList<v_visit_plan_new_DTO> GetVisitRealOptimize(VisitInputs inputs);
        List<v_visit_plan_new_DTO> GetVisitReal(VisitInputs inputs);
        List<v_visit_plan_new_DTO> GetVisitRealizationDatas(VisitInputs inputs);
        List<v_visit_plan_DTO> GetDoctorAdditionalList(VisitInputs inputs);
        void SaveAdditionalVisit(VisitInputs inputs);
        List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSpPlan(string visitid);
        List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSpRealization(string visitid);
        List<SP_SELECT_VISIT_SP_ASSOCIATED_DTO> GetSPVisitAssociated(VisitAssociatedRequestModel input);
        List<SP_VISIT_ASSOCIATE_NOTIFICATION_DTO> GetSPVisitAssociatedNotification(VisitInputs inputs);
        SP_SELECT_VISIT_SP_ASSOCIATED_DTO GetSPVisitAssociatedByVisitId(string VisitId);
        void SPConfirmVisitAssoicated(VisitAssociatedConfirmRequestModel inputs);
        void SPRejectVisitAssoicated(VisitAssociatedConfirmRequestModel inputs);
        v_visit_associated_DTO GetVisitAssociatedById(int associate_id);
        List<t_sp_attachment_DTO> GetVRealAttachment(string visitid, string host);
        IEnumerable<int> GetTopicFeedback(int vptId);
        List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid);
        int GetMaxAdditionalVisit();
        int GetParamMaxVisit();
        int CheckMaxVisit(VisitInputs inputs);
        bool CheckNewMaxVisit(VisitInputs inputs);
        bool isValidDay(VisitInputs inputs);
        List<t_visit_DTO> GetVisitData(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> GetProductTopicList(int vdid);
        List<SP_SELECT_DOCTOR_LIST_DTO> GetDataDoctorList(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductList();
        List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductListPlan();
        bool InsertVisitProduct(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_USER_DTO> dsProductLookup();
        void InsertProductTopic(VisitInputs inputs);
        bool UpdateFeedbackTopic(VisitInputs inputs);

        void DeleteVisitPlan(VisitInputs inputs);
        void DeleteVisitProduct(VisitInputs inputs);
        void DeleteProductTopic(VisitInputs inputs);
        List<v_visit_product_topic_DTO> GetProductTopic(VisitInputs inputs);
        bool isAlreadyPlannedVisitInCurrDay(VisitInputs inputs);
        bool isAlreadyPlannedDoctorInCurrDay(VisitInputs inputs);
        bool isMaxLimitedDoctorinCurrDay(VisitInputs inputs);
        string GetDeviation(VisitInputs inputs);
        string GetSign(VisitInputs inputs);
        string VerifySign(VisitInputs inputs);
        void AddDeviation(VisitInputs inputs, bool statgps);
        void AddSign(string repid, string visitid, int drcode, string filename, bool statgps);
        void AddShowTopicDuration(VisitInputs inputs);
        List<v_rep_admin_DTO> GetDataAdmin(string repid);
        List<m_event_DTO> GetEventNameList(string budget);
        bool SaveCancellationVisit(VisitInputs inputs);
        bool isAnyPlanOnChoosenDay(VisitInputs inputs);
        string GetFilePath(int spfid);
        void DeleteSPAttachment(int spfid);
        List<SummaryDoctor_DTO> GetQuadrantSummaryPlan(VisitInputs inputs);
        List<SummaryDoctor_DTO> GetQuadrantSummaryRealization(VisitInputs inputs);
        void SaveGpsLocation(VisitInputs inputs);
        List<v_visit_plan_DTO> GetDataDoctorPlaned(VisitInputs inputs);
        List<v_info_feedback_DTO> GetTopicInfo(int topicid, int topicfeedback, int vptid, string host);
        string GetTopicPath(int topicid);


        #region VisitActual

        List<v_visit_plan_new_DTO> GetVisitActual(VisitInputs inputs);
        List<v_visit_product_DTO> GetDataDetail(VisitInputs inputs);
        List<v_visit_product_DTO> GetDataDetailAll(List<string> visitid);
        List<m_product_custom_DTO> GetDataProductListActual();
        void DeleteDetailProduct(int vdid);
        void AddDetailProduct(VisitInputs inputs);
        string editValidation(string visitid);
        void InsertNotVisitedByDefault(string visitid);
        bool ExecUpdate(VisitInputs inputs);
        List<v_visit_plan_DTO> GetVisitActualExport(SP_SELECT_VISIT_PLAN_INPUTS inputs);
        List<SummaryDoctor_DTO> GetQuadrantSummaryActual(VisitInputs inputs);
        #endregion

        #region VisitHistory
        //
        List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistory(VisitInputs inputs);
        List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistorySearch(VisitInputs inputs);
        List<SummaryDoctor_DTO> GetQuadrantSummaryHistory(VisitInputs inputs);

        #endregion

        #region download data offline
        //string GenerateJsonFiles(VisitInputs inputs);
        List<string> GenerateLinkJsonFiles(VisitInputs inputs);
        List<string> GetAllDataMaster(VisitInputs inputs);
        List<string> GetAllDataTrans(VisitInputs inputs);
        List<m_topic_DTO> GetAllDataImages(VisitInputs inputs);
        #endregion

        #region SyncTrans

        string SyncTrans(RealizationOfflineInputs inputs);

        #endregion

        int CheckSignatureMessage(VisitInputs inputs);
    }
}

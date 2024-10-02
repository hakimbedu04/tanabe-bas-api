using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.DTOs.HRD;
using SF_Domain.Inputs.SP;
using SF_Domain.Inputs.User;
using SF_Domain.Inputs.Visit;

namespace SF_Repositories.Common
{
    public interface ISqlSPRepository
    {
        #region visit
        void SaveAdditionalVisit(VisitInputs inputs);
        List<SP_SELECT_VISIT_SP_PLAN_DTO> GetSpPlan(string visitid);
        List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSpRealization(string visitid);
        void DeleteVisitPlan(VisitInputs inputs);
        void DeleteVisitProduct(VisitInputs inputs);
        List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid);
        int CheckMaxVisit(VisitInputs inputs);
        bool CheckNewMaxVisit(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> GetProductTopicList(int vdid);
        List<SP_SELECT_DOCTOR_LIST_DTO> GetDataDoctorList(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductList();
        bool InsertVisitProduct(VisitInputs inputs);
        void UpdateVisitProduct(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_USER_DTO> dsProductLookup();
        void SaveReportPlan(string id);
        bool InsertProductTopic(int vdid, int topicid);
        bool SaveRealizationVisitWithAdditionalSP(VisitInputs inputs);
        void InsertSPAttachment(string visitid, string filename, string filepath);
        bool SaveRealizationVisit(VisitInputs inputs);
        List<SP_SELECT_SPR_INFO_DTO> GetSprInformation(string visitid);
        bool SubmitAdditionalRealizationVisit(VisitInputs inputs);
        bool UpdateVisitPlanByCancellation(VisitInputs inputs);
        bool UpdateVisitPlanByDate(VisitInputs inputs);
        void AddDetailProduct(VisitInputs inputs);
        void InsertNotVisitedByDefault(string visitid);
        bool ExecUpdate(VisitInputs inputs);

        List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistory(VisitInputs inputs);
        List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> GetVisitHistorySearch(VisitInputs inputs);

        #endregion


        #region SP PLAN
        List<SP_SELECT_SP_PLAN_DTO> GetSpPlan(SPInputs inputs);
        List<SP_PRODUCT_SP_PLAN_MOBILE_DTO> GetProductDropdown();
        void AddDetailProduct(string sprid, string productid);
        void DeleteProduct(int sppid);
        List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO> GetDoctor(SPInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorListFiltered(SPInputs inputs);
        void AddDetailParticipant(SPInputs inputs);
        void DeleteSpeaker(SPInputs inputs);
        void UpdateDetailSpeaker(SPInputs inputs);
        List<SP_PRODUCT_VISIT_MOBILE_DTO> GetDataProductSPList();
        void InsertSP1(SPInputs inputs);
        void InsertSP2(SPInputs inputs);
        string GetSprId();
        void DeleteSPPlan(SPInputs inputs);
        bool UpdateEventDetail(SPInputs inputs);
        bool UpdateBudgetAllocationOnPlan(SPInputs inputs);
        string GetOwnBudgetRemaining(SPInputs inputs);
        #endregion

        #region SP Realization
        List<SP_SELECT_SP_REALIZATION_DTO> GetSPRealization(SPInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorList(SPInputs inputs);
        bool AddSpeakerAdditional(SPInputs inputs);
        bool UpdateRealization(Collection inputs);
        bool SubmitRealization(SPInputs inputs);
        #endregion

        #region SP Associate
        List<SP_SELECT_VISIT_SP_ASSOCIATED_DTO> GetSPVisitAssociated(string rep_id);
        #endregion

        #region User
        void InsertSalesProductActual(UserInputs inputs);
        void UpdateSalesProductActual(UserInputs inputs);
        void DeleteSalesProductActual(UserInputs inputs);

        List<v_sales_product_DTO> GetUserActualSearch(UserInputs inputs);

        #endregion


        #region download data offline

        List<t_visit_DTO> OfflineTVisit(VisitInputs inputs);
        List<m_customer_aso_DTO> OfflineMCustomerAso(VisitInputs inputs);
        List<m_doctor_DTO> OfflineMDoctor(VisitInputs inputs);
        List<sp_offline_t_visit_product_DTO> OfflineTVisitProduct(VisitInputs inputs);
        List<sp_offline_t_visit_product_topic_DTO> OfflineTVisitProductTopic(VisitInputs inputs);
        List<m_bo_DTO> OfflineMBo(VisitInputs inputs);
        List<m_regional_DTO> OfflineMRegional(VisitInputs inputs);
        List<Karyawan_DTO> OfflineKaryawan(VisitInputs inputs);
        List<Bagian_DTO> OfflineBagian(VisitInputs inputs);
        List<Jabatan_DTO> OfflineJabatan(VisitInputs inputs);
        List<HeadQuarter_DTO> OfflineHeadQuarter(VisitInputs inputs);
        List<Departemen_DTO> OfflineDepartemen(VisitInputs inputs);
        List<m_rep_DTO> OfflineMRep(VisitInputs inputs);
        List<m_sbo_DTO> OfflineMSbo(VisitInputs inputs);
        List<m_topic_DTO> OfflineMTopic(VisitInputs inputs);
        List<m_gl_cs_DTO> OfflineMGlCs(VisitInputs inputs);
        List<m_product_DTO> OfflineMProduct(VisitInputs inputs);
        List<sp_offline_t_signature_mobile_DTO> OfflineTSignature(VisitInputs inputs);
        List<t_spr_DTO> OfflineTSpr(VisitInputs inputs);
        List<t_sp_DTO> OfflineTSp(VisitInputs inputs);
        List<t_sp_doctor_DTO> OfflineTSpDoctor(VisitInputs inputs);
        List<t_sp_sponsor_DTO> OfflineTSpSponsor(VisitInputs inputs);
        List<sp_offline_t_gps_mobile_DTO> OfflineTGps(VisitInputs inputs);

        List<m_event_DTO> OfflineMEvent(VisitInputs inputs);
        List<m_sponsor_DTO> OfflineMSponsor(VisitInputs inputs);
        List<m_status_DTO> OfflineMStatus(VisitInputs inputs);
        List<t_info_feedback_topic_mapping_mobile_DTO> OfflineInfoFeedbackMapping(VisitInputs inputs);
        List<m_info_feedback_mobile_DTO> OfflineInfoFeedback(VisitInputs inputs);

        #endregion
        int CheckSignatureMessage(VisitInputs inputs);
    }
}

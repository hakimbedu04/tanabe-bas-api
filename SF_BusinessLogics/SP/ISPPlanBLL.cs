using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.SP;
using SF_Domain.Inputs.Visit;

namespace SF_BusinessLogics.SP
{
    public interface ISPPlanBLL
    {
        List<SP_SELECT_SP_PLAN_DTO> GetSpPlan(SPInputs inputs);
        List<t_sp_approval_DTO> GetSPApproval(SPInputs inputs);
        List<SP_PRODUCT_SP_PLAN_MOBILE_DTO> GetProductDropdown();
        List<v_sp_product_DTO> GetSPProduct(SPInputs inputs);
        void AddDetailProduct(string sprid, string productid);
        void DeleteProduct(int sppid);
        List<v_doctor_sponsor_DTO> GetSPParticipant(SPInputs inputs);
        List<m_sponsor_DTO> GetSponsor(SPInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO> GetDoctor(SPInputs inputs);
        List<SP_SELECT_DOCTOR_LIST_SP_DTO> GetDoctorListFiltered(SPInputs inputs);
        void AddDetailParticipant(SPInputs inputs);
        void DeleteSpeaker(SPInputs inputs);
        void UpdateDetailSpeaker(SPInputs inputs);
        List<v_auth_sp_DTO> GetSPList(SPInputs inputs);
        List<SP_PRODUCT_VISIT_MOBILE_DTO> GetDataProductSPList();
        List<m_event_DTO> GetEventNameList(SPInputs inputs);
        List<m_topic_DTO> GetProductTopicList();
        void InsertSP1(SPInputs inputs);
        void InsertSP2(SPInputs inputs);
        string GetSprId();
        void DeleteSPPlan(SPInputs inputs);
        bool UpdateEventDetail(SPInputs inputs);
        List<v_spr_DTO> GetSPRInfo(string sprid);
        bool UpdateBudgetAllocationOnPlan(SPInputs inputs);
        string GetOwnBudgetRemaining(SPInputs inputs);
    }
}

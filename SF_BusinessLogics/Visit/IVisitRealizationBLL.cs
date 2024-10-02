using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.Visit;

namespace SF_BusinessLogics.Visit
{
    public interface IVisitRealizationBLL
    {
        List<v_visit_plan_DTO> GetVisitReal(VisitInputs inputs);
        List<v_visit_plan_DTO> GetDoctorAdditionalList(VisitInputs inputs);
        void SaveAdditionalVisit(VisitInputs inputs);
        List<SP_SELECT_VISIT_SP_REALIZATION_DTO> GetSP(VisitInputs inputs);
        IEnumerable<int> GetTopicFeedback(int vptId);
        List<SP_SELECT_SP_ATTACHMENT_DTO> GetSPAttachment(string visitid);
        int CheckMaxVisit(VisitInputs inputs);
        bool isValidDay(VisitInputs inputs);
        List<t_visit_DTO> GetVisitData(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> GetProductTopicList(int vdid);
        List<SP_SELECT_DOCTOR_LIST_DTO> GetDataDoctorList(VisitInputs inputs);
        List<SP_SELECT_PRODUCT_VISIT_DTO> GetDataProductList();
        bool InsertVisitProduct(VisitInputs inputs);

        #region delete

        void DeleteVisitPlan(VisitInputs inputs);
        void DeleteVisitProduct(VisitInputs inputs);
        void DeleteProductTopic(VisitInputs inputs);
        List<v_visit_product_topic_DTO> GetProductTopic(VisitInputs inputs);

        #endregion
    }
}

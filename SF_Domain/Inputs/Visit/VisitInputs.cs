using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.Visit
{
    public class VisitInputs : BaseInput
    {
        public string DrCode { get; set; }
        public string OldDrCode { get; set; }
        public List<VisitModel> Collections { get; set; }
        public UpdatePlanModel UpdatePlanCollections { get; set; }

        public string Address { get; set; }
        public DateTime VisitDatePlan { get; set; }
        public string Info { get; set; }
        public string SpBa { get; set; }
        public string PrdCode { get; set; }
        public int Qty { get; set; }
        public string Note { get; set; }
        public int Sp { get; set; }
        public string EventName { get; set; }
        public string BAllocation { get; set; }
        public int BAmount { get; set; }
        public int SpId { get; set; }
        public string SpdsId { get; set; }
        public int TopicId { get; set; }
        public int VptFeedBack { get; set; }
        public string Reason { get; set; }
        public string Budget { get; set; }
        public int SpfId { get; set; }
        public string IsRealized { get; set; }
        public string DrName { get; set; }
        public int? Duration { get; set; } // duration of topic shown to doctor

        //====================== part of realization action ================================
        public bool IsVisited { get; set; }
        public int SPRealizationStat { get; set; }
        public double? RealAmount { get; set; }
        public string EName { get; set; }
        public string EPlace { get; set; }
        public int VisitReal { get; set; }
        public string ListAttachment { get; set; }
        public int info_feedback_id { get; set; }
        public string ConditionText { get; set; }
        public string ConditionValue { get; set; }
        public DateTime CancellationDate { get; set; }
        public string VisitProduct { get; set; }
        public string VisitTopic { get; set; }
        public string Host { get; set; }
        public int Visited { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Email { get; set; }
        public DateTime PrevVisitDate { get; set; }
        public DateTime DestVisitDate { get; set; }
        public VisitModel VisitCollections { get; set; }
        public int VisitPlan { get; set; }
        public string VisitSp { get; set; }
        public string SearchText { get; set; }
        public int visit_type { get; set; }
    }



    public class VisitModel
    {
        public string Code { get; set; }
    }

    public class UpdatePlanModel
    {
        public string DrCode { get; set; }
        public string OldDrCode { get; set; }
        public string VisitId { get; set; }
        public DateTime VisitDatePlan { get; set; }
    }

    public class VisitAssociatedConfirmRequestModel : VisitInputs
    {
        public int AssociateId { get; set; }
    }

    public class VisitAssociatedRequestModel : VisitInputs
    {
        public DateTime? VisitDate { get; set; }
        public string AssociatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.Visit
{
    public class VisitPlanInputs : BaseInput
    {
        public string DrCode { get; set; }
        public string OldDrCode { get; set; }
        public List<VisitModel> Collections { get; set; }
        public UpdatePlanModel UpdatePlanCollections { get; set; }
    }

    //public class VisitModel
    //{
    //    public string Code { get; set; }
    //}

    //public class UpdatePlanModel
    //{
    //    public string DrCode { get; set; }
    //    public string OldDrCode { get; set; }
    //    public string VisitId { get; set; }
    //    public DateTime VisitDatePlan { get; set; }
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.Visit
{
    public class SP_SELECT_VISIT_PLAN_INPUTS : BaseInput
    {
        public string rep_id { get; set; }
        public Nullable<System.DateTime> VisitDatePlan { get; set; }
        public Nullable<int> VisitPlan { get; set; }
        public Nullable<int> DrCode { get; set; }
        public string DrName { get; set; }
        public string DrSpec { get; set; }
        public string DrSubSpec { get; set; }
        public string DrQuadrant { get; set; }
        public string DrMonitoring { get; set; }
        public string DrDkLk { get; set; }
        public string DrAreaMis { get; set; }
        public string DrCategory { get; set; }
        public string DrChannel { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.Visit
{
    public class VisitRealizationInputs : BaseInput
    {
        public string DrCode { get; set; }
        public DateTime VisitDatePlan { get; set; }
        public string Info { get; set; }
        public string SpBa { get; set; }
        public int Sp { get; set; }
        
    }
}

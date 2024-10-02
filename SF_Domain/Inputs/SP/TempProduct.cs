using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.SP
{
    public class TempProduct
    {
        public string VisitCode { get; set; }
        public string ProductBudget { get; set; }
        public decimal ProductBudgetAllocation { get; set; }
        public string ProductDesc { get; set; }
    }
}

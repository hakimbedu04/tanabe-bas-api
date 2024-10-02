using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs
{
    public class SortBaseInput
    {
        public string SortExpression { get; set; }
        public string SortOrder { get; set; }

        public string SortExpression2 { get; set; }
        public string SortOrder2 { get; set; }

        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
    }
}

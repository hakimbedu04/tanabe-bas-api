using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs
{
    public class ReportInputs : BaseInput
    {
        public string ReceiverAmName { get; set; }
        public string RepName { get; set; }
        public string RepRegion { get; set; }
        public string RepBo { get; set; }
        public string RepSBo { get; set; }
    }
}

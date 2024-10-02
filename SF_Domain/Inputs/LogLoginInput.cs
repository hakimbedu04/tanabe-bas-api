using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs
{
    public class LogLoginInput
    {
        public string rep_id { get; set; }
        public string hostname { get; set; }
        public string ip_addressv4 { get; set; }
        //public string ip_addressv6 { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
        public string source { get; set; }
        
        public DateTime? log_date { get; set; }
        public string status { get; set; }
        public string notes { get; set; }
    }
}

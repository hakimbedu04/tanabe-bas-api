using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class Temp_HCP_DTO
    {
        public string dr_name { get; set; }
        public string Spec { get; set; }
        public string Contact { get; set; }
        public string Monitoring { get; set; }
        public string Representative { get; set; }
        public string AM { get; set; }
        public string Sponsor { get; set; }
        public Nullable<double> Amount { get; set; }
    }
}

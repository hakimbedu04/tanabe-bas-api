using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_gps_mobile_DTO
    {
        public int gps_id { get; set; }
        public string visit_id { get; set; }
        public string rep_id { get; set; }
        public string dr_code { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}

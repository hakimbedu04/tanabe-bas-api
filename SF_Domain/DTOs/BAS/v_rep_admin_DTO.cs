using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class v_rep_admin_DTO
    {
        public decimal j_id { get; set; }
        public string rep_id { get; set; }
        public string rep_name { get; set; }
        public string sbo_code { get; set; }
        public string bo_code { get; set; }
        public Nullable<int> reg_id { get; set; }
        public string rep_admin { get; set; }
        public string admin_name { get; set; }
        public string reg_code { get; set; }
        public string admin_email { get; set; }
    }
}

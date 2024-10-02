using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs
{
    public class bas_v_rep_fullDTO
    {
        public string rep_id { get; set; }
        public string rep_name { get; set; }
        public Nullable<int> sbo_id { get; set; }
        public string rep_sbo { get; set; }
        public Nullable<int> bo_id { get; set; }
        public string rep_bo { get; set; }
        public string rep_position { get; set; }
        public string rep_division { get; set; }
        public string rep_email { get; set; }
        public Nullable<int> rep_status { get; set; }
        public Nullable<System.DateTime> rep_inactive_date { get; set; }
        public string rep_full_name { get; set; }
        public string sbo_description { get; set; }
        public string bo_description { get; set; }
        public string rep_am { get; set; }
        public string nama_am { get; set; }
        public string email_am { get; set; }
        public string rep_rm { get; set; }
        public string nama_rm { get; set; }
        public string email_rm { get; set; }
        public string rep_ppm { get; set; }
        public string nama_ppm { get; set; }
        public Nullable<int> sbo_status { get; set; }
        public Nullable<int> reg_id { get; set; }
        public string rep_region { get; set; }
        public string rep_bank_account { get; set; }
        public string rep_bank_code { get; set; }
    }
}

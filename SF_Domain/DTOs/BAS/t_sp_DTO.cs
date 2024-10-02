using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_sp_DTO
    {
        public int sp_id { get; set; }
        public string spr_id { get; set; }
        public string sp_type { get; set; }
        public Nullable<int> sp_status { get; set; }
        public Nullable<System.DateTime> sp_date_realization { get; set; }
        public Nullable<System.DateTime> sp_date_realization_saved { get; set; }
        public string sp_verified_by { get; set; }
        public Nullable<System.DateTime> sp_date_verified { get; set; }
        public string sp_ba { get; set; }
        public string sp_posted_by { get; set; }
        public Nullable<System.DateTime> sp_posted_date { get; set; }
    }
}

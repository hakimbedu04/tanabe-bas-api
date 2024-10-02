using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_shift_log_DTO
    {
        public int log_id { get; set; }
        public string visit_id { get; set; }
        public string rep_id { get; set; }
        public Nullable<System.DateTime> visit_date_plan { get; set; }
        public Nullable<System.DateTime> prev_visit_date { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> is_active { get; set; }
    }
}

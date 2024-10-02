using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_visit_product_topic_DTO
    {
        public int vpt_id { get; set; }
        public Nullable<long> vd_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public Nullable<int> vpt_feedback { get; set; }
        public Nullable<System.DateTime> vpt_feedback_date { get; set; }
        public string note_feedback { get; set; }
        public Nullable<int> info_feedback_id { get; set; }
    }
}

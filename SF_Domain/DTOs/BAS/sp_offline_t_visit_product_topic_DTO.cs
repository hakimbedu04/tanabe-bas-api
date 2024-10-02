using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class sp_offline_t_visit_product_topic_DTO
    {
        public int vpt_id { get; set; }
        public int m_vpt_id { get; set; }
        public int vd_id { get; set; }
        public int m_vd_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public Nullable<int> vpt_feedback { get; set; }
        public Nullable<System.DateTime> vpt_feedback_date { get; set; }
        public string note_feedback { get; set; }
        public Nullable<int> info_feedback_id { get; set; }
    }
}

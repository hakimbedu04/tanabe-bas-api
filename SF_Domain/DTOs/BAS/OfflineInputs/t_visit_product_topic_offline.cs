using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS.OfflineInputs
{
    public class t_visit_product_topic_offline
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

        public int is_inserted { get; set; }
        public int is_updated { get; set; }
        public int is_deleted { get; set; }
    }
}

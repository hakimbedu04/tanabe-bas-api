using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_info_feedback_topic_mapping_mobile_DTO
    {
        public int info_feedback_topic_mapping_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public Nullable<int> info_feedback_id { get; set; }
    }
}

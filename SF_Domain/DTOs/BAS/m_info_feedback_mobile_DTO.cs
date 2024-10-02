using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_info_feedback_mobile_DTO
    {
        public int info_feedback_id { get; set; }
        public Nullable<int> info_feedback_type { get; set; }
        public string info_description { get; set; }
    }
}

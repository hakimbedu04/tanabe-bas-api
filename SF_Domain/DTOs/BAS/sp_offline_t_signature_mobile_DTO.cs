using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class sp_offline_t_signature_mobile_DTO
    {
        public int signature_id { get; set; }
        public int m_signature_id { get; set; }
        public string visit_id { get; set; }
        public string rep_id { get; set; }
        public Nullable<int> dr_code { get; set; }
        public Nullable<bool> sign { get; set; }
        public string file_upload { get; set; }
        public string reason { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
    }
}

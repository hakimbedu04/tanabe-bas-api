using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class sp_offline_t_visit_product_DTO
    {
        public long vd_id { get; set; }
        public long m_vd_id { get; set; }
        public string visit_id { get; set; }
        public string visit_code { get; set; }
        public Nullable<int> vd_value { get; set; }
        public Nullable<System.DateTime> vd_date_saved { get; set; }
        public Nullable<byte> sp_sp { get; set; }
        public Nullable<int> sp_percentage { get; set; }
    }
}

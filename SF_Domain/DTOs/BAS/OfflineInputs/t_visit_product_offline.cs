using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS.OfflineInputs
{
    public class t_visit_product_offline
    {
        public long vd_id { get; set; }
        public Nullable<int> m_vd_id { get; set; }
        public string visit_id { get; set; }
        public string visit_code { get; set; }
        public Nullable<int> vd_value { get; set; }
        public Nullable<System.DateTime> vd_date_saved { get; set; }
        public Nullable<byte> sp_sp { get; set; }
        public Nullable<int> sp_percentage { get; set; }
        public int is_inserted { get; set; }
        public int is_updated { get; set; }
        public int is_deleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_sbo_DTO
    {
        public int sbo_id { get; set; }
        public string sbo_code { get; set; }
        public Nullable<int> bo_id { get; set; }
        public string sbo_description { get; set; }
        public string sbo_address { get; set; }
        public Nullable<int> sbo_sequence_code { get; set; }
        public string sbo_shareholders { get; set; }
        public string sbo_rep { get; set; }
        public string sbo_ppm { get; set; }
        public Nullable<int> sbo_status { get; set; }
        public Nullable<System.DateTime> effective_datestart { get; set; }
        public Nullable<System.DateTime> effective_dateend { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
        public string updated_by { get; set; }
    }
}

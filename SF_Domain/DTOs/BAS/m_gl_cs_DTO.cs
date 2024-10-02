using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_gl_cs_DTO
    {
        public string o_id { get; set; }
        public string o_description { get; set; }
        public Nullable<int> o_sbo { get; set; }
        public Nullable<int> o_status { get; set; }
        public Nullable<byte> o_budget_control { get; set; }
        public Nullable<byte> o_approval { get; set; }
        public Nullable<byte> o_budget_control_sp { get; set; }
    }
}

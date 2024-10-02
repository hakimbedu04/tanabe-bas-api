using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class v_visit_product_DTO
    {
        public long vd_id { get; set; }
        public string visit_id { get; set; }
        public string visit_code { get; set; }
        public string visit_team { get; set; }
        public string visit_product { get; set; }
        public string visit_category { get; set; }
        public int vd_value { get; set; }
        public byte sp_sp { get; set; }
        public int sp_percentage { get; set; }
        public string visit_budget_ownership { get; set; }
        public string o_description { get; set; }
    }
}

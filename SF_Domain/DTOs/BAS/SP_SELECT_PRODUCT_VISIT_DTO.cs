using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class SP_SELECT_PRODUCT_VISIT_DTO
    {
        public string visit_code { get; set; }
        public string visit_team { get; set; }
        public string visit_product { get; set; }
        public string visit_category { get; set; }
    }
}

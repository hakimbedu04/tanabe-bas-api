using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_PRODUCT_VISIT_ViewModel
    {
        public string visit_code { get; set; }
        public string visit_team { get; set; }
        public string visit_product { get; set; }
        public string visit_category { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_PRODUCT_USER_ViewModel
    {
        public string prd_code { get; set; }
        public string prd_name { get; set; }
        public string prd_focus { get; set; }
        public string prd_type { get; set; }
        public Nullable<double> prd_price_bpjs { get; set; }
        public Nullable<double> prd_price { get; set; }
    }
}
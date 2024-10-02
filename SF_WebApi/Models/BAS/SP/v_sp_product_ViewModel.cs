using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS.SP
{
    public class v_sp_product_ViewModel
    {
        public int spp_id { get; set; }
        public string spr_id { get; set; }
        public string sp_product { get; set; }
        public string visit_team { get; set; }
        public string visit_product { get; set; }
        public string visit_category { get; set; }
        public string visit_group { get; set; }
        public string visit_budget_ownership { get; set; }
        public string o_description { get; set; }
        public string o_department { get; set; }
        public Nullable<int> o_status { get; set; }
        public string o_functionary { get; set; }
        public Nullable<int> sp_product_ownership { get; set; }
    }
}
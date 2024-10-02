using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class m_product_ViewModel
    {
        public string prd_aso_id { get; set; }
        public string prd_code { get; set; }
        public string prd_focus { get; set; }
        public string prd_type { get; set; }
        public string visit_code { get; set; }
        public string visit_product { get; set; }
        public string visit_category { get; set; }
        public string visit_group { get; set; }
        public string visit_team { get; set; }
        public string visit_budget_ownership { get; set; }
        public string prd_aso_desc { get; set; }
        public string prd_aso_type { get; set; }
        public string prd_aso_category { get; set; }
        public Nullable<int> prd_aso_program { get; set; }
        public string prd_aso_join_desc { get; set; }
        public Nullable<int> price_id { get; set; }
        public string prd_aso_gp { get; set; }
        public string prd_aso_ose { get; set; }
        public string prd_aso_group { get; set; }
        public string prd_aso_group_fin { get; set; }
        public Nullable<int> prd_aso_tab { get; set; }
        public Nullable<int> prd_aso_dossage { get; set; }
        public string prd_aso_dostime { get; set; }
        public string prd_aso_tc { get; set; }
        public Nullable<int> prd_status { get; set; }
        public string prd_saf_code { get; set; }
    }
}
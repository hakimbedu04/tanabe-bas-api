using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.User
{
    public class UserExportCustomModelInputs : BaseInput
    {
        public Nullable<long> sp_id { get; set; }
        public string sales_id { get; set; }
        public string prd_code { get; set; }
        public string prd_name { get; set; }
        public Nullable<double> prd_price { get; set; }
        public string visit_category { get; set; }
        public Nullable<int> sp_target_qty { get; set; }
        public Nullable<double> sp_target_value { get; set; }
        public Nullable<double> sp_sales_qty { get; set; }
        public Nullable<double> sp_sales_value { get; set; }
        public string sp_note { get; set; }
        public string rep_id { get; set; }
        public Nullable<int> sales_date_plan { get; set; }
        public Nullable<int> sales_year_plan { get; set; }
        public Nullable<int> sales_plan { get; set; }
        public Nullable<int> sales_realization { get; set; }
        public string sales_info { get; set; }
        public Nullable<int> dr_code { get; set; }
        public Nullable<int> sales_plan_verification_status { get; set; }
        public string sales_plan_verification_by { get; set; }
        public Nullable<System.DateTime> sales_plan_verification_date { get; set; }
        public Nullable<int> sales_real_verification_status { get; set; }
        public string sales_real_verification_by { get; set; }
        public Nullable<System.DateTime> sales_real_verification_date { get; set; }
        public Nullable<System.DateTime> sales_date_plan_saved { get; set; }
        public Nullable<System.DateTime> sales_date_plan_updated { get; set; }
        public Nullable<System.DateTime> sales_date_realization_saved { get; set; }
        public string dr_name { get; set; }
        public string dr_quadrant { get; set; }
        public string dr_monitoring { get; set; }
        public string dr_spec { get; set; }
        public string dr_sub_spec { get; set; }
        public string dr_area_mis { get; set; }
        public string dr_chanel { get; set; }
        public string dr_category { get; set; }
        public string dr_sub_category { get; set; }
        public string dr_dk_lk { get; set; }
        public Nullable<int> sp_plan { get; set; }
        public Nullable<int> sp_real { get; set; }
        public string rep_name { get; set; }
        public string nama_am { get; set; }
        public string rep_region { get; set; }
        public string rep_bo { get; set; }
        public string rep_sbo { get; set; }
        public string rep_position { get; set; }
        public string rep_division { get; set; }
        public string nama_rm { get; set; }
        public string target_class_user { get; set; }
        public string real_class_user { get; set; }
        public Nullable<decimal> sp_percentage { get; set; }
        public Nullable<int> sp_sp { get; set; }
        public string visit_code { get; set; }
        public string visit_budget_ownership { get; set; }
        public string o_description { get; set; }
    }
}

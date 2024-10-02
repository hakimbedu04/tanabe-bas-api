using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_sales_DTO
    {
        public string sales_id { get; set; }
        public string rep_id { get; set; }
        public Nullable<int> sales_date_plan { get; set; }
        public Nullable<int> sales_year_plan { get; set; }
        public Nullable<int> sales_plan { get; set; }
        public string sales_code { get; set; }
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
        public Nullable<int> sales_product_count { get; set; }
    }
}

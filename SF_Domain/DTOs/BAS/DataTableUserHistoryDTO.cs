using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableUserHistoryDTO
    {
        public float sp_id { get; set; }
        public string sales_id { get; set; }
        public Nullable<int> dr_code { get; set; }
        public Nullable<int> sales_plan_verification_status { get; set; }
        public Nullable<int> sales_realization { get; set; }
        public Nullable<int> sales_real_verification_status { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_quadrant { get; set; }
        public string dr_monitoring { get; set; }
        public string prd_name { get; set; }
        public Nullable<double> prd_price { get; set; }
        public string prd_category { get; set; }
        public Nullable<int> sp_target_qty { get; set; }
        public Nullable<double> sp_target_value { get; set; }
        public Nullable<double> sp_sales_qty { get; set; }
        public Nullable<double> sp_sales_value { get; set; }
    }
}

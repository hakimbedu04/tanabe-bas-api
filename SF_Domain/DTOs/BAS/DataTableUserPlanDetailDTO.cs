using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableUserPlanDetailDTO
    {
        public Nullable<long> sp_id { get; set; }
        public string sales_id { get; set; }
        public string prd_name { get; set; }
        public Nullable<double> prd_price { get; set; }
        public Nullable<int> sp_target_qty { get; set; }
        public Nullable<double> sp_target_value { get; set; }
        public Nullable<double> sp_sales_qty { get; set; }
        public Nullable<double> sp_sales_value { get; set; }
        public string sp_note { get; set; }
    }
}

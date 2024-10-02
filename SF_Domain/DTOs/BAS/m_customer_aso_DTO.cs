using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_customer_aso_DTO
    {
        public long cust_id { get; set; }
        public string cust_name { get; set; }
        public string cust_city { get; set; }
        public string cust_geo_name { get; set; }
        public string cust_sales_ctg { get; set; }
        public string cust_bo { get; set; }
        public string cust_outlet_group { get; set; }
        public string cust_outlet_channel { get; set; }
        public Nullable<int> sbo_code { get; set; }
        public Nullable<int> sbo_code_bio { get; set; }
        public Nullable<int> sbo_code_ans { get; set; }
        public Nullable<int> sbo_code_liv { get; set; }
        public string cust_outlet_ctgy { get; set; }
        public string cust_kae { get; set; }
        public string cust_kam { get; set; }
        public string cust_kam_ctgy { get; set; }
        public Nullable<double> cust_outlet_total { get; set; }
        public string cust_outlet_type { get; set; }
        public string cust_focus { get; set; }
        public string cust_area { get; set; }
        public Nullable<System.DateTime> last_update { get; set; }
    }
}

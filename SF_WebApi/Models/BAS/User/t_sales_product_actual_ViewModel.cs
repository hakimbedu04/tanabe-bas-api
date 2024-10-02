using System;

namespace SF_WebApi.Models.BAS.User
{
    public class t_sales_product_actual_ViewModel
    {
        public long spa_id { get; set; }
        public Nullable<long> sp_id { get; set; }
        public Nullable<System.DateTime> spa_date { get; set; }
        public Nullable<long> spa_quantity { get; set; }
        public Nullable<System.DateTime> spa_date_saved { get; set; }
        public string spa_note { get; set; }
    }
}
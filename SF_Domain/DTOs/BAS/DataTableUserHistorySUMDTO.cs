using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableUserHistorySUMDTO
    {
        public Nullable<int> sp_target_qty { get; set; }
        public Nullable<double> sp_target_value { get; set; }
        public Nullable<double> sp_sales_qty { get; set; }
        public Nullable<double> sp_sales_value { get; set; }
    }
}

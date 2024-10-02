using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class TrendSalesDTO
    {
        public string tsv_region { get; set; }
        public double tsv_sales_value { get; set; }
        public string tsv_date { get; set; }
    }
}

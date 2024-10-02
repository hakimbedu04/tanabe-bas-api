using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class Temp_Sales_History_DTO
    {
        public string category { get; set; }
        public string productName { get; set; }
        public Nullable<double> qty { get; set; }
        public Nullable<double> val { get; set; }
        public Nullable<double> avg { get; set; }
    }
}

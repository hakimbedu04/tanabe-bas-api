using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_sp_sponsor_DTO
    {
        public decimal spds_id { get; set; }
        public Nullable<int> spdr_id { get; set; }
        public Nullable<int> sponsor_id { get; set; }
        public string budget_remarks { get; set; }
        public string budget_currency { get; set; }
        public Nullable<double> budget_plan_value { get; set; }
        public Nullable<double> budget_real_value { get; set; }
    }
}

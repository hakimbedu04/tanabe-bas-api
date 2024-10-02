using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DoctorActivityDTO
    {
        public string sales_id { get; set; }
        public Nullable<int> dr_code { get; set; }
        public string dr_name { get; set; }
        public string dr_quadrant { get; set; }
        public string dr_monitoring { get; set; }
        public string dr_spec { get; set; }
        public string dr_sub_spec { get; set; }
        public Nullable<int> year { get; set; }
        public Nullable<int> month { get; set; }
        public string prd_code { get; set; }
        public string prd_name { get; set; }
        public Nullable<int> sales_plan { get; set; }
        public Nullable<int> sales_realization { get; set; }
        public Nullable<int> ach_user { get; set; }
        public Nullable<int> visit_plan { get; set; }
        public Nullable<int> visit_realization { get; set; }
        public Nullable<decimal> ach_visit { get; set; }
        public string sp_type { get; set; }
        public Nullable<double> sp_plan { get; set; }
        public double sp_real { get; set; }
        public Nullable<double> ach_sp { get; set; }
    }
}

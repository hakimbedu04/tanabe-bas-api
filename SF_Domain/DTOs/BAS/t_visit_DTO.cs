using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_visit_DTO
    {
        public string visit_id { get; set; }
        public string rep_id { get; set; }
        public Nullable<System.DateTime> visit_date_plan { get; set; }
        public Nullable<int> visit_plan { get; set; }
        public string visit_code { get; set; }
        public Nullable<int> visit_realization { get; set; }
        public string visit_info { get; set; }
        public string visit_sp { get; set; }
        public Nullable<double> visit_sp_value { get; set; }
        public Nullable<int> dr_code { get; set; }
        public Nullable<int> visit_plan_verification_status { get; set; }
        public string visit_plan_verification_by { get; set; }
        public Nullable<System.DateTime> visit_plan_verification_date { get; set; }
        public Nullable<int> visit_real_verification_status { get; set; }
        public string visit_real_verification_by { get; set; }
        public Nullable<System.DateTime> visit_real_verification_date { get; set; }
        public Nullable<System.DateTime> visit_date_plan_saved { get; set; }
        public Nullable<System.DateTime> visit_date_plan_updated { get; set; }
        public Nullable<System.DateTime> visit_date_realization_saved { get; set; }
        public Nullable<int> visit_coverage_plan { get; set; }
        public Nullable<int> visit_coverage_real { get; set; }
        public string visit_sign_id { get; set; }
        public string visit_ordinat { get; set; }
        public Nullable<int> visit_product_count { get; set; }
        public string visit_comment { get; set; }
    }
}

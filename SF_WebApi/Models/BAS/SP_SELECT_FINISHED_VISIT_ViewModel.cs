using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_FINISHED_VISIT_ViewModel
    {
        public string visit_id { get; set; }
        public string rep_id { get; set; }
        public Nullable<System.DateTime> visit_date_plan { get; set; }
        public Nullable<int> visit_plan { get; set; }
        public Nullable<int> visit_realization { get; set; }
        public Nullable<int> dr_code { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_sub_spec { get; set; }
        public string dr_quadrant { get; set; }
        public string dr_monitoring { get; set; }
        public string dr_dk_lk { get; set; }
        public string dr_area_mis { get; set; }
        public string dr_category { get; set; }
        public string dr_chanel { get; set; }
        public Nullable<System.DateTime> visit_date_realization_saved { get; set; }
        public Nullable<System.DateTime> visit_date_plan_saved { get; set; }
        public Nullable<System.DateTime> visit_date_plan_updated { get; set; }
        public string visit_info { get; set; }
        public string visit_sp { get; set; }
        public Nullable<double> visit_sp_value { get; set; }
        public int visit_plan_verification_status { get; set; }
        public string visit_plan_verification_by { get; set; }
        public Nullable<System.DateTime> visit_plan_verification_date { get; set; }
        public int visit_real_verification_status { get; set; }
        public string visit_real_verification_by { get; set; }
        public Nullable<System.DateTime> visit_real_verification_date { get; set; }
        public string dr_address { get; set; }
        public string visit_code { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class SP_SELECT_SP_PLAN_DTO
    {
        public string spr_id { get; set; }
        public string spr_no { get; set; }
        public string spr_initiator { get; set; }
        public string spr_status { get; set; }
        public string spr_status_desc { get; set; }
        public Nullable<System.DateTime> spr_date_created { get; set; }
        public string sp_type { get; set; }
        public string e_name { get; set; }
        public string e_topic { get; set; }
        public string e_place { get; set; }
        public Nullable<System.DateTime> e_dt_start { get; set; }
        public Nullable<System.DateTime> e_dt_end { get; set; }
        public Nullable<int> dr_plan_sum { get; set; }
        public Nullable<int> dr_real_sum { get; set; }
        public Nullable<double> budget_plan_sum { get; set; }
        public Nullable<double> budget_real_sum { get; set; }
        public string sp_status_desc { get; set; }
        public Nullable<int> e_a_gp { get; set; }
        public Nullable<int> e_a_gp_pax { get; set; }
        public Nullable<int> e_a_specialist { get; set; }
        public Nullable<int> e_a_specialist_pax { get; set; }
        public Nullable<int> e_a_nurse { get; set; }
        public Nullable<int> e_a_nurse_pax { get; set; }
        public Nullable<int> e_a_others { get; set; }
        public Nullable<int> e_a_others_pax { get; set; }
        public Nullable<int> sp_id { get; set; }
        public string spr_note { get; set; }
        public Nullable<int> sp_status { get; set; }
        public Nullable<System.DateTime> sp_date_realization { get; set; }
        public string rep_sbo { get; set; }
        public string rep_bo { get; set; }
        public string rep_region { get; set; }
        public string rep_position { get; set; }
        public string initiator_name { get; set; }
        public string initiator_email { get; set; }
        public string rep_am { get; set; }
        public string nama_am { get; set; }
        public string rep_rm { get; set; }
        public string nama_rm { get; set; }
        public string rep_ppm { get; set; }
        public string nama_ppm { get; set; }
        public Nullable<System.DateTime> sp_date_realization_saved { get; set; }
        public Nullable<System.DateTime> sp_date_verified { get; set; }
        public string sp_verified_by { get; set; }
        public string sp_ba { get; set; }
        public string sp_posted_by { get; set; }
        public Nullable<System.DateTime> sp_posted_date { get; set; }
        public string spr_allocation_key { get; set; }
    }
}

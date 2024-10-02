using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class v_doctor_sponsor_DTO
    {
        public Nullable<int> spdr_id { get; set; }
        public string spr_id { get; set; }
        public Nullable<int> sp_id { get; set; }
        public Nullable<int> dr_code { get; set; }
        public string dr_as { get; set; }
        public Nullable<int> dr_plan { get; set; }
        public Nullable<int> dr_actual { get; set; }
        public Nullable<int> sponsor_id { get; set; }
        public string sponsor_description { get; set; }
        public string budget_remarks { get; set; }
        public string budget_currency { get; set; }
        public Nullable<double> budget_plan_value { get; set; }
        public Nullable<double> budget_real_value { get; set; }
        public string sp_type { get; set; }
        public decimal spds_id { get; set; }
        public string rep_name { get; set; }
        public string am_name { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_email { get; set; }
        public string dr_monitoring { get; set; }
        public Nullable<System.DateTime> e_dt_start { get; set; }
        public string sp_ba { get; set; }
        public string spr_status { get; set; }
        public Nullable<int> sp_status { get; set; }
        public string spr_initiator { get; set; }
        public string initiator_name { get; set; }
        public string spr_status_desc { get; set; }
        public Nullable<System.DateTime> dr_date_realization { get; set; }
        public Nullable<System.DateTime> sp_date_realization { get; set; }
        public Nullable<System.DateTime> sp_date_realization_saved { get; set; }
        public string spr_no { get; set; }
        public string e_name { get; set; }
        public string e_place { get; set; }
    }
}

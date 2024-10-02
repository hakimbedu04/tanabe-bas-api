using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class SP_SELECT_MASTER_DOCTOR_PIVOT_DTO
    {
        public int dr_code { get; set; }
        public string sbo_id { get; set; }
        public string dr_sbo { get; set; }
        public string dr_bo { get; set; }
        public string dr_region { get; set; }
        public string dr_rep { get; set; }
        public string rep_name { get; set; }
        public string dr_am { get; set; }
        public string am_name { get; set; }
        public string rep_position { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_sub_spec { get; set; }
        public string dr_quadrant { get; set; }
        public string dr_monitoring { get; set; }
        public string dr_address { get; set; }
        public string dr_area_mis { get; set; }
        public Nullable<int> dr_sum { get; set; }
        public string dr_category { get; set; }
        public string dr_sub_category { get; set; }
        public string dr_chanel { get; set; }
        public string dr_day_visit { get; set; }
        public string dr_visiting_hour { get; set; }
        public Nullable<int> dr_number_patient { get; set; }
        public string dr_kol_not { get; set; }
        public string dr_gender { get; set; }
        public string dr_phone { get; set; }
        public string dr_email { get; set; }
        public Nullable<System.DateTime> dr_birthday { get; set; }
        public string dr_dk_lk { get; set; }
        public Nullable<int> dr_used_session { get; set; }
        public string is_used { get; set; }
        public Nullable<int> dr_used_remaining { get; set; }
        public Nullable<int> dr_used_month_session { get; set; }
        public Nullable<int> dr_status { get; set; }
        public string is_used_on_sales { get; set; }
        public Nullable<int> dr_sales_session { get; set; }
        public Nullable<int> dr_sales_month_session { get; set; }
        public string dr_ppm { get; set; }
        public string ppm_name { get; set; }
        public Nullable<int> cust_id { get; set; }
        public string dr_rm { get; set; }
        public string dr_rm_name { get; set; }
    }
}

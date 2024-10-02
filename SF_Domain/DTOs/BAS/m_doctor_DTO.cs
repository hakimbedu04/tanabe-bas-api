using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_doctor_DTO
    {
        public int dr_code { get; set; }
        public string dr_sbo { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_sub_spec { get; set; }
        public string dr_quadrant { get; set; }
        public Nullable<int> cust_id { get; set; }
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
        public Nullable<int> dr_used_remaining { get; set; }
        public Nullable<int> dr_used_month_session { get; set; }
        public Nullable<int> dr_status { get; set; }
        public Nullable<int> dr_sales_session { get; set; }
        public Nullable<int> dr_sales_month_session { get; set; }
    }
}

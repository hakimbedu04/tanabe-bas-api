using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_spr_DTO
    {
        public string spr_id { get; set; }
        public string spr_no { get; set; }
        public string spr_initiator { get; set; }
        public string spr_status { get; set; }
        public Nullable<System.DateTime> spr_date_created { get; set; }
        public Nullable<System.DateTime> spr_date_updated { get; set; }
        public string spr_note { get; set; }
        public string e_name { get; set; }
        public string e_topic { get; set; }
        public string e_place { get; set; }
        public Nullable<System.DateTime> e_dt_start { get; set; }
        public Nullable<System.DateTime> e_dt_end { get; set; }
        public Nullable<int> e_a_gp { get; set; }
        public Nullable<int> e_a_gp_pax { get; set; }
        public Nullable<int> e_a_specialist { get; set; }
        public Nullable<int> e_a_specialist_pax { get; set; }
        public Nullable<int> e_a_nurse { get; set; }
        public Nullable<int> e_a_nurse_pax { get; set; }
        public Nullable<int> e_a_others { get; set; }
        public Nullable<int> e_a_others_pax { get; set; }
        public string spr_allocation_key { get; set; }
        public string input_origin { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableSPActualDTO
    {
        public string spr_id { get; set; } 
        public string spr_no { get; set; } 
        public string sp_type { get; set; } 
        public string e_name { get; set; } 
        public string e_place { get; set; }
        public string e_topic { get; set; }
        public Nullable<int>  e_a_gp { get; set; } 
        public Nullable<int> e_a_gp_pax { get; set; } 
        public Nullable<int> e_a_specialist { get; set; } 
        public Nullable<int> e_a_specialist_pax { get; set; } 
        public Nullable<int> e_a_nurse { get; set; } 
        public Nullable<int> e_a_nurse_pax { get; set; } 
        public Nullable<int> e_a_others { get; set; } 
        public Nullable<int> e_a_others_pax { get; set; } 
        public Nullable<DateTime>  e_dt_start { get; set; } 
        public Nullable<DateTime> e_dt_end { get; set; } 
        public Nullable<DateTime> sp_date_realization { get; set; } 
        public Nullable<int> dr_plan_sum { get; set; }
        public Nullable<int> dr_real_sum { get; set; }
        public Nullable<double> budget_plan_sum { get; set; } 
        public Nullable<double> budget_real_sum { get; set; } 
        public string  spr_status { get; set; } 
        public string  spr_status_desc { get; set; }
    }
}
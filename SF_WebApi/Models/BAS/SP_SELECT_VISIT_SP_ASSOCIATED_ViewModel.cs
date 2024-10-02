using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_VISIT_SP_ASSOCIATED_ViewModel
    {
        public int associate_id { get; set; }
        public string va_notif { get; set; }
        public string visit_id_initiator { get; set; }
        public Nullable<int> dr_code_initiator { get; set; }
        public string dr_name { get; set; }
        public Nullable<System.DateTime> visit_date_plan_initiator { get; set; }
        public Nullable<System.DateTime> visit_date_plan_invited { get; set; }
        public string rep_id_initiator { get; set; }
        public Nullable<System.DateTime> visit_date_realization_saved { get; set; }
        public string visit_id_invited { get; set; }
        public string rep_id_invited { get; set; }
        public Nullable<int> associate_status { get; set; }
        public string initiator_name { get; set; }
        public string initiator_email { get; set; }
        public string invited_name { get; set; }
        public string invited_email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class m_rep_viewmodel
    {
        public string rep_id { get; set; }
        public string rep_name { get; set; }
        public string rep_position { get; set; }
        public string rep_division { get; set; }
        public string rep_email { get; set; }
        public Nullable<int> rep_minvday { get; set; }
        public Nullable<int> rep_maxvday { get; set; }
        public Nullable<int> rep_maxaddvday { get; set; }
        public string rep_bank_account { get; set; }
        public string rep_bank_code { get; set; }
        public Nullable<int> rep_status { get; set; }
        public Nullable<System.DateTime> effective_datestart { get; set; }
        public Nullable<System.DateTime> rep_inactive_date { get; set; }
        public Nullable<int> role_id { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
        public string updated_by { get; set; }
        public string profile_picture_path { get; set; }
    }
}
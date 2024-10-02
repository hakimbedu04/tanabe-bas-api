using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class m_event_ViewModel
    {
        public int event_id { get; set; }
        public string event_description { get; set; }
        public string event_detail_description { get; set; }
        public string event_scale { get; set; }
        public string event_place { get; set; }
        public string event_sp { get; set; }
        public string event_budget { get; set; }
        public Nullable<System.DateTime> event_date_saved { get; set; }
        public string event_saved_by { get; set; }
        public Nullable<System.DateTime> event_last_updated { get; set; }
        public string event_last_updated_by { get; set; }
        public Nullable<int> event_status { get; set; }
    }
}
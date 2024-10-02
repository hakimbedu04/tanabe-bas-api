using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class v_visit_product_topic_ViewModel
    {
        public int vpt_id { get; set; }
        public Nullable<long> vd_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public string topic_title { get; set; }
        public string topic_group_product { get; set; }
        public string topic_filepath { get; set; }
        public string topic_file_name { get; set; }
        public Nullable<int> vpt_feedback { get; set; }
        public Nullable<System.DateTime> vpt_feedback_date { get; set; }
        public string visit_id { get; set; }
    }
}
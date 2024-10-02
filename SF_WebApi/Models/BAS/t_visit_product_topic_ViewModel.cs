using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class t_visit_product_topic_ViewModel
    {
        public int vpt_id { get; set; }
        public Nullable<long> vd_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public Nullable<int> vpt_feedback { get; set; }
        public Nullable<System.DateTime> vpt_feedback_date { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class v_info_feedback_ViewModel
    {
        public int info_feedback_id { get; set; }
        public Nullable<int> info_feedback_type { get; set; }
        public string info_description { get; set; }
        public int topic_id { get; set; }
        public string topic_title { get; set; }
        public string topic_group_product { get; set; }
        public string topic_filepath { get; set; }
        public string topic_file_name { get; set; }
        public int is_selected { get; set; }
    }
}
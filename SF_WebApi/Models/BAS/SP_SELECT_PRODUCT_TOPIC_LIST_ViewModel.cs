using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_PRODUCT_TOPIC_LIST_ViewModel
    {
        public int topic_id { get; set; }
        public string topic_title { get; set; }
        public string topic_group_product { get; set; }
        public string topic_filepath { get; set; }
        public string topic_file_name { get; set; }
        public Nullable<byte> topic_status { get; set; }
    }
}
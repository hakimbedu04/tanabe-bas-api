using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS
{
    public class SP_SELECT_SP_ATTACHMENT_ViewModel
    {
        public int spf_id { get; set; }
        public string spr_id { get; set; }
        public string spf_file_name { get; set; }
        public string spf_file_path { get; set; }
        public Nullable<System.DateTime> spf_date_uploaded { get; set; }
    }
}
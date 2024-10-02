using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS.SP
{
    public class v_auth_sp_ViewModel
    {
        public int auth_sp_id { get; set; }
        public Nullable<int> role_id { get; set; }
        public string gl_sub_code { get; set; }
        public string gl_code { get; set; }
        public string gl_sub_desc { get; set; }
        public string auth_view { get; set; }
    }
}
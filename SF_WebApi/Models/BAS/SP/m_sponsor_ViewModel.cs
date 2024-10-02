using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS.SP
{
    public class m_sponsor_ViewModel
    {
        public int sponsor_id { get; set; }
        public string sponsor_description { get; set; }
        public string sponsor_group { get; set; }
        public Nullable<int> sponsor_status { get; set; }
        public string sponsor_sp_type { get; set; }
    }
}
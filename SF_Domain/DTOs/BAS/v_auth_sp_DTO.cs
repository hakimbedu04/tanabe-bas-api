using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class v_auth_sp_DTO
    {
        public int auth_sp_id { get; set; }
        public Nullable<int> role_id { get; set; }
        public string gl_sub_code { get; set; }
        public string gl_code { get; set; }
        public string gl_sub_desc { get; set; }
        public string auth_view { get; set; }
    }
}

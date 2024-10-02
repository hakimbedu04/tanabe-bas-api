using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_sp_attachment_DTO
    {
        public int spf_id { get; set; }
        public string spr_id { get; set; }
        public string spf_file_name { get; set; }
        public string spf_file_path { get; set; }
        public Nullable<System.DateTime> spf_date_uploaded { get; set; }
    }
}

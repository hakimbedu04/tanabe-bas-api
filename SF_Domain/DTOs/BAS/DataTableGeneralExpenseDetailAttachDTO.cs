using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableGeneralExpenseDetailAttachDTO
    {
        public int gla_id { get; set; }
        public string hdr_id { get; set; }
        public string gla_file_name { get; set; }
        public string gla_file_path { get; set; }
        public Nullable<System.DateTime> gla_date_uploaded { get; set; }
    }
}

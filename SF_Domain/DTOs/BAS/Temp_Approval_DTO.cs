using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class Temp_Approval_DTO
    {
        public string position { get; set; }
        public string functional { get; set; }
        public string approval { get; set; }
        public string comment { get; set; }
        public Nullable<DateTime> date_approved { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_sp_doctor_DTO
    {
        public int spdr_id { get; set; }
        public int sp_id { get; set; }
        public Nullable<int> dr_code { get; set; }
        public string dr_as { get; set; }
        public Nullable<int> dr_plan { get; set; }
        public Nullable<int> dr_actual { get; set; }
        public Nullable<System.DateTime> dr_date_realization { get; set; }
    }
}

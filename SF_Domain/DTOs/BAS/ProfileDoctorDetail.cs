using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class ProfileDoctorDetail
    {
        public int dr_code { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_phone { get; set; }
        public Nullable<System.DateTime> dr_birthday { get; set; }
        public Nullable<int> dr_number_patient { get; set; }
        public string dr_monitoring { get; set; }
        public string dr_sbo { get; set; }
        public string dr_bo { get; set; }
        public string dr_quadrant { get; set; }
    }
}

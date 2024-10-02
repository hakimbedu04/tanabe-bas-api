using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class CallDoctorDTO
    {
        public int dr_code { get; set; }
        public string dr_name { get; set; }
        public string dr_spec { get; set; }
        public string dr_quadrant { get; set; }
        public int HerCDValue { get; set; }
        public int HerInjValue { get; set; }
        public int LIVValue { get; set; }
    }
}

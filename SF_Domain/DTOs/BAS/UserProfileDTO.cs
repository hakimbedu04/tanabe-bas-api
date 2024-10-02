using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class UserProfileDTO
    {
        public string rep_id { get; set; }
        public string rep_full_name { get; set; }
        public string phone { get; set; }
        public string birth_place { get; set; }
        public Nullable<DateTime> birth_date { get; set; }
        public string address { get; set; }
        public string rep_region { get; set; }
        public string rep_sbo { get; set; }
        public string ProfilePicture { get; set; }
        public string rep_position { get; set; }
    }
}

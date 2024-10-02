using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_bo_DTO
    {
        public int bo_id { get; set; }
        public string bo_code { get; set; }
        public Nullable<int> reg_id { get; set; }
        public string bo_description { get; set; }
        public string bo_address { get; set; }
        public Nullable<int> bo_sequence_code { get; set; }
        public string bo_am { get; set; }
        public Nullable<int> bo_status { get; set; }
        public string bo_area { get; set; }
    }
}

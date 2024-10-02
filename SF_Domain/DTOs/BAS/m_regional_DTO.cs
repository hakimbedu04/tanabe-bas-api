using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_regional_DTO
    {
        public int reg_id { get; set; }
        public string reg_code { get; set; }
        public string reg_description { get; set; }
        public string reg_functionary { get; set; }
        public Nullable<int> reg_sequence_code { get; set; }
        public Nullable<int> reg_status { get; set; }
    }
}

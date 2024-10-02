using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.HRD
{
    public class Jabatan_DTO
    {
        public string Kode_Jabatan { get; set; }
        public string Nama_Jabatan { get; set; }
        public Nullable<int> Kode_Level { get; set; }
        public Nullable<decimal> Tunjangan { get; set; }
        public string Nama_Level { get; set; }
        public Nullable<int> Level_Approval { get; set; }
        public string Nama_Jabatan2 { get; set; }
        public Nullable<int> Disabled { get; set; }
    }
}

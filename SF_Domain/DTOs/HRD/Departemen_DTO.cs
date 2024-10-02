using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.HRD
{
    public class Departemen_DTO
    {
        public string Kode_Departemen { get; set; }
        public string Nama_Departemen { get; set; }
        public string Kode_Headquarter { get; set; }
        public string NIK { get; set; }
        public string kd_dept_asal { get; set; }
        public string nama_dept_asal { get; set; }
        public string kd_hq_asal { get; set; }
        public string kd_hq_contrary { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string IndexID { get; set; }
        public Nullable<int> isActive { get; set; }
        public Nullable<int> Disabled { get; set; }
    }
}

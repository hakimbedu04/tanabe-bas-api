using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs
{
    public class hrd_All_KaryawanDTO
    {
        public string Nomor_Induk { get; set; }
        public string Nama { get; set; }
        public string Kode_Headquarter { get; set; }
        public string Kode_Departemen { get; set; }
        public string Nama_Seksi { get; set; }
        public string Kode_Bagian { get; set; }
        public string Nama_Bagian { get; set; }
        public string Singkatan_Bagian { get; set; }
        public Nullable<int> Status_Aktif { get; set; }
        public string Kode_Jabatan { get; set; }
        public string Nama_Departemen { get; set; }
        public string Email { get; set; }
        public Nullable<int> Kode_Level { get; set; }
        public string Nama_Cabang { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string id_data { get; set; }
    }
}

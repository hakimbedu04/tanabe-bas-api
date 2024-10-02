using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs
{
    public class Prod_All_KaryawanDTO
    {
        public string Nomor_Induk { get; set; }
        public string Nama { get; set; }
        public string Kode_Headquarter { get; set; }
        public string Kode_Departemen { get; set; }
        public string Nama_Departemen { get; set; }
        public string Nama_Seksi { get; set; }
        public string Kode_Bagian { get; set; }
        public string Nama_Bagian { get; set; }
        public string Singkatan_Bagian { get; set; }
        public Nullable<int> Kode_Level { get; set; }
        public Nullable<System.DateTime> Tanggal_Keluar { get; set; }
        public string Nama_Cabang { get; set; }
        public string Alamat { get; set; }
        public string Kota { get; set; }
        public string Kode_Pos { get; set; }
        public string Nomor_Telepon { get; set; }
        public string Nomor_Fax { get; set; }
        public Nullable<System.DateTime> Tanggal_Kerja { get; set; }
        public Nullable<System.DateTime> Tanggal_Diangkat { get; set; }
        public string NoTelpKaryawan { get; set; }
        public string Email { get; set; }
        public string kd_dept_cabang { get; set; }
        public string kd_dept_asal { get; set; }
        public string nama_dept_asal { get; set; }
        public string kd_hq_asal { get; set; }
        public string kd_hq_contrary { get; set; }
    }
}

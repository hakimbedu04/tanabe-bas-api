using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.HRD
{
    public class tUserModel
    {
        public string uName { get; set; }
        public string uPwd { get; set; }
        public string Kode_Bagian { get; set; }
        public string Nomor_Induk { get; set; }
        public string Kode_Level { get; set; }
        public string Nama_Seksi { get; set; }
        public string Departemen { get; set; }
        public Nullable<int> Level_No { get; set; }
        public Nullable<int> Status_Aktif { get; set; }
        public Nullable<System.DateTime> Last_Login { get; set; }
        public string Headquarter { get; set; }
        public short Pesan { get; set; }
        public Nullable<short> Desktop_App { get; set; }
        public Nullable<short> Web_App { get; set; }
        public string Kode_Office { get; set; }
        public string userPwd { get; set; }
        public Nullable<int> Min_Panjang_Pwd { get; set; }
        public Nullable<System.DateTime> Terakhir_Ganti_Pwd { get; set; }
        public Nullable<int> Lama_Ganti_Pwd { get; set; }
        public string Email_Address { get; set; }
        public Nullable<System.DateTime> last_login_decision { get; set; }
        public Nullable<int> user_right { get; set; }
        public int sec_user_id { get; set; }
        public Nullable<int> sec_role_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public Nullable<int> count_wrong_password { get; set; }
        public Nullable<int> section_id { get; set; }
    }
}
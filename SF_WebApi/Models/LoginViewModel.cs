using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models
{
    public class LoginViewModel
    {
        public string NamaLengkap { get; set; }
        public string KodeDepartemen { get; set; }
        public string KodeBagian { get; set; }
        public string Rep_Posisi { get; set; }
        public string Rep_Reg { get; set; }
        public string Rep_Name { get; set; }
        public string Rep_Bo { get; set; }
        public string Rep_Sbo { get; set; }
        public string Rep_RmName { get; set; }
        public string Rep_AmName { get; set; }
        public string Rep_Email { get; set; }
        public string Rep_RmEmail { get; set; }
        public string Rep_AmEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rep_Id { get; set; }
        public bool IsManagerUp { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsValidPosition { get; set; } = false;

    }
}
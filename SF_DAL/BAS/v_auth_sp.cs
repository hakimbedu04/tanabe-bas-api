//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SF_DAL.BAS
{
    using System;
    using System.Collections.Generic;
    
    public partial class v_auth_sp
    {
        public int auth_sp_id { get; set; }
        public Nullable<int> role_id { get; set; }
        public string gl_sub_code { get; set; }
        public string gl_code { get; set; }
        public string gl_sub_desc { get; set; }
        public Nullable<int> auth_view { get; set; }
    }
}
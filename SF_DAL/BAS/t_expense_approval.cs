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
    
    public partial class t_expense_approval
    {
        public int ea_id { get; set; }
        public string hdr_id { get; set; }
        public string ea_position { get; set; }
        public string ea_functionary { get; set; }
        public string ea_nik { get; set; }
        public Nullable<byte> ea_level { get; set; }
        public string ea_approval { get; set; }
        public Nullable<System.DateTime> ea_date_sign { get; set; }
        public string ea_comment { get; set; }
        public string ea_email { get; set; }
        public string ea_action { get; set; }
        public Nullable<byte> ea_ready { get; set; }
        public Nullable<System.DateTime> ea_date_updated { get; set; }
    }
}
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
    
    public partial class m_position
    {
        public int posId { get; set; }
        public string pos_description { get; set; }
        public Nullable<int> pos_max_visit { get; set; }
        public Nullable<int> pos_min_visit { get; set; }
        public Nullable<byte> status { get; set; }
        public Nullable<byte> pos_dr_quota { get; set; }
        public Nullable<byte> pos_dr_visit_all { get; set; }
        public Nullable<byte> pos_visit_verification { get; set; }
        public Nullable<byte> pos_visit { get; set; }
        public Nullable<byte> pos_visit_user { get; set; }
        public Nullable<byte> pos_mobile { get; set; }
    }
}
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
    
    public partial class t_visit_product
    {
        public t_visit_product()
        {
            this.t_visit_product_topic = new HashSet<t_visit_product_topic>();
        }
    
        public long vd_id { get; set; }
        public string visit_id { get; set; }
        public string visit_code { get; set; }
        public Nullable<int> vd_value { get; set; }
        public Nullable<System.DateTime> vd_date_saved { get; set; }
        public Nullable<byte> sp_sp { get; set; }
        public Nullable<int> sp_percentage { get; set; }
    
        public virtual t_visit t_visit { get; set; }
        public virtual ICollection<t_visit_product_topic> t_visit_product_topic { get; set; }
    }
}
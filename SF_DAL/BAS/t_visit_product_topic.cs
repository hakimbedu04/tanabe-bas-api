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
    
    public partial class t_visit_product_topic
    {
        public int vpt_id { get; set; }
        public Nullable<long> vd_id { get; set; }
        public Nullable<int> topic_id { get; set; }
        public Nullable<int> vpt_feedback { get; set; }
        public Nullable<System.DateTime> vpt_feedback_date { get; set; }
        public string note_feedback { get; set; }
        public Nullable<int> info_feedback_id { get; set; }
        public Nullable<int> show_duration { get; set; }
    
        public virtual t_visit_product t_visit_product { get; set; }
    }
}

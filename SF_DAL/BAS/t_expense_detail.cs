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
    
    public partial class t_expense_detail
    {
        public int dtl_id { get; set; }
        public string hdr_id { get; set; }
        public Nullable<byte> dtl_line_no { get; set; }
        public string dtl_description { get; set; }
        public string dtl_acc_debit { get; set; }
        public string dtl_acc_debit_desc { get; set; }
        public string dtl_sub_acc { get; set; }
        public string dtl_sub_acc_desc { get; set; }
        public string dtl_cost_center { get; set; }
        public string dtl_curr { get; set; }
        public string dtl_saf_1 { get; set; }
        public string dtl_saf_1_concept { get; set; }
        public string dtl_saf_1_structure { get; set; }
        public string dtl_saf_2 { get; set; }
        public string dtl_saf_2_concept { get; set; }
        public string dtl_saf_2_structure { get; set; }
        public string dtl_saf_3 { get; set; }
        public string dtl_saf_3_concept { get; set; }
        public string dtl_saf_3_structure { get; set; }
        public string dtl_saf_4 { get; set; }
        public string dtl_saf_4_concept { get; set; }
        public string dtl_saf_4_structure { get; set; }
        public Nullable<double> dtl_amount_db { get; set; }
        public Nullable<double> dtl_amount_cr { get; set; }
        public Nullable<System.DateTime> dtl_date_updated { get; set; }
    }
}

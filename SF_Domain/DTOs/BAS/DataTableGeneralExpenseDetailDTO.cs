using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableGeneralExpenseDetailDTO
    {
        public int dtl_id { get; set; }
        public string hdr_id { get; set; }
        public Byte dtl_line_no { get; set; }
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
    }
}

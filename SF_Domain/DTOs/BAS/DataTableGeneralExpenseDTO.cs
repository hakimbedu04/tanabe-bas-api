using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableGeneralExpenseDTO
    {
        public string hdr_id { get; set; }
        public string expense_lip { get; set; }
        public Nullable<DateTime> hdr_posting_date { get; set; }
        public string hdr_daybook { get; set; }
        public string hdr_description { get; set; }
        public string hdr_allocation_key { get; set; }
        public Nullable<double> total_expense { get; set; }
        public string hdr_posting_status_desc { get; set; }
        public Nullable<DateTime> hdr_created_date { get; set; }
        public string hdr_payment_status { get; set; }
        public string hdr_transfered_date { get; set; }
        public string hdr_posting_by { get; set; }
    }
}

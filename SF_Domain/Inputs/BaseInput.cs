using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs
{
    public class BaseInput : SortBaseInput
    {
        public string Auth { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? VisitDateTime { get; set; }
        public string Prm { get; set; }
        public string ActionSource { get; set; }
        public string RepId { get; set; }
        public string RepPosition { get; set; }
        public string VisitId { get; set; }
        public int VdId { get; set; }
        public int VptId { get; set; }
        public string VisitCode { get; set; }
        public int Percentage { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int posDate { get; set; }
        public string prd_code { get; set; }
        public string dr_code { get; set; }
        public string sales_id { get; set; }
        public int tx_target_qty { get; set; }
        public string tx_note { get; set; }
        public int sp_id { get; set; }
        public string sp_type { get; set; }
        public int RoleId { get; set; }
        public string spr_id { get; set; }
        public string BAllocation { get; set; }
        public string spr_no { get; set; }
        public string spr_date { get; set; }
        public string event_name { get; set; }
        public string topic { get; set; }
        public string event_place { get; set; }
        public string hrd_id { get; set; }

        public List<DetailExpenseModel> detailExpense { get; set; }
        public List<t_spAttachmentModel> attachmentExpense { get; set; }
        public int gla_id { get; set; }
        public int dtl_id { get; set; }
        public string dtl_desc { get; set; }
        public double value { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string newPass { get; set; }
        public string TableName { get; set; }
    }
}

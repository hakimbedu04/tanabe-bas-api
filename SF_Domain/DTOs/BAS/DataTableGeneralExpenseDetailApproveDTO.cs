using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableGeneralExpenseDetailApproveDTO
    {
        public int ea_id {get; set;}
        public string hdr_id { get; set;}
        public string ea_position { get; set;}
        public string ea_functionary { get; set;}
        public string ea_nik { get; set;}
        public byte ea_level { get; set;}
        public string ea_approval { get; set;}
        public Nullable<DateTime> ea_date_sign { get; set;}
        public string ea_comment { get; set;}
        public string ea_email { get; set;}
        public string ea_action { get; set;}
        public byte ea_ready { get; set;}
    }
}

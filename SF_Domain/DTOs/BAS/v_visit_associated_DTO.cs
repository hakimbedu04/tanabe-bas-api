using System;

namespace SF_Domain.DTOs.BAS
{
    public class v_visit_associated_DTO
    {
        public int associate_id { get; set; }
        public string visit_id_initiator { get; set; }
        public string rep_id_initiator { get; set; }
        public Nullable<int> dr_code_initiator { get; set; }
        public string visit_id_invited { get; set; }
        public string rep_id_invited { get; set; }
        public Nullable<int> dr_code_invited { get; set; }
        public Nullable<int> associate_status { get; set; }
        public string visit_code { get; set; }
    }
}

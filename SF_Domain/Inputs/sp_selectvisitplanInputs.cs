using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SF_Domain.Inputs
{
    public class sp_selectVisitInputs : BaseInput
    {
        //[Required]
        public int day { get; set; }
        //[Required]
        public int month { get; set; }
        //[Required]
        public int year { get; set; }

        
        public string PrdCode { get; set; }
        public int Qty { get; set; }
        public string Note { get; set; }
        public int Sp { get; set; }
        public int Percentage { get; set; }
        
        public string EventName { get; set; }
        public string BAllocation { get; set; }
        public int BAmount { get; set; }
        public string SpId { get; set; }
        public string SpdsId { get; set; }
    }
}
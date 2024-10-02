using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class EventParticipant_DTO
    {
        public string dr_name { get; set; }
        public string sponsor_description { get; set; }
        public string budget_currency { get; set; }
        public Nullable<double> budget_plan_value { get; set; }
    }
}

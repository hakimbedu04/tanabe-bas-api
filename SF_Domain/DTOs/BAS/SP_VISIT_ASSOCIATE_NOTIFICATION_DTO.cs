using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class SP_VISIT_ASSOCIATE_NOTIFICATION_DTO
    {
        public int associate_id { get; set; }
        public string va_notif { get; set; }
        public Nullable<System.DateTime> visit_date_plan_invited { get; set; }
    }
}

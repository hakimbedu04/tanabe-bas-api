using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DataTableUserHistoryDetailDTO
    {
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<long> qty { get; set; }
        public Nullable<long> qtySum { get; set; }
    }
}

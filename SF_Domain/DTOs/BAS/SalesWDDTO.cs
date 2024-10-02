using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class SalesWDDTO
    {
        public int dateSum {get; set;}
        public string sumDateThis {get; set;}
        public string sumDateLast { get; set; }
        public string sumDateAvg { get; set; }

        public string tempThisYear  { get; set; }
        public string tempThisMonth  { get; set; }
        public string tempLastYear  { get; set; }
        public string tempLastMonth  { get; set; }
        public string tempAvgMonthSt  { get; set; }
        public string tempAvgMonthEd  { get; set; }
        public string tempSwdlNow  { get; set; }
        public string tempSwdLast  { get; set; }
        public string tempSwdAvg { get; set; }
    }
}

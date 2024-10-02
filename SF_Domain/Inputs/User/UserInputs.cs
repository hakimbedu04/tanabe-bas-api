using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.Inputs.User
{
    public class UserInputs : BaseInput
    {
        public int SpaId { get; set; }
        public int SpId { get; set; }
        public DateTime SpaDate { get; set; }
        public long SpaQuantity { get; set; }
        public string SpaNote { get; set; }
        public string TextSearch { get; set; }
    }
}

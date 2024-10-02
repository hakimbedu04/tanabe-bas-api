using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.ErrLogs
{
    public interface IVTLogger
    {
        void Err(Exception err, List<object> obj = null, List<object> session = null, string message = null);
    }
}

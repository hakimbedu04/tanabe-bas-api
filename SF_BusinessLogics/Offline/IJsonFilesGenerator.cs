using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;

namespace SF_BusinessLogics.Offline
{
    public interface IJsonFilesGenerator
    {
        void GenFile(BaseInput param, List<object> obj = null);
        string GenFileLink(BaseInput param, List<object> obj = null);
        string ZipFiles(BaseInput param);
    }
}

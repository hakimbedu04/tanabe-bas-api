using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Repositories.SPRepo
{
    public interface ISPRepo
    {
        string GetFilePath(int spfid);
        bool DeleteSPAttachment(int spfid);
    }
}

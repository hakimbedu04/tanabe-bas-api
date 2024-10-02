using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;

namespace SF_Repositories.SPRepo
{
    public class SPRepo : ISPRepo
    {
        private bas_trialEntities _basContext;
        public SPRepo(bas_trialEntities basContext)
        {
            _basContext = basContext;
        }
        public string GetFilePath(int spfid)
        {
            return _basContext.t_sp_attachment.Find(spfid).spf_file_path;
        }

        public bool DeleteSPAttachment(int spfid)
        {
            try
            {
                var dbRes = _basContext.t_sp_attachment.Find(spfid);
                if (dbRes != null)
                {
                    _basContext.t_sp_attachment.Remove(dbRes);
                    _basContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

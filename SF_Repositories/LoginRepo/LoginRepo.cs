using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.HRD;
using SF_Domain.DTOs;

namespace SF_Repositories.LoginRepo
{
    public class LoginRepo : ILoginRepo
    {
        public List<hrd_tUserDTO> CheckGlobalUser(hrd_tUserDTO inputs)
        {
            using (var context = new hrdEntities())
            {
                var dbResult =
                    context.tusers
                        .Where(x => x.uName == inputs.uName && x.uPwd == inputs.uPwd)
                        .Select(x => x.Nomor_Induk)
                        .ToList();
                var viewMapper = Mapper.Map<List<hrd_tUserDTO>>(dbResult);
                return viewMapper;
            }
        }
    }
}

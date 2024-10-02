using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs;

namespace SF_Repositories.LoginRepo
{
    public interface ILoginRepo
    {
        List<hrd_tUserDTO> CheckGlobalUser(hrd_tUserDTO inputs);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;

namespace SF_BusinessLogics.User
{
    public interface IUserActualBLL
    {
        List<v_sales_product_DTO> GetUserActualDatas(UserInputs inputs);
        List<v_sales_product_DTO> GetUserActualSearch(UserInputs inputs);
        bool isHaveRemainingToSendMail(UserInputs inputs);
    }
}

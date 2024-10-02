using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;

namespace SF_BusinessLogics.User
{
    public interface IUserRealizationBLL
    {
        List<v_sales_product_DTO> GetUserRealizationDatas(UserInputs inputs);
        List<v_sales_product_DTO> GetUserRealizationDatas(UserExportCustomModelInputs inputs);
        void InsertSalesProductActual(UserInputs inputs);
        List<t_sales_product_actual_DTO> GetSalesProductActualDtos(UserInputs inputs);
        void UpdateSalesProductActual(UserInputs inputs);
        void DeleteSalesProductActual(UserInputs inputs);
    }
}

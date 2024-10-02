using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.Inputs.User;

namespace SF_BusinessLogics.User
{
    public interface IUserPlanBLL
    {
        List<DataTableUserPlanDTO> getDataTable(string rep_id, int month, int year, String SortExpression, String sortOrder, string searchColumn, string search);

        int requests(string rep_id, int Month, int Year, string rep_name, string rep_reg, string rep_bo);

        List<DataTableUserPlanDetailDTO> getDataTableDetail(string rep_id, int Month, int Year, string sortExpression, string sortOrder, string sales_id);

        List<ProductUserDTO> getListProduct();

        int addProduct(string sales_id, string rep_id, string prd_code, int tx_target_qty, string tx_note);

        void updateProduct(int sp_id, int tx_target_qty);

        void deleteProduct(int sp_id);
    }
}

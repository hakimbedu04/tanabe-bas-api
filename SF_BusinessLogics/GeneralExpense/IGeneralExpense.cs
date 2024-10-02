using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.GeneralExpense
{
    public interface IGeneralExpense
    {
        List<DataTableGeneralExpenseDTO> getDataTable(string rep_id, int month, int year, string sortExpression, string sortOrder, string searchColumn, string searchValue);

        List<DataTableGeneralExpenseDetailDTO> getDataTableDetail(string hrd_id);

        List<DataTableGeneralExpenseDetailApproveDTO> getDataTableDetailApprove(string hrd_id);

        List<DataTableGeneralExpenseDetailAttachDTO> getDataTableDetailAttach(string hrd_id);

        int deleteData(string hrd_id);

        int add(string rep_id, string bo_description, string detailexpenselist, string detailexpenseattachmentlist);

        int editDetailAdd(string repId, string hdrId, string dtl_desc, double value, string accDebit, string costCenter, string saf1);

        int editDetailEdit(int dtl_id, string dtl_desc, double value, string accDebit, string costCenter, string saf1);

        int editDetailDelete(int dtl_id);

        int editDetailAttachmentDelete(int gla_id);

        int editDetailAttachmentAdd(string hdrId, string fileName, string filePath);

        int editDescription(string hdrId, string desc);
    }
}

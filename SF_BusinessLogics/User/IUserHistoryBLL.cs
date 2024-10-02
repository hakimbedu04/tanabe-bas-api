using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.User
{
    public interface IUserHistoryBLL
    {
        List<DataTableUserHistoryDTO> getDataTable(string rep_id, int month, int year, string SortExpression, string sortOrder, string search, string searchColumn, string searchValue);

        List<DataTableUserHistoryDetailDTO> getDataTableDetail(Int64 sp_id);

        DataTableUserHistorySUMDTO getDataTableSUM(string rep_id, int month, int year, string sortExpression, string sortOrder, string search, string searchColumn, string searchValue);
    }
}

using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.SP
{
    public interface ISPActualBLL
    {
        List<DataTableSPActualDTO> getDataTable(string rep_id, int Month, int Year, string SortExpression, string SortOrder, string SearchColumn, string SearchValue);
    }
}

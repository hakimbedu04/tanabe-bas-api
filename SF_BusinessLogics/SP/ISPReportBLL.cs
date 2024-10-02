using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.SP
{
    public interface ISPReportBLL
    {
        List<DataTableSPReportDTO> getDataTable(string rep_id, int Month, int Year, string rep_position, string SortExpression, string SortOrder, string SearchColumn, string SearchValue);

        List<EventDetail_DTO> getDetailEvent(string rep_id, int Month, int Year, string rep_position, string spr_id);

        List<String> getDetailProduct(string rep_id, int Month, int Year, string rep_position, string spr_id);

        List<EventParticipant_DTO> getDetailParticipant(string rep_id, int Month, int Year, string rep_position, int sp_id, string sp_type);
    }
}

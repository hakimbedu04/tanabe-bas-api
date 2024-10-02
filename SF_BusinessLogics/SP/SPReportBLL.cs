using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.SP
{
    public class SPReportBLL : ISPReportBLL
    {
        public List<DataTableSPReportDTO> getDataTable(string rep_id, int Month, int Year, string rep_position, string SortExpression, string SortOrder, string SearchColumn, string SearchValue)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableSPReportDTO> SPReport = new List<DataTableSPReportDTO>();
            SPReport = bas.Database.SqlQuery<DataTableSPReportDTO>("SP_SELECT_SP_REPORT '" + rep_id + "', " + Month + ", " + Year + ", '" + rep_position+"'").ToList();

            if (!SearchColumn.ToLower().Equals(""))
            {
                string[] arrSearch = SearchValue.Split(',');
                string[] arrColumn = SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    SPReport = SPReport.Where(r => r.GetType().GetProperty(arrColumn[i]).GetValue(r, null).ToString().Contains(arrSearch[i])).ToList();
            }

            if (SortOrder.ToLower().Equals("asc"))
            {
                SPReport = SPReport.OrderBy(r => r.GetType().GetProperty(SortExpression).GetValue(r, null)).ToList();
            }
            else
            {
                SPReport = SPReport.OrderByDescending(r => r.GetType().GetProperty(SortExpression).GetValue(r, null)).ToList();
            }

            return SPReport;
        }

        public List<EventDetail_DTO> getDetailEvent(string rep_id, int Month, int Year, string rep_position, string spr_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableSPReportDTO> SPReport = new List<DataTableSPReportDTO>();
            SPReport = bas.Database.SqlQuery<DataTableSPReportDTO>("SP_SELECT_SP_REPORT '" + rep_id + "', " + Month + ", " + Year + ", '" + rep_position + "'").Where(x => x.spr_id.Equals(spr_id)).ToList();

            List<EventDetail_DTO> events = new List<EventDetail_DTO>();
            events = SPReport.Select(x => new EventDetail_DTO
            {
                e_a_gp = x.e_a_gp.HasValue? x.e_a_gp : 0,
                e_a_gp_pax = x.e_a_gp_pax.HasValue ? x.e_a_gp_pax : 0,
                e_a_nurse = x.e_a_nurse.HasValue ? x.e_a_nurse : 0,
                e_a_nurse_pax = x.e_a_nurse_pax.HasValue ? x.e_a_nurse_pax : 0,
                e_a_others = x.e_a_others.HasValue ? x.e_a_others : 0,
                e_a_others_pax = x.e_a_others_pax.HasValue ? x.e_a_others_pax : 0,
                e_a_specialist = x.e_a_specialist.HasValue ? x.e_a_specialist : 0,
                e_a_specialist_pax = x.e_a_specialist_pax.HasValue ? x.e_a_specialist_pax : 0, 
                e_dt_end = x.e_dt_end, 
                e_dt_start = x.e_dt_start, 
                e_name = x.e_name, 
                e_place = x.e_place, 
                e_topic = x.e_topic, 
                spr_id = x.spr_id, 
                spr_note = x.spr_note
            }).ToList();
            
            return events;
        }

        public List<String> getDetailProduct(string rep_id, int Month, int Year, string rep_position, string spr_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableSPReportDTO> SPReport = new List<DataTableSPReportDTO>();
            SPReport = bas.Database.SqlQuery<DataTableSPReportDTO>("SP_SELECT_SP_REPORT '" + rep_id + "', " + Month + ", " + Year + ", '" + rep_position + "'").Where(x => x.spr_id.Equals(spr_id)).ToList();

            List<String> prd = new List<string>();
            foreach(string item in SPReport.Select(x => x.e_topic))
            {
                prd.Add(item.Substring(0, item.IndexOf(" :")));
            }
            
            return prd;
        }

        public List<EventParticipant_DTO> getDetailParticipant(string rep_id, int Month, int Year, string rep_position, int sp_id, string sp_type)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<EventParticipant_DTO> participant = new List<EventParticipant_DTO>();
            participant = bas.v_doctor_sponsor.Where(x => x.sp_id == sp_id && x.sp_type == sp_type).Select(x => new EventParticipant_DTO { budget_currency = x.budget_currency, budget_plan_value = x.budget_plan_value, dr_name = x.dr_name, sponsor_description = x.sponsor_description }).ToList();

            return participant;
        }
    }
}

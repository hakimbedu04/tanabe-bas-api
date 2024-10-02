using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SF_WebApi.Models;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;

namespace SF_WebApi.Controllers
{
    public class PrintviewController : Controller
    {
        private static string sprIds, sp_types, spr_nos, spr_dates, initiator_positions, initiator_names, initiator_ams, initiator_branchs, initiator_regions, event_names, event_date_starts, event_topics, event_date_ends, event_places;
        private static List<Temp_HCP_DTO> tempHcpDTO, tempHcpDTODetail;
        private static List<Temp_Approval_DTO> tempApprovalDTO;
        private static List<Temp_Sales_History_DTO> tempSalesHistoryDTO;

        [System.Web.Http.HttpGet]
        public ActionResult SprViewAsPdf(string sprId, string sp_type, string spr_no, string spr_date, string initiator_position, string initiator_name, string initiator_am, string initiator_branch, string initiator_region, string event_name, string event_date_start, string event_topic, string event_date_end, string event_place)
        {
            if(sprId != null)
                sprIds = sprId;

            if (sp_type != null)
                sp_types = sp_type;

            if (spr_no != null)
                spr_nos = spr_no;

            if (spr_date != null)
                spr_dates = spr_date;

            if (initiator_position != null)
                initiator_positions = initiator_position;

            if (initiator_name != null)
                initiator_names = initiator_name;

            if (initiator_am != null)
                initiator_ams = initiator_am;

            if (initiator_branch != null)
                initiator_branchs = initiator_branch;

            if (initiator_region != null)
                initiator_regions = initiator_region;

            if (event_name != null)
                event_names = event_name;

            if (event_date_start != null)
                event_date_starts = event_date_start;

            if (event_topic != null)
                event_topics = event_topic;

            if (event_date_end != null)
                event_date_ends = event_date_end;

            if (event_place != null)
                event_places = event_place;

            TempData["sprId"] = sprIds;
            TempData["sp_type"] = sp_types;
            TempData["spr_no"] = spr_nos;
            TempData["spr_date"] = spr_dates;
            TempData["initiator_position"] = initiator_positions;
            TempData["initiator_name"] = initiator_names;
            TempData["initiator_am"] = initiator_ams;
            TempData["initiator_branch"] = initiator_branchs;
            TempData["initiator_region"] = initiator_regions;
            TempData["event_name"] = event_names;
            TempData["event_date_start"] = event_date_starts;
            TempData["event_topic"] = event_topics;
            TempData["event_date_end"] = event_date_ends;
            TempData["event_place"] = event_places;

            bas_trialEntities bas = new bas_trialEntities();
            tempHcpDTO = bas.v_doctor_sponsor.Where(x => x.spr_id == sprIds && x.sp_type.Equals("SP0")).Select(x => new Temp_HCP_DTO { AM = x.am_name, Amount = x.budget_plan_value, Contact = "", dr_name = x.dr_name, Monitoring = x.dr_monitoring, Representative = x.rep_name, Spec = x.dr_spec, Sponsor = x.sponsor_description }).OrderBy(x => x.dr_name).ToList();
            TempData["tempHcpDTO"] = tempHcpDTO;

            tempHcpDTODetail = bas.v_doctor_sponsor.Where(x => x.spr_id == sprIds && !x.sp_type.Equals("SP0")).Select(x => new Temp_HCP_DTO { AM = x.am_name, Amount = x.budget_plan_value, Contact = "", dr_name = x.dr_name, Monitoring = x.dr_monitoring, Representative = x.rep_name, Spec = x.dr_spec, Sponsor = x.sponsor_description }).OrderBy(x => x.dr_name).ToList();
            TempData["tempHcpDTODetail"] = tempHcpDTODetail;

            tempApprovalDTO = bas.t_sp_approval.Where(x => x.spr_id == sprIds).OrderBy(x => x.spa_level).Select(x => new Temp_Approval_DTO { approval = x.spa_approval, comment = x.spa_comment, date_approved = x.spa_date_sign, functional = x.spa_functionary, position = x.spa_position }).ToList();
            TempData["tempApprovalDTO"] = tempApprovalDTO;

            var temp = bas.Database.SqlQuery<SP_SELECT_REPORT_SALES_HISTORY_Result>("EXEC SP_SELECT_REPORT_SALES_HISTORY '"+ sprIds + "'").ToList();

            tempSalesHistoryDTO = temp.Select(x => new Temp_Sales_History_DTO { category = x.ctg_product, productName = x.prod_desc, qty = x.sales_quantity, val = x.sales_value, avg = x.monthly_avg}).ToList();
            TempData["tempSalesHistoryDTO"] = tempSalesHistoryDTO;

            return View();
        }

        public ActionResult PrintAll(string sprId, string sp_type, string spr_no, string spr_date, string initiator_position, string initiator_name, string initiator_am, string initiator_branch, string initiator_region, string event_name, string event_date_start, string event_topic, string event_date_end, string event_place)
        {
            sprIds = sprId;
            sp_types = sp_type;
            spr_nos = spr_no;
            spr_dates = spr_date;
            initiator_positions = initiator_position;
            initiator_names = initiator_name;
            initiator_ams = initiator_am;
            initiator_branchs = initiator_branch;
            initiator_regions = initiator_region;
            event_names = event_name;
            event_date_starts = event_date_start;
            event_topics = event_topic;
            event_date_ends = event_date_end;
            event_places = event_place;

            var report = new Rotativa.ActionAsPdf("SprViewAsPdf");
            return report;
        }
    }
}
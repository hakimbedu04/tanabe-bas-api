using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.SP
{
    public class SPActualBLL : ISPActualBLL
    {
        public List<DataTableSPActualDTO> getDataTable(string rep_id, int Month, int Year, string SortExpression, string SortOrder, string searchColumn, string searchValue)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableSPActualDTO> SPActual = new List<DataTableSPActualDTO>();
            List<DataTableSPActualDTO> SPActualFinal = new List<DataTableSPActualDTO>();
            SPActual = bas.Database.SqlQuery<DataTableSPActualDTO>("SP_SELECT_SP_ACTUAL @rep_id = '"+rep_id+"', @month = "+Month+", @year = "+Year+"").ToList();

            if (!searchColumn.ToLower().Equals(""))
            {
                string[] arrSearch = searchValue.Split(',');
                string[] arrColumn = searchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    SPActual = SPActual.Where(r => r.GetType().GetProperty(arrColumn[i]).GetValue(r, null).ToString().Contains(arrSearch[i])).ToList();
            }

            if (SortOrder.ToLower().Equals("asc"))
            {
                SPActual = SPActual.OrderBy(r => r.GetType().GetProperty(SortExpression).GetValue(r, null)).ToList();
            }
            else
            {
                SPActual = SPActual.OrderByDescending(r => r.GetType().GetProperty(SortExpression).GetValue(r, null)).ToList();
            }

            //Nullable<int> dr_plan_sum = 0;
            //Nullable <double> budget_plan_sum = 0;
            //Nullable <double> budget_real_sum = 0;
            //string spr_id = "";

            //Nullable<int> dr_plan_sum_all = 0;
            //Nullable<double> budget_plan_sum_all = 0;
            //Nullable<double> budget_real_sum_all = 0;

            foreach (DataTableSPActualDTO item in SPActual)
            {
                DataTableSPActualDTO data = new DataTableSPActualDTO();
                data.budget_plan_sum = item.budget_plan_sum;
                data.budget_real_sum = item.budget_real_sum;
                data.e_a_gp = item.e_a_gp;
                data.e_a_gp_pax = item.e_a_gp_pax;
                data.e_a_nurse = item.e_a_nurse;
                data.e_a_nurse_pax = item.e_a_nurse_pax;
                data.e_a_others = item.e_a_others;
                data.e_a_others_pax = item.e_a_others_pax;
                data.e_a_specialist = item.e_a_specialist;
                data.e_a_specialist_pax = item.e_a_specialist_pax;
                data.e_dt_end = item.e_dt_end;
                data.e_dt_start = item.e_dt_start;
                data.e_name = item.e_name;
                data.e_place = item.e_place;
                data.spr_id = item.spr_id;
                data.spr_no = item.spr_no;
                data.spr_status = item.spr_status;
                data.spr_status_desc = item.spr_status_desc;
                data.sp_date_realization = item.sp_date_realization;
                data.sp_type = item.sp_type;
                data.dr_plan_sum = item.dr_plan_sum;
                data.e_place = item.e_place;
                data.dr_real_sum = item.dr_real_sum;

                SPActualFinal.Add(data);

                //if (spr_id != item.spr_id)
                //{
                //    dr_plan_sum = dr_plan_sum + item.dr_plan_sum;
                //    budget_plan_sum = budget_plan_sum + item.budget_plan_sum;
                //    budget_real_sum = budget_real_sum + item.budget_real_sum;

                //    dr_plan_sum_all = dr_plan_sum_all + dr_plan_sum;
                //    budget_plan_sum_all = budget_plan_sum_all + budget_plan_sum;
                //    budget_real_sum_all = budget_real_sum_all + budget_real_sum;

                //    DataTableSPActualDTO dataSum = new DataTableSPActualDTO();
                //    dataSum.budget_plan_sum = budget_plan_sum;
                //    dataSum.budget_real_sum = budget_real_sum;
                //    dataSum.dr_plan_sum = dr_plan_sum;



                //    SPActualFinal.Add(dataSum);

                //    dr_plan_sum = 0;
                //    budget_plan_sum = 0;
                //    budget_real_sum = 0;
                //}
                //else
                //{
                //    dr_plan_sum = dr_plan_sum_all + item.dr_plan_sum;
                //    budget_plan_sum = budget_plan_sum + item.budget_plan_sum;
                //    budget_real_sum = budget_real_sum + item.budget_real_sum;
                //}

            }

            //DataTableSPActualDTO dataSumAll = new DataTableSPActualDTO();
            //dataSumAll.budget_plan_sum = budget_plan_sum_all;
            //dataSumAll.budget_real_sum = budget_real_sum_all;
            //dataSumAll.dr_plan_sum = dr_plan_sum_all;

            //SPActualFinal.Add(dataSumAll);

            return SPActualFinal;
        }
    }
}

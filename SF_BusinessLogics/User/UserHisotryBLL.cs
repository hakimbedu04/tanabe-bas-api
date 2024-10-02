using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_DAL.BAS;
using System.Data.Entity.SqlServer;

namespace SF_BusinessLogics.User
{
    public class UserHisotryBLL : IUserHistoryBLL
    {
        public List<DataTableUserHistoryDTO> getDataTable(string rep_id, int month, int year, string sortExpression, string sortOrder, string search, string searchColumn, string searchValue)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableUserHistoryDTO> userHistory = new List<DataTableUserHistoryDTO>();

            if (search.Equals(""))
            {
                userHistory = bas.v_sales_product.Where(x => x.rep_id == rep_id && x.sales_date_plan == month && x.sales_year_plan == year && x.sales_real_verification_status == 1)
                            .Select(x => new DataTableUserHistoryDTO { dr_code = x.dr_code, dr_monitoring = x.dr_monitoring, dr_name = x.dr_name, dr_quadrant = x.dr_quadrant, dr_spec = x.dr_spec, prd_category = "", prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sales_plan_verification_status = x.sales_plan_verification_status, sales_realization = x.sales_realization, sales_real_verification_status = x.sales_real_verification_status, sp_id = (float)x.sp_id, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                            .ToList();
            }else
            {
                userHistory = bas.v_sales_product.Where(x => x.rep_id == rep_id && x.sales_date_plan == month && x.sales_year_plan == year && x.sales_real_verification_status == 1)
                            .Select(x => new DataTableUserHistoryDTO { dr_code = x.dr_code, dr_monitoring = x.dr_monitoring, dr_name = x.dr_name, dr_quadrant = x.dr_quadrant, dr_spec = x.dr_spec, prd_category = "", prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sales_plan_verification_status = x.sales_plan_verification_status, sales_realization = x.sales_realization, sales_real_verification_status = x.sales_real_verification_status, sp_id = (float)x.sp_id, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                            .Where(x =>
                                SqlFunctions.StringConvert((double) x.dr_code.Value).Contains(search) ||
                                x.dr_monitoring.Contains(search) ||
                                x.dr_name.Contains(search) ||
                                x.dr_quadrant.Contains(search) ||
                                x.dr_spec.Contains(search) ||
                                x.prd_category.Contains(search) ||
                                x.prd_name.Contains(search) ||
                                SqlFunctions.StringConvert((double) x.prd_price.Value).Contains(search) ||
                                x.sales_id.Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sales_plan_verification_status.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sales_realization.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sales_real_verification_status.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sp_id).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sp_sales_qty.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sp_sales_value.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sp_target_qty.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double) x.sp_target_value.Value).Contains(search)
                            )
                            .ToList();
            }

            if (!String.IsNullOrEmpty(searchColumn))
            {
                string[] arrSearch = searchValue.Split(',');
                string[] arrColumn = searchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    try
                    {
                        userHistory = userHistory.Where(r => (r.GetType().GetProperty(arrColumn[i]).GetValue(r, null)+"").Contains(arrSearch[i])).ToList();
                    }
                    catch
                    {
                        
                    }
                }
            }

            if (sortOrder.ToLower().Equals("asc"))
            {
                userHistory = userHistory.OrderBy(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
            }
            else
            {
                userHistory = userHistory.OrderByDescending(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
            }
            return userHistory;
        }

        public List<DataTableUserHistoryDetailDTO> getDataTableDetail(Int64 sp_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableUserHistoryDetailDTO> userHistoryDetail = new List<DataTableUserHistoryDetailDTO>();

            userHistoryDetail = bas.t_sales_product_actual.Where(x => x.sp_id == sp_id)
                        .Select(x => new DataTableUserHistoryDetailDTO { date = x.spa_date, qty = x.spa_quantity, qtySum = 0 })
                        .ToList();
            Nullable<long> sum = userHistoryDetail.Select(x => x.qty).Sum();
            for(int i=0; i<userHistoryDetail.Count; i++)
            {
                userHistoryDetail[i].qtySum = sum;
            }

            return userHistoryDetail;
        }

        public DataTableUserHistorySUMDTO getDataTableSUM(string rep_id, int month, int year, string sortExpression, string sortOrder, string search, string searchColumn, string searchValue)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableUserHistoryDTO> userHistory = new List<DataTableUserHistoryDTO>();

            if (search.Equals(""))
            {
                userHistory = bas.v_sales_product.Where(x => x.rep_id == rep_id && x.sales_date_plan == month && x.sales_year_plan == year && x.sales_real_verification_status == 1)
                            .Select(x => new DataTableUserHistoryDTO { dr_code = x.dr_code, dr_monitoring = x.dr_monitoring, dr_name = x.dr_name, dr_quadrant = x.dr_quadrant, dr_spec = x.dr_spec, prd_category = "", prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sales_plan_verification_status = x.sales_plan_verification_status, sales_realization = x.sales_realization, sales_real_verification_status = x.sales_real_verification_status, sp_id = (float)x.sp_id, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                            .ToList();
            }
            else
            {
                userHistory = bas.v_sales_product.Where(x => x.rep_id == rep_id && x.sales_date_plan == month && x.sales_year_plan == year && x.sales_real_verification_status == 1)
                            .Select(x => new DataTableUserHistoryDTO { dr_code = x.dr_code, dr_monitoring = x.dr_monitoring, dr_name = x.dr_name, dr_quadrant = x.dr_quadrant, dr_spec = x.dr_spec, prd_category = "", prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sales_plan_verification_status = x.sales_plan_verification_status, sales_realization = x.sales_realization, sales_real_verification_status = x.sales_real_verification_status, sp_id = (float)x.sp_id, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                            .Where(x =>
                                SqlFunctions.StringConvert((double)x.dr_code.Value).Contains(search) ||
                                x.dr_monitoring.Contains(search) ||
                                x.dr_name.Contains(search) ||
                                x.dr_quadrant.Contains(search) ||
                                x.dr_spec.Contains(search) ||
                                x.prd_category.Contains(search) ||
                                x.prd_name.Contains(search) ||
                                SqlFunctions.StringConvert((double)x.prd_price.Value).Contains(search) ||
                                x.sales_id.Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sales_plan_verification_status.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sales_realization.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sales_real_verification_status.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sp_id).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sp_sales_qty.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sp_sales_value.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sp_target_qty.Value).Contains(search) ||
                                SqlFunctions.StringConvert((double)x.sp_target_value.Value).Contains(search)
                            )
                            .ToList();
            }

            if (!searchColumn.ToLower().Equals(""))
            {
                string[] arrSearch = searchValue.Split(',');
                string[] arrColumn = searchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    userHistory = userHistory.Where(r => r.GetType().GetProperty(arrColumn[i]).GetValue(r, null).ToString().Contains(arrSearch[i])).ToList();
            }

            DataTableUserHistorySUMDTO sum = new DataTableUserHistorySUMDTO();
            sum.sp_sales_qty = Math.Round(userHistory.Sum(x => x.sp_sales_qty).Value);
            sum.sp_sales_value = Math.Round(userHistory.Sum(x => x.sp_sales_value).Value);
            sum.sp_target_qty = userHistory.Sum(x => x.sp_target_qty).Value;
            sum.sp_target_value = Math.Round(userHistory.Sum(x => x.sp_target_value).Value);
            return sum;
        }
    }
}

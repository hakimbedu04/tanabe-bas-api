using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace SF_BusinessLogics.User
{
    public class UserPlanBLL : IUserPlanBLL
    {
        public List<DataTableUserPlanDTO> getDataTable(string rep_id, int month, int year, string sortExpression, string sortOrder, string searchColumn, string search)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableUserPlanDTO> userplans = new List<DataTableUserPlanDTO>();
           
            userplans = bas.v_sales_plan_2.Where(x => x.rep_id == rep_id && x.sales_date_plan == month && x.sales_year_plan == year && x.sales_plan_verification_status == 0)
                        .Select(x => new DataTableUserPlanDTO { dr_code = x.dr_code, sales_plan_verification_status = x.sales_plan_verification_status, dr_monitoring = x.dr_monitoring, dr_name = x.dr_name, dr_quadrant = x.dr_quadrant, dr_spec = x.dr_spec, prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sales_plan = x.sales_plan, sp_id = x.sp_id, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                        .ToList();

            if(userplans != null){
                if (!String.IsNullOrEmpty(searchColumn))
                {
                    string[] arrSearch = search.Split(',');
                    string[] arrColumn = searchColumn.Split(',');
                    for (int i = 0; i < arrColumn.Length; i++)
                        userplans = userplans.Where(r => r.GetType().GetProperty(arrColumn[i]).GetValue(r, null).ToString().Contains(arrSearch[i])).ToList();
                }

                if (sortOrder.ToLower().Equals("asc"))
                {
                    userplans = userplans.OrderBy(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
                }
                else
                {
                    userplans = userplans.OrderByDescending(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
                }
            }
            return userplans;
        }

        public int requests(string rep_id, int Month, int Year, string rep_name, string rep_reg, string rep_bo)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int count = bas.Database.SqlQuery<int>("SELECT COUNT(*) AS count FROM t_sales WITH(NOLOCK) "+
                "WHERE sales_plan = 0 "+
                "and ISNULL(sales_plan_verification_status, 0) = 0 "+
                "AND sales_date_plan = '"+Month+"' "+
                "and sales_year_plan = '" + Year + "' " +
                "AND rep_id = '" + rep_id + "'").FirstOrDefault();
            if (count > 0)
            {
                DateTime currentDate = DateTime.Now;
                int tb = bas.Database.SqlQuery<int>("EXEC SP_SELECT_TRANSACT_EMAIL ", new SqlParameter("@rep_id", rep_id), new SqlParameter("@month", Month), new SqlParameter("@year", Year), new SqlParameter("@transaction_id", "RVSP"), new SqlParameter("@date_sent", currentDate.ToString("yyyy-MM-dd"))).FirstOrDefault();

                return tb;
            }
            else
            {
                return 0;
            }
        }

        public List<DataTableUserPlanDetailDTO> getDataTableDetail(string rep_id, int Month, int Year, string sortExpression, string sortOrder, string sales_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableUserPlanDetailDTO> userplansdetail = new List<DataTableUserPlanDetailDTO>();

            userplansdetail = bas.v_sales_plan_2
                                .Where(x => x.rep_id == rep_id && x.sales_date_plan == Month && x.sales_year_plan == Year && x.sales_plan_verification_status == 0 && x.sales_id == sales_id)
                                .Select(x => new DataTableUserPlanDetailDTO { prd_name = x.prd_name, prd_price = x.prd_price, sales_id = x.sales_id, sp_id = x.sp_id, sp_note = x.sp_note, sp_sales_qty = x.sp_sales_qty, sp_sales_value = x.sp_sales_value, sp_target_qty = x.sp_target_qty, sp_target_value = x.sp_target_value })
                                .ToList();

            if (sortOrder.Trim().Equals("asc"))
            {
                userplansdetail = userplansdetail.OrderBy(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
            }
            else
            {
                userplansdetail = userplansdetail.OrderByDescending(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
            }

            return userplansdetail;
        }

        public List<ProductUserDTO> getListProduct()
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<ProductUserDTO> prodUser = new List<ProductUserDTO>();

            prodUser = bas.Database.SqlQuery<ProductUserDTO>("EXEC SP_SELECT_PRODUCT_USER").ToList();

            return prodUser;
        }

        public int addProduct(string sales_id, string rep_id, string prd_code, int tx_target_qty, string tx_note)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.SP_INSERT_SALES_PRODUCT(sales_id, prd_code, tx_target_qty, tx_note);
            //bas.Database.SqlQuery<int>("EXEC SP_INSERT_SALES_PRODUCT @sales_id, @prd_code, @sp_target_qty, @sp_note", new SqlParameter("@sales_id", sales_id), new SqlParameter("@prd_code", prd_code), new SqlParameter("@sp_target_qty", tx_target_qty), new SqlParameter("@sp_note", tx_note)).FirstOrDefault();
            //int tb = bas.SaveChanges();
            return result;
        }

        public void updateProduct(int sp_id, int tx_target_qty)
        {
            bas_trialEntities bas = new bas_trialEntities();
            //bas.SP_UPDATE_SALES_PRODUCT(sp_id, tx_target_qty, null, null);
            try
            {
                bas.Database.SqlQuery<int>("EXEC SP_UPDATE_SALES_PRODUCT @sp_id, @sp_target_qty, @sp_sp, @sp_percentage", new SqlParameter("@sp_id", sp_id), new SqlParameter("@sp_target_qty", tx_target_qty), new SqlParameter("@sp_sp", ""), new SqlParameter("@sp_percentage", "")).FirstOrDefault();
            }catch{}
        }

        public void deleteProduct(int sp_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.SP_DELETE_PRODUCT_SALES(sp_id);
        }
    }
}

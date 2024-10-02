using System.Linq.Expressions;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Repositories.Common;
using SF_Utils;

namespace SF_BusinessLogics.GeneralExpense
{
    public class GeneralExpense : IGeneralExpense
    {
        private readonly IBasGenericRepositories<t_expense_attachment> _tExpAttachment;
        private readonly IBasGenericRepositories<t_expense_detail> _tExpDetail;
        private readonly IBasGenericRepositories<t_expense_approval> _tExpApproval;

        public GeneralExpense(IBasGenericRepositories<t_expense_attachment> tExpAttachment, IBasGenericRepositories<t_expense_detail> tExpDetail
            , IBasGenericRepositories<t_expense_approval> tExpApproval)
        {
            _tExpAttachment = tExpAttachment;
            _tExpDetail = tExpDetail;
            _tExpApproval = tExpApproval;
        }
        public List<DataTableGeneralExpenseDTO> getDataTable(string rep_id, int month, int year, string sortExpression, string sortOrder, string searchColumn, string searchValue)
        {
            bas_trialEntities bas = new bas_trialEntities();
            List<DataTableGeneralExpenseDTO> generalExpense = new List<DataTableGeneralExpenseDTO>();

            generalExpense = bas.Database.SqlQuery<DataTableGeneralExpenseDTO>("EXEC SP_SELECT_EXPENSE_HEADER "+month+", "+year + ", " +rep_id).ToList();

            if (generalExpense != null)
            {
                if (!String.IsNullOrEmpty(searchColumn))
                {
                    string[] arrSearch = searchValue.Split(',');
                    string[] arrColumn = searchColumn.Split(',');
                    for (int i = 0; i < arrColumn.Length; i++)
                    {
                        generalExpense = generalExpense.Where(r => (r.GetType().GetProperty(arrColumn[i]).GetValue(r, null)+"").Contains(arrSearch[i])).ToList();
                    }
                }

                if (sortOrder.ToLower().Equals("asc"))
                {
                    generalExpense = generalExpense.OrderBy(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
                }
                else
                {
                    generalExpense = generalExpense.OrderByDescending(r => r.GetType().GetProperty(sortExpression).GetValue(r, null)).ToList();
                }
            }
            return generalExpense;
        }

        public List<DataTableGeneralExpenseDetailDTO> getDataTableDetail(string hrd_id)
        {
            //bas_trialEntities bas = new bas_trialEntities();
            //List<DataTableGeneralExpenseDetailDTO> generalExpense = new List<DataTableGeneralExpenseDetailDTO>();

            //generalExpense = bas.Database.SqlQuery<DataTableGeneralExpenseDetailDTO>("SELECT * FROM t_expense_detail WHERE hdr_id = '"+hrd_id+"' ORDER BY dtl_line_no ASC").ToList();

            //return generalExpense;
            Expression<Func<t_expense_detail, bool>> queryFilter = PredicateHelper.True<t_expense_detail>();
            if (!String.IsNullOrEmpty(hrd_id))
            {
                queryFilter = queryFilter.And(x => x.hdr_id == hrd_id);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { "dtl_line_no" }, "ASC");
            Func<IQueryable<t_expense_detail>, IOrderedQueryable<t_expense_detail>> orderByFilter =
                sortCriteria.GetOrderByFunc<t_expense_detail>();
            List<t_expense_detail> dbResult = _tExpDetail.Get(queryFilter, orderByFilter).ToList();
            var mapper = Mapper.Map<List<DataTableGeneralExpenseDetailDTO>>(dbResult);
            return mapper;
        }

        public List<DataTableGeneralExpenseDetailApproveDTO> getDataTableDetailApprove(string hrd_id)
        {
            //bas_trialEntities bas = new bas_trialEntities();
            //List<DataTableGeneralExpenseDetailApproveDTO> generalExpense = new List<DataTableGeneralExpenseDetailApproveDTO>();

            //generalExpense = bas.Database.SqlQuery<DataTableGeneralExpenseDetailApproveDTO>("SELECT * FROM t_expense_approval WHERE hdr_id = '"+hrd_id+"' ORDER BY ea_level ASC").ToList();

            //return generalExpense;
            Expression<Func<t_expense_approval, bool>> queryFilter = PredicateHelper.True<t_expense_approval>();
            if (!String.IsNullOrEmpty(hrd_id))
            {
                queryFilter = queryFilter.And(x => x.hdr_id == hrd_id);
            }
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { "ea_level" }, "ASC");
            Func<IQueryable<t_expense_approval>, IOrderedQueryable<t_expense_approval>> orderByFilter =
                sortCriteria.GetOrderByFunc<t_expense_approval>();
            List<t_expense_approval> dbResult = _tExpApproval.Get(queryFilter,orderByFilter).ToList();
            var mapper = Mapper.Map<List<DataTableGeneralExpenseDetailApproveDTO>>(dbResult);
            return mapper;
        }

        public List<DataTableGeneralExpenseDetailAttachDTO> getDataTableDetailAttach(string hrd_id)
        {
            //bas_trialEntities bas = new bas_trialEntities();
            //List<DataTableGeneralExpenseDetailAttachDTO> generalExpense = new List<DataTableGeneralExpenseDetailAttachDTO>();

            //generalExpense = bas.Database.SqlQuery<DataTableGeneralExpenseDetailAttachDTO>("SELECT * FROM t_expense_attachment WHERE hdr_id = '"+hrd_id+"'").ToList();

            //return generalExpense;

            Expression<Func<t_expense_attachment, bool>> queryFilter = PredicateHelper.True<t_expense_attachment>();
            if (!String.IsNullOrEmpty(hrd_id))
            {
                queryFilter = queryFilter.And(x => x.hdr_id == hrd_id);
            }

            var dbResult = _tExpAttachment.Get(queryFilter).ToList();
            var mapper = Mapper.Map<List<DataTableGeneralExpenseDetailAttachDTO>>(dbResult);
            return mapper;
        }

        public int deleteData(string hrd_id)
        {
            using (var context = new bas_trialEntities())
            {
                var commandText = "DELETE FROM t_expense WHERE hdr_id = '"+hrd_id+"'";
                //int result = context.Database.ExecuteSqlCommand(commandText, new SqlParameter("@hrd_id", hrd_id));
                int result = context.Database.ExecuteSqlCommand(commandText);

                return result;
            }

            //bas_trialEntities bas = new bas_trialEntities();
            //string query = "DELETE FROM t_expense WHERE hdr_id = '" + hrd_id + "'";
            //int generalExpense = bas.Database.ExecuteSqlCommand(query);
            //generalExpense = bas.SaveChanges();

            //return generalExpense;
        }

        public int add(string rep_id, string bo_description, string detailexpenselist, string detailexpenseattachmentlist)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("EXEC SP_INSERT_EXPENSE '" + rep_id + "', '" + bo_description + "', '" + detailexpenselist.ToString() + "', '" + detailexpenseattachmentlist.ToString()+"'");
            return result;
        }

        public int editDetailAdd(string repId, string hdrId, string dtl_desc, double value, string accDebit, string costCenter, string saf1)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("EXEC SP_INSERT_EXPENSE_DETAIL '" + repId + "', '" + hdrId + "', '" + dtl_desc + "', '" + value + "', '', '', ''");
            return result;
        }

        public int editDetailEdit(int dtl_id, string dtl_desc, double value, string accDebit, string costCenter, string saf1)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("EXEC SP_UPDATE_EXPENSE_DETAIL '" + dtl_id + "', '" + dtl_desc + "', '" + value + "', '', '', ''");
            return result;
        }

        public int editDetailDelete(int dtl_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("DELETE FROM t_expense_detail WHERE dtl_id = '"+dtl_id+"'");
            return result;
        }

        public int editDetailAttachmentDelete(int gla_id)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("Delete FROM t_expense_attachment WHERE gla_id = '" + gla_id + "'");
            return result;
        }

        public int editDetailAttachmentAdd(string hdrId, string fileName, string filePath)
        {
            bas_trialEntities bas = new bas_trialEntities();
            int result = bas.Database.ExecuteSqlCommand("EXEC SP_INSERT_EXPENSE_ATTACHMENT '"+ hdrId + "', '"+ fileName + "', '"+ filePath + "'");
            return result;
        }

        public int editDescription(string hdrId, string desc)
        {
            bas_trialEntities bas = new bas_trialEntities();
            //v_expense vEx = bas.v_expense.Where(x => x.hdr_id == hdrId).SingleOrDefault();
            //vEx.hdr_description = desc;
            //int result = bas.SaveChanges();
            int result = bas.Database.ExecuteSqlCommand("UPDATE v_expense SET hdr_description = '" + desc + "' WHERE hdr_id = '" + hdrId + "'");
            return result;
        }
    }
}

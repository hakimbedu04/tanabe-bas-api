using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;
using SF_Repositories.Common;
using SF_Utils;

namespace SF_BusinessLogics.User
{
    public class UserRealizationBLL : IUserRealizationBLL
    {

        private readonly IBasGenericRepositories<v_sales_product> _vSalesProductRepository;
        private readonly IBasGenericRepositories<t_sales_product_actual> _tSalesProductActualRepo;
        private ISqlSPRepository _spRepo;
        public UserRealizationBLL
        (
            IBasGenericRepositories<v_sales_product> vSalesProductRepository,
            IBasGenericRepositories<t_sales_product_actual> tSalesProductActualRepo,
            ISqlSPRepository spRepo
        )
        {
            _vSalesProductRepository = vSalesProductRepository;
            _spRepo = spRepo;
            _tSalesProductActualRepo = tSalesProductActualRepo;
        }

        public List<v_sales_product_DTO> GetUserRealizationDatas(UserInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_sales_product>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Month != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_date_plan.Value == inputs.Month);
            }
            if (inputs.Year != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_year_plan == inputs.Year);
            }
            queryFilter = queryFilter.And(x => x.sales_plan_verification_status == 1);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_sales_product>();
            var dbResult = _vSalesProductRepository.Get(queryFilter, orderByFilter).ToList();
            var viewMapper = Mapper.Map<List<v_sales_product_DTO>>(dbResult);
            if (!String.IsNullOrEmpty(inputs.SearchColumn))
            {
                string[] arrSearch = inputs.SearchValue.Split(',');
                string[] arrColumn = inputs.SearchColumn.Split(',');
                for (int i = 0; i < arrColumn.Length; i++)
                    viewMapper =
                        viewMapper.Where(
                            r =>
                                r.GetType()
                                    .GetProperty(arrColumn[i])
                                    .GetValue(r, null)
                                    .ToString().ToLower()
                                    .Contains(arrSearch[i].ToLower())).ToList();
            }

            return viewMapper;
            
        }

        public List<v_sales_product_DTO> GetUserRealizationDatas(UserExportCustomModelInputs inputs)
        {
            var queryFilter = PredicateHelper.True<v_sales_product>();
            if (!String.IsNullOrEmpty(inputs.RepId))
            {
                queryFilter = queryFilter.And(x => x.rep_id == inputs.RepId);
            }
            if (inputs.Month != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_date_plan.Value == inputs.Month);
            }
            if (inputs.Year != 0)
            {
                queryFilter = queryFilter.And(x => x.sales_year_plan == inputs.Year);
            }
            if (inputs.dr_code.HasValue)
            {
                queryFilter = queryFilter.And(x => x.dr_code == inputs.dr_code);
            }
            if (inputs.sales_realization.HasValue)
            {
                queryFilter = queryFilter.And(x => x.sales_realization == inputs.sales_realization);
            }
            if (!String.IsNullOrEmpty(inputs.dr_spec))
            {
                queryFilter = queryFilter.And(x => x.dr_spec == inputs.dr_spec);
            }
            if (!String.IsNullOrEmpty(inputs.dr_quadrant))
            {
                queryFilter = queryFilter.And(x => x.dr_quadrant == inputs.dr_quadrant);
            }
            if (!String.IsNullOrEmpty(inputs.dr_monitoring))
            {
                queryFilter = queryFilter.And(x => x.dr_monitoring == inputs.dr_monitoring);
            }
            if (!String.IsNullOrEmpty(inputs.prd_name))
            {
                queryFilter = queryFilter.And(x => x.prd_name == inputs.prd_name);
            }
            if (inputs.prd_price.HasValue)
            {
                queryFilter = queryFilter.And(x => x.prd_price == inputs.prd_price);
            }
            if (inputs.sp_target_qty.HasValue)
            {
                queryFilter = queryFilter.And(x => x.sp_target_qty == inputs.sp_target_qty);
            }
            if (inputs.sp_sales_qty.HasValue)
            {
                queryFilter = queryFilter.And(x => x.sp_sales_qty == inputs.sp_sales_qty);
            }
            if (inputs.sp_target_value.HasValue)
            {
                queryFilter = queryFilter.And(x => x.sp_target_value == inputs.sp_target_value);
            }
            if (inputs.sp_sales_value.HasValue)
            {
                queryFilter = queryFilter.And(x => x.sp_sales_value == inputs.sp_sales_value);
            }
            queryFilter = queryFilter.And(x => x.sales_plan_verification_status == 1);

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_sales_product>();
            var dbResult = _vSalesProductRepository.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_sales_product_DTO>>(dbResult);
        }

        public void InsertSalesProductActual(UserInputs inputs)
        {
            try
            {
                _spRepo.InsertSalesProductActual(inputs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<t_sales_product_actual_DTO> GetSalesProductActualDtos(UserInputs inputs)
        {
            var queryFilter = PredicateHelper.True<t_sales_product_actual>();
            if (inputs.SpId != 0)
            {
                queryFilter = queryFilter.And(x => x.sp_id == inputs.SpId);
            }

            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<t_sales_product_actual>();
            var dbResult = _tSalesProductActualRepo.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<t_sales_product_actual_DTO>>(dbResult);
        }

        public void UpdateSalesProductActual(UserInputs inputs)
        {
            try
            {
                _spRepo.UpdateSalesProductActual(inputs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSalesProductActual(UserInputs inputs)
        {
            try
            {
                _spRepo.DeleteSalesProductActual(inputs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

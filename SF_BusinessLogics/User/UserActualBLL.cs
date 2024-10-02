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
using SF_Repositories.VisitRepo;
using SF_Utils;

namespace SF_BusinessLogics.User
{
    public class UserActualBLL : IUserActualBLL
    {
        private readonly IBasGenericRepositories<v_sales_product> _vSalesProductRepository;
        private ISqlSPRepository _spRepo;
        private readonly IVisitRepo _visitRepo;

        public UserActualBLL(IBasGenericRepositories<v_sales_product> vSalesProductRepository, ISqlSPRepository spRepo, VisitRepo visitRepo)
        {
            _vSalesProductRepository = vSalesProductRepository;
            _spRepo = spRepo;
            _visitRepo = visitRepo;
        }
        public List<v_sales_product_DTO> GetUserActualDatas(UserInputs inputs)
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
            queryFilter = queryFilter.And(x => x.sales_realization == 1);
            var sortCriteria = new Tuple<IEnumerable<string>, string>(new[] { inputs.SortExpression }, inputs.SortOrder);
            var orderByFilter = sortCriteria.GetOrderByFunc<v_sales_product>();
            var dbResult = _vSalesProductRepository.Get(queryFilter, orderByFilter).ToList();

            return Mapper.Map<List<v_sales_product_DTO>>(dbResult);
        }

        public List<v_sales_product_DTO> GetUserActualSearch(UserInputs inputs)
        {
            return _spRepo.GetUserActualSearch(inputs);
        }

        public bool isHaveRemainingToSendMail(UserInputs inputs)
        {
            return _visitRepo.isHaveRemainingToSendMail(inputs);
        }
    }
}

using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SF_Domain.Inputs;
using SF_WebApi.Models;
using SF_Utils;
using AutoMapper;
using SF_Domain.DTOs.BAS;
using SF_BusinessLogics.ErrLogs;

namespace SF_WebApi.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardBLL _bll;
        private readonly ILoginBLL _loginbll;
        private IVTLogger _vtLogger;

        public DashboardController(IDashboardBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginbll = loginbll;
            _vtLogger = vtLogger;
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetTopPercentage(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);
                DateTime dt = DateTime.Now;
                if (inputs.Month == 0)
                    inputs.Month = dt.Month;
                if (inputs.Year == 0)
                    inputs.Year = dt.Year;

                var isValidLoginRole = _loginbll.GetPositionByRole(model.rep_position);
                var dbResult = _bll.getTopPercentage(inputs.Year, inputs.Month, id, model.rep_position, isValidLoginRole);
                var viewMapper = Mapper.Map<List<TopPercentageDTO>>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetTopRank(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);
                List<TopRankDTO> data = new List<TopRankDTO>();
                if (inputs.startDate.Equals("") || inputs.startDate == null)
                {
                    DateTime date = DateTime.Now;
                    data = _bll.getTopRank(date.Month.ToString(), date.Year.ToString());
                }
                else
                    data = _bll.getTopRankByDate(inputs.startDate, inputs.endDate);
                var viewMapper = Mapper.Map<List<TopRankDTO>>(data);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetTrendSales(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);

                if (inputs.startDate == "" || inputs.startDate == null)
                    inputs.startDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Date;
                var today = DateTime.Parse(inputs.startDate);
                var yesterday = today.AddMonths(inputs.posDate);
                List<string> monthsYear = new List<string>();
                string monthString = "";

                for (int x = 12; x >= 1; x += -1)
                {
                    yesterday = today.AddMonths(inputs.posDate - x);
                    string str = new EnumHelper().GetMonth(string.Format("{0:MM}", yesterday)) + "-" + string.Format("{0:yy}", yesterday);
                    monthsYear.Add(str);
                    monthString = monthString + str + "|";
                }
                monthString = monthString.Remove(monthString.Length - 1);
                var isValidLoginRole = _loginbll.GetPositionByRole(model.rep_position);
                var dbResult = _bll.getTrendSales(monthString, model.rep_position, model.rep_id, isValidLoginRole);
                var viewMapper = Mapper.Map<List<TrendSalesDTO>>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetAchievement(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);

                List<AchievemtnDTO> data = new List<AchievemtnDTO>();
                var isValidLoginRole = _loginbll.GetPositionByRole(model.rep_position);
                if (inputs.startDate.Equals("") || inputs.startDate == null)
                {
                    DateTime date = DateTime.Now;
                    inputs.startDate = date.Year + "-" + date.Month + "-1";
                    inputs.endDate = date.Year + "-" + date.Month + "-" + DateTime.DaysInMonth(date.Year, date.Month);
                    data = _bll.getAchievement(inputs.startDate, inputs.endDate, model.rep_position, model.rep_id, isValidLoginRole);
                }
                else
                    data = _bll.getAchievement(inputs.startDate, inputs.endDate, model.rep_position, model.rep_id, isValidLoginRole);
                var viewMapper = Mapper.Map<List<AchievemtnDTO>>(data);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetSWD(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);

                if (inputs.startDate == "" || inputs.startDate == null)
                    inputs.startDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Date;
                var today = DateTime.Parse(inputs.startDate);
                var yesterday = today.AddMonths(0);
                int posDt = 0;
                posDt = posDt - inputs.posDate;
                var isValidLoginRole = _loginbll.GetPositionByRole(model.rep_position);
                var dbResult = _bll.getSWD(today, posDt, model.rep_position, model.rep_id, isValidLoginRole);
                var viewMapper = Mapper.Map<List<SalesWDDTO>>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetCallDocter(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(inputs.Auth, true);
                var model = _loginbll.CheckMvaUserInfo(id);

                List<CallDoctorDTO> data = new List<CallDoctorDTO>();
                var isValidLoginRole = _loginbll.GetPositionByRole(model.rep_position);
                if (inputs.startDate.Equals("") || inputs.startDate == null)
                {
                    DateTime date = DateTime.Now;
                    inputs.startDate = date.Year + "-" + date.Month + "-1";
                    inputs.endDate = date.Year + "-" + date.Month + "-" + DateTime.DaysInMonth(date.Year, date.Month);
                    data = _bll.getCallDoctor(inputs.startDate, inputs.endDate, model.rep_position, model.rep_id, isValidLoginRole);
                }
                else
                    data = _bll.getCallDoctor(inputs.startDate, inputs.endDate, model.rep_position, model.rep_id, isValidLoginRole);
                var viewMapper = Mapper.Map<List<CallDoctorDTO>>(data);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }
    }
}

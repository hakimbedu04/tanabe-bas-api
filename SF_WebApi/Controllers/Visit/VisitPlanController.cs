using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.Visit;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.Visit;
using SF_Utils;
using SF_WebApi.Models;
using SF_WebApi.Models.BAS;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.Visit
{
    public class VisitPlanController : BaseController
    {
        private readonly IVisitBLL _bll;
        private readonly ILoginBLL _loginbll;
        private readonly IVTLogger _vtLogger;

        public VisitPlanController(IVisitBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginbll = loginbll;
            _vtLogger = vtLogger;
        }

        #region view visit plan

        [HttpPost]
        public HttpResponseMessage DataViewPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage DataViewPartialCustomCallback(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                int paramday = 0;
                int parammonth = 0;
                int paramyear = 0;
                bool state = false;

                if (inputs.Prm == "retrieve" || inputs.Prm == "filter")
                {
                    paramday = (inputs.Prm == "retrieve") ? 0 : inputs.Day;
                    parammonth = inputs.Month;
                    paramyear = inputs.Year;
                    state = true;
                }
                else if (inputs.Prm == "addvisit")
                {
                    //ccek disini
                    string visitDatePlan;
                    if (inputs.VisitDateTime.HasValue)
                    {
                        visitDatePlan = inputs.VisitDateTime.Value.ToString("yyyy-MM-dd");
                        List<VisitModel> drCollection = inputs.Collections;
                        string insertVisit = _bll.InsertVisitPlan(id, visitDatePlan, drCollection, model.rep_position, model.rep_region);
                        if (insertVisit == EnumHelper.GetDescription(Enums.ResponseType.Success))
                        {
                            state = true;
                            paramday = 0;
                            parammonth = inputs.VisitDateTime.Value.Month;
                            paramyear = inputs.VisitDateTime.Value.Year;
                        }
                        objResponseModel.Message = insertVisit;
                    }
                }
                else if (inputs.Prm == "request")
                {
                    int month = inputs.Month;
                    DateTime currentDate = DateTime.Now;
                    int currMonth = currentDate.Month;
                    int currYear = currentDate.Year;
                    int currDayRequest = currentDate.Day;
                    int currMonthRequest = currMonth;

                    if (currMonthRequest == 0)
                    {
                        currMonthRequest = currMonth;
                    }

                    if (!_bll.isAnyDoctorUnverificatedRealInPrevMonth(currMonthRequest - 1, id))
                    {
                        if (_bll.isAnyDoctorUnplaned(currMonthRequest, id))
                        {
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.DoctorUnplanedVisit);
                            state = false;
                        }
                        else if (_bll.isAnyVisitUnplanedProduct(currMonthRequest, id))
                        {
                            objResponseModel.Message = //"param request - month" + currMonthRequest + "; id" + id;
                                EnumHelper.GetDescription(Enums.ResponseType.DoctorUnplanedProduct);
                            state = false;
                        }
                        else if (_bll.isAnyDoctorUnplanedSales(id, currMonth, currYear))
                        {
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.DoctorUnplanedSales);
                            state = false;
                        }
                        else
                        {
                            if (!_bll.isAnyDayLessThenMinimumDoctor(model.rep_position, id))
                            {
                                if (_bll.isHaveRemainingToSendMail("RVP", id))
                                {
                                    SetReport(id, currDayRequest, currMonthRequest, currYear, inputs.Auth);
                                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessSend);
                                    state = true;
                                }
                                else
                                {
                                    objResponseModel.Message =
                                        EnumHelper.GetDescription(Enums.ResponseType.SendLimitation);
                                    state = false;
                                }
                            }
                            else
                            {
                                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.LessDoctor);
                                state = false;
                            }
                        }
                    }
                    else
                    {
                        objResponseModel.Message =
                            EnumHelper.GetDescription(Enums.ResponseType.PrevMonthUnverificatedReal);
                        state = false;
                    }
                }
                else
                {
                    DateTime currentDate = DateTime.Now;
                    paramday = 0;
                    parammonth = currentDate.Month;
                    paramyear = currentDate.Year;
                }

                var newinputs = new VisitInputs
                {
                    RepId = id,
                    Day = paramday,
                    Month = parammonth,
                    Year = paramyear
                };

                if (!state)
                {
                    List<SP_SelectVisitPlanDTO> dbResult1 = _bll.GetVisitPlan(newinputs);
                    var viewMapper1 = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult1);
                    objResponseModel.Status = false;
                    if (viewMapper1.Count != 0)
                    {
                        objResponseModel.TotalRecords = viewMapper1.Count;
                        objResponseModel.TotalPages = (viewMapper1.Count/inputs.PageSize) +
                                                      (viewMapper1.Count%inputs.PageSize != 0 ? 1 : 0);
                        objResponseModel.Result =
                            viewMapper1.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                    }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(newinputs);
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public void SetReport(string id, int day, int month, int year, string auth)
        {
            bas_v_rep_fullDTO userDetail = _loginbll.CheckMvaUserInfo(id);
            string monthName = (new DateTime(year, month, day)).ToString("MMMM", CultureInfo.InvariantCulture);
            var inputs = new VisitInputs
            {
                RepId = id,
                Day = day,
                Month = month,
                Year = year,
                Auth = auth
            };
            string originPath = GeneratePdf(inputs);
            string pdfFile = HttpContext.Current.Server.MapPath(@"~" + originPath.Replace(GetHostNoHttp(), ""));
            var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_plan.htm"));
            string readFile = reader.ReadToEnd();
            string email_body = "";
            email_body = readFile;
            DateTime currentDate = DateTime.Now;
            email_body = email_body.Replace("$$RECEIVER$$", userDetail.nama_am);
            email_body = email_body.Replace("$$rep_name$$", userDetail.rep_name);
            email_body = email_body.Replace("$$rep_region$$", userDetail.rep_region);
            email_body = email_body.Replace("$$bo$$", userDetail.rep_bo);
            email_body = email_body.Replace("$$sbo$$", userDetail.rep_sbo);
            email_body = email_body.Replace("$$rep_id$$", userDetail.rep_id);
            email_body = email_body.Replace("$$month_plan$$", monthName);
            email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();
            var settingsReader = new AppSettingsReader();
            var key = (string) settingsReader.GetValue("fromEmailAddress", typeof (String));
            // Get the key from config file
            mailMessage.From = new MailAddress(key);
            mailMessage.To.Add(new MailAddress(userDetail.email_am));
            ////mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
            ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));

            mailMessage.Attachments.Add(new Attachment(pdfFile));
            mailMessage.Subject = "Request Verification for " + userDetail.rep_name + " - " + "Schedule Visit Plan - " +
                                  monthName;

            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = email_body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            reader.Dispose();
            _bll.SaveReportPlan(id);
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage DeletePlan(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                if (!String.IsNullOrEmpty(inputs.VisitId))
                {
                    _bll.DeleteVisitPlan(inputs.VisitId, model.rep_position);
                }

                List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage DeleteProduct(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                string id = Decrypt(inputs.Auth, true);
                if (inputs.SpId != 0)
                {
                    _bll.DeleteSalesProduct(inputs.SpId);
                }
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDeleteProduct);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg.Contains("sp_already_plan")
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                string id = Decrypt(inputs.Auth, true);
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        #endregion

        #region Doctor List View

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage GridDoctorPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                string id = Decrypt(inputs.Auth, true);
                List<v_visit_plan_mobileDTO> dbResult = _bll.GetDoctorListPerDay(id, inputs.VisitDateTime,inputs.SearchColumn,inputs.SearchValue);
                var viewMapper = Mapper.Map<List<v_visit_plan_mobileViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage GridDoctorCustomCallbackPartialResult(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                string paramId = Decrypt(inputs.Auth, true);
                DateTime param2 = inputs.VisitDateTime.HasValue ? inputs.VisitDateTime.Value : DateTime.Now;
                List<v_visit_plan_mobileDTO> dbResult = _bll.GetDoctorListPerDay(paramId, param2,inputs.SearchColumn,inputs.SearchValue);
                var viewMapper = Mapper.Map<List<v_visit_plan_mobileViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage GridLookupPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<SP_SELECT_DOCTOR_LIST_NEW_DTO> dbResult = _bll.GetDoctorList(id, model.rep_position,inputs.SearchColumn,inputs.SearchValue);
                var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_NEW_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                //if (viewMapper.Count != 0)
                //{
                    objResponseModel.TotalRecords = viewMapper.Count;
                    //objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                    //                              (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper;
                //}
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        #endregion

        #region User Visit Product

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage dsProductLookup(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            try
            {
                List<SP_SELECT_PRODUCT_USER_DTO> dbResult = _bll.dsProductLookup();
                var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_USER_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage GridUserProductPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                //if (inputs.Prm == "reset")
                //{
                string param1 = Decrypt(inputs.Auth, true);
                int param2 = DateTime.Now.Month;
                int param3 = DateTime.Now.Year;
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(param1, null, param2,
                    param3);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage GridUserCustomCallbackProductPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                string param1 = Decrypt(inputs.Auth, true);
                int paramMonth = inputs.VisitDateTime.HasValue ? inputs.VisitDateTime.Value.Month : 0;
                int paramYear = inputs.VisitDateTime.HasValue ? inputs.VisitDateTime.Value.Year : 0;
                if (inputs.Prm == "copy")
                {
                    string param2 = inputs.VisitId;
                    if (inputs.VisitDateTime != null)
                    {
                        bool validate1 = _bll.isAnyPlannedSalesInSelectedMonth(param1, paramMonth, paramYear);
                        if (validate1)
                        {
                            bool validate2 = _bll.isAnyPlannedSalesInCurrMonth(param1, param2);
                            if (!validate2)
                            {
                                bool validate3 = _bll.CopySalesProductPlan(param1, param2, paramMonth, paramYear);
                                if (validate3)
                                {
                                    objResponseModel.Status = true;
                                    objResponseModel.Message = "Success copy";
                                    return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
                                }
                                objResponseModel.Status = false;
                                objResponseModel.Message = "Copy error. Please try again later";
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                            }
                            objResponseModel.Status = false;
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UserAlreadyPlanned);
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                        }
                        objResponseModel.Status = false;
                        objResponseModel.Message = "Null planned";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                    }
                }


                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(param1, inputs.VisitId,
                    paramMonth, paramYear);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddProduct(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            string id = Decrypt(inputs.Auth, true);
            if (inputs.Month == 0)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullDatePlan);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }

            if (string.IsNullOrEmpty(inputs.VisitId))
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullDoctorPlan);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }

            try
            {
                bool dbResultInsert = _bll.InsertSalesProduct(id, inputs.VisitId, inputs.Month, inputs.Year,
                    inputs.PrdCode, inputs.Qty, inputs.Note, inputs.Sp, inputs.Percentage);
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg.Contains("sp_already_plan"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SpAlreadyPlan);
                }
                else if (msg.Contains("percent_not_match"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.PercentNotMatch);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                }
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }


        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage AddProductVisit(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            string id = Decrypt(inputs.Auth, true);
            if (!String.IsNullOrEmpty(inputs.VisitId))
            {
                objResponseModel.Message =
                    EnumHelper.GetDescription(Enums.ResponseType.NullDoctorPlanAddVisitProductFlag);
            }
            try
            {
                _bll.InsertVisitProduct(id, inputs.VisitId, inputs.VisitCode, inputs.Sp, inputs.Percentage);
                List<v_visit_product_DTO> dbResult = _bll.GetVisitProduct(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg.Contains("sp_already_plan"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SpAlreadyPlan);
                }
                else if (msg.Contains("percent_not_match"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.PercentNotMatch);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                }
                List<v_visit_product_DTO> dbResult = _bll.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        #endregion

        #region SP

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage gridSPView(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = _bll.GetSpPlan(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage gridSPViewCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = (inputs.Prm == "retrievelistsp")
                    ? _bll.GetSpPlan(inputs.VisitId)
                    : _bll.GetSpPlan(null);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage AddSP(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            string id = Decrypt(inputs.Auth, true);
            try
            {
                _bll.InsertVisitSP(inputs.VisitId, inputs.EventName, inputs.BAllocation, inputs.BAmount);
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = _bll.GetSpPlan(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg.Contains("null_product"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullProduct);
                }
                else if (msg.Contains("exists_sp"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ExistsSp);
                }
                else if (msg.Contains("product_incomplete"))
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ProductIncompleteSp);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                }
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = _bll.GetSpPlan(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage DeleteSP(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                _bll.DeleteVisitSP(inputs.SpdsId);
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = _bll.GetSpPlan(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg == "null_product")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullProduct);
                }
                else if (msg == "exists_sp")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ExistsSp);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                }
                List<SP_SELECT_VISIT_SP_PLAN_DTO> dbResult = _bll.GetSpPlan(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        #endregion

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage DeleteProductVisit(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                _bll.DeleteVisitProduct(inputs.VdId);
                List<v_visit_product_DTO> dbResult = _bll.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "sp_already_plan"
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                List<v_visit_product_DTO> dbResult = _bll.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (VisitInputs))]
        public HttpResponseMessage LoadOnDemandPartial(VisitInputs inputs)
            // flag untuk data dokter yang sudah ada pada pewarnaan calendar
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                string id = Decrypt(inputs.Auth, true);
                List<FullVisitDateDTO> dbResult = _bll.getFullVisitDate(id);
                var viewMapper = Mapper.Map<List<FullVisitDateViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                //if (viewMapper.Count != 0)
                //{
                //    objResponseModel.TotalRecords = viewMapper.Count;
                //    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                //                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                //    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                //}
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage SummaryDoctor(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UnauthorizeUser);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SummaryDoctor_DTO> dbResult = _bll.GetQuadrantSummaryPlan(inputs);
                var viewMapper = Mapper.Map<List<SummaryDoctor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UnauthorizeUser);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                objResponseModel.Result = GenerateExcel(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public string GenerateExcel(VisitInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(inputs);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
            string addressPath = headerPath + "VisitPlan" + "/" + model.rep_id;
            string resultFileName = "visit_plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day +
                                    "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
            string resultFilePath = addressPath + "/" + resultFileName;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
            }
            var ms = new MemoryStream();

            string strTempFileName = Path.GetTempFileName();
            string strFileName = strTempFileName.Replace(".tmp", string.Empty) + ".xlsx";
            if (File.Exists(strFileName))
            {
                File.Delete(strFileName);
            }

            string templateFile = Enums.ExcelTemplate.Visit_Plan + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.VisitPlan + templateFile);
            if (File.Exists(templateFileName))
            {
                File.Copy(templateFileName, strFileName);
            }

            string tempField = "";
            using (var slDoc = new SLDocument(strFileName))
            {
                #region content

                //start row values
                int iRow = 2;

                foreach (SP_SelectVisitPlanDTO item in dbResult)
                {
                    #region sub header

                    SLStyle styleSubHeader = slDoc.CreateStyle();
                    styleSubHeader.Font.FontName = "Calibri";
                    styleSubHeader.Font.FontSize = 11;
                    styleSubHeader.Font.Bold = true;

                    if (tempField != item.dr_name)
                    {
                        slDoc.SetCellValue(iRow, 1,
                            "DATE PLAN : " +
                            (item.visit_date_plan.HasValue ? item.visit_date_plan.Value.ToString("yyyy MMMM dd") : " "));
                        tempField = item.visit_date_plan.HasValue
                            ? item.visit_date_plan.Value.ToString("yyyy MMMM dd")
                            : " ";
                        slDoc.SetCellStyle(iRow, 1, iRow, 1, styleSubHeader);
                        iRow++;
                    }

                    #endregion

                    SLStyle style = slDoc.CreateStyle();
                    style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Font.FontName = "Calibri";
                    style.Font.FontSize = 11;

                    slDoc.SetCellValue(iRow, 1, item.visit_id);
                    slDoc.SetCellValue(iRow, 2,
                        item.visit_date_plan.HasValue ? item.visit_date_plan.Value.ToString("yyyy MMMM dd") : " ");
                    slDoc.SetCellValue(iRow, 3, item.visit_plan.HasValue ? item.visit_plan.Value : 0);
                    slDoc.SetCellValue(iRow, 4, item.visit_realization.HasValue ? item.visit_realization.Value : 0);
                    slDoc.SetCellValue(iRow, 5, item.visit_plan_verification_status);
                    slDoc.SetCellValue(iRow, 6, item.dr_code.HasValue ? item.dr_code.Value.ToString() : "");
                    slDoc.SetCellValue(iRow, 7, item.dr_name);
                    slDoc.SetCellValue(iRow, 8, item.dr_spec);
                    slDoc.SetCellValue(iRow, 9, item.dr_sub_spec);
                    slDoc.SetCellValue(iRow, 10, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 11, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 12, item.dr_dk_lk);
                    slDoc.SetCellValue(iRow, 13, item.dr_area_mis);
                    slDoc.SetCellValue(iRow, 14, item.dr_category);
                    slDoc.SetCellValue(iRow, 15, item.dr_chanel);
                    slDoc.SetCellStyle(iRow, 1, iRow, 14, style);
                    iRow++;
                }

                var slSheetProtection = new SLSheetProtection
                {
                    AllowInsertRows = false,
                    AllowInsertColumns = false,
                    AllowDeleteRows = false,
                    AllowDeleteColumns = false,
                    AllowFormatCells = true,
                    AllowFormatColumns = true,
                    AllowFormatRows = true,
                    AllowAutoFilter = true,
                    AllowSort = true
                };

                slDoc.ProtectWorksheet(slSheetProtection);
                slDoc.AutoFitColumn(1, 13);

                #endregion

                //System.IO.File.Delete(strFileName);
                slDoc.SaveAs(ms);
            }
            // this is important. Otherwise you get an empty file
            // (because you'd be at EOF after the stream is written to, I think...).
            ms.Position = 0;

            FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(resultFilePath));
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            ms.Close();
            return GetHostNoHttp() + resultFilePath.Substring(1);
        }

        //public string GenerateExcels(VisitInputs inputs)
        //{
        //    inputs.RepId = Decrypt(inputs.Auth, true);
        //    var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
        //    var dbResult = _bll.GetVisitPlan(inputs);
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("PLAN ID", typeof(string));
        //    dt.Columns.Add("DATE PLAN", typeof(string));
        //    dt.Columns.Add("VPLAN", typeof(string));
        //    dt.Columns.Add("VREAL", typeof(string));
        //    dt.Columns.Add("PLAN Ver.STATUS", typeof(string));
        //    dt.Columns.Add("DR CODE", typeof(string));
        //    dt.Columns.Add("DR NAME", typeof(string));
        //    dt.Columns.Add("DR SPEC", typeof(string));

        //    dt.Columns.Add("SUB SPEC", typeof(string));
        //    dt.Columns.Add("QRD", typeof(string));
        //    dt.Columns.Add("MTG", typeof(string));
        //    dt.Columns.Add("DK/LK", typeof(string));
        //    dt.Columns.Add("AREA MIS", typeof(string));
        //    dt.Columns.Add("CATEGORY", typeof(string));
        //    dt.Columns.Add("CHANNEL", typeof(string));
        //    foreach (SP_SelectVisitPlanDTO item in dbResult)
        //    {
        //        dynamic dr = dt.NewRow();
        //        dr["PLAN ID"] = item.visit_id;
        //        dr["DATE PLAN"] = item.visit_date_plan.HasValue? item.visit_date_plan.Value.ToString("yyyy MMMM dd"): " ";
        //        dr["VPLAN"] = item.visit_plan;
        //        dr["VREAL"] = item.visit_realization;
        //        dr["PLAN Ver.STATUS"] = item.visit_plan_verification_status;
        //        dr["DR CODE"] = item.dr_code.HasValue ? item.dr_code.Value.ToString() : "";
        //        dr["DR NAME"] = item.dr_name;
        //        dr["DR SPEC"] = item.dr_spec;

        //        dr["SUB SPEC"] = item.dr_sub_spec;
        //        dr["QRD"] = item.dr_quadrant;
        //        dr["MTG"] = item.dr_monitoring;
        //        dr["DK/LK"] = item.dr_dk_lk;
        //        dr["AREA MIS"] = item.dr_area_mis;
        //        dr["CATEGORY"] = item.dr_category;
        //        dr["CHANNEL"] = item.dr_chanel;
        //        dt.Rows.Add(dr);
        //    }
        //    var currentDate = DateTime.Now;
        //    inputs.Day = currentDate.Day;
        //    inputs.Month = currentDate.Month;
        //    inputs.Year = currentDate.Year;
        //    var dataTableToExcel = new DataTableToExcel();

        //    #region upload file
        //    var settingsReader = new AppSettingsReader();
        //    var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
        //    var addressPath = headerPath + "VisitPlan" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
        //    var resultFileName = "visit_Plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
        //    var resultFilePath = addressPath + "/" + resultFileName;

        //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
        //    {
        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
        //    }
        //    dataTableToExcel.generateExcels(dt, HttpContext.Current.Server.MapPath(resultFilePath));

        //    #endregion
        //    return GetHostNoHttp() + resultFilePath.Substring(1);
        //}

        [HttpPost]
        public HttpResponseMessage ExportToPdf(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UnauthorizeUser);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                objResponseModel.Result = GeneratePdf(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public string GeneratePdf(VisitInputs inputs)
        {
            #region initialitation

            string returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(inputs);
            var newinputs = new VisitInputs
            {
                RepId = inputs.RepId,
                Month = inputs.Month,
                Year = inputs.Year
            };
            List<SummaryDoctor_DTO> detailDoctor = _bll.GetQuadrantSummaryPlan(newinputs);
            string fonttype = BaseFont.TIMES_ROMAN;
            Font tableHeaderFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.WHITE);
            Font tableContentFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.BLACK);
            Font TitleFont = FontFactory.GetFont(fonttype, 11, Font.NORMAL, BaseColor.BLACK);
            DateTime currentDate = DateTime.Now;
            var document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

            #endregion

            #region upload file

            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GeneratePdfPath", typeof (String));
            //~/Asset/Files/Downloads/Pdf/
            string addressPath = headerPath + "VisitPlan" + "/" + model.rep_id;
            // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "visit_plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
                                    currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
            string resultFilePath = addressPath + "/" + resultFileName;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            var output = new FileStream(HttpContext.Current.Server.MapPath(resultFilePath), FileMode.Create);
            PdfWriter.GetInstance(document, output);
            returnVal = GetHostNoHttp() + resultFilePath.Substring(1);

            #endregion

            document.Open();

            #region file content

            var title = new Paragraph();
            title.Add(new Paragraph("PT. TANABE INDONESIA ", TitleFont));
            title.Add(new Paragraph("SHCEDULE VISIT PLAN ", TitleFont));
            title.IndentationLeft = 10;

            var header = new Paragraph();
            header.Add(new Paragraph("Nama       : " + model.rep_name, TitleFont));
            header.Add(new Paragraph("Regional   : " + model.rep_region, TitleFont));
            header.Add(new Paragraph("BO         : " + model.rep_bo, TitleFont));
            header.Add(new Paragraph("Month      : ____________________________", TitleFont));
            header.IndentationLeft = 10;

            document.Add(title);
            document.Add(header);

            #region table

            var dt = new DataTable();
            dt.Columns.Add("PLAN ID", typeof (string));
            dt.Columns.Add("DATE PLAN", typeof (string));
            dt.Columns.Add("VPLAN", typeof (string));
            dt.Columns.Add("VREAL", typeof (string));
            dt.Columns.Add("PLAN Ver.STATUS", typeof (string));
            dt.Columns.Add("DR CODE", typeof (string));
            dt.Columns.Add("DR NAME", typeof (string));
            dt.Columns.Add("DR SPEC", typeof (string));

            dt.Columns.Add("SUB SPEC", typeof (string));
            dt.Columns.Add("QRD", typeof (string));
            dt.Columns.Add("MTG", typeof (string));
            dt.Columns.Add("DK/LK", typeof (string));
            dt.Columns.Add("AREA MIS", typeof (string));
            dt.Columns.Add("CATEGORY", typeof (string));
            dt.Columns.Add("CHANNEL", typeof (string));
            foreach (SP_SelectVisitPlanDTO item in dbResult)
            {
                dynamic dr = dt.NewRow();
                dr["PLAN ID"] = item.visit_id;
                dr["DATE PLAN"] = item.visit_date_plan.HasValue
                    ? item.visit_date_plan.Value.ToString("dd/MM/yyyy")
                    : " ";
                dr["VPLAN"] = item.visit_plan;
                dr["VREAL"] = item.visit_realization;
                dr["PLAN Ver.STATUS"] = item.visit_plan_verification_status;
                dr["DR CODE"] = item.dr_code.HasValue ? item.dr_code.Value.ToString() : "";
                dr["DR NAME"] = item.dr_name;
                dr["DR SPEC"] = item.dr_spec;

                dr["SUB SPEC"] = item.dr_sub_spec;
                dr["QRD"] = item.dr_quadrant;
                dr["MTG"] = item.dr_monitoring;
                dr["DK/LK"] = item.dr_dk_lk;
                dr["AREA MIS"] = item.dr_area_mis;
                dr["CATEGORY"] = item.dr_category;
                dr["CHANNEL"] = item.dr_chanel;
                dt.Rows.Add(dr);
            }

            //Adding line break  
            //document.Add(new Chunk("\n", TitleFont));  

            //Adding  PdfPTable  
            var table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = {5f, 5f, 3f, 3f, 7f, 4f, 12f, 4f, 6f, 3f, 15f, 5f, 5f, 5f, 5f};
            table.SetWidths(widths);

            var cell = new PdfPCell();

            cell.PaddingBottom = 5;
            cell.Border = 2;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#999999"));
                string cellText = HttpContext.Current.Server.HtmlDecode(dt.Columns[i].ColumnName);
                cell.Phrase = new Phrase(cellText, tableHeaderFont);
                cell.BorderWidthTop = .2f;
                cell.BorderWidthBottom = .2f;
                cell.BorderWidthLeft = .2f;
                cell.BorderWidthRight = .2f;
                cell.BorderColorTop = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorBottom = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorLeft = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorRight = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //bfbfbf
                    cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#ffffff"));
                    string rowText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][j].ToString());
                    cell.Phrase = new Phrase(rowText, tableContentFont);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
            }
            document.Add(new Chunk("\n", TitleFont));
            document.Add(table);
            document.Add(new Chunk("\n", TitleFont));

            #endregion

            //document.Add(new Chunk("\n", TitleFont));  

            var footer = new Paragraph();
            footer.SpacingBefore = 5;
            footer.Add(new Paragraph("Total Doctor : ", TitleFont));
            footer.Add(
                new Paragraph(
                    "Total Planned Doctor : " +
                    (detailDoctor[0].DrTotal + detailDoctor[1].DrTotal + detailDoctor[2].DrTotal), TitleFont));
            for (int i = 0; i < detailDoctor.Count; i++)
            {
                footer.Add(new Paragraph("Total Planned Doctor Q" + (i + 1) + " : " + detailDoctor[i].DrTotal, TitleFont));
            }
            footer.IndentationLeft = 10;
            document.Add(footer);

            #endregion

            document.Close();

            return returnVal;
        }

        #region Update

        [HttpPost]
        public HttpResponseMessage UpdatePlan(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                if (!String.IsNullOrEmpty(inputs.VisitId) && !String.IsNullOrEmpty(inputs.DrCode))
                {
                    var coll = new UpdatePlanModel
                    {
                        DrCode = inputs.DrCode,
                        OldDrCode = inputs.OldDrCode,
                        VisitDatePlan = inputs.VisitDateTime.Value,
                        VisitId = inputs.VisitId
                    };
                    if (inputs.VisitDateTime != null)
                        _bll.UpdatePlan(coll, model.rep_position);
                    inputs.Day = 0;
                    inputs.Month = inputs.VisitDateTime.Value.Month;
                    inputs.Year = inputs.VisitDateTime.Value.Year;
                    List<SP_SelectVisitPlanDTO> dbResult = _bll.GetVisitPlan(inputs);
                    var viewMapper = Mapper.Map<List<SP_SelectVisitPlanViewModel>>(dbResult);
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdatePlan);
                    if (viewMapper.Count != 0)
                    {
                        objResponseModel.TotalRecords = viewMapper.Count;
                        objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                      (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                        objResponseModel.Result =
                            viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
                }

                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = EnumHelper.GetDescription(Enums.ResponseType.NullInputs);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateProduct(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }
                string id = Decrypt(inputs.Auth, true);
                _bll.UpdateSalesProduct(inputs.SpId, inputs.Qty, inputs.Sp, inputs.Percentage, id,
                    Convert.ToInt32(inputs.DrCode), inputs.VisitDatePlan);

                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdateProduct);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "sp_already_plan"
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                string id = Decrypt(inputs.Auth, true);
                List<SP_SELECT_VISIT_USER_PRODUCT_DTO> dbResult = _bll.GetUserProductPartial(id, inputs.VisitId,
                    inputs.Month, inputs.Year);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_USER_PRODUCT_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        //[System.Web.Http.HttpPost]
        //[ResponseType(typeof(VisitInputs))]
        //public HttpResponseMessage UpdateProductVisit(VisitInputs inputs)
        //{
        //    var objResponseModel = new ResponseModel();
        //    try
        //    {
        //        var validateAccess = ValidateAuth(inputs.Auth);
        //        if (!validateAccess)
        //        {
        //            objResponseModel.Status = false;
        //            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //            return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //        }
        //        _bll.UpdateVisitProduct(inputs.VdId, inputs.VisitCode, inputs.Sp, inputs.Percentage);

        //        var dbResult = _bll.GetVisitProduct(inputs.VisitId);
        //        var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
        //        objResponseModel.Status = true;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
        //        if (viewMapper.Count != 0)
        //        {
        //            objResponseModel.TotalRecords = viewMapper.Count;
        //            objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
        //                                          (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
        //            objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
        //        }

        //        _vtLogger.Err(ex, new List<object> {inputs});  return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //        objResponseModel.Message = EnumHelper.GetDescription(msg == "sp_already_plan" ? Enums.ResponseType.SpAlreadyPlan : Enums.ResponseType.QueryError);
        //        var dbResult = _bll.GetVisitProduct(inputs.VisitId);
        //        var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
        //        objResponseModel.Status = false;
        //        objResponseModel.DetailMessage = ex.Message;
        //        objResponseModel.Result = viewMapper;
        //        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //    }
        //}

        //[System.Web.Http.HttpPost]
        //[ResponseType(typeof(VisitInputs))]
        //public HttpResponseMessage UpdateSP(VisitInputs inputs)
        //{
        //    var objResponseModel = new ResponseModel();
        //    try
        //    {
        //        var validateAccess = ValidateAuth(inputs.Auth);
        //        if (!validateAccess)
        //        {
        //            objResponseModel.Status = false;
        //            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //            return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //        }
        //        _bll.UpdateSalesProduct(inputs.SpId, inputs.Qty, inputs.Sp, inputs.Percentage);

        //        var dbResult = _bll.GetSpPlan(inputs.VisitId);
        //        var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_PLAN_ViewModel>>(dbResult);
        //        objResponseModel.Status = true;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
        //        if (viewMapper.Count != 0)
        //        {
        //            objResponseModel.TotalRecords = viewMapper.Count;
        //            objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
        //                                          (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
        //            objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
        //        }

        //        _vtLogger.Err(ex, new List<object> {inputs});  return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        objResponseModel.Status = false;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //        objResponseModel.DetailMessage = ex.Message;
        //        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //    }
        //}

        #endregion

        #region Visit Product Tab

        [HttpPost]
        public HttpResponseMessage cbProductVisitLookup(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
            }
            try
            {
                List<SP_SELECT_PRODUCT_VISIT_DTO> dbResult = _bll.GetDataProductListPlan();
                var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_VISIT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GridVisitProductPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                List<v_visit_product_DTO> dbResult = _bll.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GridVisitProductCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
                }

                List<v_visit_product_DTO> dbResult =
                    _bll.GetVisitProduct(inputs.Prm == "retrieveVisitProduct" ? inputs.VisitId : "");
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        #endregion
    }
}
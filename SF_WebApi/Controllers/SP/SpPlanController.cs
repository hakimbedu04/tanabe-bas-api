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
using System.Xml.Linq;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.SP;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.SP;
using SF_Utils;
using SF_WebApi.Models;
using SF_WebApi.Models.BAS;
using SF_WebApi.Models.BAS.SP;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.SP
{
    public class SpPlanController : BaseController
    {
        private readonly ILoginBLL _loginbll;
        private readonly ISPPlanBLL _mainBLL;
        private readonly IVTLogger _vtLogger;

        public SpPlanController(ILoginBLL loginbll, ISPPlanBLL mainBLL, IVTLogger vtLogger)
        {
            _loginbll = loginbll;
            _mainBLL = mainBLL;
            _vtLogger = vtLogger;
        }

        #region index

        [HttpPost]
        public HttpResponseMessage DataViewPartial(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
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
        public HttpResponseMessage BudgetRemainingPartialView(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                if (String.IsNullOrEmpty(inputs.SpType) || String.IsNullOrEmpty(inputs.BAllocation))
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = "Invalid Inputs";
                    objResponseModel.Result = "";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                
                if (inputs.SpType == "SP2" && inputs.BAllocation.Contains("OWN"))
                {
                    inputs.RepId = Decrypt(inputs.Auth, true);
                    var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                    var dbResult = _mainBLL.GetOwnBudgetRemaining(inputs);
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    objResponseModel.Result = model.rep_region.Replace(" ", String.Empty) + ":" + dbResult;
                    objResponseModel.TotalRecords = 0;
                    objResponseModel.TotalPages = 0;
                    return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
                }
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = "0";
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage DoctorSummary(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
                var res = viewMapper.Select(x => new
                {
                    x.dr_plan_sum,
                    x.budget_plan_sum
                }).ToList();

                var data = new SpSummaryDoctor
                {
                    TotalDoctor = (int) res.Sum(y => y.dr_plan_sum),
                    Exp = (int) res.Sum(y => y.budget_plan_sum)
                };

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = data;
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
        public HttpResponseMessage DetailViewProgress(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<t_sp_approval_DTO> dbResult = _mainBLL.GetSPApproval(inputs);
                var viewMapper = Mapper.Map<List<t_sp_approval_ViewModel>>(dbResult);
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

        #endregion

        #region Edit Form

        #region | Tab Product

        [HttpPost]
        public HttpResponseMessage DropdownProductResult(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_PRODUCT_SP_PLAN_MOBILE_DTO> dbResult = _mainBLL.GetProductDropdown();
                var viewMapper = Mapper.Map<List<SP_PRODUCT_SP_PLAN_MOBILE_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage DetailViewProductSP(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<v_sp_product_DTO> dbResult = _mainBLL.GetSPProduct(inputs);
                var viewMapper = Mapper.Map<List<v_sp_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage AddProductSP(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                if (!String.IsNullOrEmpty(inputs.SpProduct))
                {
                    _mainBLL.AddDetailProduct(inputs.SprId, inputs.SpProduct);
                }
                List<v_sp_product_DTO> dbResult = _mainBLL.GetSPProduct(inputs);
                var viewMapper = Mapper.Map<List<v_sp_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage DeleteProductSP(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                _mainBLL.DeleteProduct(inputs.SppId);
                List<v_sp_product_DTO> dbResult = _mainBLL.GetSPProduct(inputs);
                var viewMapper = Mapper.Map<List<v_sp_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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

        #endregion

        #region | Tab Participant

        [HttpPost]
        public HttpResponseMessage DetailViewParticipantEdit(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<v_doctor_sponsor_DTO> dbResult = _mainBLL.GetSPParticipant(inputs);
                var viewMapper = Mapper.Map<List<v_doctor_sponsor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage GetDoctor(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                inputs.RepPosition = _loginbll.CheckMvaUserInfo(inputs.RepId).rep_position;
                //List<SP_SELECT_DOCTOR_LIST_SPEAKER_DTO> dbResult = _mainBLL.GetDoctor(inputs);
                //var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_SPEAKER_ViewModel>>(dbResult);
                var dbResult = _mainBLL.GetDoctorListFiltered(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_SP_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage GetSponsor(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                List<m_sponsor_DTO> dbResult = _mainBLL.GetSponsor(inputs);
                var viewMapper = Mapper.Map<List<m_sponsor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage AddParticipantDetail(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                _mainBLL.AddDetailParticipant(inputs);
                List<v_doctor_sponsor_DTO> dbResult = _mainBLL.GetSPParticipant(inputs);
                var viewMapper = Mapper.Map<List<v_doctor_sponsor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage UpdateParticipantDetail(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                _mainBLL.UpdateDetailSpeaker(inputs);
                List<v_doctor_sponsor_DTO> dbResult = _mainBLL.GetSPParticipant(inputs);
                var viewMapper = Mapper.Map<List<v_doctor_sponsor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage DeleteParticipantDetail(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                _mainBLL.DeleteSpeaker(inputs);
                List<v_doctor_sponsor_DTO> dbResult = _mainBLL.GetSPParticipant(inputs);
                var viewMapper = Mapper.Map<List<v_doctor_sponsor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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

        #endregion

        #region | Tab Event

        [HttpPost]
        public HttpResponseMessage SaveEditEvent(SPInputs inputs)
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
                try
                {
                    bool updateEvent = _mainBLL.UpdateEventDetail(inputs);
                    if (updateEvent)
                    {
                        objResponseModel.Status = true;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdate);
                        if (inputs.SprStatus == 9)
                        {
                            SendEmailAuthorizerGivenRevision(inputs);
                        }
                    }
                    else
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorUpdate);
                    }
                }
                catch (Exception ex)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorAdd);
                    objResponseModel.DetailMessage = ex.Message;
                    _vtLogger.Err(ex, new List<object> {inputs});
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }


                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
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

        public void SendEmailAuthorizerGivenRevision(SPInputs inputs)
        {
            bas_v_rep_fullDTO userDetail = _loginbll.CheckMvaUserInfo(inputs.RepId);
            var reader =
                new StreamReader(HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_sp_plan.htm"));
            string readFile = reader.ReadToEnd();
            string email_body;
            DateTime currentDate = DateTime.Now;
            List<v_spr_DTO> model = _mainBLL.GetSPRInfo(inputs.SprId);
            List<t_sp_approval_DTO> modelFunc = _mainBLL.GetSPApproval(inputs);
            string monthName = "";
            if (model[0].e_dt_start.HasValue)
                monthName =
                    (new DateTime(model[0].e_dt_start.Value.Year, model[0].e_dt_start.Value.Month,
                        model[0].e_dt_start.Value.Day)).ToString("MMMM", CultureInfo.InvariantCulture);
            email_body = readFile;
            email_body = email_body.Replace("$$RECEIVER$$", modelFunc[0].spa_functionary);
            email_body = email_body.Replace("$$sp$$", model[0].e_name);
            email_body = email_body.Replace("$$spr_id$$", inputs.SprId);
            email_body = email_body.Replace("$$dt_event$$",
                model[0].e_dt_start.HasValue ? model[0].e_dt_start.Value.ToString("yyyy MMMM dd") : "");
            email_body = email_body.Replace("$$rep_name$$", userDetail.rep_name);
            email_body = email_body.Replace("$$rep_region$$", userDetail.rep_region);
            email_body = email_body.Replace("$$bo$$", userDetail.rep_bo);
            email_body = email_body.Replace("$$sbo$$", userDetail.rep_sbo);
            email_body = email_body.Replace("$$rep_id$$", inputs.RepId);
            email_body = email_body.Replace("$$month_plan$$", monthName);
            email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();
            var settingsReader = new AppSettingsReader();
            var key = (string) settingsReader.GetValue("fromEmailAddress", typeof (String));
            // Get the key from config file
            mailMessage.From = new MailAddress(key);
            //mailMessage.To.Add(new MailAddress(modelFunc[0].spa_email)); 
            ////mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
            ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));

            mailMessage.Subject = "Revised New Sales Promotion Request with Req Number : " + inputs.SprId;

            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = email_body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            reader.Dispose();
        }

        [HttpPost]
        public HttpResponseMessage SaveEditBa(SPInputs inputs)
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
                _mainBLL.UpdateBudgetAllocationOnPlan(inputs);
                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdateBa);
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

        #endregion

        #region | tab approval

        [HttpPost]
        public HttpResponseMessage ViewApprovalRealization(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);

                List<t_sp_approval_DTO> dbResult = _mainBLL.GetSPApproval(inputs);
                var viewMapper = Mapper.Map<List<t_sp_approval_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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

        #endregion

        #endregion

        #region Add Form

        [HttpPost]
        public HttpResponseMessage ListBoxPartial(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                int? roleID = _loginbll.CheckMvaUserRole(inputs.RepId).role_id;
                if (roleID != null)
                    inputs.RoleId = roleID.Value;
                List<v_auth_sp_DTO> dbResult = _mainBLL.GetSPList(inputs);
                var viewMapper = Mapper.Map<List<v_auth_sp_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage LoadRealProductMultiple(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                List<SP_PRODUCT_VISIT_MOBILE_DTO> dbResult = _mainBLL.GetDataProductSPList();
                var viewMapper = Mapper.Map<List<SP_PRODUCT_VISIT_MOBILE_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage EventNamePartialView(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                List<m_event_DTO> dbResult = _mainBLL.GetEventNameList(inputs);
                var viewMapper = Mapper.Map<List<m_event_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage cbProductTopicLookup(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                List<m_topic_DTO> dbResult = _mainBLL.GetProductTopicList();
                var viewMapper = Mapper.Map<List<m_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = viewMapper.Count;
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
        public HttpResponseMessage RepDetailPartialView(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                bas_v_rep_fullDTO dbResult = _loginbll.CheckMvaUserInfo(Decrypt(inputs.Auth, true));
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = 1;
                objResponseModel.Result = dbResult;

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
        //[HttpPost]
        //public HttpResponseMessage AddNewPlan(SPInputs inputs)
        //{
        //    var objResponseModel = new ResponseModel();
        //    try
        //    {
        //        bool validateAccess = ValidateAuth(inputs.Auth);
        //        if (!validateAccess)
        //        {
        //            objResponseModel.Status = false;
        //            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponseModel);
        //        }

        //        objResponseModel.Status = false;
        //        objResponseModel.Message = "Input SP Plan silahkan melalui web site terlebih dahulu!";
        //        objResponseModel.Result = "";
        //        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponseModel.Status = false;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //        objResponseModel.DetailMessage = ex.Message;
        //        _vtLogger.Err(ex, new List<object> { inputs });
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
        //    }
        //}
        [HttpPost]
        public HttpResponseMessage AddNewPlan(SPInputs inputs)
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
                if (inputs.SpType == "SP1")
                {
                    try
                    {
                        #region validasi products

                        if (!CheckTotalPercentage(inputs.ListProducts))
                        {
                            objResponseModel.Status = false;
                            objResponseModel.Message = "The total percentage you specified is not even 100 percent";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, objResponseModel);
                        }

                        #endregion

                        inputs.ProductXmlDoc = GenerateListProductToXML(inputs.ListProducts);
                        inputs.SprId = _mainBLL.GetSprId();
                        _mainBLL.InsertSP1(inputs);
                        SendRequest(inputs);
                        objResponseModel.Status = true;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessAdd);
                    }
                    catch (Exception ex)
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorAdd);
                        objResponseModel.DetailMessage = ex.Message;
                        _vtLogger.Err(ex, new List<object> { inputs });
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                    }
                }

                if (inputs.SpType == "SP2")
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(inputs.Products))
                        {
                            objResponseModel.Status = false;
                            objResponseModel.Message = "Product cannot be null value";
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                        }
                        if (inputs.BAllocation == "PMD")
                            inputs.ProductXmlDoc = GenerateListProductToXML(inputs.ListProducts);
                        inputs.SprId = _mainBLL.GetSprId();
                        _mainBLL.InsertSP2(inputs);
                        SendRequest(inputs);
                        objResponseModel.Status = true;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessAdd);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.InnerException.Message;
                        objResponseModel.Status = false;
                        if (msg.Contains("exists_sp"))
                        {
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ExistsSp);
                        }
                        else if (msg.Contains("error_add"))
                        {
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorAdd);    
                        }
                        objResponseModel.DetailMessage = msg;
                        _vtLogger.Err(ex, new List<object> { inputs });
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                    }
                }

                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public bool CheckTotalPercentage(List<TempProduct> listProducts)
        {
            List<TempProduct> newlist = listProducts.GroupBy(x => x.ProductBudget).Select(x => new TempProduct
            {
                ProductBudget = x.Key,
                ProductBudgetAllocation = x.Max(y => Convert.ToInt32(y.ProductBudgetAllocation))
            }).ToList();
            decimal totalAllocation = newlist.Sum(x => x.ProductBudgetAllocation);
            return totalAllocation == 100;
        }

        public void SendRequest(SPInputs inputs)
        {
            bas_v_rep_fullDTO userDetail = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<t_sp_approval_DTO> model = _mainBLL.GetSPApproval(inputs);
            var viewMapper = Mapper.Map<List<t_sp_approval_ViewModel>>(model);
            List<t_sp_approval_ViewModel> receiverData = viewMapper.Where(x => x.spa_action == "AUTHORIZER").ToList();
            if (!receiverData.Any())
            {
                receiverData = viewMapper.Where(x => x.spa_action == "APPROVER").ToList();
            }

            var reader =
                new StreamReader(HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_sp_plan.htm"));
            string readFile = reader.ReadToEnd();
            string email_body = "";
            email_body = readFile;
            DateTime currentDate = DateTime.Now;
            string monthName =
                (new DateTime(inputs.DateStart.Year, inputs.DateStart.Month, inputs.DateStart.Day)).ToString("MMMM",
                    CultureInfo.InvariantCulture);
            foreach (t_sp_approval_ViewModel receiver in receiverData)
            {
                email_body = email_body.Replace("$$RECEIVER$$", receiver.spa_functionary);
                email_body = email_body.Replace("$$spr_id$$", inputs.SprId);
                email_body = email_body.Replace("$$sp$$", inputs.EName);
                email_body = email_body.Replace("$$spr_id$$", inputs.SprId);
                email_body = email_body.Replace("$$dt_event$$", inputs.DateStart.ToString("yyyy MMMM dd"));
                email_body = email_body.Replace("$$rep_name$$", userDetail.rep_name);
                email_body = email_body.Replace("$$rep_region$$", userDetail.rep_region);
                email_body = email_body.Replace("$$bo$$", userDetail.rep_bo);
                email_body = email_body.Replace("$$sbo$$", userDetail.rep_sbo);
                email_body = email_body.Replace("$$rep_id$$", inputs.RepId);
                email_body = email_body.Replace("$$month_plan$$", monthName);
                email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

                var mailMessage = new MailMessage();
                var smtpClient = new SmtpClient();
                var settingsReader = new AppSettingsReader();
                var key = (string) settingsReader.GetValue("fromEmailAddress", typeof (String));
                // Get the key from config file
                mailMessage.From = new MailAddress(key);
                mailMessage.To.Add(new MailAddress(receiver.spa_email));
                //mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
                //mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));

                mailMessage.Subject = "Sales Promotion Approval Request for " + userDetail.rep_name + " - " +
                                      "Month Event Plan - " + monthName;

                mailMessage.Priority = MailPriority.High;
                mailMessage.Body = email_body;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                reader.Dispose();
            }
        }

        private XDocument GenerateListProductToXML(List<TempProduct> list)
        {
            var xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(
                    "ListDetail", from cls in list
                        select new XElement("List",
                            new XElement("visit_code", cls.VisitCode),
                            new XElement("product_budget", cls.ProductBudget),
                            new XElement("allocation", cls.ProductBudgetAllocation))
                    )
                );
            return xmlDocument;
        }

        [HttpPost]
        public HttpResponseMessage DeletePlan(SPInputs inputs)
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
                try
                {
                    _mainBLL.DeleteSPPlan(inputs);
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDelete);
                }
                catch (Exception ex)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorAdd);
                    objResponseModel.DetailMessage = ex.Message;
                    _vtLogger.Err(ex, new List<object> {inputs});
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
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

        #endregion

        [HttpPost]
        public HttpResponseMessage ExportToPdf(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UnauthorizeUser);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
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
                objResponseModel.DetailMessage = ex.InnerException.ToString();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public string GeneratePdf(SPInputs inputs)
        {
            #region initialitation

            string returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
            var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
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
            string addressPath = headerPath + "SpPlan" + "/" + model.rep_id;
            // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "sp_plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + currentDate.Day +
                                    "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
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

            string monthName = (new DateTime(inputs.Year, inputs.Month, currentDate.Day)).ToString("MMMM",
                CultureInfo.InvariantCulture);
            var title = new Paragraph();
            title.Add(new Paragraph("PT. TANABE INDONESIA ", TitleFont));
            title.Add(new Paragraph("SALES PROMOTION PLAN ", TitleFont));
            title.IndentationLeft = 10;

            var header = new Paragraph();
            header.Add(new Paragraph("Nama       : " + model.rep_name, TitleFont));
            header.Add(new Paragraph("Regional   : " + model.rep_region, TitleFont));
            header.Add(new Paragraph("BO         : " + model.rep_bo, TitleFont));
            header.Add(new Paragraph("Month      : " + monthName, TitleFont));
            header.IndentationLeft = 10;

            document.Add(title);
            document.Add(header);

            #region table

            var dt = new DataTable();
            dt.Columns.Add("REQ_ID", typeof (string));
            dt.Columns.Add("SPR NO", typeof (string));
            dt.Columns.Add("SP", typeof (string));
            dt.Columns.Add("EVENT NAME", typeof (string));
            dt.Columns.Add("TOPIC", typeof (string));
            dt.Columns.Add("PLACE", typeof (string));
            dt.Columns.Add("GP", typeof (string));
            dt.Columns.Add("GP Pax", typeof (string));
            dt.Columns.Add("Spec", typeof (string));
            dt.Columns.Add("Spec Pax", typeof (string));
            dt.Columns.Add("Nurse", typeof (string));
            dt.Columns.Add("Nurse Pax", typeof (string));
            dt.Columns.Add("Others", typeof (string));
            dt.Columns.Add("Others Pax", typeof (string));
            dt.Columns.Add("DATE START", typeof (string));
            dt.Columns.Add("DATE END", typeof (string));
            dt.Columns.Add("PARTICIPANT PLAN", typeof (string));
            dt.Columns.Add("BUDGET PLAN", typeof (string));
            dt.Columns.Add("STATUS", typeof (string));

            string tempField = "";
            foreach (SP_SELECT_SP_PLAN_ViewModel item in viewMapper)
            {
                dynamic dr = dt.NewRow();
                if (tempField != item.spr_id)
                {
                    dr["REQ_ID"] = "";
                    dr["SPR NO"] = item.spr_id;
                    dr["SP"] = "";
                    dr["EVENT NAME"] = "";
                    dr["TOPIC"] = "";
                    dr["PLACE"] = "";
                    dr["GP"] = "";
                    dr["GP Pax"] = "";
                    dr["Spec"] = "";
                    dr["Spec Pax"] = "";
                    dr["Nurse"] = "";
                    dr["Nurse Pax"] = "";
                    dr["Others"] = "";
                    dr["Others Pax"] = "";
                    dr["DATE START"] = "";
                    dr["DATE END"] = "";
                    dr["PARTICIPANT PLAN"] = "";
                    dr["BUDGET PLAN"] = "";
                    dr["STATUS"] = "";
                    dt.Rows.Add(dr);
                    tempField = item.spr_id;
                }

                dr = dt.NewRow();
                dr["REQ_ID"] = item.spr_id;
                dr["SPR NO"] = item.spr_no;
                dr["SP"] = item.sp_type;
                dr["EVENT NAME"] = item.e_name;
                dr["TOPIC"] = item.e_topic;
                dr["PLACE"] = item.e_place;
                dr["GP"] = item.e_a_gp == 0 ? "Unchecked" : "Checked";
                dr["GP Pax"] = item.e_a_gp_pax.HasValue ? item.e_a_gp_pax.Value : 0;
                dr["Spec"] = item.e_a_specialist == 0 ? "Unchecked" : "Checked";
                dr["Spec Pax"] = item.e_a_specialist_pax.HasValue ? item.e_a_specialist_pax.Value : 0;
                dr["Nurse"] = item.e_a_nurse == 0 ? "Unchecked" : "Checked";
                dr["Nurse Pax"] = item.e_a_nurse_pax.HasValue ? item.e_a_nurse_pax.Value : 0;
                dr["Others"] = item.e_a_others == 0 ? "Unchecked" : "Checked";
                dr["Others Pax"] = item.e_a_others_pax.HasValue ? item.e_a_others_pax.Value : 0;
                dr["DATE START"] = item.e_dt_start.HasValue ? item.e_dt_start.Value.ToString("MM/dd/yyyy") : "";
                dr["DATE END"] = item.e_dt_end.HasValue ? item.e_dt_end.Value.ToString("MM/dd/yyyy") : "";
                dr["PARTICIPANT PLAN"] = item.dr_plan_sum;
                dr["BUDGET PLAN"] = item.budget_plan_sum;
                dr["STATUS"] = item.spr_status_desc;
                dt.Rows.Add(dr);
            }

            //Adding  PdfPTable  
            var table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = {5f, 5f, 3f, 6f, 4f, 5f, 5f, 3f, 4f, 4f, 4.5f, 3.5f, 4f, 4f, 5f, 5f, 4f, 4f, 8f};
            table.SetWidths(widths);

            var cell = new PdfPCell();
            cell.PaddingBottom = 5;
            cell.Border = 2;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#666666"));
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
                cell.FixedHeight = 14f;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == "")
                {
                    cell.Colspan = dt.Columns.Count;
                    cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#cccccc"));
                    string cellText = HttpContext.Current.Server.HtmlDecode("REQ_ID: " + dt.Rows[i][1]);
                    cell.Phrase = new Phrase(cellText, tableContentFont);
                    cell.BorderColorTop = new BaseColor(ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorBottom = new BaseColor(ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorLeft = new BaseColor(ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorRight = new BaseColor(ColorTranslator.FromHtml("#999999"));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
                else
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //bfbfbf
                        cell.Colspan = 0;
                        cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#ffffff"));
                        string rowText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][j].ToString());
                        cell.Phrase = new Phrase(rowText, tableContentFont);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);
                    }
                }
            }
            document.Add(new Chunk("\n", TitleFont));
            document.Add(table);

            #endregion

            document.Add(new Chunk("\n", TitleFont));


            //Paragraph footer = new Paragraph();
            //footer.SpacingBefore = 5;
            //footer.Add(new Paragraph("Total Doctor : ", TitleFont));
            //footer.Add(new Paragraph("Total Planned Doctor : " + (detailDoctor[0].DrTotal + detailDoctor[1].DrTotal + detailDoctor[2].DrTotal), TitleFont));
            //for (var i = 0; i < detailDoctor.Count; i++)
            //{
            //    footer.Add(new Paragraph("Total Planned Doctor Q" + (i + 1) + " : " + detailDoctor[i].DrTotal, TitleFont));
            //}
            //footer.IndentationLeft = 10;
            //document.Add(footer);

            #endregion

            document.Close();

            return returnVal;
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(SPInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.UnauthorizeUser);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
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
                objResponseModel.DetailMessage = ex.InnerException.ToString();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public string GenerateExcel(SPInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<SP_SELECT_SP_PLAN_DTO> dbResult = _mainBLL.GetSpPlan(inputs);
            var viewMapper = Mapper.Map<List<SP_SELECT_SP_PLAN_ViewModel>>(dbResult);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
            // folder asset sampai excel
            string addressPath = headerPath + "SpPlan" + "/" + model.rep_id; // folder asset sampai rep_id
            string resultFileName = "Sp_Plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day +
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

            string templateFile = Enums.ExcelTemplate.SP_Plan + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.SPPlan + templateFile);
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

                foreach (SP_SELECT_SP_PLAN_ViewModel item in viewMapper)
                {
                    #region sub header

                    SLStyle styleSubHeader = slDoc.CreateStyle();
                    styleSubHeader.Font.FontName = "Calibri";
                    styleSubHeader.Font.FontSize = 11;
                    styleSubHeader.Font.Bold = true;

                    if (tempField != item.spr_id)
                    {
                        slDoc.SetCellValue(iRow, 1, "REQ_ID : " + item.spr_id);
                        tempField = item.spr_id;
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

                    slDoc.SetCellValue(iRow, 1, item.spr_id);
                    slDoc.SetCellValue(iRow, 2, item.spr_no);
                    slDoc.SetCellValue(iRow, 3, item.sp_type);
                    slDoc.SetCellValue(iRow, 4, item.e_name);
                    slDoc.SetCellValue(iRow, 5, item.e_topic);
                    slDoc.SetCellValue(iRow, 6, item.e_place);
                    slDoc.SetCellValue(iRow, 7, item.e_a_gp == 0 ? "Unchecked" : "Checked");
                    slDoc.SetCellValue(iRow, 8, item.e_a_gp_pax.HasValue ? item.e_a_gp_pax.Value : 0);
                    slDoc.SetCellValue(iRow, 9, item.e_a_specialist == 0 ? "Unchecked" : "Checked");
                    slDoc.SetCellValue(iRow, 10, item.e_a_specialist_pax.HasValue ? item.e_a_specialist_pax.Value : 0);
                    slDoc.SetCellValue(iRow, 11, item.e_a_nurse == 0 ? "Unchecked" : "Checked");
                    slDoc.SetCellValue(iRow, 12, item.e_a_nurse_pax.HasValue ? item.e_a_nurse_pax.Value : 0);
                    slDoc.SetCellValue(iRow, 13, item.e_a_others == 0 ? "Unchecked" : "Checked");
                    slDoc.SetCellValue(iRow, 14, item.e_a_others_pax.HasValue ? item.e_a_others_pax.Value : 0);
                    slDoc.SetCellValue(iRow, 15,
                        item.e_dt_start.HasValue ? item.e_dt_start.Value.ToString("MM/dd/yyyy") : "");
                    slDoc.SetCellValue(iRow, 16,
                        item.e_dt_end.HasValue ? item.e_dt_end.Value.ToString("MM/dd/yyyy") : "");
                    slDoc.SetCellValue(iRow, 17, item.dr_plan_sum.Value);
                    slDoc.SetCellValue(iRow, 18, item.budget_plan_sum.Value);
                    slDoc.SetCellValue(iRow, 19, item.spr_status_desc);
                    slDoc.SetCellStyle(iRow, 1, iRow, 18, style);
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
                slDoc.AutoFitColumn(1, 19);

                #endregion

                File.Delete(strFileName);
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
    }
}
using System;
using System.Data.Entity.Core.Common.EntitySql;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using SF_BusinessLogics.LoginBLL;
using SF_Domain.Inputs.Visit;
using SF_WebApi.Models;
using SF_WebApi.Models.BAS;
using SF_WebApi.Models.InputModels;
using WebGrease.Css.ImageAssemblyAnalysis.LogModel;
using System.Web.Http.Description;
using SF_Domain.Inputs;
using SF_Utils;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using SF_BusinessLogics.ErrLogs;
using SF_Domain.DTOs.BAS;
using System.Web;
using System.Configuration;
using System.IO;

namespace SF_WebApi.Controllers.Login
{
    public class LoginController : BaseController
    {
        private ILoginBLL _bll;
        private IVTLogger _vtLogger;

        public LoginController(ILoginBLL bll, IVTLogger vtLogger)
        {
            _bll = bll;
            _vtLogger = vtLogger;
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult LoginResult(LoginInputs inputs)
        {
            var objReturn = new LoginResponseModel();
            var logLogin = new LogLoginInput();
            try
            {
                var enPwd = EncryptPassword(inputs.Password);
                var dbResult = CheckUserLogin(inputs.Username, enPwd);

                

                if (dbResult != null)
                {
                    if (!dbResult.IsValidPosition)
                    {
                        objReturn.Status = false;
                        objReturn.Message = "user not authorize";
                        objReturn.Auth = null;
                        return Content(HttpStatusCode.BadRequest, objReturn); 
                    }
                    var auth = Encrypt(dbResult.Rep_Id, true);
                    var dbResponse = _bll.InsertAuth(dbResult.Rep_Id, auth);
                    if (dbResponse != 0)
                    {
                        objReturn.Status = true;
                        objReturn.Message = "Success";
                        objReturn.Auth = auth;
                        objReturn.Result = dbResult;
                        logLogin.status = objReturn.Message;

                        logLogin.rep_id = dbResult.Rep_Id;
                        logLogin.hostname = inputs.hostname;
                        logLogin.ip_addressv4 = inputs.ip_addressv4;
                        //logLogin.ip_addressv6 = inputs.ip_addressv6;
                        logLogin.latitude = inputs.latitude;
                        logLogin.longitude = inputs.longitude;
                        logLogin.address = inputs.address;
                        logLogin.source = "IPAD";
                        logLogin.log_date = DateTime.Now;
                        _bll.LogLogin(logLogin);
                    }
                }
                else
                {
                    objReturn.Status = false;
                    objReturn.Message = "invalid username or password";
                    objReturn.Auth = null;
                    //logLogin.status = "Error";
                    //logLogin.notes = objReturn.Message;
                    //_bll.LogLogin(logLogin);
                    return Content(HttpStatusCode.BadRequest, objReturn);    
                }
            }
            catch (Exception ex)
            {
                objReturn.Status = false;
                objReturn.DetailMessage = ex.InnerException.Message;
                objReturn.Message = "Oops! there was an unexpected error. please try again later";
                //logLogin.status = "Error";
                //logLogin.notes = objReturn.Message;
                //_bll.LogLogin(logLogin);
                return Json(objReturn);
            }
            return Ok(objReturn); 
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetData(BaseInput inputs)
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
                var model = _bll.CheckMvaUserInfo(id);
                var dbResult = _bll.getData(model.rep_id,GetHost());
                var viewMapper = Mapper.Map<List<UserProfileDTO>>(dbResult);
                //if (viewMapper.Count != 0)
                //{
                //    objResponseModel.TotalRecords = viewMapper.Count;
                //    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                //                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                //    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                //}

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult EditProfile()
        {
            var objResponseModel = new ResponseModel();
            var httpRequest = HttpContext.Current.Request;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validateAccess = ValidateAuth(httpRequest.Form["Auth"].ToString());
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }
                var id = Decrypt(httpRequest.Form["Auth"].ToString(), true);
                var model = _bll.CheckMvaUserInfo(id);
                string pass = "";
                string picture = "";

                if (!httpRequest.Form["newPass"].ToString().Equals("") || httpRequest.Form["newPass"].ToString() != null)
                    pass = EncryptPassword(httpRequest.Form["newPass"].ToString());

                try
                {
                    string fileName = "", path = "", headerPath = "";

                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            #region initialize

                            var settingsReader = new AppSettingsReader();
                            headerPath = (string) settingsReader.GetValue("UploadFotoProfile", typeof (String)) +
                                         model.rep_id + "/";

                            #endregion initialize

                            #region validation

                            if (httpRequest.Files[file] != null)
                            {
                                fileName = httpRequest.Files[file].FileName;
                                path = Path.Combine(headerPath, fileName);

                                if (!Directory.Exists(HttpContext.Current.Server.MapPath(headerPath)))
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(headerPath));
                                }
                                httpRequest.Files[file].SaveAs(HttpContext.Current.Server.MapPath(path));
                                //_mainBLL.InsertSPAttachment(visitid, fileName, path.Substring(1));
                            }

                            #endregion validation
                        }
                    }
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                    picture = headerPath + fileName;

                }
                catch (Exception e)
                {

                    objResponseModel.DetailMessage = e.Message;
                }

                int dbResult = _bll.editProfile(model.rep_id, pass, picture);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = (dbResult == 1)?"Success":"Failed";
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { objResponseModel });
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetProfileDoctor(BaseInput inputs)
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
                var model = _bll.CheckMvaUserInfo(id);
                var dbResult = _bll.getProfileDoctor(inputs.dr_code);
                var viewMapper = Mapper.Map<ProfileDoctorDetail>(dbResult);
                //if (viewMapper.Count != 0)
                //{
                //    objResponseModel.TotalRecords = viewMapper.Count;
                //    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                //                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                //    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                //}

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                //_vtLogger.Err(ex, new List<object> { inputs });
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetDoctorActivity(BaseInput inputs)
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
                var model = _bll.CheckMvaUserInfo(id);
                inputs.Year = inputs.Year == 0 ? DateTime.Now.Year : inputs.Year;
                var dbResult = _bll.getDoctorActivity(inputs.dr_code,inputs.Year);
                var viewMapper = Mapper.Map<List<DoctorActivityDTO>>(dbResult);
                //if (viewMapper.Count != 0)
                //{
                    objResponseModel.TotalRecords = viewMapper.Count;
                //    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                //                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                //    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                //}
                objResponseModel.Result = viewMapper;
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                //objResponseModel.Result = viewMapper;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                //_vtLogger.Err(ex, new List<object> { inputs });
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetProfilePicture(BaseInput inputs)
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
                var dbResult = _bll.GetProfilePic(inputs.RepId);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = GetHost().Replace("bas_api_mobile", "bas") + dbResult.Substring(1);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                //_vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }
    }
}

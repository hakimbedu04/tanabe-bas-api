using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using AutoMapper;
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
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace SF_WebApi.Controllers.Visit
{
    public class VisitRealizationController : BaseController
    {
        private readonly ILoginBLL _loginbll;
        private readonly IVisitBLL _mainBLL;
        private readonly IVTLogger _vtLogger;

        public VisitRealizationController(ILoginBLL loginbll, IVisitBLL mainBLL, IVTLogger vtLogger)
        {
            _loginbll = loginbll;
            _mainBLL = mainBLL;
            _vtLogger = vtLogger;
        }

        [HttpPost]
        public IHttpActionResult gridSPView(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                List<SP_SELECT_VISIT_SP_REALIZATION_DTO> dbResult = _mainBLL.GetSpRealization(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_REALIZATION_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult gridSPViewCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.VisitId = inputs.Prm == "retrieveattachment" ? inputs.VisitId : null;
                List<SP_SELECT_VISIT_SP_REALIZATION_DTO> dbResult = _mainBLL.GetSpRealization(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_REALIZATION_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetDeviation(VisitInputs inputs) // blm selesai semua
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                string dbresult = _mainBLL.GetDeviation(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = dbresult;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetSignature(VisitInputs inputs) // blm selesai semua
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                string dbresult = _mainBLL.GetSign(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (dbresult == null)
                {
                    objResponseModel.DetailMessage = "Signature is not available";
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    objResponseModel.Result = "";
                    return Json(objResponseModel);
                }
                objResponseModel.Result = GetHostNoHttp() + dbresult.Substring(1);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult AddDeviation(VisitInputs inputs) // blm selesai semua
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                bool statgps = (!String.IsNullOrEmpty(inputs.Latitude) || !String.IsNullOrEmpty(inputs.Longitude));
                inputs.RepId = Decrypt(inputs.Auth, true);
                _mainBLL.AddDeviation(inputs, statgps);
                if (!String.IsNullOrEmpty(inputs.Latitude) || !String.IsNullOrEmpty(inputs.Longitude))
                {
                    //save latitude & longitude
                    _mainBLL.SaveGpsLocation(inputs);
                }
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = inputs.Reason;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult gridAttachmentRealizationViewCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.Host = GetHostNoHttp();
                string visitid = inputs.Prm == "retrieveattachment" ? inputs.VisitId : null;
                //List<SP_SELECT_SP_ATTACHMENT_DTO> dbResult = _mainBLL.GetSPAttachment(visitid);
                //List<SP_SELECT_SP_ATTACHMENT_ViewModel> viewMapper =
                //    dbResult.Select(x => new SP_SELECT_SP_ATTACHMENT_ViewModel
                //    {
                //        spf_id = x.spf_id,
                //        spr_id = x.spr_id,
                //        spf_file_name = x.spf_file_name,
                //        spf_file_path = GetHost() + x.spf_file_path,
                //        spf_date_uploaded = x.spf_date_uploaded
                //    }).ToList();
                List<t_sp_attachment_DTO> dbResult = _mainBLL.GetVRealAttachment(visitid, inputs.Host);
                //var viewMapper = Mapper.Map<List<SP_SELECT_SP_ATTACHMENT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (dbResult.Count != 0)
                {
                    objResponseModel.TotalRecords = dbResult.Count;
                    objResponseModel.TotalPages = (dbResult.Count / inputs.PageSize) +
                                                  (dbResult.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        dbResult.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage UploadAttachment()
        {
            var objResponseModel = new ResponseModel();
            string auth = Convert.ToString(HttpContext.Current.Request.Params["Auth"]);
            string visitid = HttpContext.Current.Request.Params["VisitId"];
            string repid = Decrypt(auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(repid);
            string rep_reg = model.rep_region.Replace(" ", String.Empty);
            bool validateAccess = ValidateAuth(auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.NotFound, objResponseModel);
            }
            try
            {
                //upload file to server
                HttpRequest httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        #region initialize

                        var settingsReader = new AppSettingsReader();
                        string headerPath = (string)settingsReader.GetValue("UploadAttachmentPath", typeof(String)) +
                                            rep_reg + "/" + repid + "/";

                        #endregion initialize

                        #region validation

                        if (httpRequest.Files[file] != null)
                        {
                            string fileName = visitid + "-" + httpRequest.Files[file].FileName;
                            string path = Path.Combine(headerPath, fileName);

                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(headerPath)))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(headerPath));
                            }
                            httpRequest.Files[file].SaveAs(HttpContext.Current.Server.MapPath(path));
                            _mainBLL.InsertSPAttachment(visitid, fileName, path.Replace("WebVirtualDir", "Files"));
                        }

                        #endregion validation
                    }
                }
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.Created, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddSignature()
        {
            var objResponseModel = new ResponseModel();
            string auth = Convert.ToString(HttpContext.Current.Request.Params["Auth"]);
            string visitid = HttpContext.Current.Request.Params["VisitId"];
            int drcode = Convert.ToInt32(HttpContext.Current.Request.Params["DrCode"]);
            string Address = HttpContext.Current.Request.Params["Address"];
            string repid = Decrypt(auth, true);

            var inputs = new VisitInputs
            {
                VisitId = visitid,
                Latitude = HttpContext.Current.Request.Params["Latitude"],
                Longitude = HttpContext.Current.Request.Params["Longitude"],
                RepId = repid,
                DrCode = Convert.ToString(drcode),
                Address = Address
            };
            bool validateAccess = ValidateAuth(auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.NotFound, objResponseModel);
            }

            try
            {
                //upload file to server
                HttpRequest httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var settingsReader = new AppSettingsReader();
                        var headerPath = (string)settingsReader.GetValue("UploadSignaturePath", typeof(String));
                        if (httpRequest.Files[file] != null)
                        {
                            string fileName = httpRequest.Files[file].FileName;
                            string path = Path.Combine(headerPath, fileName);
                            string temppath = Path.Combine(headerPath + "/temp", fileName);


                            bool statgps = (!String.IsNullOrEmpty(inputs.Latitude) || !String.IsNullOrEmpty(inputs.Longitude));

                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(headerPath)))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(headerPath));
                            }

                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(headerPath + "/temp")))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(headerPath + "/temp"));
                            }

                            httpRequest.Files[file].SaveAs(HttpContext.Current.Server.MapPath(temppath));

                            #region updates
                            Image objImg = Image.FromFile(HttpContext.Current.Server.MapPath(temppath));
                            var destImg = ResizeImage(objImg, 1280, 800);
                            destImg.Save(HttpContext.Current.Server.MapPath(path));
                            objImg.Dispose();
                            if (File.Exists(HttpContext.Current.Server.MapPath(temppath)))
                            {
                                File.Delete(HttpContext.Current.Server.MapPath(temppath));
                            }
                            #endregion

                            _mainBLL.AddSign(repid, visitid, drcode, path, statgps);
                            objResponseModel.Result = GetHost() + path.Substring(1);
                            if (!String.IsNullOrEmpty(inputs.Latitude) || !String.IsNullOrEmpty(inputs.Longitude))
                            {
                                //save latitude & longitude
                                _mainBLL.SaveGpsLocation(inputs);
                            }
                        }
                    }
                }

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.Created, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        [HttpPost]
        public IHttpActionResult ShiftRealization(VisitInputs inputs) // blm selesai semua
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(inputs.PrevVisitDate)))
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = "Please update your application version!";
                    return Json(objResponseModel);
                }
                inputs.RepId = Decrypt(inputs.Auth, true);
                #region validation
                var validate = _mainBLL.ValidateShiftCounter(inputs);
                if (validate > 0)
                {
                    objResponseModel.Message = (validate == 1) ? "You have reach your shift limit on this week!" : "Please change shift date within Monday and Sunday on current week";
                    objResponseModel.Status = false;
                    objResponseModel.DetailMessage = "";
                    return Json(objResponseModel);
                }

                #endregion

                _mainBLL.ShiftRealization(inputs);

                #region log
                _mainBLL.InsertShiftLog(inputs);
                #endregion
                inputs.ActionSource = "DataViewPartialCustomCallback";
                List<v_visit_plan_new_DTO> dbResult = _mainBLL.GetVisitReal(inputs);
                var viewMapper = Mapper.Map<List<v_visit_plan_new_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult FolderBindingPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.Host = GetHost();
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult AddShowTopicDuration(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                _mainBLL.AddShowTopicDuration(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = inputs.Reason;
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult cbProductTopicLookup(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                List<SP_SELECT_PRODUCT_TOPIC_LIST_DTO> dbResult = _mainBLL.GetProductTopicList(inputs.VdId);
                var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_TOPIC_LIST_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult LoadDoctorList(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                inputs.RepPosition = model.rep_position;
                List<SP_SELECT_DOCTOR_LIST_DTO> dbResult = _mainBLL.GetDataDoctorList(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_DOCTOR_LIST_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage EventNamePartialView(VisitInputs inputs)
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
                List<m_event_DTO> dbResult = _mainBLL.GetEventNameList(inputs.Budget);
                var viewMapper = Mapper.Map<List<m_event_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteAttachment(VisitInputs inputs)
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
                string filePath = _mainBLL.GetFilePath(inputs.SpfId);
                _mainBLL.DeleteSPAttachment(inputs.SpfId);
                //File.Delete(HttpContext.Current.Server.MapPath(filePath).Replace("/mtid/dist/", "/bas/"));
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                List<SP_SELECT_SP_ATTACHMENT_DTO> dbResult = _mainBLL.GetSPAttachment("V00");
                List<SP_SELECT_SP_ATTACHMENT_ViewModel> viewMapper =
                    dbResult.Select(x => new SP_SELECT_SP_ATTACHMENT_ViewModel
                    {
                        spf_id = x.spf_id,
                        spr_id = x.spr_id,
                        spf_file_name = x.spf_file_name,
                        spf_file_path = GetHost() + x.spf_file_path,
                        spf_date_uploaded = x.spf_date_uploaded
                    }).ToList();
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage LoadCancellation(VisitInputs inputs)
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
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                inputs.RepPosition = model.rep_position;
                if (inputs.Prm == "submit")
                {
                    bool submitted = _mainBLL.SaveCancellationVisit(inputs);
                    if (submitted)
                    {
                        objResponseModel.Status = true;
                        objResponseModel.Message =
                            EnumHelper.GetDescription(Enums.ResponseType.SuccessSubmitCancellation);
                        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
                    }
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorSubmitCancellation);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }
                if (inputs.Prm == "validate")
                {
                    if (_mainBLL.isAnyPlanOnChoosenDay(inputs))
                    {
                        if (inputs.ConditionValue != "full")
                        {
                            List<v_visit_plan_DTO> dbResult = _mainBLL.GetDataDoctorPlaned(inputs);
                            var viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
                            objResponseModel.Status = true;
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
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
                    }
                    else
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.nullPlanned);
                        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage ShareTopicFile(VisitInputs inputs)
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
                SetReport(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessSendFile);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public void SetReport(VisitInputs inputs)
        {
            var reader =
                new StreamReader(
                    HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_send_file_to_doctor_mobile.htm"));
            string topicPath = _mainBLL.GetTopicPath(inputs.TopicId);
            string pdfFile = HttpContext.Current.Server.MapPath(@"~" + topicPath);
            string readFile = reader.ReadToEnd();
            string email_body;
            email_body = readFile;
            DateTime currentDate = DateTime.Now;
            email_body = email_body.Replace("$$RECEIVER$$", inputs.DrName);
            email_body = email_body.Replace("$$ProductName$$", inputs.VisitProduct);
            email_body = email_body.Replace("$$TopicName$$", inputs.VisitTopic);
            email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("fromEmailAddress", typeof(String));
            // Get the key from config file
            mailMessage.From = new MailAddress(key);
            //mailMessage.To.Add(new MailAddress(userDetail.email_am)); 
            mailMessage.To.Add(new MailAddress(inputs.Email));
            ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));

            mailMessage.Attachments.Add(new Attachment(pdfFile));
            mailMessage.Subject = "File Attachment topic - " + inputs.VisitProduct;

            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = email_body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                inputs.ActionSource = "DataViewPartial";
                List<SummaryDoctor_DTO> dbResult = _mainBLL.GetQuadrantSummaryRealization(inputs);
                //var viewMapper = Mapper.Map<List<SummaryDoctor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = dbResult;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetTopicInfo(VisitInputs inputs)
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
                List<v_info_feedback_DTO> dbResult = _mainBLL.GetTopicInfo(inputs.TopicId, inputs.VptFeedBack,
                    inputs.VptId, GetHost());
                var viewMapper = Mapper.Map<List<v_info_feedback_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
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
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public string GenerateExcel(VisitInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            inputs.ActionSource = "DataViewPartial";
            List<v_visit_plan_new_DTO> dbResult = _mainBLL.GetVisitReal(inputs);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String));
            string addressPath = headerPath + "VisitRealization" + "/" + model.rep_id;
            string resultFileName = "visit_realization_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
                                    inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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

            string templateFile = Enums.ExcelTemplate.Visit_Realization + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.VisitRealization + templateFile);
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

                foreach (v_visit_plan_new_DTO item in dbResult)
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
                    slDoc.SetCellValue(iRow, 5, item.visit_sp);
                    slDoc.SetCellValue(iRow, 6, item.visit_sp_value.HasValue ? item.visit_sp_value.Value : 0);
                    slDoc.SetCellValue(iRow, 7, item.visit_info);
                    slDoc.SetCellValue(iRow, 8, item.visit_real_verification_status);
                    slDoc.SetCellValue(iRow, 9, item.dr_code.HasValue ? Convert.ToString(item.dr_code.Value) : "");
                    slDoc.SetCellValue(iRow, 10, item.dr_name);
                    slDoc.SetCellValue(iRow, 11, item.dr_spec);

                    slDoc.SetCellValue(iRow, 12, item.dr_sub_spec);
                    slDoc.SetCellValue(iRow, 13, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 14, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 15, item.dr_dk_lk);
                    slDoc.SetCellValue(iRow, 16, item.dr_area_mis);
                    slDoc.SetCellValue(iRow, 17, item.dr_category);
                    slDoc.SetCellValue(iRow, 18, item.dr_chanel);
                    slDoc.SetCellStyle(iRow, 1, iRow, 17, style);
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

        //public string GenerateExcel(VisitInputs inputs)
        //{
        //    inputs.RepId = Decrypt(inputs.Auth, true);
        //    var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
        //    inputs.ActionSource = "DataViewPartial";
        //    var dbResult = _mainBLL.GetVisitReal(inputs);
        //    DataTable boundTable = new DataTable();
        //    boundTable.Columns.Add("PLAN ID", typeof(string));
        //    boundTable.Columns.Add("DATE PLAN", typeof(string));
        //    boundTable.Columns.Add("VPLAN", typeof(string));
        //    boundTable.Columns.Add("VREAL", typeof(string));
        //    boundTable.Columns.Add("SP", typeof(string));
        //    boundTable.Columns.Add("VALUE", typeof(string));
        //    boundTable.Columns.Add("VISIT INFO", typeof(string));
        //    boundTable.Columns.Add("REAL Ver.STATUS", typeof(string));
        //    boundTable.Columns.Add("DR CODE", typeof(string));
        //    boundTable.Columns.Add("DR NAME", typeof(string));
        //    boundTable.Columns.Add("DR SPEC", typeof(string));

        //    boundTable.Columns.Add("SUB SPEC", typeof(string));
        //    boundTable.Columns.Add("QRD", typeof(string));
        //    boundTable.Columns.Add("MTG", typeof(string));
        //    boundTable.Columns.Add("DK/LK", typeof(string));
        //    boundTable.Columns.Add("AREA MIS", typeof(string));
        //    boundTable.Columns.Add("CATEGORY", typeof(string));
        //    boundTable.Columns.Add("CHANNEL", typeof(string));
        //    foreach (v_visit_plan_new_DTO item in dbResult)
        //    {
        //        dynamic dr = boundTable.NewRow();
        //        dr["PLAN ID"] = item.visit_id;
        //        dr["DATE PLAN"] = item.visit_date_plan.HasValue ? item.visit_date_plan.Value.ToString("yyyy MMMM dd") : " ";
        //        dr["VPLAN"] = item.visit_plan;
        //        dr["VREAL"] = item.visit_realization;
        //        dr["SP"] = item.visit_sp;
        //        dr["VALUE"] = item.visit_sp_value;
        //        dr["VISIT INFO"] = item.visit_info;
        //        dr["REAL Ver.STATUS"] = item.visit_real_verification_status;
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
        //        boundTable.Rows.Add(dr);
        //    }
        //    var currentDate = DateTime.Now;
        //    inputs.Day = currentDate.Day;
        //    inputs.Month = currentDate.Month;
        //    inputs.Year = currentDate.Year;
        //    var dataTableToExcel = new DataTableToExcel();

        //    #region upload file
        //    var settingsReader = new AppSettingsReader();
        //    var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
        //    var addressPath = headerPath + "/" + txtNik.Text + "/VisitRealization" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
        //    var resultFileName = "visit_Realization_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
        //    var resultFilePath = addressPath + "/" + resultFileName;

        //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
        //    {
        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
        //    }
        //    dataTableToExcel.generateExcels(boundTable, HttpContext.Current.Server.MapPath(resultFilePath));

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
                objResponseModel.DetailMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public string GeneratePdf(VisitInputs inputs)
        {
            #region initialitation

            string returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            inputs.ActionSource = "DataViewPartial";
            List<v_visit_plan_new_DTO> dbResult = _mainBLL.GetVisitReal(inputs);
            List<SummaryDoctor_DTO> detailDoctor = _mainBLL.GetQuadrantSummaryRealization(inputs);
            string fonttype = BaseFont.TIMES_ROMAN;
            Font tableHeaderFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.WHITE);
            Font tableContentFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.BLACK);
            Font TitleFont = FontFactory.GetFont(fonttype, 11, Font.NORMAL, BaseColor.BLACK);
            DateTime currentDate = DateTime.Now;
            var document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

            #endregion

            #region upload file

            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GeneratePdfPath", typeof(String));
            //~/Asset/Files/Downloads/Pdf/
            string addressPath = headerPath + "VisitRealization" + "/" + model.rep_id;
            // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "visit_realization_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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
            title.Add(new Paragraph("REALIZATION VISIT ", TitleFont));
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
            dt.Columns.Add("PLAN ID", typeof(string));
            dt.Columns.Add("DATE PLAN", typeof(string));
            dt.Columns.Add("VPLAN", typeof(string));
            dt.Columns.Add("VREAL", typeof(string));
            dt.Columns.Add("SP", typeof(string));
            dt.Columns.Add("VALUE", typeof(string));
            dt.Columns.Add("VISIT INFO", typeof(string));
            dt.Columns.Add("REAL Ver.STATUS", typeof(string));
            dt.Columns.Add("DR CODE", typeof(string));
            dt.Columns.Add("DR NAME", typeof(string));
            dt.Columns.Add("DR SPEC", typeof(string));

            dt.Columns.Add("SUB SPEC", typeof(string));
            dt.Columns.Add("QRD", typeof(string));
            dt.Columns.Add("MTG", typeof(string));
            dt.Columns.Add("DK/LK", typeof(string));
            dt.Columns.Add("AREA MIS", typeof(string));
            dt.Columns.Add("CATEGORY", typeof(string));
            dt.Columns.Add("CHANNEL", typeof(string));
            foreach (v_visit_plan_new_DTO item in dbResult)
            {
                dynamic dr = dt.NewRow();
                dr["PLAN ID"] = item.visit_id;
                dr["DATE PLAN"] = item.visit_date_plan.HasValue
                    ? item.visit_date_plan.Value.ToString("yyyy MMMM dd")
                    : " ";
                dr["VPLAN"] = item.visit_plan;
                dr["VREAL"] = item.visit_realization;
                dr["SP"] = item.visit_sp;
                dr["VALUE"] = item.visit_sp_value;
                dr["VISIT INFO"] = item.visit_info;
                dr["REAL Ver.STATUS"] = item.visit_real_verification_status;
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

            //Adding  PdfPTable  
            var table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = { 5f, 6f, 4f, 4f, 3f, 4f, 6f, 7f, 4f, 12f, 4f, 5f, 3f, 15f, 5f, 5f, 6f, 5f };
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

            #endregion

            document.Add(new Chunk("\n", TitleFont));


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
            document.Dispose();

            return returnVal;
        }

        #region Doctor Additional List View

        [HttpPost]
        public IHttpActionResult GridDoctorAdditionalPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                inputs.ActionSource = "GridDoctorAdditionalPartial";
                DateTime currentDate = DateTime.Now;
                inputs.Month = currentDate.Month;
                inputs.Year = currentDate.Year;
                List<v_visit_plan_DTO> dbResult = _mainBLL.GetDoctorAdditionalList(inputs);
                var viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GridDoctorAdditionalCustomCallbackPartial(VisitInputs inputs) // blm selesai semua
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            var dbResult = new List<v_visit_plan_DTO>();
            var viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            inputs.RepPosition = model.rep_position;
            inputs.ActionSource = "GridDoctorAdditionalCustomCallbackPartial";
            if (inputs.Prm == "addvisitadditional")
            {
                #region validation

                //'jika sudah buat plan leave/absen pada tanggal tersebut, additional visit tidak bisa dilakukan
                if (!_mainBLL.isValidDay(inputs))
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = "You've made an absent plan on that day";
                    return Json(objResponseModel);
                }
                //'jika belum ada plan sama sekali pada tanggal dimana akan dibuat additional visit, maka AV tidak bisa dilakukan
                if (inputs.RepPosition != "RM" && inputs.RepPosition != "AM")
                {
                    if (!_mainBLL.isAlreadyPlannedVisitInCurrDay(inputs))
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = "You haven't made some plans on that day";
                        return Json(objResponseModel);
                    }
                }

                //'jika ada doctor yg sama pada tanggal tersebut, maka AV tidak bisa dilakukan
                if (inputs.RepPosition != "RM" && inputs.RepPosition != "AM")
                {
                    if (_mainBLL.isAlreadyPlannedDoctorInCurrDay(inputs))
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = "There is already planned doctor with code " + inputs.DrCode +
                                                   " on that day";
                        return Json(objResponseModel);
                    }
                }


                var maxVisitAv = _mainBLL.GetMaxAdditionalVisit();
                //'jika max visit lebih besar atau sama dengan maxVisitAv (dari parameter di db), maka AV tidak bisa dilakukan berlaku hanya 7(contoh) AV
                if (inputs.RepPosition != "RM" && inputs.RepPosition != "AM")
                {
                    if (_mainBLL.CheckMaxVisit(inputs) >= maxVisitAv)
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = $"Additional visit actual is only given {maxVisitAv} times per day";
                        return Json(objResponseModel);
                    }
                }

                if (inputs.RepPosition != "RM" && inputs.RepPosition != "AM")
                {
                    if (_mainBLL.isMaxLimitedDoctorinCurrDay(inputs))
                    {
                        objResponseModel.Status = false;
                        objResponseModel.Message = "You've exceeded the maximum limit doctor visits on that day";
                        return Json(objResponseModel);
                    }
                }

                #endregion validation

                try
                {
                    _mainBLL.SaveAdditionalVisit(inputs);
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    return Ok(objResponseModel);
                }
                catch (Exception ex)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    //disini
                    objResponseModel.DetailMessage = ex.Message;
                    _vtLogger.Err(ex, new List<object> { inputs });
                    return Json(objResponseModel);
                }
            }

            if (inputs.Prm == "submit_additional")
            {
                // get sign
                string validateSign = _mainBLL.VerifySign(inputs);
                if (String.IsNullOrEmpty(validateSign))
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullSign);
                    return Json(objResponseModel);
                }

                #endregion

                try
                {
                    if (String.IsNullOrEmpty(inputs.SpBa))
                    {
                        inputs.SpBa = null;
                    }
                    bool state = _mainBLL.SubmitAdditionalRealizationVisit(inputs);
                    if (state)
                    {
                        if (!String.IsNullOrEmpty(inputs.SpBa))
                        {
                            SendRequestVerification(inputs.VisitId);
                        }
                        ////save latitude & longitude
                        //_mainBLL.SaveGpsLocation(inputs);
                        objResponseModel.Message =
                            EnumHelper.GetDescription(Enums.ResponseType.SuccessAdditionalRealization);
                        objResponseModel.Status = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (msg == "null_topic")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.RealizationNullTopic);
                    }
                    else if (msg == "null_feedback")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.RealizationNullFeedback);
                    }
                    else if (msg == "null_attachment")
                    {
                        objResponseModel.Message =
                            EnumHelper.GetDescription(Enums.ResponseType.RealizationNullAttachment);
                    }
                    else if (msg == "exists_sp")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ExistsSp);
                    }
                    else if (msg == "product_incomplete")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ProductIncomplete);
                    }
                    else
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                    }
                    objResponseModel.Status = false;
                    objResponseModel.DetailMessage = ex.Message;
                    return Ok(objResponseModel);
                }
            }
            try
            {
                DateTime currentDate = DateTime.Now;
                inputs.Month = currentDate.Month;
                inputs.Year = currentDate.Year;
                dbResult = _mainBLL.GetDoctorAdditionalList(inputs);
                viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Ok(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult LoadDoctorPlaned(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.ActionSource = "LoadDoctorPlaned";
                inputs.RepId = Decrypt(inputs.Auth, true);
                inputs.Month = inputs.VisitDatePlan.Month;
                List<v_visit_plan_DTO> dbResult = _mainBLL.GetDoctorAdditionalList(inputs);
                var viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult LoadRealization(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_SELECT_VISIT_SP_REALIZATION_DTO> dbResult = _mainBLL.GetSpRealization(inputs.VisitId);
                var viewMapper = Mapper.Map<List<SP_SELECT_VISIT_SP_REALIZATION_DTO>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult LoadProduct(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            try
            {
                bool validateAccess = ValidateAuth(inputs.Auth);
                if (!validateAccess)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    return Json(objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                List<SP_SELECT_PRODUCT_VISIT_DTO> dbResult = _mainBLL.GetDataProductList();
                var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_VISIT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult LoadRealProduct(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                List<SP_SELECT_PRODUCT_VISIT_DTO> dbResult = _mainBLL.GetDataProductList();
                var viewMapper = Mapper.Map<List<SP_SELECT_PRODUCT_VISIT_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        #region Delete

        [HttpPost]
        public IHttpActionResult DeleteVisitAdditional(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            if (String.IsNullOrEmpty(inputs.VisitId))
            {
                try
                {
                    inputs.RepId = Decrypt(inputs.Auth, true);
                    bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
                    inputs.RepPosition = model.rep_position;
                    _mainBLL.DeleteVisitPlan(inputs);
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDeleteDefault);
                }
                catch (Exception ex)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                    objResponseModel.DetailMessage = ex.Message;
                }
            }
            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                DateTime currentDate = DateTime.Now;
                inputs.Month = currentDate.Month;
                inputs.Year = currentDate.Year;
                List<v_visit_plan_DTO> dbResult = _mainBLL.GetDoctorAdditionalList(inputs);
                var viewMapper = Mapper.Map<List<v_visit_plan_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteProductVisitRealization(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            var dataIn = new VisitInputs
            {
                VisitId = inputs.VisitId
            };
            try
            {
                _mainBLL.DeleteVisitProduct(inputs);
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDeleteDefault);
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "sp_already_plan"
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Json(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteProductVisitAdditional(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            var dataIn = new VisitInputs
            {
                VisitId = inputs.VisitId
            };
            try
            {
                _mainBLL.DeleteVisitProduct(inputs);

                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDeleteProduct);
                objResponseModel.DetailMessage = "";
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "sp_already_plan"
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Json(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteProductTopic(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                _mainBLL.DeleteProductTopic(inputs);
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessDeleteDefault);
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                objResponseModel.DetailMessage = ex.Message;
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Json(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Json(objResponseModel);
            }
        }

        #endregion

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
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                inputs.RepId = Decrypt(inputs.Auth, true);
                var dbResult = _mainBLL.GetVisitRealOptimize(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (dbResult.Count != 0)
                {
                    objResponseModel.TotalRecords = dbResult.Count;
                    objResponseModel.TotalPages = (dbResult.Count / inputs.PageSize) +
                                                  (dbResult.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result = dbResult.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
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

        [HttpPost]
        public IHttpActionResult DataViewPartialCustomCallback(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            inputs.RepId = Decrypt(inputs.Auth, true);
            inputs.ActionSource = "DataViewPartialCustomCallback";
            if (inputs.Prm == "retrieve")
            {
                var dbResult1 = _mainBLL.GetVisitRealOptimize(inputs);
                //var viewMapper1 = Mapper.Map<List<v_visit_plan_new_ViewModel>>(dbResult1);
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Status = true;
                if (dbResult1.Count != 0)
                {
                    objResponseModel.TotalRecords = dbResult1.Count;
                    objResponseModel.TotalPages = (dbResult1.Count / inputs.PageSize) +
                                                  (dbResult1.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        dbResult1.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }

            if (inputs.Prm == "realization")
            {
                inputs.Month = inputs.VisitDateTime.Value.Month;
                inputs.Year = inputs.VisitDateTime.Value.Year;
                inputs.ActionSource = "DataViewPartial";
                try
                {
                    #region validation signature

                    // get sign
                    string validateSign = _mainBLL.VerifySign(inputs);
                    if (inputs.Visited != 0)
                    {
                        if (String.IsNullOrEmpty(validateSign))
                        {
                            objResponseModel.Status = false;
                            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullSign);
                            return Json(objResponseModel);
                        }
                    }

                    #endregion

                    List<SP_SELECT_VISIT_SP_REALIZATION_DTO> spPlan = _mainBLL.GetSpRealization(inputs.VisitId);
                    int valAmount = Convert.ToInt32(inputs.RealAmount);
                    if (!spPlan.Any())
                    {
                        if (valAmount != 0)
                        {
                            if (String.IsNullOrEmpty(inputs.SpBa))
                            {
                                objResponseModel.Status = false;
                                objResponseModel.Message =
                                    EnumHelper.GetDescription(Enums.ResponseType.NullBudgetRealization);
                                return Json(objResponseModel);
                            }
                            inputs.SPRealizationStat = 2;
                            if (_mainBLL.SaveRealizationVisitWithAdditionalSP(inputs))
                            {
                                SendRequestVerification(inputs.VisitId);
                            }
                            ////save latitude & longitude
                            //_mainBLL.SaveGpsLocation(inputs);
                        }
                        else
                        {
                            inputs.SPRealizationStat = -1;
                            _mainBLL.SaveRealizationVisit(inputs);
                            ////save latitude & longitude
                            //_mainBLL.SaveGpsLocation(inputs);
                        }
                    }
                    else
                    {
                        //if (!String.IsNullOrEmpty(inputs.IsRealized))
                        //{
                        //    objResponseModel.Status = false;
                        //    objResponseModel.Message =
                        //        EnumHelper.GetDescription(Enums.ResponseType.NullButtonRealization);
                        //    return Json(objResponseModel);
                        //}

                        /**
                         * edited by hakim
                         * date : 20200331
                         * case reference : https://mail.google.com/mail/u/0/#inbox/FMfcgxwHMZNXPlClsNfWkpVfCXWskLbX
                         * description : error by case, when user select not visit so button realize or not realize sp realization document become unable to clik,
                         * then inputs.SPRealizationStat value should be -1 (handled by SP), but due to mobile send in empty string, 
                         * then this code should convert empty string value to -1
                         */
                        inputs.SPRealizationStat = String.IsNullOrEmpty(inputs.IsRealized) ? -1 : Convert.ToInt32(inputs.IsRealized); // * changed code 20200331

                        if (_mainBLL.SaveRealizationVisit(inputs))
                        {
                            if (inputs.SPRealizationStat == 1) // * changed code 20200331
                            {
                                try
                                {
                                    SendRequestVerification(inputs.VisitId);
                                }
                                catch
                                {
                                    objResponseModel.Status = true;
                                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessRealization);
                                }
                            }
                        }
                        ////save latitude & longitude
                        //_mainBLL.SaveGpsLocation(inputs);
                    }

                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessRealization);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (msg == "null_topic")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.RealizationNullTopic);
                    }
                    else if (msg == "null_feedback")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.RealizationNullFeedback);
                    }
                    else if (msg == "null_attachment")
                    {
                        objResponseModel.Message =
                            EnumHelper.GetDescription(Enums.ResponseType.RealizationNullAttachment);
                    }
                    else if (msg == "product_incomplete")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ProductIncomplete);
                    }
                    else if (msg == "error_query")
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                    }
                    else if (msg.Contains("SMTP"))
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.EmailError);
                    }
                    else
                    {
                        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                        _vtLogger.Err(ex, new List<object> { inputs },
                        new List<object> { _loginbll.CheckMvaUserInfo(inputs.RepId) });
                    }
                    objResponseModel.Status = false;
                    objResponseModel.DetailMessage = ex.Message;

                    return Ok(objResponseModel);
                }
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                inputs.Month = currentDate.Month;
                inputs.Year = currentDate.Year;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
            }
            var dbResult = _mainBLL.GetVisitRealOptimize(inputs);
            objResponseModel.Status = true;
            if (dbResult.Count != 0)
            {
                objResponseModel.TotalRecords = dbResult.Count;
                objResponseModel.TotalPages = (dbResult.Count / inputs.PageSize) +
                                              (dbResult.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    dbResult.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
            }

            return Ok(objResponseModel);
        }

        public void SendRequestVerification(string visitid)
        {
            try
            {
                var reader =
                    new StreamReader(
                        HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_sp_verification.htm"));
                string readFile = reader.ReadToEnd();

                List<SP_SELECT_SPR_INFO_DTO> model = _mainBLL.GetSprInformation(visitid);
                List<v_rep_admin_DTO> dataReceiver = _mainBLL.GetDataAdmin(model[0].spr_initiator);
                bas_v_rep_fullDTO userDetail = _loginbll.CheckMvaUserInfo(model[0].spr_initiator);
                DateTime currentDate = DateTime.Now;
                foreach (v_rep_admin_DTO data in dataReceiver)
                {
                    string email_body = readFile;
                    email_body = email_body.Replace("$$RECEIVER$$", data.admin_name);
                    email_body = email_body.Replace("$$spr_no$$", model[0].spr_no);
                    email_body = email_body.Replace("$$sp$$", model[0].e_name);
                    email_body = email_body.Replace("$$spr_id$$", model[0].spr_id);
                    email_body = email_body.Replace("$$dt_event$$", model[0].e_dt_start.Value.ToString("yyyy MMMM dd"));
                    email_body = email_body.Replace("$$rep_name$$", userDetail.rep_name);
                    email_body = email_body.Replace("$$rep_region$$", userDetail.rep_region);
                    email_body = email_body.Replace("$$bo$$", userDetail.rep_bo);
                    email_body = email_body.Replace("$$sbo$$", userDetail.rep_sbo);
                    email_body = email_body.Replace("$$sp_type$$", model[0].sp_type);
                    email_body = email_body.Replace("$$month_plan$$", currentDate.Month.ToString());
                    email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

                    var mailMessage = new MailMessage();
                    var smtpClient = new SmtpClient();
                    var settingsReader = new AppSettingsReader();
                    var key = (string)settingsReader.GetValue("fromEmailAddress", typeof(String));
                    // Get the key from config file
                    mailMessage.From = new MailAddress(key);
                    ////mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
                    ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));
                    if (!String.IsNullOrEmpty(data.admin_email))
                    {
                        mailMessage.To.Add(new MailAddress(data.admin_email));
                    }
                    mailMessage.Subject = "Sales Promotion Verification Request for " + data.admin_name + " - " +
                                          "Month Event Plan - " + currentDate.Month;

                    mailMessage.Priority = MailPriority.High;
                    mailMessage.Body = email_body;
                    mailMessage.IsBodyHtml = true;

                    smtpClient.Send(mailMessage);

                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Product Visit

        [HttpPost]
        public IHttpActionResult GridVisitProductPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        #endregion

        #region Visit Product Additional

        [HttpPost]
        public IHttpActionResult GridVisitProductAdditionalCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            try
            {
                inputs.VisitId = inputs.Prm == "retrieveVisitProductAdditional" ? inputs.VisitId : null;
                var dataIn = new VisitInputs
                {
                    VisitId = inputs.VisitId
                };
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult AddProductVisitRealization(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            if (String.IsNullOrEmpty(inputs.VisitId))
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullInputs);
                return Json(objResponseModel);
            }
            try
            {
                _mainBLL.InsertVisitProduct(inputs);
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg == "sp_already_plan")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SpAlreadyPlan);
                }
                else if (msg == "percent_not_match")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.PercentNotMatch);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                }
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Json(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult AddProductVisitAdditional(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            if (String.IsNullOrEmpty(inputs.VisitId))
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.NullInputs);
                return Json(objResponseModel);
            }
            var dataIn = new VisitInputs
            {
                VisitId = inputs.VisitId
            };
            try
            {
                _mainBLL.InsertVisitProduct(inputs);
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Ok(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //objResponseModel.Message = EnumHelper.GetDescription(msg == "sp_already_plan" ? Enums.ResponseType.SpAlreadyPlan : Enums.ResponseType.QueryError);
                if (msg == "sp_already_plan")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SpAlreadyPlan);
                }
                else if (msg == "percent_not_match")
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.PercentNotMatch);
                }
                else
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.QueryError);
                    _vtLogger.Err(ex, new List<object> { inputs },
                        new List<object> { _loginbll.CheckMvaUserInfo(inputs.RepId) });
                }
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(dataIn);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                if (viewMapper.Count == 0) return Json(objResponseModel);
                objResponseModel.TotalRecords = viewMapper.Count;
                objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                              (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                objResponseModel.Result =
                    viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateProductVisitRealization(VisitInputs inputs)
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
                int result = _mainBLL.UpdateVisitProduct(inputs);
                if (result == 0)
                {
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SpAlreadyPlan);
                    objResponseModel.Status = false;
                    objResponseModel.Result = "";
                    return Request.CreateResponse(HttpStatusCode.Conflict, objResponseModel);
                }
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdateProduct);
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
                string msg = ex.Message;
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "sp_already_plan"
                        ? Enums.ResponseType.SpAlreadyPlan
                        : Enums.ResponseType.QueryError);
                List<v_visit_product_DTO> dbResult = _mainBLL.GetVisitProduct(inputs.VisitId);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        //[HttpPost]
        //public IHttpActionResult UpdateProductVisitAdditional(VisitInputs inputs)
        //{
        //    var objResponseModel = new ResponseModel();

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var validateAccess = ValidateAuth(inputs.Auth);
        //    if (!validateAccess)
        //    {
        //        objResponseModel.Status = false;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //        return Json(objResponseModel);
        //    }

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[HttpPost]
        //public IHttpActionResult AddProductVisitRealization(VisitInputs inputs)
        //{

        //}

        #endregion

        #region Product Topic

        [HttpPost]
        public IHttpActionResult GridProductTopicPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                inputs.Host = GetHost();
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GridProductTopicCustomCallbackPartial(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();
            //try
            //{
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }

            if (inputs.Prm == "bindFeedback")
            {
                if (inputs.VptId == 0)
                {
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    return Ok(objResponseModel);
                }
                int vptId = inputs.VptId;
                IEnumerable<int> res = _mainBLL.GetTopicFeedback(vptId);
                objResponseModel.Status = true;
                objResponseModel.Result = res;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Ok(objResponseModel);
            }
            if (inputs.Prm == "updateFeedback")
            {
                if (_mainBLL.UpdateFeedbackTopic(inputs))
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessUpdateFeedback);
                objResponseModel.Status = true;
                return Ok(objResponseModel);
            }
            inputs.VdId = inputs.Prm == "retrievetopic" ? inputs.VdId : 0;

            try
            {
                inputs.Host = GetHost();
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult AddProductTopic(VisitInputs inputs)
        {
            var objResponseModel = new ResponseModel();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool validateAccess = ValidateAuth(inputs.Auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Json(objResponseModel);
            }
            try
            {
                _mainBLL.InsertProductTopic(inputs);
                inputs.Host = GetHost();
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }
                return Ok(objResponseModel);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg != "topic_exists")
                {
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    objResponseModel.DetailMessage = ex.Message;
                    return Json(objResponseModel);
                }
                objResponseModel.Message =
                    EnumHelper.GetDescription(msg == "topic_exists"
                        ? Enums.ResponseType.TopicExists
                        : Enums.ResponseType.QueryError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                return Json(objResponseModel);
            }
        }

        #endregion


        #region download data offline
        [HttpPost]
        public HttpResponseMessage GetAllDataOffline(VisitInputs inputs)
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
                var dbResult = _mainBLL.GenerateLinkJsonFiles(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                objResponseModel.Result = dbResult;

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = "Error";
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllDataMaster(VisitInputs inputs)
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
                var dbResult = _mainBLL.GetAllDataMaster(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                objResponseModel.Result = dbResult;

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = "Error";
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllDataTrans(VisitInputs inputs)
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
                inputs.Host = GetHostNoHttp();
                inputs.RepId = Decrypt(inputs.Auth, true);
                var dbResult = _mainBLL.GetAllDataTrans(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                objResponseModel.Result = dbResult;

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = "Error";
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllDataImages(VisitInputs inputs)
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
                inputs.Host = GetHostNoHttp();
                inputs.RepId = Decrypt(inputs.Auth, true);
                var dbResult = _mainBLL.GetAllDataImages(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

                objResponseModel.Result = dbResult;

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = "Error";
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }
        #endregion

        #region sync transaction
        [HttpPost]
        public HttpResponseMessage SyncProduct(RealizationOfflineInputs inputs)
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
                var res = _mainBLL.SyncTrans(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                if (res == "success")
                {
                    objResponseModel.Status = true;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                    objResponseModel.Result = res;
                }
                else
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = res;
                    objResponseModel.Result = res;
                }

                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = "Error";
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }


        #endregion

        [HttpPost]
        public HttpResponseMessage CheckSignatureMessage(VisitInputs inputs)
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

                var dbResult = _mainBLL.CheckSignatureMessage(inputs);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.TotalRecords = 0;
                objResponseModel.TotalPages = 0;
                objResponseModel.Result = dbResult;
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
    }
}
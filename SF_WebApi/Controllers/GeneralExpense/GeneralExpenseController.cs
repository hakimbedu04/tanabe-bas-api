using AutoMapper;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.GeneralExpense;
using SF_BusinessLogics.LoginBLL;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using SF_Utils;
using SF_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;

namespace SF_WebApi.Controllers.GeneralExpense
{
    public class GeneralExpenseController : BaseController
    {
        private readonly IGeneralExpense _bll;
        private readonly ILoginBLL _loginbll;
        private IVTLogger _vtLogger;

        public GeneralExpenseController(IGeneralExpense bll, ILoginBLL loginbll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginbll = loginbll;
            _vtLogger = vtLogger;
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetDataTable(BaseInput inputs)
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
                var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
                var viewMapper = Mapper.Map<List<DataTableGeneralExpenseDTO>>(dbResult);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count / inputs.PageSize) +
                                                  (viewMapper.Count % inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize).Take(inputs.PageSize);
                }

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                //objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
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
        public IHttpActionResult GetDataTableDetail(BaseInput inputs)
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
                var dbResult = _bll.getDataTableDetail(inputs.hrd_id);
                var viewMapper = Mapper.Map<List<DataTableGeneralExpenseDetailDTO>>(dbResult);

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
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetDataTableDetailApprove(BaseInput inputs)
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
                var dbResult = _bll.getDataTableDetailApprove(inputs.hrd_id);
                var viewMapper = Mapper.Map<List<DataTableGeneralExpenseDetailApproveDTO>>(dbResult);

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
                _vtLogger.Err(ex, new List<object> { inputs});
                return Json(objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult getDataTableDetailAttach(BaseInput inputs)
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
                var dbResult = _bll.getDataTableDetailAttach(inputs.hrd_id);
                //var viewMapper = Mapper.Map<List<DataTableGeneralExpenseDetailAttachDTO>>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = dbResult;
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
        public IHttpActionResult DeleteData(BaseInput inputs)
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
                var dbResult = _bll.deleteData(inputs.hrd_id);
                var viewMapper = Mapper.Map<int>(dbResult);

                objResponseModel.Status = (viewMapper > 0) ? true : false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
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
        public IHttpActionResult Add(BaseInput inputs)
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

                XDocument detailExpenseXmlDoc = null;
                List<DetailExpenseModel> detailexpenselist = inputs.detailExpense as List<DetailExpenseModel>;
                detailExpenseXmlDoc = GenerateListExpenseDetailToXML(detailexpenselist);

                XDocument detailExpenseAttachmentXmlDoc = null;
                List<t_spAttachmentModel> detailexpenseattachmentlist = inputs.attachmentExpense as List<t_spAttachmentModel>;
                detailExpenseAttachmentXmlDoc = GenerateListAttachmentToXML(detailexpenseattachmentlist);

                var dbResult = _bll.add(model.rep_id, inputs.dtl_desc, detailExpenseXmlDoc.ToString(), detailExpenseAttachmentXmlDoc.ToString());
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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
        public HttpResponseMessage UploadAttachment()
        {
            var objResponseModel = new ResponseModel();

            var auth = Convert.ToString(HttpContext.Current.Request.Params["Auth"]);

            var repid = Decrypt(auth, true);
            var model = _loginbll.CheckMvaUserInfo(repid);
            var rep_reg = model.rep_region.Replace(" ", String.Empty);
            var validateAccess = ValidateAuth(auth);
            if (!validateAccess)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.NotFound, objResponseModel);
            }
            try
            {
                //upload file to server
                var httpRequest = HttpContext.Current.Request;
                string fileName = "", path = "", headerPath = "";

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        #region initialize
                        var settingsReader = new AppSettingsReader();
                        headerPath = (string)settingsReader.GetValue("UploadAttachmentPath", typeof(String)) + "Expense/" + rep_reg + "/" + repid + "/";
                        //headerPath = "~/WebVirtualDir" + "/Expense/" + rep_reg + "/" + repid + "/";
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

                TempFile tempFile = new TempFile();
                tempFile.nameFile = fileName;
                tempFile.pathFile = headerPath.Replace("WebVirtualDir", "Files") + fileName;

                objResponseModel.Result = tempFile;
                return Request.CreateResponse(HttpStatusCode.Created, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { objResponseModel });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }

        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult EditDetailAdd(BaseInput inputs)
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

                var dbResult = _bll.editDetailAdd(model.rep_id, inputs.hrd_id, inputs.dtl_desc, inputs.value, null, null, null);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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
        public IHttpActionResult EditDetailEdit(BaseInput inputs)
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

                var dbResult = _bll.editDetailEdit(inputs.dtl_id, inputs.dtl_desc, inputs.value, null, null, null);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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

        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult EditDetailDelete(BaseInput inputs)
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

                var dbResult = _bll.editDetailDelete(inputs.dtl_id);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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

        public IHttpActionResult EditDetailAttachmentDelete(BaseInput inputs)
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

                var dbResult = _bll.editDetailAttachmentDelete(inputs.gla_id);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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

        public IHttpActionResult EditDetailAttachmentAdd(BaseInput inputs)
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

                var dbResult = _bll.editDetailAttachmentAdd(inputs.hrd_id, inputs.fileName, inputs.filePath);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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
        public IHttpActionResult EditDescription(BaseInput inputs)
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

                var dbResult = _bll.editDescription(inputs.hrd_id, inputs.dtl_desc);
                if (dbResult > 0)
                    objResponseModel.Result = "Succeess";
                else
                    objResponseModel.Result = "Failed";
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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

        private XDocument GenerateListAttachmentToXML(List<t_spAttachmentModel> list)
        {
            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("ListDetail", from cls in list select new XElement("List", new XElement("spf_id", cls.spf_id), new XElement("spr_id", cls.spr_id), new XElement("spf_file_name", cls.spf_file_name), new XElement("spf_file_path", cls.spf_file_path))));
            return xmlDocument;
        }


        private XDocument GenerateListExpenseDetailToXML(List<DetailExpenseModel> list)
        {
            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("ListDetail", from cls in list select new XElement("List", new XElement("id", cls.id), new XElement("detail_description", cls.detail_description), new XElement("detail_value", cls.detail_value))));

            return xmlDocument;
        }

        private class TempFile
        {
            public string nameFile { get; set; }
            public string pathFile { get; set; }
        }
    }
}

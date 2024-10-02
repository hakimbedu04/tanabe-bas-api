using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.Visit;
using SF_Domain.Inputs.Visit;
using SF_Utils;
using SF_WebApi.Models;

namespace SF_WebApi.Controllers.Visit
{
    public class VisitAssociatedController : BaseController
    {
        private readonly ILoginBLL _loginbll;
        private readonly IVisitBLL _mainBLL;
        private readonly IVTLogger _vtLogger;

        public VisitAssociatedController(ILoginBLL loginbll, IVisitBLL mainBLL, IVTLogger vtLogger)
        {
            _loginbll = loginbll;
            _mainBLL = mainBLL;
            _vtLogger = vtLogger;
        }

        [HttpPost]
        public HttpResponseMessage DataViewPartial(VisitAssociatedRequestModel inputs)
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
                var dbResult = _mainBLL.GetSPVisitAssociated(inputs);
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
        public HttpResponseMessage GetNotification(VisitInputs inputs)
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
                var dbResult = _mainBLL.GetSPVisitAssociatedNotification(inputs);
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
        public HttpResponseMessage Confirm(VisitAssociatedConfirmRequestModel inputs)
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

                var VisitAssociated = _mainBLL.GetVisitAssociatedById(inputs.AssociateId);
                if (VisitAssociated == null)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorVisitAssociatedIdNotFound);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                if (inputs.RepId != VisitAssociated.rep_id_invited)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorVisitAssociatedNotAuthorized);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                _mainBLL.SPConfirmVisitAssoicated(inputs);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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
        public HttpResponseMessage Reject(VisitAssociatedConfirmRequestModel inputs)
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

                var VisitAssociated = _mainBLL.GetVisitAssociatedById(inputs.AssociateId);
                if (VisitAssociated == null)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorVisitAssociatedIdNotFound);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                if (inputs.RepId != VisitAssociated.rep_id_invited)
                {
                    objResponseModel.Status = false;
                    objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.ErrorVisitAssociatedNotAuthorized);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
                }

                _mainBLL.SPRejectVisitAssoicated(inputs);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);

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
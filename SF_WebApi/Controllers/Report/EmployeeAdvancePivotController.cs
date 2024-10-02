using SF_BusinessLogics.LoginBLL;
using SF_Domain.Inputs;
using SF_Utils;
using SF_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SF_WebApi.Controllers.Report
{
    public class EmployeeAdvancePivotController : BaseController
    {
        private readonly ILoginBLL _loginbll;

        public EmployeeAdvancePivotController(ILoginBLL loginbll)
        {
            _loginbll = loginbll;
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult GetReport(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
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
            objResponseModel.Status = true;
            objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
            objResponseModel.Result = GetHost() + "/Report/EmployeeAdvancePivot.aspx?r=" + model.rep_id.Replace(" ", string.Empty);
            return Ok(objResponseModel);
        }
    }
}
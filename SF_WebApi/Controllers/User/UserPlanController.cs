using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.User;
using SF_Domain.DTOs.BAS;
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
using System.Data.Entity.Core.Objects;
using System.Data;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Configuration;
using SF_BusinessLogics.ErrLogs;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.User
{
    public class UserPlanController : BaseController
    {
        private readonly IUserPlanBLL _bll;
        private readonly ILoginBLL _loginbll;
        private IVTLogger _vtLogger;

        public UserPlanController(IUserPlanBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
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
                var viewMapper = Mapper.Map<List<DataTableUserPlanDTO>>(dbResult);
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
        public IHttpActionResult Requests(BaseInput inputs)
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
                var dbResult = _bll.requests(model.rep_id, inputs.Month, inputs.Year, model.rep_name, model.rep_region, model.rep_bo);
                //if (dbResult == 1)
                SendRequestVerification(model.rep_name, model.rep_region, model.rep_bo, model.bo_description, model.rep_id, inputs.Month + "-" + inputs.Year, model.nama_am);
                var viewMapper = Mapper.Map<int>(dbResult);

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
                List<DataTableUserPlanDetailDTO> dbResult = _bll.getDataTableDetail(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.sales_id).ToList();
                var viewMapper = Mapper.Map<List<DataTableUserPlanDetailDTO>>(dbResult.ToList());

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
        public IHttpActionResult GetListProduct(BaseInput inputs)
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
                List<ProductUserDTO> dbResult = _bll.getListProduct().ToList();
                var viewMapper = Mapper.Map<List<ProductUserDTO>>(dbResult.ToList());

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
        public IHttpActionResult AddProduct(BaseInput inputs)
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
                int dbResult = _bll.addProduct(inputs.sales_id, model.rep_id, inputs.prd_code, inputs.tx_target_qty, inputs.tx_note);
                var viewMapper = Mapper.Map<int>(dbResult);

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
        public IHttpActionResult UpdateProduct(BaseInput inputs)
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
                _bll.updateProduct(inputs.sp_id, inputs.tx_target_qty);

                //var viewMapper = Mapper.Map<int>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = "";
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
        public IHttpActionResult DeleteProduct(BaseInput inputs)
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
                _bll.deleteProduct(inputs.sp_id);

                //var viewMapper = Mapper.Map<int>(dbResult);

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = "";
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
        public HttpResponseMessage ExportExcel(BaseInput inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
            var currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "UserPlan" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "user_plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
            var resultFilePath = addressPath + "/" + resultFileName;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
            }
            MemoryStream ms = new MemoryStream();

            string strTempFileName = Path.GetTempFileName();
            string strFileName = strTempFileName.Replace(".tmp", string.Empty) + ".xlsx";
            if (File.Exists(strFileName))
            {
                File.Delete(strFileName);
            }

            var templateFile = Enums.ExcelTemplate.User_Plan + ".xlsx";
            var templateFileName = HttpContext.Current.Server.MapPath(Constants.UserPlan + templateFile);
            if (File.Exists(templateFileName))
            {
                File.Copy(templateFileName, strFileName);
            }

            using (var slDoc = new SLDocument(strFileName))
            {
                #region content
                //start row values
                var iRow = 2;
                var tempField = "";
                foreach (var item in dbResult)
                {

                    #region sub header
                    SLStyle styleSubHeader = slDoc.CreateStyle();
                    styleSubHeader.Font.FontName = "Calibri";
                    styleSubHeader.Font.FontSize = 11;
                    styleSubHeader.Font.Bold = true;

                    if (tempField != item.dr_name)
                    {
                        slDoc.SetCellValue(iRow, 1, "DR. NAME : " + item.dr_name);
                        tempField = item.dr_name;
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

                    slDoc.SetCellValue(iRow, 1, item.sales_id);
                    slDoc.SetCellValue(iRow, 2, item.dr_code.HasValue ? item.dr_code.Value : 0);
                    slDoc.SetCellValue(iRow, 3, item.sales_plan.HasValue ? item.sales_plan.Value : 0);
                    slDoc.SetCellValue(iRow, 4, item.sales_plan_verification_status.HasValue ? item.sales_plan_verification_status.Value : 0);
                    slDoc.SetCellValue(iRow, 5, item.dr_spec);
                    slDoc.SetCellValue(iRow, 6, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 7, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 8, item.prd_name);
                    slDoc.SetCellValue(iRow, 9, item.prd_price.HasValue ? item.prd_price.Value : 0);
                    slDoc.SetCellValue(iRow, 10, "");
                    slDoc.SetCellValue(iRow, 11, item.sp_target_qty.HasValue ? item.sp_target_qty.Value : 0);
                    slDoc.SetCellValue(iRow, 12, item.sp_target_value.HasValue ? item.sp_target_value.Value : 0);
                    slDoc.SetCellValue(iRow, 13, item.sp_sales_qty.HasValue ? item.sp_sales_qty.Value : 0);
                    slDoc.SetCellValue(iRow, 14, item.sp_sales_value.HasValue ? item.sp_sales_value.Value : 0);
                    slDoc.SetCellStyle(iRow, 1, iRow, 13, style);
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
            var objResponseModel = new ResponseModel();

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                objResponseModel.Result = GetHostNoHttp() + resultFilePath.Substring(1);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.InnerException.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public HttpResponseMessage ExportExcels(BaseInput inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
            DataTable boundTable = new DataTable();
            boundTable.Columns.Add("USER ID", typeof(string));
            boundTable.Columns.Add("DR. CODE", typeof(string));
            boundTable.Columns.Add("USER PLAN", typeof(string));
            boundTable.Columns.Add("VER. PLAN", typeof(string));
            //boundTable.Columns.Add("DR. NAME", typeof(string));
            boundTable.Columns.Add("DR. SPEC.", typeof(string));
            boundTable.Columns.Add("QUAD", typeof(string));
            boundTable.Columns.Add("MONITORING", typeof(string));
            boundTable.Columns.Add("PRODUCT NAME", typeof(string));
            boundTable.Columns.Add("PRICE", typeof(string));
            boundTable.Columns.Add("CATEGORY", typeof(string));

            boundTable.Columns.Add("TARGET QTY", typeof(string));
            boundTable.Columns.Add("TARGET VALUE", typeof(string));
            boundTable.Columns.Add("SALES QTY", typeof(string));
            boundTable.Columns.Add("SALES VALUE", typeof(string));
            string tempDrName = "";
            foreach (DataTableUserPlanDTO item in dbResult)
            {
                dynamic dr = boundTable.NewRow();
                if (tempDrName != item.dr_name)
                {
                    dr["USER ID"] = item.dr_name;
                    dr["DR. CODE"] = "";
                    dr["USER PLAN"] = "";
                    dr["VER. PLAN"] = "";
                    //dr["DR. NAME"] = "warp";
                    dr["DR. SPEC."] = "";
                    dr["QUAD"] = "";
                    dr["MONITORING"] = "";
                    dr["PRODUCT NAME"] = "";
                    dr["PRICE"] = "";
                    dr["CATEGORY"] = "";
                    dr["TARGET QTY"] = "";
                    dr["TARGET VALUE"] = "";
                    dr["SALES QTY"] = "";
                    dr["SALES VALUE"] = "";
                    boundTable.Rows.Add(dr);
                    tempDrName = item.dr_name;
                }

                dr = boundTable.NewRow();
                dr["USER ID"] = item.sales_id;
                dr["DR. CODE"] = item.dr_code;
                dr["USER PLAN"] = item.sales_plan;
                dr["VER. PLAN"] = item.sales_plan_verification_status;
                //dr["DR. NAME"] = item.dr_name;
                dr["DR. SPEC."] = item.dr_spec;
                dr["QUAD"] = item.dr_quadrant;
                dr["MONITORING"] = item.dr_monitoring;
                dr["PRODUCT NAME"] = item.prd_name;
                dr["PRICE"] = item.prd_price;
                dr["CATEGORY"] = "";
                dr["TARGET QTY"] = item.sp_target_qty;
                dr["TARGET VALUE"] = item.sp_target_value;
                dr["SALES QTY"] = item.sp_sales_qty;
                dr["SALES VALUE"] = item.sp_sales_value;
                boundTable.Rows.Add(dr);
            }

            var currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var dataTableToExcel = new DataTableToExcel();

            #region upload file
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "UserPlan" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "user_Plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
            var resultFilePath = addressPath + "/" + resultFileName;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            dataTableToExcel.generateExcels(boundTable, HttpContext.Current.Server.MapPath(resultFilePath));

            #endregion

            var objResponseModel = new ResponseModel();

            try
            {
                inputs.RepId = Decrypt(inputs.Auth, true);
                objResponseModel.Result = GetHostNoHttp() + resultFilePath.Substring(1);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
                objResponseModel.DetailMessage = ex.InnerException.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExportPdf(BaseInput inputs)
        {
            var objResponseModel = new ResponseModel();
            var validateAccess = ValidateAuth(inputs.Auth);
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
                objResponseModel.DetailMessage = ex.InnerException.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        const string UploadPdfDirectory = "/Asset/Files/Downloads/Pdf/UserPlan/";
        public string GeneratePdf(BaseInput inputs)
        {
            #region initialitation
            var returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
            var fonttype = BaseFont.TIMES_ROMAN;
            Font tableHeaderFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.WHITE);
            Font tableContentFont = FontFactory.GetFont(fonttype, 7, Font.NORMAL, BaseColor.BLACK);
            Font TitleFont = FontFactory.GetFont(fonttype, 11, Font.NORMAL, BaseColor.BLACK);
            var currentDate = DateTime.Now;
            Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            #endregion
            #region upload file
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GeneratePdfPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "UserPlan" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "user_plan_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
            var resultFilePath = addressPath + "/" + resultFileName;

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
            Paragraph title = new Paragraph();
            title.Add(new Paragraph("PT. TANABE INDONESIA ", TitleFont));
            title.Add(new Paragraph("USER PLAN ", TitleFont));
            title.IndentationLeft = 10;

            Paragraph header = new Paragraph();
            header.Add(new Paragraph("Nama       : " + model.rep_name, TitleFont));
            header.Add(new Paragraph("Regional   : " + model.rep_region, TitleFont));
            header.Add(new Paragraph("BO         : " + model.rep_bo, TitleFont));
            header.Add(new Paragraph("Month      : ____________________________", TitleFont));
            header.IndentationLeft = 10;

            document.Add(title);
            document.Add(header);

            #region table
            DataTable dt = new DataTable();
            dt.Columns.Add("USER ID", typeof(string));
            dt.Columns.Add("DR. CODE", typeof(string));
            dt.Columns.Add("USER PLAN", typeof(string));
            dt.Columns.Add("VER. PLAN", typeof(string));
            //dt.Columns.Add("DR. NAME", typeof(string));
            dt.Columns.Add("DR. SPEC.", typeof(string));
            dt.Columns.Add("QUAD", typeof(string));
            dt.Columns.Add("MONITORING", typeof(string));
            dt.Columns.Add("PRODUCT NAME", typeof(string));
            dt.Columns.Add("PRICE", typeof(string));
            dt.Columns.Add("CATEGORY", typeof(string));

            dt.Columns.Add("TARGET QTY", typeof(string));
            dt.Columns.Add("TARGET VALUE", typeof(string));
            dt.Columns.Add("SALES QTY", typeof(string));
            dt.Columns.Add("SALES VALUE", typeof(string));

            string tempDrName = "";
            foreach (DataTableUserPlanDTO item in dbResult)
            {
                dynamic dr = dt.NewRow();
                if (tempDrName != item.dr_name)
                {
                    dr["USER ID"] = item.dr_name;
                    dr["DR. CODE"] = "";
                    dr["USER PLAN"] = "";
                    dr["VER. PLAN"] = "";
                    //dr["DR. NAME"] = "warp";
                    dr["DR. SPEC."] = "";
                    dr["QUAD"] = "";
                    dr["MONITORING"] = "";
                    dr["PRODUCT NAME"] = "";
                    dr["PRICE"] = "";
                    dr["CATEGORY"] = "";
                    dr["TARGET QTY"] = "";
                    dr["TARGET VALUE"] = "";
                    dr["SALES QTY"] = "";
                    dr["SALES VALUE"] = "";
                    dt.Rows.Add(dr);
                    tempDrName = item.dr_name;
                }

                dr = dt.NewRow();
                dr["USER ID"] = item.sales_id;
                dr["DR. CODE"] = item.dr_code;
                dr["USER PLAN"] = item.sales_plan;
                dr["VER. PLAN"] = item.sales_plan_verification_status;
                //dr["DR. NAME"] = item.dr_name;
                dr["DR. SPEC."] = item.dr_spec;
                dr["QUAD"] = item.dr_quadrant;
                dr["MONITORING"] = item.dr_monitoring;
                dr["PRODUCT NAME"] = item.prd_name;
                dr["PRICE"] = item.prd_price;
                dr["CATEGORY"] = "";
                dr["TARGET QTY"] = item.sp_target_qty;
                dr["TARGET VALUE"] = item.sp_target_value;
                dr["SALES QTY"] = item.sp_sales_qty;
                dr["SALES VALUE"] = item.sp_sales_value;
                dt.Rows.Add(dr);
            }

            //Adding  PdfPTable  
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //float[] widths = new float[] { 5f, 6f, 4f, 4f, 3f, 4f, 6f, 7f, 4f, 12f, 4f, 5f, 3f, 15f, 5f, 5f, 6f, 5f };
            //table.SetWidths(widths);

            PdfPCell cell = new PdfPCell();

            cell.PaddingBottom = 5;
            cell.Border = 2;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#666666"));
                string cellText = HttpContext.Current.Server.HtmlDecode(dt.Columns[i].ColumnName);
                cell.Phrase = new Phrase(cellText, tableHeaderFont);
                cell.BorderWidthTop = .2f;
                cell.BorderWidthBottom = .2f;
                cell.BorderWidthLeft = .2f;
                cell.BorderWidthRight = .2f;
                cell.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#bfbfbf"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.FixedHeight = 15f;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == "")
                {
                    cell.Colspan = dt.Columns.Count;
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#cccccc"));
                    string cellText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][0].ToString());
                    cell.Phrase = new Phrase(cellText, tableContentFont);
                    cell.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#999999"));
                    cell.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#999999"));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
                else
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //bfbfbf
                        cell.Colspan = 0;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                        string cellText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][j].ToString());
                        cell.Phrase = new Phrase(cellText, tableContentFont);
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
            //footer.IndentationLeft = 10;
            //document.Add(footer);

            #endregion
            document.Close();
            //byte[] bytes = memoryStream.ToArray();
            //memoryStream.Close();
            return returnVal;
        }

        public void SendRequestVerification(string rep_name, string rep_region, string bo, string sbo, string rep_id, string month_plan, string am)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_sales_plan.htm"));
            string readFile = reader.ReadToEnd();
            var userDetail = _loginbll.CheckMvaUserInfo(rep_id);
            string email_body = readFile;
            var currentDate = DateTime.Now;
            email_body = email_body.Replace("$$RECEIVER$$", am);
            email_body = email_body.Replace("$$rep_name$$", rep_name);
            email_body = email_body.Replace("$$rep_region$$", rep_region);
            email_body = email_body.Replace("$$bo$$", bo);
            email_body = email_body.Replace("$$sbo$$", sbo);
            email_body = email_body.Replace("$$rep_id$$", rep_id);
            email_body = email_body.Replace("$$month_plan$$", month_plan);
            email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();
            var settingsReader = new AppSettingsReader();
            var key = (string)settingsReader.GetValue("fromEmailAddress", typeof(String)); // Get the key from config file
            mailMessage.From = new MailAddress(key);
            //mailMessage.To.Add(new MailAddress(userDetail.email_am)); 
            //mailMessage.To.Add(new MailAddress("sidiq.miftah@gmail.com"));
            ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));
            ////mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
            mailMessage.Subject = "Sales Promotion Verification Request for " + rep_name + " - " + "Month Event Plan - " + month_plan;

            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = email_body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            reader.Dispose();
        }
    }
}

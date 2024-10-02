using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace SF_WebApi.Controllers.Visit
{
    public class VisitHistoryController : BaseController
    {
        private const string UploadPdfDirectory = "/Asset/Files/Downloads/Pdf/VisitHistory/";
        private readonly ILoginBLL _loginbll;
        private readonly IVisitBLL _mainBLL;
        private readonly IVTLogger _vtLogger;

        public VisitHistoryController(ILoginBLL loginbll, IVisitBLL mainBLL, IVTLogger vtLogger)
        {
            _loginbll = loginbll;
            _mainBLL = mainBLL;
            _vtLogger = vtLogger;
        }

        [HttpPost]
        public HttpResponseMessage DataViewPartial(VisitInputs inputs)
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
                List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> dbResult = _mainBLL.GetVisitHistory(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel>>(dbResult);
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
        public HttpResponseMessage DataViewPartialSearch(VisitInputs inputs)
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
                List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> dbResult = _mainBLL.GetVisitHistorySearch(inputs);
                var viewMapper = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel>>(dbResult);
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
        public HttpResponseMessage GetDetailHistory(VisitInputs inputs)
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
                List<v_visit_product_DTO> dbResult = _mainBLL.GetDataDetail(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_ViewModel>>(dbResult);
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
        public HttpResponseMessage ShowAllTopic(VisitInputs inputs)
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
                inputs.Host = GetHost();
                List<v_visit_product_topic_DTO> dbResult = _mainBLL.GetProductTopic(inputs);
                var viewMapper = Mapper.Map<List<v_visit_product_topic_ViewModel>>(dbResult);
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
                List<SummaryDoctor_DTO> dbResult = _mainBLL.GetQuadrantSummaryHistory(inputs);
                var viewMapper = Mapper.Map<List<SummaryDoctor_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper;
                return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
            }
            catch (Exception ex)
            {
                objResponseModel.Status = false;
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public string GenerateExcel(VisitInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            inputs.ActionSource = "visitrealization";
            List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> dbResult = _mainBLL.GetVisitHistory(inputs);
            var viewModel = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel>>(dbResult);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
            string addressPath = headerPath + "VisitHistory" + "/" + model.rep_id;
            string resultFileName = "visit_history_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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

            string templateFile = Enums.ExcelTemplate.Visit_History + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.VisitHistory + templateFile);
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

                foreach (SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel item in viewModel)
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
                    slDoc.SetCellValue(iRow, 8, item.visit_plan_verification_status);
                    slDoc.SetCellValue(iRow, 9, item.visit_real_verification_status);
                    slDoc.SetCellValue(iRow, 10, item.dr_code.HasValue ? Convert.ToString(item.dr_code.Value) : "");
                    slDoc.SetCellValue(iRow, 11, item.dr_name);
                    slDoc.SetCellValue(iRow, 12, item.dr_spec);
                    slDoc.SetCellValue(iRow, 13, item.dr_sub_spec);
                    slDoc.SetCellValue(iRow, 14, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 15, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 16, item.dr_dk_lk);
                    slDoc.SetCellStyle(iRow, 1, iRow, 15, style);
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
        //    inputs.ActionSource = "visitrealization";
        //    var dbResult = _mainBLL.GetVisitHistory(inputs);
        //    var viewModel = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_ViewModel>>(dbResult);
        //    DataTable boundTable = new DataTable();
        //    boundTable.Columns.Add("PLAN ID", typeof(string));
        //    boundTable.Columns.Add("DATE PLAN", typeof(string));
        //    boundTable.Columns.Add("VPLAN", typeof(string));
        //    boundTable.Columns.Add("VREAL", typeof(string));
        //    boundTable.Columns.Add("SP", typeof(string));
        //    boundTable.Columns.Add("VALUE", typeof(string));
        //    boundTable.Columns.Add("VISIT INFO", typeof(string));

        //    boundTable.Columns.Add("PLAN Ver.STATUS", typeof(string));
        //    boundTable.Columns.Add("REAL Ver.STATUS", typeof(string));
        //    boundTable.Columns.Add("DR CODE", typeof(string));
        //    boundTable.Columns.Add("DR NAME", typeof(string));
        //    boundTable.Columns.Add("DR SPEC", typeof(string));

        //    boundTable.Columns.Add("SUB SPEC", typeof(string));
        //    boundTable.Columns.Add("QRD", typeof(string));
        //    boundTable.Columns.Add("MTG", typeof(string));
        //    boundTable.Columns.Add("DK/LK", typeof(string));
        //    foreach (SP_SELECT_FINISHED_VISIT_ViewModel item in viewModel)
        //    {
        //        dynamic dr = boundTable.NewRow();
        //        dr["PLAN ID"] = item.visit_id;
        //        dr["DATE PLAN"] = item.visit_date_plan.HasValue ? item.visit_date_plan.Value.ToString("yyyy MMMM dd") : " ";
        //        dr["VPLAN"] = item.visit_plan;
        //        dr["VREAL"] = item.visit_realization;
        //        dr["SP"] = item.visit_sp;
        //        dr["VALUE"] = item.visit_sp_value;
        //        dr["VISIT INFO"] = item.visit_info;
        //        dr["PLAN Ver.STATUS"] = item.visit_plan_verification_status;
        //        dr["REAL Ver.STATUS"] = item.visit_real_verification_status;
        //        dr["DR CODE"] = item.dr_code.HasValue ? item.dr_code.Value.ToString() : "";
        //        dr["DR NAME"] = item.dr_name;
        //        dr["DR SPEC"] = item.dr_spec;

        //        dr["SUB SPEC"] = item.dr_sub_spec;
        //        dr["QRD"] = item.dr_quadrant;
        //        dr["MTG"] = item.dr_monitoring;
        //        dr["DK/LK"] = item.dr_dk_lk;
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
        //    var addressPath = headerPath + "VisitHistory" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
        //    var resultFileName = "visit_History_" + GetStringPattern().Replace(model.rep_name, "_") + "_"+ inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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
            inputs.ActionSource = "visitrealization";
            List<SP_SELECT_FINISHED_VISIT_MOBILE_DTO> dbResult = _mainBLL.GetVisitHistory(inputs);
            var viewModel = Mapper.Map<List<SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel>>(dbResult);
            var newinputs = new VisitInputs
            {
                RepId = inputs.RepId,
                Month = inputs.Month,
                Year = inputs.Year
            };
            List<SummaryDoctor_DTO> detailDoctor = _mainBLL.GetQuadrantSummaryHistory(newinputs);
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
            string addressPath = headerPath + "VisitHistory" + "/" + model.rep_id;
                // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "visit_history_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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
            //title.Add(new Paragraph("ACTUAL VISIT ", TitleFont));
            //title.IndentationLeft = 10;

            //Paragraph header = new Paragraph();
            //header.Add(new Paragraph("Nama       : " + model.rep_name, TitleFont));
            //header.Add(new Paragraph("Regional   : " + model.rep_region, TitleFont));
            //header.Add(new Paragraph("BO         : " + model.rep_bo, TitleFont));
            //header.Add(new Paragraph("Month      : ____________________________", TitleFont));
            //header.IndentationLeft = 10;

            document.Add(title);
            //document.Add(header);

            #region table

            var dt = new DataTable();
            dt.Columns.Add("PLAN ID", typeof (string));
            dt.Columns.Add("DATE PLAN", typeof (string));
            dt.Columns.Add("VPLAN", typeof (string));
            dt.Columns.Add("VREAL", typeof (string));
            dt.Columns.Add("SP", typeof (string));
            dt.Columns.Add("VALUE", typeof (string));
            dt.Columns.Add("VISIT INFO", typeof (string));

            dt.Columns.Add("PLAN Ver.STATUS", typeof (string));
            dt.Columns.Add("REAL Ver.STATUS", typeof (string));
            dt.Columns.Add("DR CODE", typeof (string));
            dt.Columns.Add("DR NAME", typeof (string));
            dt.Columns.Add("DR SPEC", typeof (string));

            dt.Columns.Add("SUB SPEC", typeof (string));
            dt.Columns.Add("QRD", typeof (string));
            dt.Columns.Add("MTG", typeof (string));
            dt.Columns.Add("DK/LK", typeof (string));
            foreach (SP_SELECT_FINISHED_VISIT_MOBILE_ViewModel item in viewModel)
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
                dr["PLAN Ver.STATUS"] = item.visit_plan_verification_status;
                dr["REAL Ver.STATUS"] = item.visit_real_verification_status;
                dr["DR CODE"] = item.dr_code.HasValue ? item.dr_code.Value.ToString() : "";
                dr["DR NAME"] = item.dr_name;
                dr["DR SPEC"] = item.dr_spec;

                dr["SUB SPEC"] = item.dr_sub_spec;
                dr["QRD"] = item.dr_quadrant;
                dr["MTG"] = item.dr_monitoring;
                dr["DK/LK"] = item.dr_dk_lk;
                dt.Rows.Add(dr);
            }

            //Adding  PdfPTable  
            var table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;

            float[] widths = {4.5f, 4.5f, 3.5f, 3.5f, 2f, 3.5f, 4.5f, 6f, 6f, 4.5f, 9f, 4f, 5f, 4.5f, 14f, 3f};
            table.SetWidths(widths);
            table.HeaderRows = 10;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var cell = new PdfPCell();

                cell.PaddingBottom = 5;
                cell.Border = 2;
                cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#7a7a79"));
                string cellText = HttpContext.Current.Server.HtmlDecode(dt.Columns[i].ColumnName);
                cell.Phrase = new Phrase(cellText, tableHeaderFont);
                cell.BorderWidthTop = .5f;
                cell.BorderWidthBottom = .5f;
                cell.BorderWidthLeft = .5f;
                cell.BorderWidthRight = .5f;
                cell.BorderColorTop = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorBottom = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorLeft = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.BorderColorRight = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }


            var row = new PdfPCell();
            //writing table Data  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //bfbfbf
                    row.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#ffffff"));
                    string rowText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][j].ToString());
                    row.Phrase = new Phrase(rowText, tableContentFont);
                    row.BorderWidthTop = .5f;
                    row.BorderWidthBottom = .5f;
                    row.BorderWidthLeft = .5f;
                    row.BorderWidthRight = .5f;
                    row.BorderColorTop = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                    row.BorderColorBottom = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                    row.BorderColorLeft = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                    row.BorderColorRight = new BaseColor(ColorTranslator.FromHtml("#bfbfbf"));
                    row.HorizontalAlignment = Element.ALIGN_LEFT;

                    table.AddCell(row);
                }
            }
            document.Add(new Chunk("\n", TitleFont));
            document.Add(table);

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
            document.Dispose();

            return returnVal;
        }
    }
}
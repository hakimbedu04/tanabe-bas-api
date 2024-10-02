using System.Configuration;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.SP;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using SF_Utils;
using SF_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using SF_WebApi.Models.BAS.SP;
using SF_BusinessLogics.ErrLogs;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.SP
{
    public class SpActualController : BaseController
    {
        private readonly ISPActualBLL _bll;
        private readonly ILoginBLL _loginbll;
        private IVTLogger _vtLogger;

        public SpActualController(ISPActualBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginbll = loginbll;
            _vtLogger = vtLogger;
        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(BaseInput))]
        public IHttpActionResult DoctorSummary(BaseInput inputs)
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
                var viewMapper = Mapper.Map<List<DataTableSPActualDTO>>(dbResult);
                var res = viewMapper.Select(x => new
                {
                    x.dr_plan_sum,
                    x.budget_plan_sum
                }).ToList();

                var data = new SpSummaryDoctor()
                {
                    TotalDoctor = (int)res.Sum(y => y.dr_plan_sum),
                    Exp = (int)res.Sum(y => y.budget_plan_sum)
                };

                objResponseModel.Result = data;
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
                var viewMapper = Mapper.Map<List<DataTableSPActualDTO>>(dbResult);

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
        public IHttpActionResult PrintView(BaseInput inputs)
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

                //ViewData("spr_no") = model(0).spr_no
                //ViewData("spr_date") = CDate(model(0).spr_date_created).ToShortDateString()
                //ViewData("initiator_position") = model(0).rep_position
                //ViewData("initiator_name") = model(0).initiator_name
                //ViewData("initiator_am") = model(0).nama_am
                //ViewData("initiator_branch") = model(0).rep_bo
                //ViewData("initiator_region") = model(0).rep_region

                //ViewData("event_name") = model(0).e_name
                //ViewData("event_topic") = model(0).e_topic
                //ViewData("event_place") = model(0).e_place
                //ViewData("event_date_start") = CDate(model(0).e_dt_start).ToShortDateString()
                //ViewData("event_date_end") = CDate(model(0).e_dt_end).ToShortDateString()
                var result = GetHost() + "/Printview/PrintAll?sprId=" + inputs.spr_id +
                    "&sp_type=" + inputs.sp_type +
                    "&spr_no=" + inputs.spr_no +
                    "&spr_date=" + inputs.spr_date +
                    "&initiator_position=" + model.rep_position +
                    "&initiator_name=" + model.rep_name +
                    "&initiator_am=" + model.rep_am +
                    "&initiator_branch=" + model.rep_bo +
                    "&initiator_region=" + model.rep_region +
                    "&event_name=" + inputs.event_name +
                    "&event_date_start=" + inputs.startDate +
                    "&event_topic=" + inputs.topic +
                    "&event_date_end=" + inputs.endDate +
                    "&event_place=" + inputs.event_place;

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = result.Replace(" ", "%20");
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
        public IHttpActionResult ExpenseView(BaseInput inputs)
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

                //ViewData("spr_no") = model(0).spr_no
                //ViewData("spr_date") = CDate(model(0).spr_date_created).ToShortDateString()
                //ViewData("initiator_position") = model(0).rep_position
                //ViewData("initiator_name") = model(0).initiator_name
                //ViewData("initiator_am") = model(0).nama_am
                //ViewData("initiator_branch") = model(0).rep_bo
                //ViewData("initiator_region") = model(0).rep_region

                //ViewData("event_name") = model(0).e_name
                //ViewData("event_topic") = model(0).e_topic
                //ViewData("event_place") = model(0).e_place
                //ViewData("event_date_start") = CDate(model(0).e_dt_start).ToShortDateString()
                //ViewData("event_date_end") = CDate(model(0).e_dt_end).ToShortDateString()
                var result = GetHost() + "/ExpenseSlipPrintView/PrintAll?spr_id=" + inputs.spr_id +
                    "&spr_date=" + inputs.spr_date +
                    "&initiator_position=" + model.rep_position +
                    "&initiator_name=" + model.rep_name +
                    "&initiator_am=" + model.rep_am +
                    "&initiator_branch=" + model.rep_bo +
                    "&initiator_region=" + model.rep_region +
                    "&event_name=" + inputs.event_name +
                    "&event_date_start=" + inputs.startDate;

                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = result.Replace(" ", "%20");
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
            var addressPath = headerPath + "SalesPromotion" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "sales_promotion_actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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

            var templateFile = Enums.ExcelTemplate.SP_Actual + ".xlsx";
            var templateFileName = HttpContext.Current.Server.MapPath(Constants.SPActual + templateFile);
            if (File.Exists(templateFileName))
            {
                File.Copy(templateFileName, strFileName);
            }

            using (var slDoc = new SLDocument(strFileName))
            {
                #region content
                //start row values
                var iRow = 2;

                foreach (var item in dbResult)
                {
                    SLStyle style = slDoc.CreateStyle();
                    style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Font.FontName = "Calibri";
                    style.Font.FontSize = 11;

                    slDoc.SetCellValue(iRow, 1, item.spr_id);
                    slDoc.SetCellValue(iRow, 2, item.sp_type);
                    slDoc.SetCellValue(iRow, 3, item.e_name);
                    slDoc.SetCellValue(iRow, 4, item.e_topic);
                    slDoc.SetCellValue(iRow, 5, item.e_place);
                    slDoc.SetCellValue(iRow, 6, item.e_dt_start.HasValue ? item.e_dt_start.Value.ToString("MM/dd/yyyy") : "");
                    slDoc.SetCellValue(iRow, 7, item.e_dt_end.HasValue ? item.e_dt_end.Value.ToString("MM/dd/yyyy") : "");
                    slDoc.SetCellValue(iRow, 8, item.dr_plan_sum.HasValue ? item.dr_plan_sum.Value : 0);
                    slDoc.SetCellValue(iRow, 9, item.dr_real_sum.HasValue ? item.dr_real_sum.Value : 0);
                    slDoc.SetCellValue(iRow, 10, item.budget_plan_sum.HasValue ? item.budget_plan_sum.Value : 0);
                    slDoc.SetCellStyle(iRow, 1, iRow, 9, style);
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
                slDoc.AutoFitColumn(1, 9);
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
            boundTable.Columns.Add("REQ ID", typeof(string));
            boundTable.Columns.Add("SP TYPE", typeof(string));
            boundTable.Columns.Add("EVENT NAME", typeof(string));
            boundTable.Columns.Add("EVENT TOPIC", typeof(string));
            boundTable.Columns.Add("EVENT PLACE", typeof(string));
            boundTable.Columns.Add("DATE START", typeof(string));
            boundTable.Columns.Add("DATE END", typeof(string));
            boundTable.Columns.Add("PARTICIPANT PLAN", typeof(string));
            boundTable.Columns.Add("PARTICIPANT REAL", typeof(string));
            boundTable.Columns.Add("NOMINAL BUDGET PLAN", typeof(string));

            foreach (DataTableSPActualDTO item in dbResult)
            {
                if (item.e_name != null)
                {
                    dynamic dr = boundTable.NewRow();
                    dr = boundTable.NewRow();
                    dr["REQ ID"] = item.spr_id;
                    dr["SP TYPE"] = item.sp_type;
                    dr["EVENT NAME"] = item.e_name;
                    dr["EVENT TOPIC"] = item.e_topic;
                    dr["EVENT PLACE"] = item.e_place;
                    dr["DATE START"] = item.e_dt_start;
                    dr["DATE END"] = item.e_dt_end;
                    dr["PARTICIPANT PLAN"] = item.dr_plan_sum;
                    dr["PARTICIPANT REAL"] = item.dr_real_sum;
                    dr["NOMINAL BUDGET PLAN"] = item.budget_plan_sum;

                    boundTable.Rows.Add(dr);
                }
            }

            var currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var dataTableToExcel = new DataTableToExcel();

            #region upload file
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "SalesPromotion" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "sales_promotion_actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [System.Web.Http.HttpPost]
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
                objResponseModel.DetailMessage = ex.Message;
                _vtLogger.Err(ex, new List<object> { inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

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
            var addressPath = headerPath + "SalesPromotion" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "sales_promotion_actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
            var resultFilePath = addressPath + "/" + resultFileName;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            var output = new FileStream(HttpContext.Current.Server.MapPath(resultFilePath), FileMode.Create);
            PdfWriter.GetInstance(document, output);
            returnVal = GetHostNoHttp() + resultFilePath.Substring(1);

            #endregion

            //Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

            //PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            //string addressPath = UploadPdfDirectory + model.rep_id;
            //var currentDate = DateTime.Now;
            //var resultFileName = "sales_promotion_actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_"+ currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
            //var resultFilePath = addressPath + "/" + resultFileName;

            //if (!Directory.Exists(HttpContext.Current.Server.MapPath(UploadPdfDirectory + model.rep_id)))
            //{
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UploadPdfDirectory + model.rep_id));
            //}
            //if (File.Exists(Path.Combine(UploadPdfDirectory + model.rep_id, resultFileName)))
            //{
            //    // If file found, delete it    
            //    File.Delete(Path.Combine(UploadPdfDirectory + model.rep_id, resultFileName));
            //}
            //if (!Directory.Exists(HttpContext.Current.Server.MapPath(resultFilePath + model.rep_id)))
            //{
            //    PdfWriter.GetInstance(document, new FileStream(AppDomain.CurrentDomain.BaseDirectory + resultFilePath, FileMode.Create));
            //    returnVal = GetHost() + resultFilePath;
            //}

            document.Open();

            Paragraph title = new Paragraph();
            title.Add(new Paragraph("PT. TANABE INDONESIA ", TitleFont));
            title.Add(new Paragraph("SALES PROMOTION ACTUAL ", TitleFont));
            title.IndentationLeft = 10;

            document.Add(title);

            #region table
            DataTable boundTable = new DataTable();
            boundTable.Columns.Add("REQ ID", typeof(string));
            boundTable.Columns.Add("SP TYPE", typeof(string));
            boundTable.Columns.Add("EVENT NAME", typeof(string));
            boundTable.Columns.Add("EVENT TOPIC", typeof(string));
            boundTable.Columns.Add("EVENT PLACE", typeof(string));
            boundTable.Columns.Add("DATE START", typeof(string));
            boundTable.Columns.Add("DATE END", typeof(string));
            boundTable.Columns.Add("PARTICIPANT PLAN", typeof(string));
            boundTable.Columns.Add("PARTICIPANT REAL", typeof(string));
            boundTable.Columns.Add("NOMINAL BUDGET PLAN", typeof(string));

            foreach (DataTableSPActualDTO item in dbResult)
            {
                if (item.e_name != null)
                {
                    dynamic dr = boundTable.NewRow();
                    dr = boundTable.NewRow();
                    dr["REQ ID"] = item.spr_id;
                    dr["SP TYPE"] = item.sp_type;
                    dr["EVENT NAME"] = item.e_name;
                    dr["EVENT TOPIC"] = item.e_topic;
                    dr["EVENT PLACE"] = item.e_place;
                    dr["DATE START"] = item.e_dt_start;
                    dr["DATE END"] = item.e_dt_end;
                    dr["PARTICIPANT PLAN"] = item.dr_plan_sum;
                    dr["PARTICIPANT REAL"] = item.dr_real_sum;
                    dr["NOMINAL BUDGET PLAN"] = item.budget_plan_sum;

                    boundTable.Rows.Add(dr);
                }
            }

            //Adding  PdfPTable  
            PdfPTable table = new PdfPTable(boundTable.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //float[] widths = new float[] { 5f, 6f, 4f, 4f, 3f, 4f, 6f, 7f, 4f, 12f, 4f, 5f, 3f, 15f, 5f, 5f, 6f, 5f };
            //table.SetWidths(widths);

            PdfPCell cell = new PdfPCell();

            cell.PaddingBottom = 5;
            cell.Border = 2;

            for (int i = 0; i < boundTable.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#666666"));
                string cellText = HttpContext.Current.Server.HtmlDecode(boundTable.Columns[i].ColumnName);
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
            for (int i = 0; i < boundTable.Rows.Count; i++)
            {
                if (boundTable.Rows[i][1].ToString() == "")
                {
                    cell.Colspan = boundTable.Columns.Count;
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#cccccc"));
                    string cellText = HttpContext.Current.Server.HtmlDecode(boundTable.Rows[i][0].ToString());
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
                    for (int j = 0; j < boundTable.Columns.Count; j++)
                    {
                        //bfbfbf
                        cell.Colspan = 0;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ffffff"));
                        string cellText = HttpContext.Current.Server.HtmlDecode(boundTable.Rows[i][j].ToString());
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


            document.Close();
            //byte[] bytes = memoryStream.ToArray();
            //memoryStream.Close();



            return returnVal;
        }
    }
}

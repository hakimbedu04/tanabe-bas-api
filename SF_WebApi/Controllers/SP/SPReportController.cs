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
using System.Web.Http.Description;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SF_BusinessLogics.ErrLogs;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.SP;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs;
using SF_Utils;
using SF_WebApi.Models;
using SF_WebApi.Models.BAS.SP;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.SP
{
    public class SPReportController : BaseController
    {
        private const string UploadPdfDirectory = "/Asset/Files/Downloads/Pdf/SalesReport/";
        private readonly ISPReportBLL _bll;
        private readonly ILoginBLL _loginbll;
        private readonly IVTLogger _vtLogger;

        public SPReportController(ISPReportBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginbll = loginbll;
            _vtLogger = vtLogger;
        }

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult GetDataTable(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<DataTableSPReportDTO> dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year,
                    model.rep_position, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
                var viewMapper = Mapper.Map<List<DataTableSPReportDTO>>(dbResult);
                if (viewMapper.Count != 0)
                {
                    objResponseModel.TotalRecords = viewMapper.Count;
                    objResponseModel.TotalPages = (viewMapper.Count/inputs.PageSize) +
                                                  (viewMapper.Count%inputs.PageSize != 0 ? 1 : 0);
                    objResponseModel.Result =
                        viewMapper.Skip((inputs.PageIndex - 1)*inputs.PageSize).Take(inputs.PageSize);
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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult DoctorSummary(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<DataTableSPReportDTO> dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year,
                    model.rep_position, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
                var viewMapper = Mapper.Map<List<DataTableSPReportDTO>>(dbResult);
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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult PrintView(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);

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
                string result = GetHost() + "/Printview/PrintAll?sprId=" + inputs.spr_id +
                                "&sp_type=" + inputs.sp_type +
                                "&spr_no=" + inputs.spr_no +
                                "&spr_date=" + string.Format("{0:dd/MM/yyyy}", inputs.spr_date) +
                                "&initiator_position=" + model.rep_position +
                                "&initiator_name=" + model.rep_name +
                                "&initiator_am=" + model.rep_am +
                                "&initiator_branch=" + model.rep_bo +
                                "&initiator_region=" + model.rep_region +
                                "&event_name=" + inputs.event_name +
                                "&event_date_start=" + string.Format("{0:dd/MM/yyyy}", inputs.startDate) +
                                "&event_topic=" + inputs.topic +
                                "&event_date_end=" + string.Format("{0:dd/MM/yyyy}", inputs.endDate) +
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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExportExcel(BaseInput inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<DataTableSPReportDTO> dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year,
                model.rep_position, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
                //~/Asset/Files/Downloads/Pdf/
            string addressPath = headerPath + "SalesPromotion" + "/" + model.rep_id;
                // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "sales_promotion_report_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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

            string templateFile = Enums.ExcelTemplate.SP_Report + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.SPReport + templateFile);
            if (File.Exists(templateFileName))
            {
                File.Copy(templateFileName, strFileName);
            }

            using (var slDoc = new SLDocument(strFileName))
            {
                #region content

                //start row values
                int iRow = 2;

                foreach (DataTableSPReportDTO item in dbResult)
                {
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
                    slDoc.SetCellValue(iRow, 17, item.dr_real_sum.Value);
                    slDoc.SetCellValue(iRow, 18, item.budget_plan_sum.Value);
                    slDoc.SetCellValue(iRow, 19, item.budget_real_sum.Value);
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

        //const string UploadExcelDirectory = "/Asset/Files/Downloads/Excel/SalesPromotion/";
        //public HttpResponseMessage ExportExcels(BaseInput inputs)
        //{
        //    inputs.RepId = Decrypt(inputs.Auth, true);
        //    var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
        //    var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, model.rep_position, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
        //    DataTable boundTable = new DataTable();
        //    boundTable.Columns.Add("REQ_ID", typeof(string));
        //    boundTable.Columns.Add("SPR NO", typeof(string));
        //    boundTable.Columns.Add("SP", typeof(string));
        //    boundTable.Columns.Add("EVENT NAME", typeof(string));
        //    boundTable.Columns.Add("TOPIC", typeof(string));
        //    boundTable.Columns.Add("PLACE", typeof(string));
        //    boundTable.Columns.Add("GP", typeof(string));
        //    boundTable.Columns.Add("GP Pax", typeof(string));
        //    boundTable.Columns.Add("Spec", typeof(string));
        //    boundTable.Columns.Add("Spec Pax", typeof(string));
        //    boundTable.Columns.Add("Nurse", typeof(string));
        //    boundTable.Columns.Add("Nurse Pax", typeof(string));
        //    boundTable.Columns.Add("Others", typeof(string));
        //    boundTable.Columns.Add("Others Pax", typeof(string));
        //    boundTable.Columns.Add("DATE START", typeof(string));
        //    boundTable.Columns.Add("DATE END", typeof(string));
        //    boundTable.Columns.Add("PARTICIPANT", typeof(string));
        //    boundTable.Columns.Add("PLAN BUDGET", typeof(string));
        //    boundTable.Columns.Add("PLAN STATUS", typeof(string));

        //    foreach (DataTableSPReportDTO item in dbResult)
        //    {
        //        if (item.e_name != null)
        //        {
        //            dynamic dr = boundTable.NewRow();
        //            dr = boundTable.NewRow();
        //            dr["REQ_ID"] = item.spr_id;
        //            dr["SPR NO"] = item.spr_no;
        //            dr["SP"] = item.sp_type;
        //            dr["EVENT NAME"] = item.e_name;
        //            dr["TOPIC"] = item.e_topic;
        //            dr["PLACE"] = item.e_place;
        //            dr["GP"] = item.e_a_gp;
        //            dr["GP Pax"] = item.e_a_gp_pax;
        //            dr["Spec"] = item.e_a_specialist;
        //            dr["Spec Pax"] = item.e_a_specialist_pax;
        //            dr["Nurse"] = item.e_a_nurse;
        //            dr["Nurse Pax"] = item.e_a_nurse_pax;
        //            dr["Others"] = item.e_a_others;
        //            dr["Others Pax"] = item.e_a_others_pax;
        //            dr["DATE START"] = item.e_dt_start;
        //            dr["DATE END"] = item.e_dt_end;
        //            dr["PARTICIPANT"] = item.dr_real_sum;
        //            dr["PLAN BUDGET"] = item.budget_plan_sum;
        //            dr["PLAN STATUS"] = item.budget_real_sum;

        //            boundTable.Rows.Add(dr);
        //        }
        //    }

        //    var currentDate = DateTime.Now;
        //    inputs.Day = currentDate.Day;
        //    inputs.Month = currentDate.Month;
        //    inputs.Year = currentDate.Year;
        //    var dataTableToExcel = new DataTableToExcel();

        //    #region upload file
        //    var settingsReader = new AppSettingsReader();
        //    var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
        //    var addressPath = headerPath + "SalesPromotion" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
        //    var resultFileName = "sales_promotion_report_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
        //    var resultFilePath = addressPath + "/" + resultFileName;

        //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
        //    {
        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
        //    }
        //    if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
        //    {
        //        File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
        //    }
        //    //dataTableToExcel.generateExcels(boundTable, HttpContext.Current.Server.MapPath(resultFilePath));
        //    BuildWorkbook.BuildWorkbooks(boundTable, HttpContext.Current.Server.MapPath(resultFilePath));
        //    #endregion

        //    var objResponseModel = new ResponseModel();

        //    try
        //    {
        //        inputs.RepId = Decrypt(inputs.Auth, true);
        //        objResponseModel.Result = GetHostNoHttp() + resultFilePath.Substring(1);
        //        objResponseModel.Status = true;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
        //        return Request.CreateResponse(HttpStatusCode.OK, objResponseModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponseModel.Status = false;
        //        objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.InternalServerError);
        //        objResponseModel.DetailMessage = ex.InnerException.Message;
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage ExportPdf(BaseInput inputs)
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
                objResponseModel.DetailMessage = ex.InnerException.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public string GeneratePdf(BaseInput inputs)
        {
            #region initialitation

            string returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            List<DataTableSPReportDTO> dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year,
                model.rep_position, inputs.SortExpression, inputs.SortOrder, inputs.SearchColumn, inputs.SearchValue);
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
            string addressPath = headerPath + "SalesReport" + "/" + model.rep_id;
                // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "sales_promotion_report_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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

            var title = new Paragraph();
            title.Add(new Paragraph("PT. TANABE INDONESIA ", TitleFont));
            title.Add(new Paragraph("SALES PROMOTION ACTUAL ", TitleFont));
            title.Add(new Paragraph("Nama\t\t: " + model.rep_full_name, TitleFont));
            title.Add(new Paragraph("Regional\t\t: " + model.rep_region, TitleFont));
            title.Add(new Paragraph("BO\t\t: " + model.rep_bo, TitleFont));
            title.Add(new Paragraph("Month\t\t: " + inputs.Month, TitleFont));
            title.IndentationLeft = 10;

            document.Add(title);

            #region table

            var boundTable = new DataTable();
            boundTable.Columns.Add("REQ_ID", typeof (string));
            boundTable.Columns.Add("SPR NO", typeof (string));
            boundTable.Columns.Add("SP", typeof (string));
            boundTable.Columns.Add("EVENT NAME", typeof (string));
            boundTable.Columns.Add("TOPIC", typeof (string));
            boundTable.Columns.Add("PLACE", typeof (string));
            boundTable.Columns.Add("GP", typeof (string));
            boundTable.Columns.Add("GP Pax", typeof (string));
            boundTable.Columns.Add("Spec", typeof (string));
            boundTable.Columns.Add("Spec Pax", typeof (string));
            boundTable.Columns.Add("Nurse", typeof (string));
            boundTable.Columns.Add("Nurse Pax", typeof (string));
            boundTable.Columns.Add("Others", typeof (string));
            boundTable.Columns.Add("Others Pax", typeof (string));
            boundTable.Columns.Add("DATE START", typeof (string));
            boundTable.Columns.Add("DATE END", typeof (string));
            boundTable.Columns.Add("PARTICIPANT", typeof (string));
            boundTable.Columns.Add("PLAN BUDGET", typeof (string));
            boundTable.Columns.Add("PLAN STATUS", typeof (string));

            foreach (DataTableSPReportDTO item in dbResult)
            {
                if (item.e_name != null)
                {
                    dynamic dr = boundTable.NewRow();
                    dr = boundTable.NewRow();
                    dr["REQ_ID"] = item.spr_id;
                    dr["SPR NO"] = item.spr_no;
                    dr["SP"] = item.sp_type;
                    dr["EVENT NAME"] = item.e_name;
                    dr["TOPIC"] = item.e_topic;
                    dr["PLACE"] = item.e_place;
                    dr["GP"] = item.e_a_gp;
                    dr["GP Pax"] = item.e_a_gp_pax;
                    dr["Spec"] = item.e_a_specialist;
                    dr["Spec Pax"] = item.e_a_specialist_pax;
                    dr["Nurse"] = item.e_a_nurse;
                    dr["Nurse Pax"] = item.e_a_nurse_pax;
                    dr["Others"] = item.e_a_others;
                    dr["Others Pax"] = item.e_a_others_pax;
                    dr["DATE START"] = item.e_dt_start;
                    dr["DATE END"] = item.e_dt_end;
                    dr["PARTICIPANT"] = item.dr_real_sum;
                    dr["PLAN BUDGET"] = item.budget_plan_sum;
                    dr["PLAN STATUS"] = item.budget_real_sum;

                    boundTable.Rows.Add(dr);
                }
            }

            //Adding  PdfPTable  
            var table = new PdfPTable(boundTable.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //float[] widths = new float[] { 5f, 6f, 4f, 4f, 3f, 4f, 6f, 7f, 4f, 12f, 4f, 5f, 3f, 15f, 5f, 5f, 6f, 5f };
            //table.SetWidths(widths);

            var cell = new PdfPCell();

            cell.PaddingBottom = 5;
            cell.Border = 2;

            for (int i = 0; i < boundTable.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#666666"));
                string cellText = HttpContext.Current.Server.HtmlDecode(boundTable.Columns[i].ColumnName);
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
                cell.FixedHeight = 15f;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < boundTable.Rows.Count; i++)
            {
                if (boundTable.Rows[i][1].ToString() == "")
                {
                    cell.Colspan = boundTable.Columns.Count;
                    cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#cccccc"));
                    string cellText = HttpContext.Current.Server.HtmlDecode(boundTable.Rows[i][0].ToString());
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
                    for (int j = 0; j < boundTable.Columns.Count; j++)
                    {
                        //bfbfbf
                        cell.Colspan = 0;
                        cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#ffffff"));
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

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult GetDetailEvent(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<EventDetail_DTO> dbResult = _bll.getDetailEvent(model.rep_id, inputs.Month, inputs.Year,
                    model.rep_position, inputs.spr_id);
                var viewMapper = Mapper.Map<List<EventDetail_DTO>>(dbResult);

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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult GetDetailProduct(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<string> dbResult = _bll.getDetailProduct(model.rep_id, inputs.Month, inputs.Year,
                    model.rep_position, inputs.spr_id);
                var viewMapper = Mapper.Map<List<string>>(dbResult);

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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }

        [HttpPost]
        [ResponseType(typeof (BaseInput))]
        public IHttpActionResult GetDetailParticipant(BaseInput inputs)
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
                string id = Decrypt(inputs.Auth, true);
                bas_v_rep_fullDTO model = _loginbll.CheckMvaUserInfo(id);
                List<EventParticipant_DTO> dbResult = _bll.getDetailParticipant(model.rep_id, inputs.Month, inputs.Year,
                    model.rep_position, inputs.sp_id, inputs.sp_type);
                var viewMapper = Mapper.Map<List<EventParticipant_DTO>>(dbResult);

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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Json(objResponseModel);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DocumentFormat.OpenXml.Spreadsheet;
using SF_BusinessLogics.LoginBLL;
using SF_BusinessLogics.User;
using SF_Domain.Inputs;
using System.Web.Http.Description;
using SF_WebApi.Models;
using SF_Utils;
using AutoMapper;
using SF_Domain.DTOs.BAS;
using System.Data;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using SF_BusinessLogics.ErrLogs;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.User
{
    public class UserHistoryController : BaseController
    {
        private readonly IUserHistoryBLL _bll;
        private readonly ILoginBLL _loginbll;
        private IVTLogger _vtLogger;

        public UserHistoryController(IUserHistoryBLL bll, ILoginBLL loginbll, IVTLogger vtLogger)
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
                var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.Search, inputs.SearchColumn, inputs.SearchValue);
                var viewMapper = Mapper.Map<List<DataTableUserHistoryDTO>>(dbResult);
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
        public IHttpActionResult GetDataTableSUM(BaseInput inputs)
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
                var dbResult = _bll.getDataTableSUM(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.Search, inputs.SearchColumn, inputs.SearchValue);
                var viewMapper = Mapper.Map<DataTableUserHistorySUMDTO>(dbResult);

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
                var dbResult = _bll.getDataTableDetail(inputs.sp_id);
                var viewMapper = Mapper.Map<List<DataTableUserHistoryDetailDTO>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.Success);
                objResponseModel.Result = viewMapper.Skip((inputs.PageIndex - 1) * inputs.PageSize);
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
            var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.Search, inputs.SearchColumn, inputs.SearchValue);
            var currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("GenerateExcelPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "UserHistory" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "user_history_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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

            var templateFile = Enums.ExcelTemplate.User_History + ".xlsx";
            var templateFileName = HttpContext.Current.Server.MapPath(Constants.UserHistory + templateFile);
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
                        slDoc.SetCellValue(iRow, 1, "PRODUCT. NAME:  " + item.prd_name);
                        tempField = item.prd_name;
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

                    slDoc.SetCellValue(iRow, 1, item.dr_code.HasValue ? item.dr_code.Value : 0);
                    slDoc.SetCellValue(iRow, 2, item.sales_plan_verification_status.HasValue ? item.sales_plan_verification_status.Value : 0);
                    slDoc.SetCellValue(iRow, 3, item.sales_real_verification_status.HasValue ? item.sales_real_verification_status.Value : 0);
                    slDoc.SetCellValue(iRow, 4, item.dr_name);
                    slDoc.SetCellValue(iRow, 5, item.dr_spec);
                    slDoc.SetCellValue(iRow, 6, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 7, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 8, item.prd_price.HasValue ? item.prd_price.Value : 0);
                    slDoc.SetCellValue(iRow, 9, item.prd_category);
                    slDoc.SetCellValue(iRow, 10, item.sp_target_qty.HasValue ? item.sp_target_qty.Value : 0);
                    slDoc.SetCellValue(iRow, 11, item.sp_target_value.HasValue ? item.sp_target_value.Value : 0);
                    slDoc.SetCellValue(iRow, 12, item.sp_sales_qty.HasValue ? item.sp_sales_qty.Value : 0);
                    slDoc.SetCellValue(iRow, 13, item.sp_sales_value.HasValue ? item.sp_sales_value.Value : 0);
                    slDoc.SetCellStyle(iRow, 1, iRow, 12, style);
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

        //const string UploadExcelDirectory = "/Asset/Files/Downloads/Excel/UserHistory/";
        //public HttpResponseMessage ExportExcel(BaseInput inputs)
        //{
        //    inputs.RepId = Decrypt(inputs.Auth, true);
        //    var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
        //    var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.Search, inputs.SearchColumn, inputs.SearchValue);
        //    DataTable boundTable = new DataTable();
        //    boundTable.Columns.Add("DR. CODE", typeof(string));
        //    boundTable.Columns.Add("Ver. Plan", typeof(string));
        //    boundTable.Columns.Add("Ver. Real", typeof(string));
        //    boundTable.Columns.Add("DOCTOR NAME	SPEC", typeof(string));
        //    boundTable.Columns.Add("DR. QUADRANT", typeof(string));
        //    boundTable.Columns.Add("DR. MONITORING", typeof(string));
        //    boundTable.Columns.Add("PRICE", typeof(string));
        //    boundTable.Columns.Add("CATEGORY", typeof(string));
        //    boundTable.Columns.Add("TARGET QTY", typeof(string));
        //    boundTable.Columns.Add("TARGET VALUE", typeof(string));
        //    boundTable.Columns.Add("SALES QTY", typeof(string));
        //    boundTable.Columns.Add("SALES VALUE", typeof(string));
        //    string tempPrdName = "";
        //    foreach (DataTableUserHistoryDTO item in dbResult)
        //    {
        //        dynamic dr = boundTable.NewRow();
        //        if (tempPrdName != item.prd_name)
        //        {
        //            dr["DR. CODE"] = item.prd_name;
        //            dr["Ver. Plan"] = "";
        //            dr["Ver. Real"] = "";
        //            dr["DOCTOR NAME	SPEC"] = "";
        //            dr["DR. QUADRANT"] = "";
        //            dr["DR. MONITORING"] = "";
        //            dr["PRICE"] = "";
        //            dr["CATEGORY"] = "";
        //            dr["TARGET QTY"] = "";
        //            dr["TARGET VALUE"] = "";
        //            dr["SALES QTY"] = "";
        //            dr["SALES VALUE"] = "";
        //            boundTable.Rows.Add(dr);
        //            tempPrdName = item.prd_name;
        //        }

        //        dr = boundTable.NewRow();
        //        dr["DR. CODE"] = item.dr_code;
        //        dr["Ver. Plan"] = item.sales_plan_verification_status;
        //        dr["Ver. Real"] = item.sales_real_verification_status;
        //        dr["DOCTOR NAME	SPEC"] = item.dr_name;
        //        dr["DR. QUADRANT"] = item.dr_quadrant;
        //        dr["DR. MONITORING"] = item.dr_monitoring;
        //        dr["PRICE"] = item.prd_price;
        //        dr["CATEGORY"] = item.prd_category;
        //        dr["TARGET QTY"] = item.sp_target_qty;
        //        dr["TARGET VALUE"] = item.sp_target_value;
        //        dr["SALES QTY"] = item.sp_sales_qty;
        //        dr["SALES VALUE"] = item.sp_sales_value;
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
        //    var addressPath = headerPath + "UserHistory" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
        //    var resultFileName = "User_History_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day + "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
        //    var resultFilePath = addressPath + "/" + resultFileName;

        //    if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
        //    {
        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
        //    }
        //    dataTableToExcel.generateExcels(boundTable, HttpContext.Current.Server.MapPath(resultFilePath));

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

        const string UploadPdfDirectory = "/Asset/Files/Downloads/Pdf/UserHistory/";
        public string GeneratePdf(BaseInput inputs)
        {
            #region initialitation
            var returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            var model = _loginbll.CheckMvaUserInfo(inputs.RepId);
            var dbResult = _bll.getDataTable(model.rep_id, inputs.Month, inputs.Year, inputs.SortExpression, inputs.SortOrder, inputs.Search, inputs.SearchColumn, inputs.SearchValue);
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
            var addressPath = headerPath + "UserHistory" + "/" + model.rep_id; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "user_history_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + currentDate.Day + "_" + currentDate.Month + "_" + currentDate.Year + ".pdf";
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
            title.Add(new Paragraph("USER HISTORY ", TitleFont));
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
            DataTable boundTable = new DataTable();
            boundTable.Columns.Add("DR. CODE", typeof(string));
            boundTable.Columns.Add("Ver. Plan", typeof(string));
            boundTable.Columns.Add("Ver. Real", typeof(string));
            boundTable.Columns.Add("DOCTOR NAME	SPEC", typeof(string));
            boundTable.Columns.Add("DR. QUADRANT", typeof(string));
            boundTable.Columns.Add("DR. MONITORING", typeof(string));
            boundTable.Columns.Add("PRICE", typeof(string));
            boundTable.Columns.Add("CATEGORY", typeof(string));
            boundTable.Columns.Add("TARGET QTY", typeof(string));
            boundTable.Columns.Add("TARGET VALUE", typeof(string));
            boundTable.Columns.Add("SALES QTY", typeof(string));
            boundTable.Columns.Add("SALES VALUE", typeof(string));
            string tempPrdName = "";
            foreach (DataTableUserHistoryDTO item in dbResult)
            {
                dynamic dr = boundTable.NewRow();
                if (tempPrdName != item.prd_name)
                {
                    dr["DR. CODE"] = item.prd_name;
                    dr["Ver. Plan"] = "";
                    dr["Ver. Real"] = "";
                    dr["DOCTOR NAME	SPEC"] = "";
                    dr["DR. QUADRANT"] = "";
                    dr["DR. MONITORING"] = "";
                    dr["PRICE"] = "";
                    dr["CATEGORY"] = "";
                    dr["TARGET QTY"] = "";
                    dr["TARGET VALUE"] = "";
                    dr["SALES QTY"] = "";
                    dr["SALES VALUE"] = "";
                    boundTable.Rows.Add(dr);
                    tempPrdName = item.prd_name;
                }

                dr = boundTable.NewRow();
                dr["DR. CODE"] = item.dr_code;
                dr["Ver. Plan"] = item.sales_plan_verification_status;
                dr["Ver. Real"] = item.sales_real_verification_status;
                dr["DOCTOR NAME	SPEC"] = item.dr_name;
                dr["DR. QUADRANT"] = item.dr_quadrant;
                dr["DR. MONITORING"] = item.dr_monitoring;
                dr["PRICE"] = item.prd_price;
                dr["CATEGORY"] = item.prd_category;
                dr["TARGET QTY"] = item.sp_target_qty;
                dr["TARGET VALUE"] = item.sp_target_value;
                dr["SALES QTY"] = item.sp_sales_qty;
                dr["SALES VALUE"] = item.sp_sales_value;
                boundTable.Rows.Add(dr);
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

            #endregion
            document.Close();
            //byte[] bytes = memoryStream.ToArray();
            //memoryStream.Close();
            return returnVal;
        }
    }
}

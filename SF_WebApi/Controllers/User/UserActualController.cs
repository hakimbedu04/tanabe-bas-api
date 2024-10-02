using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
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
using SF_BusinessLogics.User;
using SF_BusinessLogics.Visit;
using SF_Domain.DTOs;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;
using SF_Utils;
using SF_WebApi.Models;
using SF_WebApi.Models.BAS.User;
using SpreadsheetLight;
using Font = iTextSharp.text.Font;

namespace SF_WebApi.Controllers.User
{
    public class UserActualController : BaseController
    {
        private readonly IUserActualBLL _bll;
        private readonly ILoginBLL _loginBll;
        private readonly IVisitBLL _visitBll;
        private readonly IVTLogger _vtLogger;

        public UserActualController(IUserActualBLL bll, ILoginBLL loginBll, IVisitBLL visitBll, IVTLogger vtLogger)
        {
            _bll = bll;
            _loginBll = loginBll;
            _visitBll = visitBll;
            _vtLogger = vtLogger;
        }

        [HttpPost]
        public HttpResponseMessage ViewUserActual(UserInputs inputs)
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
                List<v_sales_product_DTO> dbResult = _bll.GetUserActualDatas(inputs);
                var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);
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
        public HttpResponseMessage ViewUserActualSearchText(UserInputs inputs)
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
                List<v_sales_product_DTO> dbResult = _bll.GetUserActualSearch(inputs);
                var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);
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
        public HttpResponseMessage RequestVerificationActual(UserInputs inputs)
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
                //if (_bll.isHaveRemainingToSendMail(inputs))
                //{
                //send email
                SetReport(inputs);
                //}
                List<v_sales_product_DTO> dbResult = _bll.GetUserActualDatas(inputs);
                var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);
                objResponseModel.Status = true;
                objResponseModel.Message = EnumHelper.GetDescription(Enums.ResponseType.SuccessSend);
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

        public void SetReport(UserInputs input)
        {
            bas_v_rep_fullDTO userDetail = _loginBll.CheckMvaUserInfo(input.RepId);
            DateTime currentDate = DateTime.Now;
            string monthName = (new DateTime(input.Year, input.Month, currentDate.Day)).ToString("MMMM",
                CultureInfo.InvariantCulture);
            string originPath = GeneratePdf(input);
            string pdfFile = HttpContext.Current.Server.MapPath(@"~" + originPath.Replace(GetHost(), ""));
            var reader =
                new StreamReader(HttpContext.Current.Server.MapPath("~/ContentEmailPage/email_page_new_sales_real.htm"));
            string readFile = reader.ReadToEnd();
            string email_body = "";
            email_body = readFile;

            email_body = email_body.Replace("$$RECEIVER$$", userDetail.nama_am);
            email_body = email_body.Replace("$$rep_name$$", userDetail.rep_name);
            email_body = email_body.Replace("$$rep_region$$", userDetail.rep_region);
            email_body = email_body.Replace("$$bo$$", userDetail.rep_bo);
            email_body = email_body.Replace("$$sbo$$", userDetail.rep_sbo);
            email_body = email_body.Replace("$$rep_id$$", userDetail.rep_id);
            email_body = email_body.Replace("$$date_plan$$", monthName);
            email_body = email_body.Replace("$$Date$$", currentDate.ToString("yyyy MMMM dd"));

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();
            var settingsReader = new AppSettingsReader();
            var key = (string) settingsReader.GetValue("fromEmailAddress", typeof (String));
            // Get the key from config file
            mailMessage.From = new MailAddress(key);
            //mailMessage.To.Add(new MailAddress(userDetail.email_am)); 
            ////mailMessage.To.Add(new MailAddress("abdurahman.hakim@vodjo.com"));
            ////mailMessage.To.Add(new MailAddress("edi.suherman@vodjo.com"));

            mailMessage.Attachments.Add(new Attachment(pdfFile));
            mailMessage.Subject = "Request Verification for " + userDetail.rep_name + " - " + "User Realization - " +
                                  monthName;

            mailMessage.Priority = MailPriority.High;
            mailMessage.Body = email_body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            reader.Dispose();
            //_visitBll.SaveReportPlan(input.RepId);
        }

        [HttpPost]
        public HttpResponseMessage ExportToPdf(UserInputs inputs)
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
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        public string GeneratePdf(UserInputs inputs)
        {
            #region initialitation

            string returnVal = "";
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginBll.CheckMvaUserInfo(inputs.RepId);
            List<v_sales_product_DTO> dbResult = _bll.GetUserActualDatas(inputs);
            var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);

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
            string addressPath = headerPath + "UserActual" + "/" + model.rep_id;
            // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "user_actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" +
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
            title.Add(new Paragraph("USER ACTUAL ", TitleFont));
            title.IndentationLeft = 10;

            var header = new Paragraph();
            header.Add(new Paragraph("Nama       : " + model.rep_name, TitleFont));
            header.Add(new Paragraph("Regional   : " + model.rep_region, TitleFont));
            header.Add(new Paragraph("BO         : " + model.rep_bo, TitleFont));
            header.IndentationLeft = 10;

            document.Add(title);
            document.Add(header);

            #region table

            var dt = new DataTable();
            dt.Columns.Add("DR. CODE", typeof (string));
            dt.Columns.Add("Ver Plan", typeof (string));
            dt.Columns.Add("Ver Real", typeof (string));
            dt.Columns.Add("DOCTOR NAME", typeof (string));
            dt.Columns.Add("SPEC", typeof (string));
            dt.Columns.Add("DR. QUADRANT", typeof (string));
            dt.Columns.Add("DR. MONITORING", typeof (string));
            dt.Columns.Add("PRICE", typeof (string));
            dt.Columns.Add("CATEGORY", typeof (string));
            dt.Columns.Add("TARGET QTY", typeof (string));
            dt.Columns.Add("TARGET VALUE", typeof (string));
            dt.Columns.Add("SALES QTY", typeof (string));
            dt.Columns.Add("SALES VALUE", typeof (string));

            string tempDrName = "";
            foreach (v_sales_product_ViewModel item in viewMapper)
            {
                dynamic dr = dt.NewRow();
                if (tempDrName != item.prd_name)
                {
                    dr["DR. CODE"] = item.prd_name;
                    dr["Ver Plan"] = "";
                    dr["Ver Real"] = "";
                    dr["DOCTOR NAME"] = "";
                    dr["SPEC"] = "";
                    dr["DR. QUADRANT"] = "";
                    dr["DR. MONITORING"] = "";
                    dr["PRICE"] = "";
                    dr["CATEGORY"] = "";
                    dr["TARGET QTY"] = "";
                    dr["TARGET VALUE"] = "";
                    dr["SALES QTY"] = "";
                    dr["SALES VALUE"] = "";
                    dt.Rows.Add(dr);
                    tempDrName = item.prd_name;
                }

                dr = dt.NewRow();
                dr["DR. CODE"] = item.dr_code;
                dr["Ver Plan"] = item.sales_plan_verification_status;
                dr["Ver Real"] = item.sales_real_verification_status;
                dr["DOCTOR NAME"] = item.dr_name;
                dr["SPEC"] = item.dr_spec;
                dr["DR. QUADRANT"] = item.dr_quadrant;
                dr["DR. MONITORING"] = item.dr_monitoring;
                dr["PRICE"] = item.prd_price;
                dr["CATEGORY"] = "";
                dr["TARGET QTY"] = item.sp_target_qty;
                dr["TARGET VALUE"] = item.sp_target_value;
                dr["SALES QTY"] = item.sp_sales_qty;
                dr["SALES VALUE"] = item.sp_sales_value;
                dt.Rows.Add(dr);
            }

            //Adding  PdfPTable  
            var table = new PdfPTable(dt.Columns.Count);
            table.DefaultCell.FixedHeight = 10;
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = {3f, 2f, 2f, 6f, 3f, 4f, 9f, 3f, 3f, 4f, 4.5f, 3.5f, 4f};
            table.SetWidths(widths);

            var cell = new PdfPCell();
            cell.PaddingBottom = 5;
            cell.Border = 2;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#666666"));
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
                cell.FixedHeight = 14f;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == "")
                {
                    cell.Colspan = dt.Columns.Count;
                    cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#cccccc"));
                    string cellText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][0].ToString());
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
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //bfbfbf
                        cell.Colspan = 0;
                        cell.BackgroundColor = new BaseColor(ColorTranslator.FromHtml("#ffffff"));
                        string rowText = HttpContext.Current.Server.HtmlDecode(dt.Rows[i][j].ToString());
                        cell.Phrase = new Phrase(rowText, tableContentFont);
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
            //footer.Add(new Paragraph("Total Planned Doctor : " + (detailDoctor[0].DrTotal + detailDoctor[1].DrTotal + detailDoctor[2].DrTotal), TitleFont));
            //for (var i = 0; i < detailDoctor.Count; i++)
            //{
            //    footer.Add(new Paragraph("Total Planned Doctor Q" + (i + 1) + " : " + detailDoctor[i].DrTotal, TitleFont));
            //}
            //footer.IndentationLeft = 10;
            //document.Add(footer);

            #endregion

            document.Close();
            //}

            return returnVal;
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(UserInputs inputs)
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
                objResponseModel.DetailMessage = ex.InnerException.Message;
                _vtLogger.Err(ex, new List<object> {inputs});
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponseModel);
            }
        }

        [HttpPost]
        public string GenerateExcel(UserInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginBll.CheckMvaUserInfo(inputs.RepId);
            List<v_sales_product_DTO> dbResult = _bll.GetUserActualDatas(inputs);
            var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);
            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
            string addressPath = headerPath + "UserActual" + "/" + model.rep_id;
            string resultFileName = "User_Actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day +
                                    "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
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

            string templateFile = Enums.ExcelTemplate.User_Actual + ".xlsx";
            string templateFileName = HttpContext.Current.Server.MapPath(Constants.UserActual + templateFile);
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

                foreach (v_sales_product_ViewModel item in viewMapper)
                {
                    #region sub header

                    SLStyle styleSubHeader = slDoc.CreateStyle();
                    styleSubHeader.Font.FontName = "Calibri";
                    styleSubHeader.Font.FontSize = 11;
                    styleSubHeader.Font.Bold = true;

                    if (tempField != item.dr_name)
                    {
                        slDoc.SetCellValue(iRow, 1, "PRODUCT. NAME : " + item.prd_name);
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
                    slDoc.SetCellValue(iRow, 2,
                        item.sales_plan_verification_status.HasValue ? item.sales_plan_verification_status.Value : 0);
                    slDoc.SetCellValue(iRow, 3,
                        item.sales_real_verification_status.HasValue ? item.sales_real_verification_status.Value : 0);
                    slDoc.SetCellValue(iRow, 4, item.dr_name);
                    slDoc.SetCellValue(iRow, 5, item.dr_spec);
                    slDoc.SetCellValue(iRow, 6, item.dr_quadrant);
                    slDoc.SetCellValue(iRow, 7, item.dr_monitoring);
                    slDoc.SetCellValue(iRow, 8, item.prd_price.HasValue ? item.prd_price.Value : 0);
                    slDoc.SetCellValue(iRow, 9, "");
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
            return GetHostNoHttp() + resultFilePath.Substring(1);
        }

        public string GenerateExcels(UserInputs inputs)
        {
            inputs.RepId = Decrypt(inputs.Auth, true);
            bas_v_rep_fullDTO model = _loginBll.CheckMvaUserInfo(inputs.RepId);
            List<v_sales_product_DTO> dbResult = _bll.GetUserActualDatas(inputs);
            var viewMapper = Mapper.Map<List<v_sales_product_ViewModel>>(dbResult);

            var dt = new DataTable();
            dt.Columns.Add("DR. CODE", typeof (string));
            dt.Columns.Add("Ver Plan", typeof (string));
            dt.Columns.Add("Ver Real", typeof (string));
            dt.Columns.Add("DOCTOR NAME", typeof (string));
            dt.Columns.Add("SPEC", typeof (string));
            dt.Columns.Add("DR. QUADRANT", typeof (string));
            dt.Columns.Add("DR. MONITORING", typeof (string));
            dt.Columns.Add("PRICE", typeof (string));
            dt.Columns.Add("CATEGORY", typeof (string));
            dt.Columns.Add("TARGET QTY", typeof (string));
            dt.Columns.Add("TARGET VALUE", typeof (string));
            dt.Columns.Add("SALES QTY", typeof (string));
            dt.Columns.Add("SALES VALUE", typeof (string));

            foreach (v_sales_product_ViewModel item in viewMapper)
            {
                dynamic dr = dt.NewRow();
                dr["DR. CODE"] = item.dr_code;
                dr["Ver Plan"] = item.sales_plan_verification_status;
                dr["Ver Real"] = item.sales_real_verification_status;
                dr["DOCTOR NAME"] = item.dr_name;
                dr["SPEC"] = item.dr_spec;
                dr["DR. QUADRANT"] = item.dr_quadrant;
                dr["DR. MONITORING"] = item.dr_monitoring;
                dr["PRICE"] = item.prd_price;
                dr["CATEGORY"] = "";
                dr["TARGET QTY"] = item.sp_target_qty;
                dr["TARGET VALUE"] = item.sp_target_value;
                dr["SALES QTY"] = item.sp_sales_qty;
                dr["SALES VALUE"] = item.sp_sales_value;
                dt.Rows.Add(dr);
            }

            DateTime currentDate = DateTime.Now;
            inputs.Day = currentDate.Day;
            inputs.Month = currentDate.Month;
            inputs.Year = currentDate.Year;
            var dataTableToExcel = new DataTableToExcel();

            #region upload file

            var settingsReader = new AppSettingsReader();
            var headerPath = (string) settingsReader.GetValue("GenerateExcelPath", typeof (String));
            //~/Asset/Files/Downloads/Pdf/
            string addressPath = headerPath + "UserActual" + "/" + model.rep_id;
            // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            string resultFileName = "User_Actual_" + GetStringPattern().Replace(model.rep_name, "_") + "_" + inputs.Day +
                                    "_" + inputs.Month + "_" + inputs.Year + ".xlsx";
            string resultFilePath = addressPath + "/" + resultFileName;

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            dataTableToExcel.generateExcels(dt, HttpContext.Current.Server.MapPath(resultFilePath));

            #endregion

            return GetHostNoHttp() + resultFilePath.Substring(1);
        }
    }
}
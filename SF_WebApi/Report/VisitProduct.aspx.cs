﻿using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Customization;
using DevExpress.XtraPrinting;
using DevExpress.Utils;


namespace SF_WebApi.Report
{
    public partial class VisitProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxPivotGrid1.Width = Unit.Percentage(100);
            ASPxPivotGrid1.CustomizationFieldsVisible = true;
            ASPxPivotGrid1.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
            ASPxPivotGrid1.OptionsView.RowTotalsLocation = PivotRowTotalsLocation.Far;
            ASPxPivotGrid1.OptionsView.ShowGrandTotalsForSingleValues = true;
            ASPxPivotGrid1.OptionsView.ShowRowGrandTotals = true;
            ASPxPivotGrid1.OptionsView.ShowRowTotals = false;
            ASPxPivotGrid1.OptionsView.ShowTotalsForSingleValues = false;
            ASPxPivotGrid1.OptionsView.ShowFilterHeaders = true;

            ASPxPivotGrid1.Styles.FieldValueStyle.Wrap = 0;
            ASPxPivotGrid1.Styles.HeaderStyle.Font.Size = 7;
            ASPxPivotGrid1.Styles.CellStyle.Font.Size = 7;
            ASPxPivotGrid1.Styles.ColumnAreaStyle.Font.Size = 7;
            ASPxPivotGrid1.Styles.RowAreaStyle.Font.Size = 7;
            ASPxPivotGrid1.Styles.FieldValueGrandTotalStyle.Font.Size = 7;
            ASPxPivotGrid1.Styles.GrandTotalCellStyle.Font.Size = 7;

            if (!IsPostBack)
            {
                //startDate.Text = Request.QueryString["s"];
                //endDate.Text = Request.QueryString["e"];
                //txtPosition.Text = Request.QueryString["p"];
                txtNik.Text = Request.QueryString["r"];
            }
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            ASPxPivotGrid1.ReloadData();
            //Response.Redirect("~/Report/VisitProduct.aspx?p=" + txtPosition.Text + "&r=" + txtNik.Text + "&s=" + startDate.Text + "&e=" + endDate.Text);
        }

        protected void BtnExportExcel_Click(object sender, EventArgs e)
        {
            //ASPxPivotGridExporter1.ExportXlsxToResponse("ASPxPivotGrid", new XlsxExportOptionsEx
            //{
            //    AllowFixedColumns = DefaultBoolean.False,
            //    SheetName = "Pivot Grid Export"
            //},
            //true);
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("ReportPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "/" + txtNik.Text + "/VisitProduct"; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "visitwithproductpivot.xlsx";
            var resultFilePath = addressPath + "/" + resultFileName;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
            }

            MemoryStream stream = new MemoryStream();
            Response.Clear();
            ASPxPivotGridExporter1.ExportToXlsx(stream);

            FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(resultFilePath));
            stream.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            stream.Close();

            var hostLink = (string)settingsReader.GetValue("host", typeof(String));
            var hostPath = hostLink.Replace("bas_api_mobile", String.Empty) + resultFilePath; //http://tanabe-id.intra.sharedom.net/bas_api_mobile/ReportPath/Files/Report/12.36/VisitOnlyPivot/visitonlypivot_rowdata.xlsx
            Response.Redirect(hostPath);
        }

        protected void BtnExportDataRow_Click(object sender, EventArgs e)
        {
            //ASPxGridViewExporter1.WriteXlsToResponse();
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("ReportPath", typeof(String)); //~/Asset/Files/Report/
            var addressPath = headerPath + "/" + txtNik.Text + "/VisitProduct"; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "visitwithproductpivot_rowdata.xlsx";
            var resultFilePath = addressPath + "/" + resultFileName;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
            }

            MemoryStream stream = new MemoryStream();
            Response.Clear();
            ASPxGridViewExporter1.WriteXlsx(stream);

            FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(resultFilePath));
            stream.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            stream.Close();
            var hostLink = (string) settingsReader.GetValue("host", typeof (String));
            var hostPath = hostLink.Replace("bas_api_mobile", String.Empty) + resultFilePath; //http://tanabe-id.intra.sharedom.net/bas_api_mobile/ReportPath/Files/Report/12.36/VisitOnlyPivot/visitonlypivot_rowdata.xlsx
            Response.Redirect(hostPath);
        }

        protected void BtnExportPdf_Click(object sender, EventArgs e)
        {
            //ASPxPivotGridExporter1.ExportPdfToResponse("ASPxPivotGrid", new PdfExportOptions()
            //{
            //    ShowPrintDialogOnOpen = true,
            //}, true);
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("ReportPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "/" + txtNik.Text + "/VisitProduct"; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "visitwithproductpivot.pdf";
            var resultFilePath = addressPath + "/" + resultFileName;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(addressPath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(addressPath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(resultFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(resultFilePath));
            }

            MemoryStream stream = new MemoryStream();
            Response.Clear();
            ASPxPivotGridExporter1.ExportToPdf(stream);

            FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(resultFilePath));
            stream.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            stream.Close();

            var hostLink = (string)settingsReader.GetValue("host", typeof(String));
            var hostPath = hostLink.Replace("bas_api_mobile", String.Empty) + resultFilePath; //http://tanabe-id.intra.sharedom.net/bas_api_mobile/ReportPath/Files/Report/12.36/VisitOnlyPivot/visitonlypivot_rowdata.xlsx
            Response.Redirect(hostPath);
        }

        protected void ASPxPivotGrid1_CustomCellDisplayText(object sender, DevExpress.Web.ASPxPivotGrid.PivotCellDisplayTextEventArgs e)
        {

            if (object.ReferenceEquals(e.DataField, fieldplan))
            {
                e.DisplayText = string.Format(System.Globalization.CultureInfo.GetCultureInfo("de-DE"), "{0:N}", e.GetCellValue(fieldplan));
            }

            if (object.ReferenceEquals(e.DataField, fieldrealization))
            {
                e.DisplayText = string.Format(System.Globalization.CultureInfo.GetCultureInfo("de-DE"), "{0:N}", e.GetCellValue(fieldrealization));
            }

            if (object.ReferenceEquals(e.DataField, achv))
            {
                e.DisplayText = string.Format(System.Globalization.CultureInfo.GetCultureInfo("de-DE"), "{0:p}", e.GetCellValue(achv));
            }

            if (object.ReferenceEquals(e.DataField, rem))
            {
                e.DisplayText = string.Format(System.Globalization.CultureInfo.GetCultureInfo("de-DE"), "{0:N}", e.GetCellValue(rem));
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}
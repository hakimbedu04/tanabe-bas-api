using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.Globalization;

namespace SF_WebApi.Report
{
    public partial class Target : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDep.Text = Request.QueryString["d"];
                txtPosition.Text = Request.QueryString["p"];
                txtNik.Text = Request.QueryString["r"];
            }

            grid.Width = Unit.Percentage(100);
            grid.KeyFieldName = "target_id";

            grid.SettingsPager.PageSize = 20;
            grid.Settings.ShowGroupPanel = true;
            grid.Settings.ShowFilterRow = true;
            grid.SettingsEditing.Mode = GridViewEditingMode.Inline;
            grid.SettingsBehavior.ConfirmDelete = true;
            grid.Styles.EmptyDataRow.CssClass = "HideEmptyDataRow";
            grid.Styles.Header.Font.Size = 8;
            grid.Styles.Cell.Font.Size = 7;
            grid.Styles.Cell.CssClass = "myCss";
            grid.Styles.Cell.Paddings.PaddingLeft = 2;
            grid.Styles.Cell.Paddings.PaddingRight = 2;
            grid.SettingsPopup.EditForm.ShowHeader = true;
            grid.SettingsPopup.EditForm.Modal = true;
            grid.SettingsPopup.EditForm.Width = Unit.Pixel(700);
            grid.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
            grid.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            grid.Settings.ShowHeaderFilterButton = true;
            //grid.Settings.ShowFilterBar = true;
            grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
            grid.SettingsBehavior.EnableRowHotTrack = true;
        }

        protected void BtnExportPdf_Click(object sender, EventArgs e)
        {
            //ASPxGridViewExporter1.WritePdfToResponse();
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("ReportPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "/" + txtNik.Text + "/Target"; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "target.pdf";
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
            ASPxGridViewExporter1.WritePdf(stream);

            FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(resultFilePath));
            stream.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            stream.Close();

            var hostLink = (string)settingsReader.GetValue("host", typeof(String));
            var hostPath = hostLink.Replace("bas_api_mobile", String.Empty) + resultFilePath; //http://tanabe-id.intra.sharedom.net/bas_api_mobile/ReportPath/Files/Report/12.36/VisitOnlyPivot/visitonlypivot_rowdata.xlsx
            Response.Redirect(hostPath);
        }

        protected void BtnExportExcel_Click(object sender, EventArgs e)
        {
            //ASPxGridViewExporter1.WriteXlsToResponse();
            var settingsReader = new AppSettingsReader();
            var headerPath = (string)settingsReader.GetValue("ReportPath", typeof(String)); //~/Asset/Files/Downloads/Pdf/
            var addressPath = headerPath + "/" + txtNik.Text + "/Target"; // ~/Asset/Files/Downloads/Pdf/VisitRealization/12.36
            var resultFileName = "target.xlsx";
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

            var hostLink = (string)settingsReader.GetValue("host", typeof(String));
            var hostPath = hostLink.Replace("bas_api_mobile", String.Empty) + resultFilePath; //http://tanabe-id.intra.sharedom.net/bas_api_mobile/ReportPath/Files/Report/12.36/VisitOnlyPivot/visitonlypivot_rowdata.xlsx
            Response.Redirect(hostPath);
        }

        protected void ASPxGridView1_DataBound(object sender, EventArgs e)
        {
            var grid = (ASPxGridView)sender;
            //for (var i = 0; i <= grid.DataColumns.Count; i++)
            //{
                var column = grid.DataColumns;
                column[0].FieldName = "target_id";
                column[0].Caption = "SBO Code";
                column[0].Visible = false;
                column[0].Width = Unit.Pixel(60);
                column[0].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[0].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[0].HeaderStyle.Wrap = DefaultBoolean.True;
                column[0].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[0].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[0].PropertiesEdit.Style.Font.Size = 8;

                column[1].FieldName = "target_year";
                column[1].Caption = "YEAR";
                column[1].Width = Unit.Pixel(40);
                column[1].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[1].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[1].HeaderStyle.Wrap = DefaultBoolean.True;
                column[1].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[1].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[1].PropertiesEdit.Style.Font.Size = 8;
                
                column[2].FieldName = "month";
                column[2].Caption = "MONTH";
                column[2].Width = Unit.Pixel(50);
                column[2].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[2].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[2].HeaderStyle.Wrap = DefaultBoolean.True;
                column[2].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[2].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[2].PropertiesEdit.Style.Font.Size = 8;

                column[3].FieldName = "target_sbo";
                column[3].Caption = "SBO";
                column[3].Width = Unit.Pixel(55);
                column[3].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[3].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[3].HeaderStyle.Wrap = DefaultBoolean.True;
                column[3].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[3].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[3].PropertiesEdit.Style.Font.Size = 8;
                column[3].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[4].FieldName = "target_product_code";
                column[4].Caption = "PRODUCT CODE";
                column[4].Width = Unit.Pixel(60);
                column[4].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[4].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[4].HeaderStyle.Wrap = DefaultBoolean.True;
                column[4].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[4].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[4].PropertiesEdit.Style.Font.Size = 8;
                column[4].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[5].FieldName = "prd_aso_desc";
                column[5].Caption = "PRODUCT DESCRIPTION";
                column[5].Width = Unit.Pixel(225);
                column[5].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[5].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[5].HeaderStyle.Wrap = DefaultBoolean.True;
                column[5].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[5].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[5].PropertiesEdit.Style.Font.Size = 8;
                column[5].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[6].FieldName = "cust_name";
                column[6].Caption = "CUSTOMER";
                column[6].Width = Unit.Pixel(225);
                column[6].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[6].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[6].HeaderStyle.Wrap = DefaultBoolean.True;
                column[6].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[6].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[6].PropertiesEdit.Style.Font.Size = 8;
                column[6].Settings.ShowFilterRowMenu = DefaultBoolean.True;
                column[6].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[7].FieldName = "cust_outlet_channel";
                column[7].Caption = "CHANNEL";
                column[7].Width = Unit.Pixel(175);
                column[7].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[7].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[7].HeaderStyle.Wrap = DefaultBoolean.True;
                column[7].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[7].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[7].PropertiesEdit.Style.Font.Size = 7;
                column[7].Settings.ShowFilterRowMenu = DefaultBoolean.True;
                column[7].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[8].FieldName = "target_description";
                column[8].Caption = "DATA TYPE";
                column[8].Width = Unit.Pixel(75);
                column[8].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[8].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[8].HeaderStyle.Wrap = DefaultBoolean.True;
                column[8].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[8].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[8].PropertiesEdit.Style.Font.Size = 8;
                column[8].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[9].FieldName = "target_product_category";
                column[9].Caption = "PRODUCT CATEGORY";
                column[9].Width = Unit.Pixel(60);
                column[9].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[9].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[9].HeaderStyle.Wrap = DefaultBoolean.True;
                column[9].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[9].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[9].PropertiesEdit.Style.Font.Size = 8;
                column[9].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[10].FieldName = "target_product_price";
                column[10].Caption = "PRODUCT PRICE";
                column[10].Width = Unit.Pixel(60);
                column[10].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[10].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[10].HeaderStyle.Wrap = DefaultBoolean.True;
                column[10].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[10].CellStyle.HorizontalAlign = HorizontalAlign.Right;
                column[10].PropertiesEdit.Style.Font.Size = 8;
                column[10].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column[10].PropertiesEdit.DisplayFormatString = "N2";

                column[11].FieldName = "target_plan_qty";
                column[11].Caption = "PLAN QTY";
                column[11].Width = Unit.Pixel(50);
                column[11].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[11].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[11].HeaderStyle.Wrap = DefaultBoolean.True;
                column[11].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[11].CellStyle.HorizontalAlign = HorizontalAlign.Right;
                column[11].PropertiesEdit.Style.Font.Size = 8;
                column[11].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[12].FieldName = "target_plan_value";
                column[12].Caption = "PLAN VALUE";
                column[12].Width = Unit.Pixel(60);
                column[12].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[12].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[12].HeaderStyle.Wrap = DefaultBoolean.True;
                column[12].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[12].CellStyle.HorizontalAlign = HorizontalAlign.Right;
                column[12].PropertiesEdit.Style.Font.Size = 8;
                column[12].PropertiesEdit.DisplayFormatString = "N2";
                column[12].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[13].FieldName = "rep_name";
                column[13].Caption = "MR";
                column[13].Width = Unit.Pixel(175);
                column[13].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[13].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[13].HeaderStyle.Wrap = DefaultBoolean.True;
                column[13].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[13].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[13].PropertiesEdit.Style.Font.Size = 8;
                column[13].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[14].FieldName = "rep_bo";
                column[14].Caption = "BO";
                column[14].Width = Unit.Pixel(50);
                column[14].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[14].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[14].HeaderStyle.Wrap = DefaultBoolean.True;
                column[14].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[14].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[14].PropertiesEdit.Style.Font.Size = 8;
                column[14].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[15].FieldName = "nama_am";
                column[15].Caption = "AM";
                column[15].Width = Unit.Pixel(125);
                column[15].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[15].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[15].HeaderStyle.Wrap = DefaultBoolean.True;
                column[15].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[15].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[15].PropertiesEdit.Style.Font.Size = 8;
                column[15].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[16].FieldName = "nama_rm";
                column[16].Caption = "RM";
                column[16].Width = Unit.Pixel(100);
                column[16].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[16].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[16].HeaderStyle.Wrap = DefaultBoolean.True;
                column[16].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[16].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                column[16].PropertiesEdit.Style.Font.Size = 8;
                column[16].Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                column[17].FieldName = "rep_region";
                column[17].Caption = "REG";
                column[17].Width = Unit.Pixel(50);
                column[17].HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                column[17].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                column[17].HeaderStyle.Wrap = DefaultBoolean.True;
                column[17].CellStyle.VerticalAlign = VerticalAlign.Middle;
                column[17].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                column[17].PropertiesEdit.Style.Font.Size = 8;
                column[17].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            //}
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            var grid = (ASPxGridView)sender;
            if (object.ReferenceEquals(e.Column.Caption, "MONTH"))
            {
                e.DisplayText = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)e.Value);
            }
        }
    }
}
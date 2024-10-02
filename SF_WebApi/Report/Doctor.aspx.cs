using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.XtraTreeList.Columns;
using AutoFilterCondition = DevExpress.Web.AutoFilterCondition;

namespace SF_WebApi.Report
{
    public partial class Doctor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDep.Text = Request.QueryString["d"];
                txtPosition.Text = Request.QueryString["p"];
                txtNik.Text = Request.QueryString["i"];
            }

            ASPxGridView1.Width = Unit.Percentage(99.9);
            ASPxGridView1.SettingsPager.PageSize = 50;
            ASPxGridView1.SettingsPager.PageSizeItemSettings.Visible = true;
            ASPxGridView1.Settings.ShowGroupPanel = true;
            ASPxGridView1.Settings.ShowFilterRowMenu = true;
            ASPxGridView1.Settings.ShowFilterRow = true;
            ASPxGridView1.SettingsSearchPanel.Visible = false;
            ASPxGridView1.Paddings.Padding = System.Web.UI.WebControls.Unit.Pixel(1);
            ASPxGridView1.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            ASPxGridView1.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            ASPxGridView1.Styles.Cell.Font.Size = 7;
            ASPxGridView1.Styles.Cell.Paddings.Padding = 2;
            ASPxGridView1.Styles.Header.Font.Size = 7;
            ASPxGridView1.Styles.Header.Paddings.Padding = 2;
            ASPxGridView1.Styles.GroupRow.Font.Size = 7;
            ASPxGridView1.Styles.Footer.Font.Size = 7;
            ASPxGridView1.Styles.Header.HorizontalAlign = HorizontalAlign.Center;
            ASPxGridView1.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
            ASPxGridView1.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            ASPxGridView1.Settings.VerticalScrollableHeight = 330;
            ASPxGridView1.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;
            ASPxGridView1.SettingsBehavior.ConfirmDelete = true;
            ASPxGridView1.SettingsBehavior.AllowSelectByRowClick = true;
            ASPxGridView1.SettingsBehavior.EnableRowHotTrack = true;

            ASPxGridView1.SettingsPopup.EditForm.Width = 800;
            ASPxGridView1.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
            ASPxGridView1.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
            ASPxGridView1.SettingsPopup.EditForm.ShowHeader = true;
            ASPxGridView1.SettingsPopup.EditForm.Modal = true;
            ASPxGridView1.SettingsPopup.EditForm.ResizingMode = ResizingMode.Live;
            ASPxGridView1.SettingsPopup.EditForm.AllowResize = true;


            //                    ASPxGridView1.Command = true;
            //ASPxGridView1.CommandColumn.Width = 50;
            //                    ASPxGridView1.CommandColumn.Visible = true;
            //ASPxGridView1.CommandColumn.ShowSelectCheckbox = false;

            //ASPxGridView1.CommandColumn.ShowNewButtonInHeader = false;
            //ASPxGridView1.CommandColumn.VisibleIndex = 0;
            //ASPxGridView1.CommandColumn.FixedStyle = GridViewColumnFixedStyle.Left;
            //ASPxGridView1.CommandColumn.CellStyle.BackColor = System.Drawing.Color.Bisque;
            //ASPxGridView1.CommandColumn.ShowEditButton = true;
            ASPxGridView1.SettingsCommandButton.EditButton.Image.Url = "~/Content/Images/Edit.png";
            ASPxGridView1.SettingsCommandButton.EditButton.Text = " ";
            ASPxGridView1.SettingsCommandButton.NewButton.Image.Url = "~/Content/Images/Plus.png";
            ASPxGridView1.SettingsCommandButton.NewButton.Text = " ";
            ASPxGridView1.SettingsCommandButton.CancelButton.Text = "Cancel";
            ASPxGridView1.SettingsCommandButton.CancelButton.RenderMode = GridCommandButtonRenderMode.Button;
            ASPxGridView1.SettingsCommandButton.UpdateButton.Text = "Save";
            ASPxGridView1.SettingsCommandButton.UpdateButton.RenderMode = GridCommandButtonRenderMode.Button;
            ASPxGridView1.SettingsCommandButton.DeleteButton.Image.Url = "~/Content/Images/delete-icon2.png";
            ASPxGridView1.SettingsCommandButton.DeleteButton.Text = " ";
            ASPxGridView1.SettingsBehavior.EnableCustomizationWindow = true;
            ASPxGridView1.SettingsPopup.CustomizationWindow.HorizontalAlign = PopupHorizontalAlign.RightSides;
            ASPxGridView1.SettingsPopup.CustomizationWindow.VerticalAlign = PopupVerticalAlign.TopSides;
            ASPxGridView1.SettingsPopup.CustomizationWindow.Height = 300;
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Report/Doctor.aspx?d=" + txtDep.Text + "&p=" + txtPosition.Text + "&i=" + txtNik.Text + "");
        }

        protected void BtnExportExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsToResponse();
        }

        protected void BtnExportPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WritePdfToResponse();
        }

        protected void ASPxGridView1_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }

        protected void ASPxGridView1_DataBound(object sender, EventArgs e)
        {
            var grid = (ASPxGridView)sender;
            for (var i = 0; i < grid.DataColumns.Count; i++)
            {
                grid.DataColumns[0].Width = 50;
                grid.DataColumns[0].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[0].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[0].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[0].Caption = "DOCTOR CODE";
                grid.DataColumns[0].CellStyle.ForeColor = System.Drawing.Color.Blue;
                grid.DataColumns[0].VisibleIndex = 1;
                grid.DataColumns[0].CellStyle.Wrap = DefaultBoolean.True;
                //grid.DataColumns[1].ColumnType = MVCxGridViewColumnType.TextBox;
                grid.DataColumns[0].ReadOnly = true;
                grid.DataColumns[0].EditFormSettings.VisibleIndex = 1;
                grid.DataColumns[0].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                grid.DataColumns[0].CellStyle.BackColor = System.Drawing.Color.Bisque;
                grid.DataColumns[0].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[0].PropertiesEdit.Style.BackColor = System.Drawing.Color.Beige;
                grid.DataColumns[0].PropertiesEdit.Style.Font.Bold = true;
                grid.DataColumns[0].PropertiesEdit.Style.Font.Size = 12;
                grid.DataColumns[0].PropertiesEdit.Style.Width = 100;

                grid.DataColumns[1].Width = 175;
                grid.DataColumns[1].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[1].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[1].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[1].Caption = "NAME";
                grid.DataColumns[1].VisibleIndex = 2;
                grid.DataColumns[1].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[1].EditFormSettings.VisibleIndex = 3;
                grid.DataColumns[1].EditFormSettings.Visible = DefaultBoolean.True;
                grid.DataColumns[1].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                grid.DataColumns[1].CellStyle.BackColor = System.Drawing.Color.Bisque;
                grid.DataColumns[1].PropertiesEdit.Style.Width = Unit.Percentage(100);
                
                grid.DataColumns[2].Width = 80;
                grid.DataColumns[2].FieldName = "is_used";
                grid.DataColumns[2].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[2].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[2].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[2].Caption = "CONDITION";
                grid.DataColumns[2].VisibleIndex = 3;
                grid.DataColumns[2].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[2].Settings.AllowHeaderFilter = DefaultBoolean.True;
                grid.DataColumns[2].Settings.AllowAutoFilter = DefaultBoolean.False;
                grid.DataColumns[2].Settings.FilterMode = ColumnFilterMode.Value;
                grid.DataColumns[2].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[2].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                grid.DataColumns[2].CellStyle.BackColor = System.Drawing.Color.Bisque;

                grid.DataColumns[3].FieldName = "cust_id";
                grid.DataColumns[3].Width = 65;
                grid.DataColumns[3].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[3].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[3].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[3].Caption = "CUST ID";
                grid.DataColumns[3].VisibleIndex = 4;
                grid.DataColumns[3].EditFormSettings.Visible = DefaultBoolean.True;
                grid.DataColumns[3].EditFormSettings.VisibleIndex = 2;
                grid.DataColumns[3].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[3].Settings.AllowHeaderFilter = DefaultBoolean.True;
                grid.DataColumns[3].Settings.ShowFilterRowMenu = DefaultBoolean.False;


                grid.DataColumns[4].FieldName = "dr_sbo";
                grid.DataColumns[4].Width = 105;
                grid.DataColumns[4].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[4].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[4].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[4].Caption = "SBO AREA";
                grid.DataColumns[4].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[4].VisibleIndex = 5;
                grid.DataColumns[4].EditFormSettings.VisibleIndex = 5;
                grid.DataColumns[4].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[4].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[4].Settings.AllowHeaderFilter = DefaultBoolean.True;
                grid.DataColumns[4].Settings.AllowAutoFilter = DefaultBoolean.True;
                grid.DataColumns[4].EditFormSettings.Visible = DefaultBoolean.False;

                grid.DataColumns[5].FieldName = "dr_rep";
                grid.DataColumns[5].Width = 50;
                grid.DataColumns[5].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[5].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[5].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[5].Caption = "REP IN CHARGE";
                grid.DataColumns[5].VisibleIndex = 6;
                grid.DataColumns[5].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[5].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[5].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[5].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[6].FieldName = "rep_name";
                grid.DataColumns[6].Width = 175;
                grid.DataColumns[6].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[6].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[6].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[6].Caption = "REP NAME";
                grid.DataColumns[6].VisibleIndex = 7;
                grid.DataColumns[6].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[6].EditFormSettings.Visible = DefaultBoolean.False;

                grid.DataColumns[7].FieldName = "dr_am";
                grid.DataColumns[7].Width = 50;
                grid.DataColumns[7].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[7].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[7].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[7].Caption = "AM IN CHARGE";
                grid.DataColumns[7].VisibleIndex = 8;
                grid.DataColumns[7].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[7].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[7].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[7].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[7].EditFormSettings.Visible = DefaultBoolean.False;

                grid.DataColumns[8].FieldName = "am_name";
                grid.DataColumns[8].Width = 150;
                grid.DataColumns[8].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[8].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[8].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[8].Caption = "AM NAME";
                grid.DataColumns[8].VisibleIndex = 9;
                grid.DataColumns[8].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[8].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[8].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[8].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[9].FieldName = "dr_region";
                grid.DataColumns[9].Width = 65;
                grid.DataColumns[9].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[9].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[9].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[9].Caption = "REG.";
                grid.DataColumns[9].VisibleIndex = 10;
                grid.DataColumns[9].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[9].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[9].Settings.AllowHeaderFilter = DefaultBoolean.True;
                grid.DataColumns[9].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[9].EditFormSettings.Visible = DefaultBoolean.False;

                grid.DataColumns[10].FieldName = "dr_spec";
                grid.DataColumns[10].Width = 55;
                grid.DataColumns[10].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[10].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[10].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[10].Caption = "SPEC.";
                grid.DataColumns[10].VisibleIndex = 11;
                grid.DataColumns[10].EditFormSettings.VisibleIndex = 11;
                grid.DataColumns[10].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[10].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[10].EditFormSettings.Visible = DefaultBoolean.True;

                grid.DataColumns[11].FieldName = "dr_sub_spec";
                grid.DataColumns[11].Width = 75;
                grid.DataColumns[11].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[11].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[11].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[11].Caption = "SUB SPEC.";
                grid.DataColumns[11].VisibleIndex = 12;
                grid.DataColumns[11].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[11].EditFormSettings.VisibleIndex = 12;
                grid.DataColumns[11].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[11].EditFormSettings.Visible = DefaultBoolean.True;
                grid.DataColumns[11].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[12].FieldName = "dr_quadrant";
                grid.DataColumns[12].Width = 55;
                grid.DataColumns[12].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[12].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[12].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[12].Caption = "QUAD";
                grid.DataColumns[12].VisibleIndex = 13;
                grid.DataColumns[12].EditFormSettings.VisibleIndex = 13;
                grid.DataColumns[12].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[12].EditFormSettings.Visible = DefaultBoolean.True;
                grid.DataColumns[12].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                grid.DataColumns[12].ReadOnly = true;

                grid.DataColumns[13].FieldName = "dr_monitoring";
                grid.DataColumns[13].Width = 175;
                grid.DataColumns[13].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[13].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[13].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[13].Caption = "MONITORING";
                grid.DataColumns[13].VisibleIndex = 14;
                grid.DataColumns[13].EditFormSettings.VisibleIndex = 14;
                grid.DataColumns[13].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[13].EditFormSettings.Visible = DefaultBoolean.False;

                grid.DataColumns[14].FieldName = "dr_address";
                grid.DataColumns[14].Width = 225;
                grid.DataColumns[14].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[14].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[14].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[14].Caption = "ADDRESS";
                grid.DataColumns[14].VisibleIndex = 15;
                grid.DataColumns[14].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[14].EditFormSettings.VisibleIndex = 15;

                grid.DataColumns[15].FieldName = "dr_area_mis";
                grid.DataColumns[15].Width = 80;
                grid.DataColumns[15].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[15].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[15].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[15].Caption = "AREA MIS";
                grid.DataColumns[15].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[15].VisibleIndex = 16;
                grid.DataColumns[15].EditFormSettings.VisibleIndex = 16;
                grid.DataColumns[15].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[15].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[16].FieldName = "dr_sum";
                grid.DataColumns[16].Width = 35;
                grid.DataColumns[16].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[16].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[16].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[16].Caption = "SUM";
                grid.DataColumns[16].VisibleIndex = 17;
                grid.DataColumns[16].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[16].EditFormSettings.Visible = DefaultBoolean.False;
                grid.DataColumns[16].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[16].Settings.ShowFilterRowMenu = DefaultBoolean.False;


                grid.DataColumns[17].FieldName = "dr_category";
                grid.DataColumns[17].Width = 110;
                grid.DataColumns[17].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[17].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[17].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[17].Caption = "CTG.";
                grid.DataColumns[17].VisibleIndex = 18;
                grid.DataColumns[17].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[17].EditFormSettings.VisibleIndex = 18;
                grid.DataColumns[17].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[17].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[18].FieldName = "dr_sub_category";
                grid.DataColumns[18].Width = 110;
                grid.DataColumns[18].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[18].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[18].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[18].Caption = "SUB CTG.";
                grid.DataColumns[18].VisibleIndex = 19;
                grid.DataColumns[18].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[18].EditFormSettings.VisibleIndex = 19;
                grid.DataColumns[18].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[18].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[19].FieldName = "dr_chanel";
                grid.DataColumns[19].Width = 75;
                grid.DataColumns[19].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[19].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[19].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[19].Caption = "CHANNEL";
                grid.DataColumns[19].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[19].VisibleIndex = 20;
                grid.DataColumns[19].EditFormSettings.VisibleIndex = 20;
                grid.DataColumns[19].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[19].Settings.ShowFilterRowMenu = DefaultBoolean.False;


                grid.DataColumns[20].FieldName = "dr_day_visit";
                grid.DataColumns[20].Caption = "DAY VISIT";
                grid.DataColumns[20].Width = 75;
                grid.DataColumns[20].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[20].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[20].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[20].VisibleIndex = 21;
                grid.DataColumns[20].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[20].EditFormSettings.VisibleIndex = 21;
                grid.DataColumns[20].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[20].Settings.ShowFilterRowMenu = DefaultBoolean.False;


                grid.DataColumns[21].FieldName = "dr_visiting_hour";
                grid.DataColumns[21].Width = 75;
                grid.DataColumns[21].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[21].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[21].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[21].Caption = "VISIT HOUR";
                grid.DataColumns[21].VisibleIndex = 22;
                grid.DataColumns[21].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[21].EditFormSettings.VisibleIndex = 22;
                grid.DataColumns[21].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[21].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[22].FieldName = "dr_number_patient";
                grid.DataColumns[22].Width = 50;
                grid.DataColumns[22].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[22].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[22].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[22].Caption = "PATIENT NUMBER";
                grid.DataColumns[22].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[22].VisibleIndex = 23;
                grid.DataColumns[22].EditFormSettings.VisibleIndex = 23;
                grid.DataColumns[22].Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                grid.DataColumns[22].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[23].FieldName = "dr_kol_not";
                grid.DataColumns[23].Width = 65;
                grid.DataColumns[23].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[23].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[23].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[23].Caption = "KOL/NOT";
                grid.DataColumns[23].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[23].VisibleIndex = 24;
                grid.DataColumns[23].EditFormSettings.VisibleIndex = 24;
                grid.DataColumns[23].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[23].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[24].FieldName = "dr_gender";
                grid.DataColumns[24].Width = 70;
                grid.DataColumns[24].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[24].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[24].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[24].Caption = "GENDER";
                grid.DataColumns[24].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[24].VisibleIndex = 25;
                grid.DataColumns[24].EditFormSettings.VisibleIndex = 25;
                grid.DataColumns[24].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[24].Settings.ShowFilterRowMenu = DefaultBoolean.False;


                grid.DataColumns[25].FieldName = "dr_phone";
                grid.DataColumns[25].Width = 75;
                grid.DataColumns[25].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[25].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[25].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[25].Caption = "PHONE";
                grid.DataColumns[25].VisibleIndex = 26;
                grid.DataColumns[25].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[25].EditFormSettings.VisibleIndex = 26;
                grid.DataColumns[25].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[25].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[26].FieldName = "dr_email";
                grid.DataColumns[26].Width = 75;
                grid.DataColumns[26].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[26].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[26].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                grid.DataColumns[26].Caption = "EMAIL";
                grid.DataColumns[26].VisibleIndex = 27;
                grid.DataColumns[26].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[26].EditFormSettings.VisibleIndex = 27;
                grid.DataColumns[26].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[26].Settings.ShowFilterRowMenu = DefaultBoolean.False;
                                                        
                                                         grid.DataColumns[27].FieldName = "dr_birthday";
                grid.DataColumns[27].Width = 75;
                grid.DataColumns[27].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[27].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[27].CellStyle.HorizontalAlign = HorizontalAlign.Center;

                grid.DataColumns[27].Caption = "BIRTHDAY";
                grid.DataColumns[27].VisibleIndex = 28;
                grid.DataColumns[27].EditFormSettings.VisibleIndex = 28;
                grid.DataColumns[27].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[27].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[28].FieldName = "dr_dk_lk";
                grid.DataColumns[28].Width = 60;
                grid.DataColumns[28].HeaderStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[28].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[28].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[28].Caption = "DK/LK";
                grid.DataColumns[28].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[28].VisibleIndex = 29;
                grid.DataColumns[28].EditFormSettings.VisibleIndex = 29;
                grid.DataColumns[28].Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                grid.DataColumns[28].Settings.ShowFilterRowMenu = DefaultBoolean.False;

                grid.DataColumns[29].Caption = "STATUS";
                grid.DataColumns[29].Width = 75;
                grid.DataColumns[29].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[29].FieldName = "dr_status";
                grid.DataColumns[29].UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
                grid.DataColumns[29].VisibleIndex = 30;
                grid.DataColumns[29].CellStyle.Wrap = DefaultBoolean.True;
                grid.DataColumns[29].CellStyle.HorizontalAlign = HorizontalAlign.Center;
                grid.DataColumns[29].EditFormSettings.VisibleIndex = 30;
                grid.DataColumns[29].EditFormSettings.Visible = DefaultBoolean.True;
                grid.DataColumns[29].ReadOnly = true;

            }
        }
    }
}
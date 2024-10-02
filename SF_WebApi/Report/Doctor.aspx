<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Doctor.aspx.cs" Inherits="SF_WebApi.Report.Doctor" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
    .table {
        padding-right: 5px;
    }

    .cell-backround {
        background-color: #F0F0F0;
        color: black;
    }

    .cell-newplan {
        padding-top: 4px;
        padding-bottom: 4px;
    }

    .cell-month {
        width: 150px;
        padding-top: 4px;
        padding-bottom: 4px;
        padding-left: 5px;
    }

    .cell-year {
        width: 50px;
        padding-top: 4px;
        padding-bottom: 4px;
        padding-left: 20px;
    }

    .cell-retrieve {
        padding-top: 4px;
        padding-bottom: 4px;
        padding-left: 10px;
    }

    .cell-reset {
        padding-top: 4px;
        padding-bottom: 4px;
        padding-left: 5px;
    }

    .panel-content {
        margin-left: 8px;
        margin-top: 8px;
    }

    .distance-left {
        margin-left: 5px;
    }

    .distance-right {
        margin-right: 5px;
    }

    .cell-blank {
        width: 100%;
        background-color: #F0F0F0;
    }

    .cell-divider {
        height: 5px;
        width: 100%;
    }

    .title-form {
        padding-bottom: 4px;
        background-color:gainsboro;
    }
    .dxpc-shadow{ box-shadow: none !important }
</style>
</head>
    <form id="form1" runat="server">
        <table class="table" style="margin:0px">
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: REPORT DOCTOR ::</td></tr>
            <tr>
                <td class="cell-month cell-backround">
                    
                </td>
                <td class="cell-month cell-backround">

                </td>
                <td class="cell-retrieve cell-backround">

                </td>
                <td class="cell-reset cell-backround">

                </td>
                <td class="cell-blank">
                    <asp:TextBox ID="txtDep" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtPosition" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtNik" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td class="cell-blank"></td>
                <td class="cell-reset cell-backround"></td>
                <td class="cell-reset cell-backround"></td>
                <td class="cell-reset cell-backround">
                    <dx:ASPxButton ID="PDF" runat="server" Text="Export to PDF" ClientInstanceName="PDF" CssClass="button" ToolTip="Export to PDF" Theme="PlasticBlue" OnClick="BtnExportPdf_Click">
                        <Paddings PaddingLeft="1px" />
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Content/Images/pdf.png" Repeat="NoRepeat" VerticalPosition="center" />
                    </dx:ASPxButton>
                </td>
                <td class="cell-reset cell-backround">
                    <dx:ASPxButton ID="XLS" runat="server" Text="Export to Excell" ClientInstanceName="XLS" CssClass="distance-right" ToolTip="Export to Excell" Theme="PlasticBlue" OnClick="BtnExportExcel_Click">
                        <Paddings PaddingLeft="1px" />
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Content/Images/excel.png" Repeat="NoRepeat" VerticalPosition="center" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td style="padding-top:0px;vertical-align:top;width:80%">
                    <dx:ASPxGridView ID="ASPxGridView1" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Theme="PlasticBlue" KeyFieldName="dr_code" OnRowUpdated="ASPxGridView1_RowUpdated" OnDataBound="ASPxGridView1_DataBound" OnLoad="Page_Load">
                         <Columns>
                            <dx:GridViewDataTextColumn Caption="DOCTOR CODE" FieldName="dr_code" VisibleIndex="1" ShowInCustomizationForm="True" ReadOnly="True">
                                <CellStyle ForeColor="#0066CC">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="NAME" FieldName="dr_name" VisibleIndex="2" ShowInCustomizationForm="True">
                                <Settings AllowHeaderFilter="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowInFilterControl="True" />
                                <SettingsHeaderFilter Mode="List">
                                </SettingsHeaderFilter>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CONDITION" FieldName="is_used" ReadOnly="True" VisibleIndex="3" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CUST ID" FieldName="cust_id" VisibleIndex="4" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SBO AREA" FieldName="dr_sbo" VisibleIndex="5" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP IN CHARGE" FieldName="dr_rep" VisibleIndex="6" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP NAME" FieldName="rep_name" VisibleIndex="7" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM IN CHARGE" FieldName="dr_am" VisibleIndex="8" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM NAME" FieldName="am_name" VisibleIndex="9" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REG." FieldName="dr_region" VisibleIndex="10" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPEC." FieldName="dr_spec" VisibleIndex="11" ShowInCustomizationForm="True">
                                <PropertiesTextEdit DisplayFormatInEditMode="True">
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB SPEC." FieldName="dr_sub_spec" VisibleIndex="12" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="QUAD" FieldName="dr_quadrant" VisibleIndex="13" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MONITORING" FieldName="dr_monitoring" ReadOnly="True" VisibleIndex="14" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="ADDRESS" FieldName="dr_address" ReadOnly="True" VisibleIndex="15" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AREA MIS" FieldName="dr_area_mis" ReadOnly="True" VisibleIndex="16" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUM" FieldName="dr_sum" VisibleIndex="17" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CTG." FieldName="dr_category" VisibleIndex="18" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB CTG." FieldName="dr_sub_category" VisibleIndex="19" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CHANNEL" FieldName="dr_chanel" VisibleIndex="20" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DAY VISIT" FieldName="dr_day_visit" VisibleIndex="21" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="VISIT HOUR" FieldName="dr_visiting_hour" VisibleIndex="22" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PATIENT NUMBER" FieldName="dr_number_patient" VisibleIndex="23" ShowInCustomizationForm="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="KOL/NOT" FieldName="dr_kol_not" VisibleIndex="24" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="GENDER" FieldName="dr_gender" ReadOnly="True" VisibleIndex="25" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PHONE" FieldName="dr_phone" VisibleIndex="26" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="EMAIL" FieldName="dr_email" VisibleIndex="27" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn Caption="BIRTHDAY" FieldName="dr_birthday" VisibleIndex="28" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn Caption="DK/LK" FieldName="dr_dk_lk" ReadOnly="True" VisibleIndex="29" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn Caption="STATUS" FieldName="dr_status" VisibleIndex="31">
                                <EditFormSettings Visible="True" />
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0" ShowClearFilterButton="True" FixedStyle="Left" Width="45px">
                                <CellStyle BackColor="Bisque">
                                </CellStyle>
                            </dx:GridViewCommandColumn>
                        </Columns>
                        <SettingsEditing EditFormColumnCount="2" Mode="PopupEditForm" />
                            <Settings ShowFilterRow="True" />
                            <EditFormLayoutProperties>
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="900" />
                            </EditFormLayoutProperties>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_MASTER_DOCTOR_REPORT" SelectCommandType="StoredProcedure" UpdateCommand="SP_UPDATE_MASTER_DOCTOR_BY_REP" UpdateCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="department" QueryStringField="d" Type="String" />
                            <asp:QueryStringParameter Name="rep_position" QueryStringField="p" Type="String" />
                            <asp:QueryStringParameter Name="rep_id" QueryStringField="r" Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="dr_code" Type="Int32" />
                            <asp:Parameter Name="dr_name" Type="String" />
                            <asp:Parameter Name="dr_spec" Type="String" />
                            <asp:Parameter Name="dr_sub_spec" Type="String" />
                            <asp:Parameter Name="dr_address" Type="String" />
                            <asp:Parameter Name="dr_area_mis" Type="String" />
                            <asp:Parameter Name="dr_category" Type="String" />
                            <asp:Parameter Name="dr_sub_category" Type="String" />
                            <asp:Parameter Name="dr_chanel" Type="String" />
                            <asp:Parameter Name="dr_day_visit" Type="String" />
                            <asp:Parameter Name="dr_visiting_hour" Type="String" />
                            <asp:Parameter Name="dr_number_patient" Type="Int32" />
                            <asp:Parameter Name="dr_kol_not" Type="String" />
                            <asp:Parameter Name="dr_gender" Type="String" />
                            <asp:Parameter Name="dr_phone" Type="String" />
                            <asp:Parameter Name="dr_email" Type="String" />
                            <asp:Parameter Name="dr_birthday" Type="DateTime" />
                            <asp:Parameter Name="dr_dk_lk" Type="String" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
        </table>

    </form>
</html>

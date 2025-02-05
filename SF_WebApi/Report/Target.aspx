﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Target.aspx.cs" Inherits="SF_WebApi.Report.Target" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .table {
            width: 100%;
            padding-right: 5px;
            /*border:1px solid red;*/
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
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: REPORT TARGET ::</td></tr>
            <tr>
                <td class="cell-month cell-backround">
                    
                </td>
                <td class="cell-month cell-backround">

                </td>
                <td class="cell-retrieve cell-backround">

                </td>
                <td class="cell-blank">
                    <asp:TextBox ID="txtDep" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtPosition" runat="server" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtNik" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td class="cell-reset cell-backround">

                </td>
                <td class="cell-blank">
                    
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
                    <%--@Html.Action("ViewTarget", "ReportTarget")--%>
                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Theme="PlasticBlue" OnDataBound="ASPxGridView1_DataBound" OnCustomColumnDisplayText="grid_CustomColumnDisplayText">
                        <SettingsEditing EditFormColumnCount="2" Mode="PopupEditForm" />
                            <Settings ShowFilterRow="True" />
                            <EditFormLayoutProperties>
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="900" />
                            </EditFormLayoutProperties>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="SBO Code" FieldName="target_id" Visible="False" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="YEAR" FieldName="target_year" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Name="Month" Caption="MONTH" FieldName="month" ReadOnly="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SBO" FieldName="target_sbo" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PRODUCT CODE" FieldName="target_product_code" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PRODUCT DESCRIPTION" FieldName="prd_aso_desc" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CUSTOMER" FieldName="cust_name" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CHANNEL" FieldName="cust_outlet_channel" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DATA TYPE" FieldName="target_description" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PRODUCT CATEGORY" FieldName="target_product_category" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PRODUCT PRICE" FieldName="target_product_price" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PLAN QTY" FieldName="target_plan_qty" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="target_plan_value" VisibleIndex="12" Caption="PLAN VALUE">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MR" FieldName="rep_name" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="BO" FieldName="rep_bo" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM" FieldName="nama_am" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RM" FieldName="nama_rm" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REG" FieldName="rep_region" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_REPORT_TARGET" SelectCommandType="StoredProcedure" UpdateCommand="SP_UPDATE_MASTER_DOCTOR_BY_REP" UpdateCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtDep" Name="department" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtPosition" Name="rep_position" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" />
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

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPPivot.aspx.cs" Inherits="SF_WebApi.Report.SPPivot" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
    <style type="text/css">
        .table {
            padding-right: 5px;
            width: 100%;
            /*border:1px solid red;*/
        }

        .cell-backround {
            background-color: #F0F0F0;
            color: black;
        }

        .cell-newplan {
            padding-bottom: 4px;
            padding-top: 4px;
        }

        .cell-month {
            padding-bottom: 4px;
            padding-left: 5px;
            padding-top: 4px;
            width: 150px;
        }

        .cell-year {
            padding-bottom: 4px;
            padding-left: 20px;
            padding-top: 4px;
            width: 50px;
        }

        .cell-retrieve {
            padding-bottom: 4px;
            padding-left: 10px;
            padding-top: 4px;
        }

        .cell-reset {
            padding-bottom: 4px;
            padding-left: 5px;
            padding-top: 4px;
        }

        .panel-content {
            margin-left: 8px;
            margin-top: 8px;
        }

        .distance-left { margin-left: 5px; }

        .distance-right { margin-right: 5px; }

        .cell-blank {
            background-color: #F0F0F0;
            width: 100%;
        }

        .cell-divider {
            height: 5px;
            width: 100%;
        }

        .title-form {
            background-color: gainsboro;
            padding-bottom: 4px;
        }

        .dxpc-shadow{ box-shadow: none !important }
    </style>
    <script>
        function do_retrieve() {
            ASPxPivotGrid1.PerformCallback();
        }

        function do_reset() {
            dateStart.SetValue(null);
            endDate.SetValue(null);
            var param = 'reset;' + null + ';' + null + ';' + null;
            ASPxPivotGrid1.PerformCallback({ prm: param });
            
            return false;
        }
    </script>
    <form id="form1" runat="server">
        <table class="table" style="margin: 0px">
            <tr><td class="title-form" style="font-weight: bold; text-align: center;" colspan="11">:: SALES PROMOTION PIVOT ::</td></tr>
            <tr>
                <td class="cell-month cell-backround">
                    <dx:ASPxDateEdit ID="startDate" runat="server" Caption="Date Start" CaptionSettings="left" ClientInstanceName="dateStart" EditFormatString="dd/MM/yyyy" Theme="PlasticBlue" >
                    </dx:ASPxDateEdit>
                </td>
                <td class="cell-month cell-backround">
                    <dx:ASPxDateEdit ID="endDate" runat="server" Caption="Date End" CaptionSettings="left" ClientInstanceName="endDate" EditFormatString="dd/MM/yyyy" Theme="PlasticBlue" >
                    </dx:ASPxDateEdit>
                </td>
                <td class="cell-retrieve cell-backround">
                    <dx:ASPxButton ID="btRetrieve" runat="server" Text="Retrieve" ClientInstanceName="btnRetrieve" CssClass="button" ToolTip="Retrieve" Theme="PlasticBlue" ClientSideEvents-Click="do_retrieve" AutoPostBack="false">
                        <Paddings PaddingLeft="1px" />
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Content/Images/retrieve.png" Repeat="NoRepeat" VerticalPosition="center" />
                    </dx:ASPxButton>
                </td>
                <td class="cell-reset cell-backround">
                    <dx:ASPxButton ID="btnReset" runat="server" Text="Reset" ClientInstanceName="btnReset" CssClass="button" ToolTip="Reset" Theme="PlasticBlue"  ClientSideEvents-Click="do_reset" AutoPostBack="false">
                        <Paddings PaddingLeft="1px" />
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Content/Images/reset.png" Repeat="NoRepeat" VerticalPosition="center" />
                    </dx:ASPxButton>
                </td>
                <td class="cell-blank">
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
                    <dx:ASPxButton ID="RawData" runat="server" Text="Export Row Data" ClientInstanceName="RawData" CssClass="button" ToolTip="Export Row Data" Theme="PlasticBlue" OnClick="BtnExportDataRow_Click">
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
        <table style="width: 100%;">
            <tr>
                <td style="padding-top: 0px; vertical-align: top; width: 80%">
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="PlasticBlue" Width="100%" OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                        <Fields>
                            <dx:PivotGridField ID="fieldsptype" Area="RowArea" AreaIndex="0" Caption="SP TYPE" FieldName="sp_type">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldproduct" Area="RowArea" AreaIndex="1" Caption="PRODUCT" FieldName="product">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamaam" Area="DataArea" AreaIndex="6" Caption="AM" FieldName="nama_am" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldename" Area="DataArea" AreaIndex="5" Caption="EVENT NAME" FieldName="e_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldeplace" Area="DataArea" AreaIndex="5" Caption="PLACE" FieldName="e_place" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldedtstart" Area="DataArea" AreaIndex="5" Caption="START DATE" FieldName="e_dt_start" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldedtend" Area="DataArea" AreaIndex="5" Caption="END DATE" FieldName="e_dt_end" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcode" Area="DataArea" AreaIndex="5" Caption="DR CODE" FieldName="dr_code" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrname" Area="DataArea" AreaIndex="5" Caption="DR NAME" FieldName="dr_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldodescription" Area="DataArea" AreaIndex="5" Caption="COST CENTER" FieldName="o_description" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepname" Area="DataArea" AreaIndex="5" Caption="REP NAME" FieldName="rep_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepbo" Area="DataArea" AreaIndex="5" Caption="BO" FieldName="rep_bo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepregion" Area="DataArea" AreaIndex="5" Caption="REGION" FieldName="rep_region" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamappm" Area="DataArea" AreaIndex="5" Caption="PPM" FieldName="nama_ppm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrquadrant" Area="DataArea" AreaIndex="5" Caption="QUADRANT" FieldName="dr_quadrant" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspid" Area="DataArea" AreaIndex="5" Caption="SP ID" FieldName="sp_id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsprid" Area="DataArea" AreaIndex="5" Caption="SPR ID" FieldName="spr_id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsprno" Area="DataArea" AreaIndex="5" Caption="SPR NO" FieldName="spr_no" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldetopic" Area="DataArea" AreaIndex="5" Caption="TOPIC" FieldName="e_topic" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsponsor" Area="DataArea" AreaIndex="5" Caption="SPONSOR" FieldName="sponsor" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepsbo" Area="DataArea" AreaIndex="5" Caption="SBO" FieldName="rep_sbo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamarm" Area="DataArea" AreaIndex="5" Caption="RM" FieldName="nama_rm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrspec" Area="DataArea" AreaIndex="5" Caption="SPEC" FieldName="dr_spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsubspec" Area="DataArea" AreaIndex="5" Caption="SUB SPEC" FieldName="dr_sub_spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsprstatus" Area="DataArea" AreaIndex="5" Caption="SPR STATUS" FieldName="spr_status" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspstatus" Area="DataArea" AreaIndex="5" Caption="SP STATUS" FieldName="sp_status" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcountspplan" Area="DataArea" AreaIndex="4" Caption="COUNT SP PLAN" FieldName="count_sp_plan" CellFormat-FormatString="N2" CellFormat-FormatType="Numeric" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcountspreal" Area="DataArea" AreaIndex="5" Caption="COUNT SP REAL" FieldName="count_sp_real" CellFormat-FormatString="N2" CellFormat-FormatType="Numeric" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcountdrplan" Area="DataArea" AreaIndex="0" Caption="DR PLAN" FieldName="count_dr_plan" UnboundType="Integer" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcountdrreal" Area="DataArea" AreaIndex="1" Caption="DR ACTUAL" FieldName="count_dr_real" UnboundType="Integer" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric" CellFormat-FormatString="N2" CellFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldbudgetrealvalue" Area="DataArea" AreaIndex="3" Caption="NOMINAL ACTUAL" FieldName="budget_real_value" CellFormat-FormatString="N2" CellFormat-FormatType="Numeric" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldbudgetplanvalue" Area="DataArea" AreaIndex="2" Caption="NOMINAL PLAN" FieldName="budget_plan_value" CellFormat-FormatString="N2" CellFormat-FormatType="Numeric" ValueFormat-FormatString="N2" ValueFormat-FormatType="Numeric">
                            </dx:PivotGridField>
                        </Fields>
                        <OptionsView ShowGrandTotalsForSingleValues="True" ShowRowGrandTotals="True" ShowRowTotals="False" ShowTotalsForSingleValues="False" ShowFilterHeaders="True" RowTotalsLocation="Far" HorizontalScrollBarMode="Auto"/>
                        <Styles>
                            <HeaderStyle Font-Size="7pt" />
                            <CellStyle Font-Size="7"/>
                            <ColumnAreaStyle Font-Size="7"/>
                            <RowAreaStyle Font-Size="7"/>
                            <FieldValueGrandTotalStyle Font-Size="7"/>
                            <GrandTotalCellStyle Font-Size="7"/>
                            <FieldValueStyle Wrap="False">
                            </FieldValueStyle>
                        </Styles>
                    </dx:ASPxPivotGrid>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_SP_PIVOT_REPORT" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value" DbType="DateTime" DefaultValue="2019-01-01"/>
                            <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value" DbType="DateTime" DefaultValue="2019-01-01"/>
                            <asp:ControlParameter ControlID="txtPosition" DefaultValue="0" Name="position" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtNik" DefaultValue="0" Name="nik" PropertyName="Text" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="PlasticBlue" Visible="False">
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="SP ID" FieldName="sp_id" ReadOnly="True" VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPR ID" FieldName="spr_id" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPR NO" FieldName="spr_no" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SP TYPE" FieldName="sp_type" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="EVENT NAME" FieldName="e_name" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TOPIC" FieldName="e_topic" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PLACE" FieldName="e_place" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn Caption="DATE START" FieldName="e_dt_start" VisibleIndex="8">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="DATE END" FieldName="e_dt_end" VisibleIndex="9">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="DATE REALIZATION" FieldName="date_realization" VisibleIndex="10">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn Caption="PRODUCT" FieldName="product" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPONSOR" FieldName="sponsor" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR NAME" FieldName="dr_name" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR PLAN" FieldName="count_sp_plan" ReadOnly="True" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR REAL" FieldName="count_sp_real" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="COUNT SP PLAN" FieldName="count_dr_plan" ReadOnly="True" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="COUNT SP REAL" FieldName="count_dr_real" ReadOnly="True" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="NOMINAL PLAN" FieldName="budget_plan_value" ReadOnly="True" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="NOMINAL ACTUAL" FieldName="budget_real_value" ReadOnly="True" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP NAME" FieldName="rep_name" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SBO" FieldName="rep_sbo" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="BO" FieldName="rep_bo" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REGION" FieldName="rep_region" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM" FieldName="nama_am" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RM" FieldName="nama_rm" VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PPM" FieldName="nama_ppm" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPEC" FieldName="dr_spec" VisibleIndex="30">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB SPEC" FieldName="dr_sub_spec" VisibleIndex="31">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="QUADRANT" FieldName="dr_quadrant" VisibleIndex="32">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MONITORING" FieldName="dr_monitoring" VisibleIndex="33">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR CODE" FieldName="dr_code" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="COST CENTER" FieldName="o_description" VisibleIndex="34">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="ASPxGridView1"></dx:ASPxGridViewExporter>
                </td>
                <td style="width: 20%">
                    <%--@Html.DevExpress().PivotCustomizationExtension(TANABE_MVC.SPPivotGridLayout.SPPivotGridSettings).GetHtml()--%>
                    <dx:ASPxPivotCustomizationControl ID="ASPxPivotCustomizationControl1" runat="server" AllowFilter="True" AllowSort="True" ASPxPivotGridID="ASPxPivotGrid1" Height="480px" Layout="StackedSideBySide" Theme="PlasticBlue" Width="300px">
                    </dx:ASPxPivotCustomizationControl>
                </td>
            </tr>
        </table>
        <asp:TextBox runat="server" Text="<%$appSettings:host%>" ID="baseUrl" style="display: none;"/>
    </form>
    
    <script type="text/javascript">
        //function gup(name, url) {
        //    name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
        //    var results = new RegExp('[?&]' + name + '=?([^&#]*)').exec(url || window.location.href);
        //    return results == null ? null : results[1] || true;
        //}

        //var p = gup("p");
        //var r = gup("r");
            <%--$(document).ready(function () {
            function gup(name, url) {
                name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
                var results = new RegExp('[?&]' + name + '=?([^&#]*)').exec(url || window.location.href);
                return results == null ? null : results[1] || true;
            }

            var p = gup("p");
            var n = gup("n");
            $('#btnReset').click(function () {
                window.location.href = document.getElementById("<%= baseUrl.ClientID %>").value + "/Report/SalesUser.aspx?p=" + p + "&n=" + n;
            });
            
            $('#btRetrieve').click(function () {
                var d1 = new Date(document.getElementById("startDate").value);
                var startDate = d1.getDate() + "/" + (d1.getMonth() + 1) + "/" + d1.getFullYear();
                var d2 = new Date(document.getElementById("endDate").value);
                var endDate = d2.getDate() + "/" + (d2.getMonth() + 1) + "/" + d2.getFullYear();
                console.log(startDate+" ---  "+endDate);
                window.location.href = document.getElementById("<%= baseUrl.ClientID %>").value + "/Report/SPPivot.aspx?p=" + p + "&n=" + n + "&s=" + startDate + "&e=" + endDate;
            });
            
            if (gup("s") != "")
                document.getElementById("startDate").value = gup("s");

            if (gup("e") != "")
                document.getElementById("endDate").value = gup("e");
        });--%>
        
    </script>
    
</html>
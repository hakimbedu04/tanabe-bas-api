﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitProduct.aspx.cs" Inherits="SF_WebApi.Report.VisitProduct" %>

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
        <table class="table" style="margin:0px">
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: VISIT WITH PRODUCT PIVOT ::</td></tr>
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
                    <dx:ASPxButton ID="XLS" runat="server" Text="Export to Excel" ClientInstanceName="XLS" CssClass="distance-right" ToolTip="Export to Excel" Theme="PlasticBlue" OnClick="BtnExportExcel_Click">
                        <Paddings PaddingLeft="1px" />
                        <BackgroundImage HorizontalPosition="left" ImageUrl="~/Content/Images/excel.png" Repeat="NoRepeat" VerticalPosition="center" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td style="padding-top:0px;vertical-align:top;width:80%">
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" Theme="PlasticBlue" OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                        <Fields>
                            <dx:PivotGridField ID="fieldplan" Area="DataArea" AreaIndex="0" Caption="PLAN" FieldName="plan">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrealization" Area="DataArea" AreaIndex="1" Caption="REAL" FieldName="realization">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="achv" Area="DataArea" AreaIndex="2" Caption="ACHV" UnboundExpression="Iif([fieldplan] &lt;&gt; 0, [fieldrealization] / [fieldplan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([fieldplan] &lt;&gt; 0, [fieldrealization] / [fieldplan], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="rem" Area="DataArea" AreaIndex="3" Caption="REM" UnboundExpression="Iif([fieldplan] &lt;&gt; 0, [fieldrealization] - [fieldplan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([fieldplan] &lt;&gt; 0, [fieldrealization] - [fieldplan], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldposition" AreaIndex="0" Caption="POSITION" FieldName="position" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddivision" AreaIndex="0" Caption="DIVISION" FieldName="division" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldbo" AreaIndex="0" Caption="BO" FieldName="bo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsbo" AreaIndex="0" Caption="SBO" FieldName="sbo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepam" AreaIndex="0" Caption="REP AM" FieldName="rep_am" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldreprm" AreaIndex="0" Caption="REP RM" FieldName="rep_rm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrm" AreaIndex="0" Caption="RM" FieldName="rm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepppm" AreaIndex="0" Caption="REP PPM" FieldName="rep_ppm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldppm" AreaIndex="0" Caption="PPM" FieldName="ppm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmonth" AreaIndex="0" Caption="MONTH" FieldName="month" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcode" AreaIndex="0" Caption="DR CODE" FieldName="dr_code" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldinfo" AreaIndex="0" Caption="INFO" FieldName="info" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsp" AreaIndex="0" Caption="SP" FieldName="sp" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspvalue" AreaIndex="0" Caption="SP VALUE" FieldName="sp_value" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddocname" AreaIndex="0" Caption="DR NAME" FieldName="doc_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspec" AreaIndex="0" Caption="SPEC" FieldName="spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsubspec" AreaIndex="0" Caption="SUB SPEC" FieldName="sub_spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldquadrant" AreaIndex="0" Caption="QUADRANT" FieldName="quadrant" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldregion" Area="RowArea" AreaIndex="0" Caption="REGION" FieldName="region">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmonitoring" AreaIndex="0" Caption="MONITORING" FieldName="monitoring" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddklk" AreaIndex="0" Caption="DKLK" FieldName="dklk" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldareamis" AreaIndex="0" Caption="AREA MIS" FieldName="area_mis" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcategory" AreaIndex="0" Caption="CTGY" FieldName="category" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldchannel" AreaIndex="0" Caption="CHANNEL" FieldName="channel" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepname" Area="RowArea" AreaIndex="2" Caption="TI NAME" FieldName="rep_name">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldam" AreaIndex="1" Caption="AM" FieldName="am" Area="RowArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldvisittype" AreaIndex="1" Caption="VISIT TYPE" FieldName="visit_type" Visible="False">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="MVC_SP_SELECT_VISIT_PIVOT_MOBILE" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="date_start" PropertyName="Value"  DefaultValue="01-01-2019" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="endDate" Name="date_end" PropertyName="Value"  DefaultValue="01-01-2019" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="0"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False" KeyFieldName="visit_id">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="rep_id" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="position" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="division" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="bo" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sbo" ReadOnly="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_am" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="am" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="region" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_rm" ReadOnly="True" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rm" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_ppm" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ppm" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="visit_id" VisibleIndex="13" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="plan" VisibleIndex="14" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="realization" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="month" VisibleIndex="16" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="visit_date" VisibleIndex="17" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_code" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_name" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="spec" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sub_spec" VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="quadrant" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="monitoring" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dklk" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="area_mis" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="category" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="channel" VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="info" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_value" VisibleIndex="30">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="visit_product" VisibleIndex="31">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="vd_value" VisibleIndex="32">
                            </dx:GridViewDataTextColumn>
                            <%--<dx:GridViewDataTextColumn FieldName="topic_id" VisibleIndex="33">
                            </dx:GridViewDataTextColumn>--%>
                            <dx:GridViewDataTextColumn FieldName="topic_title" VisibleIndex="34">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="vpt_feedback" VisibleIndex="35">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="feedback_desc" VisibleIndex="36">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="vsit_type" VisibleIndex="37">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="ASPxGridView1"></dx:ASPxGridViewExporter>
                </td>
                <td style="width:20%">
                <dx:ASPxPivotCustomizationControl ID="ASPxPivotCustomizationControl1" runat="server" AllowFilter="True" AllowSort="True" ASPxPivotGridID="ASPxPivotGrid1" Height="480px" Layout="StackedSideBySide" Theme="PlasticBlue" Width="300px">
                    </dx:ASPxPivotCustomizationControl>
            </td>
            </tr>
            
        </table>
        <asp:TextBox runat="server" Text="<%$appSettings:host%>" ID="baseUrl" style="display: none;"/>
    </form>

    <script type="text/javascript">
        <%--function gup(name, url) {
            name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
            var results = new RegExp('[?&]' + name + '=?([^&#]*)').exec(url || window.location.href);
            return results == null ? null : results[1] || true;
        }

        var p = gup("p");
        var r = gup("r");

        function reloadPage(){
            window.location.href = document.getElementById("<%= baseUrl.ClientID %>").value + "/Report/VisitOnlyPivot.aspx?p="+p+"&r="+r;
        }

        function filterPage(){
            var startDate = document.getElementById("startDate").value;
            var endDate = document.getElementById("endDate").value;
            window.location.href = document.getElementById("<%= baseUrl.ClientID %>").value + "/Report/VisitOnlyPivot.aspx?p="+p+"&r="+r+"&s=" + startDate +"&e="+endDate;
        }

        if (gup("s") != "")
            document.getElementById("startDate").value = gup("s");
        
        if (gup("e") != "")
            document.getElementById("endDate").value = gup("e");--%>
    </script>
</html>

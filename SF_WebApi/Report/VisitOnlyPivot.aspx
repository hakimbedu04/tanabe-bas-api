﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitOnlyPivot.aspx.cs" Inherits="SF_WebApi.Report.VisitOnlyPivot" %>

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
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: VISIT ONLY PIVOT ::</td></tr>
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
        <table style="width:100%;">
            <tr>
                <td style="padding-top:0px;vertical-align:top;width:80%">
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" Theme="PlasticBlue" OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                        <Fields>
                            <dx:PivotGridField ID="Plan" Area="DataArea" AreaIndex="0" Caption="PLAN" FieldName="plan">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Real" Area="DataArea" AreaIndex="1" Caption="REAL" FieldName="realization">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Percent" Area="DataArea" AreaIndex="2" Caption="ACHV" UnboundExpression="Iif([Plan] &lt;&gt; 0, [Real] / [Plan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([PLAN] &lt;&gt; 0, [REAL] / [PLAN], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Remaining" Area="DataArea" AreaIndex="3" Caption="REM"  UnboundExpression="Iif([Plan] &lt;&gt; 0, [Real] - [Plan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([PLAN] &lt;&gt; 0, [REAL] - [PLAN], 0)" UnboundType="String">
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
                            <dx:PivotGridField ID="fieldregion" Area="RowArea" AreaIndex="0" Caption="REGION" FieldName="region" Visible="False">
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
                            <dx:PivotGridField ID="fieldrepname" Area="RowArea" AreaIndex="1" Caption="TI NAME" FieldName="rep_name">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldam" AreaIndex="0" Caption="AM" FieldName="am" Area="RowArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddatevisit" Area="RowArea" AreaIndex="2" Caption="DATE" FieldName="date_visit">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="
                        SELECT v.rep_id,mr.rep_full_name as rep_name,mr.rep_position as position,mr.rep_division as division,mr.rep_bo as bo,
		                        mr.rep_sbo as sbo,mr.rep_am,ISNULL(mr.nama_am,'VACANT') as am,mr.rep_region as region,mr.rep_rm,mr.nama_rm as rm,
		                        DATENAME(month,v.visit_date_plan) AS [month],DATEPART(DAY,v.visit_date_plan) AS [date_visit],v.dr_code as dr_code,
		                        v.visit_plan as [plan],CONVERT(VARCHAR(500),v.visit_info) as info,v.visit_sp AS sp,v.visit_sp_value as sp_value,
		                        v.visit_realization as realization, md.dr_name as doc_name, md.dr_spec as spec,md.dr_sub_spec as sub_spec,
		                        md.dr_quadrant as quadrant,c.cust_name as monitoring,md.dr_dk_lk as dklk,md.dr_area_mis as area_mis,
		                        md.dr_category as category,md.dr_chanel as channel, mr.rep_ppm, mr.nama_ppm as ppm, v.visit_type
                        FROM t_visit v WITH(NOLOCK)
                        LEFT JOIN v_rep_full mr ON mr.rep_id = v.rep_id
                        LEFT JOIN m_doctor md ON  v.dr_code = md.dr_code
                        LEFT JOIN m_customer_aso c ON c.cust_id = md.cust_id
                        WHERE v.rep_id = @rep_id AND (CONVERT (DATE,v.visit_date_plan) &gt;= @date_start AND CONVERT (DATE,v.visit_date_plan) &lt;= @date_end)
                        AND v.visit_plan_verification_status= 1 ">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="date_start" PropertyName="Value" DefaultValue="2019-01-01" DbType="Date"/>
                            <asp:ControlParameter ControlID="endDate" Name="date_end" PropertyName="Value" DefaultValue="2019-07-17" DbType="Date"/>
                            <asp:ControlParameter ControlID="txtPosition" Name="position" PropertyName="Text" Type="String" DefaultValue="MR"/>
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="12.36"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="REP ID" FieldName="rep_id" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP NAME" FieldName="rep_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="POSITION" FieldName="position" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DIVISION" FieldName="division" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="BO" FieldName="bo" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SBO" FieldName="sbo" ReadOnly="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP AM" FieldName="rep_am" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM" FieldName="am" ReadOnly="True" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REGION" FieldName="region" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP RM" FieldName="rep_rm" ReadOnly="True" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RM" FieldName="rm" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MONTH" FieldName="month" ReadOnly="True" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DATE VISIT" FieldName="date_visit" ReadOnly="True" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR CODE" FieldName="dr_code" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PLAN" FieldName="plan" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="INFO" FieldName="info" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SP" FieldName="sp" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SP VALUE" FieldName="sp_value" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REALIZATION" FieldName="realization" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DOC NAME" FieldName="doc_name" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPEC" FieldName="spec" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB SPEC" FieldName="sub_spec" VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="QUADRANT" FieldName="quadrant" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MONITORING" FieldName="monitoring" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DKLK" FieldName="dklk" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AREA MIS" FieldName="area_mis" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CATEGORY" FieldName="category" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CHANEL" FieldName="channel" VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP PPM" FieldName="rep_ppm" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PPM" FieldName="ppm" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="VISIT TYPE" FieldName="visit_type" VisibleIndex="30">
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

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coverage.aspx.cs" Inherits="SF_WebApi.Report.Coverage" %>

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
            monthVal.SetValue(null);
            ASPxSpinEdit1.SetValue(2019);
            var param = 'reset;' + null + ';' + null + ';' + null;
            ASPxPivotGrid1.PerformCallback({ prm: param });
            
            return false;
        }
    </script>
    <form id="form1" runat="server">
        <table class="table" style="margin:0px">
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: COVERAGE PIVOT ::</td></tr>
            <tr>
                <td class="cell-month cell-backround">
                    <%--<asp:dropdownlist runat="server" id="monthDate" EnableTheming="True">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">Maret</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">Mei</asp:ListItem>
                        <asp:ListItem Value="6">Juni</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">Agustus</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">Oktobery</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">Desember</asp:ListItem>
                    </asp:dropdownlist>--%>
                    <dx:ASPxComboBox ID="monthVal" ClientInstanceName="monthVal" runat="server" EnableTheming="True" Theme="PlasticBlue">
                        <Items>
                            <dx:ListEditItem Text="January" Value="1" />
                            <dx:ListEditItem Text="February" Value="2" />
                            <dx:ListEditItem Text="March" Value="3" />
                            <dx:ListEditItem Text="April" Value="4" />
                            <dx:ListEditItem Text="May" Value="5" />
                            <dx:ListEditItem Text="June" Value="6" />
                            <dx:ListEditItem Text="July" Value="7" />
                            <dx:ListEditItem Text="Augustus" Value="8" />
                            <dx:ListEditItem Text="September" Value="9" />
                            <dx:ListEditItem Text="October" Value="10" />
                            <dx:ListEditItem Text="November" Value="11" />
                            <dx:ListEditItem Text="December" Value="12" />
                        </Items>
                    </dx:ASPxComboBox>
                </td>
                <td class="cell-month cell-backround">
                    <dx:ASPxSpinEdit ID="ASPxSpinEdit1" ClientInstanceName="ASPxSpinEdit1" runat="server" MaxValue="2100" MinValue="2014" Number="2019" Theme="PlasticBlue">
                    </dx:ASPxSpinEdit>
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
                            <dx:PivotGridField ID="fielddrplan" Area="DataArea" AreaIndex="0" Caption="PLAN" FieldName="dr_plan">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrreal" Area="DataArea" AreaIndex="1" Caption="REAL" FieldName="dr_real">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Percent" Area="DataArea" AreaIndex="2" Caption="SUM COVERAGE IN %" UnboundExpression="Iif([fielddrplan] &lt;&gt; null, [fielddrreal] / [fielddrplan], 0)" UnboundFieldName="Iif([fielddrplan] &lt;&gt; null, [fielddrreal] / [fielddrplan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="field1" Area="DataArea" AreaIndex="3" Caption="REM"  UnboundExpression="Iif([fielddrplan] &lt;&gt; null, [fielddrreal] - [fielddrplan], 0)" UnboundFieldName="Iif([fielddrplan] &lt;&gt; null, [fielddrreal] - [fielddrplan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundType="String">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcode" AreaIndex="0" Caption="DR CODE" FieldName="dr_code" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamarm" AreaIndex="0" Caption="RM" FieldName="nama_rm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepbo" AreaIndex="0" Caption="BO" FieldName="rep_bo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepsbo" AreaIndex="0" Caption="SBO" FieldName="rep_sbo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepposition" AreaIndex="0" Caption="POSITION" FieldName="rep_position" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepdivision" AreaIndex="0" Caption="DIVISION" FieldName="rep_division" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrname" AreaIndex="0" Caption="DR NAME" FieldName="dr_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrspec" AreaIndex="0" Caption="SPEC" FieldName="dr_spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamaam" AreaIndex="1" Caption="AM" FieldName="nama_am" Area="RowArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrquadrant" AreaIndex="0" Caption="QUADRANT" FieldName="dr_quadrant" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddraddress" AreaIndex="0" Caption="ADDRESS" FieldName="dr_address" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrareamis" AreaIndex="0" Caption="AREA MIS" FieldName="dr_area_mis" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcategory" AreaIndex="0" Caption="CATEGORY" FieldName="dr_category" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrchanel" AreaIndex="0" Caption="CHANNEL" FieldName="dr_chanel" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepname" AreaIndex="2" Caption="TI NAME" FieldName="rep_name" Area="RowArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepregion" AreaIndex="0" Caption="REGION" FieldName="rep_region" Area="RowArea">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_COVERAGE_PIVOT" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="monthVal" Name="month_coverage" PropertyName="Value" Type="Int32" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ASPxSpinEdit1" Name="year_coverage" PropertyName="Number" Type="Int32" DefaultValue="0" />
                            <asp:ControlParameter ControlID="txtPosition" Name="position" PropertyName="Text" Type="String" DefaultValue="0" />
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="rep_id" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_code" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_plan" VisibleIndex="2" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_real" VisibleIndex="3" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_region" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_rm" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_am" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_name" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_position" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_division" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_sbo" VisibleIndex="10" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_bo" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_ppm" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_code1" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sbo" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_name" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_spec" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sub_spec" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_quadrant" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cust_id" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_address" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_area_mis" VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sum" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_category" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sub_category" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_chanel" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_day_visit" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_visiting_hour" VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_number_patient" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_kol_not" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_gender" VisibleIndex="30">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_phone" VisibleIndex="31">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_email" VisibleIndex="32">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="dr_birthday" VisibleIndex="33">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_dk_lk" VisibleIndex="34">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_used_session" VisibleIndex="35">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_used_remaining" VisibleIndex="36">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_used_month_session" VisibleIndex="37">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_status" VisibleIndex="38">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sales_session" VisibleIndex="39">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sales_month_session" VisibleIndex="40">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_monitoring" VisibleIndex="41">
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

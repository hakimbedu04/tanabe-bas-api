<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAdvancePivot.aspx.cs" Inherits="SF_WebApi.Report.EmployeeAdvancePivot" %>

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
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: EMPLOYEE ADVANCE PIVOT ::</td></tr>
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
                <td class="cell-reset cell-backround"></td>
            </tr>
        </table>
        <table style="width:100%;">
            <tr>
                <td style="padding-top:0px;vertical-align:top;width:80%">
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" ClientInstanceName="ASPxPivotGrid1" runat="server" DataSourceID="SqlDataSource1" Theme="PlasticBlue" OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                        <Fields>
                            <dx:PivotGridField ID="fieldeadebit" Area="DataArea" AreaIndex="0" Caption="EA DEBIT" FieldName="ea_debit">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldid" Area="DataArea" AreaIndex="0" Caption="ID" FieldName="id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldhdrid" Area="DataArea" AreaIndex="0" Caption="HDR ID" FieldName="hdr_id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldfy" Area="DataArea" AreaIndex="0" Caption="FY" FieldName="fy" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldyear" Area="DataArea" AreaIndex="0" Caption="YEAR" FieldName="year" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmonth" Area="DataArea" AreaIndex="0" Caption="MONTH" FieldName="month" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddate" Area="DataArea" AreaIndex="0" Caption="DATE" FieldName="date" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldpostingdate" Area="RowArea" AreaIndex="1" Caption="POSTING DATE" FieldName="posting_date">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsprno" Area="RowArea" AreaIndex="2" Caption="SPR NO" FieldName="spr_no">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldstatus" Area="ColumnArea" AreaIndex="0" Caption="STATUS" FieldName="status">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldallocationkey" AreaIndex="0" Caption="ALLOCATION KEY" FieldName="allocation_key" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldniksh" Area="DataArea" AreaIndex="0" Caption="NIK" FieldName="nik_sh" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamesh" Area="RowArea" AreaIndex="0" Caption="NAME" FieldName="name_sh">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddaybook" Area="DataArea" AreaIndex="0" Caption="DAYBOOK" FieldName="daybook" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldglaccount" AreaIndex="0" Caption="ACCOUNT" FieldName="gl_account" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescription" AreaIndex="0" Caption="DESCRIPTION" FieldName="description" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescriptionheader" Area="RowArea" AreaIndex="3" Caption="DESCRIPTION HEADER" FieldName="description_header">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescriptiondetail" AreaIndex="0" Caption="DESCRIPTION DETAIL" FieldName="description_detail" Visible="False">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_EMPLOYEE_ADVANCE_PIVOT" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="hdr_id" ReadOnly="True" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="fy" ReadOnly="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="year" ReadOnly="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="month" ReadOnly="True" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="date" ReadOnly="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="posting_date" ReadOnly="True" VisibleIndex="6">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="spr_no" ReadOnly="True" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="allocation_key" ReadOnly="True" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nik_sh" ReadOnly="True" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="name_sh" ReadOnly="True" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="daybook" ReadOnly="True" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="gl_account" ReadOnly="True" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description" ReadOnly="True" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description_header" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description_detail" ReadOnly="True" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ea_debit" ReadOnly="True" VisibleIndex="17">
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
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpensePivot.aspx.cs" Inherits="SF_WebApi.Report.ExpensePivot" %>
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
        <tr><td class="title-form" style="font-weight: bold; text-align: center;" colspan="11">:: EXPENSE PIVOT ::</td></tr>
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
                <td style="padding-top: 0px; vertical-align: top; width: 85%">
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" Theme="PlasticBlue" OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                        <Fields>
                            <dx:PivotGridField ID="fieldcostcenter" Area="RowArea" AreaIndex="0" Caption="COST CENTER" FieldName="cost_center">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldglaccount" Area="RowArea" AreaIndex="1" Caption="GL ACCOUNT" FieldName="gl_account">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescription" Area="RowArea" AreaIndex="2" Caption="DESCRIPTION" FieldName="description">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldbudget" Area="DataArea" AreaIndex="0" FieldName="budget" Caption="BUDGET">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldactual" Area="DataArea" AreaIndex="1" FieldName="actual" Caption="ACTUAL">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Percent" Area="DataArea" AreaIndex="2" Caption="%ACH" UnboundExpression="Iif([fieldbudget] &lt;&gt; 0, [fieldactual] / [fieldbudget], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([fieldbudget] &lt;&gt; 0, [fieldactual] / [fieldbudget], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="field" Area="DataArea" AreaIndex="3" Caption="+/-" UnboundExpression="Iif([fieldbudget] &lt;&gt; 0, [fieldactual] - [fieldbudget], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([fieldbudget] &lt;&gt; 0, [fieldactual] - [fieldbudget], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldid" Area="DataArea" AreaIndex="3" FieldName="id" Visible="False" Caption="ID">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldfy" Area="DataArea" AreaIndex="3" FieldName="fy" Visible="False" Caption="FY">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldyear" Area="DataArea" AreaIndex="3" FieldName="year" Visible="False" Caption="YEAR">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmonth" Area="DataArea" AreaIndex="3" FieldName="month" Visible="False" Caption="MONTH">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldpostingdate" Area="DataArea" AreaIndex="3" FieldName="posting_date" Visible="False" Caption="PSOTING DATE">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsprno" Area="DataArea" AreaIndex="3" FieldName="spr_no" Visible="False" Caption="SPR NO">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcostcentercode" Area="DataArea" AreaIndex="3" FieldName="cost_center_code" Visible="False" Caption="COST CENTER CODE">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescriptionheader" Area="DataArea" AreaIndex="3" FieldName="description_header" Visible="False" Caption="DESCRIPTION HEADER">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddescriptiondetail" Area="DataArea" AreaIndex="3" FieldName="description_detail" Visible="False" Caption="DESCRIPTION DETAIL">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafstructure" Area="DataArea" AreaIndex="3" FieldName="saf_structure" Visible="False" Caption="SAF STRUCTURE">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafcodeadmexpensetype" Area="DataArea" AreaIndex="3" FieldName="saf_code_adm_expense_type" Visible="False" Caption="SAF CODE (Adm Expense Type)">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafdescriptionadmexpensetype" Area="DataArea" AreaIndex="3" FieldName="saf_description_adm_expense_type" Visible="False" Caption="SAF DESCRIPTION (Adm Expense Type)">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafcodeproduct" Area="DataArea" AreaIndex="3" FieldName="saf_code_product" Visible="False" Caption="SAF CODE PRODUCT">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafdescriptionproduct" Area="DataArea" AreaIndex="3" FieldName="saf_description_product" Visible="False" Caption="SAF DESCRIPTION (Product)">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnikcostcenter" Area="DataArea" AreaIndex="3" FieldName="nik_cost_center" Visible="False" Caption="NIK COST CENTER">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnikinitiator" Area="DataArea" AreaIndex="3" FieldName="nik_initiator" Visible="False" Caption="NIK INITIATOR">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafcodeemployee" Area="DataArea" AreaIndex="3" FieldName="saf_code_employee" Visible="False" Caption="SAF CODE(Employee)">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsafcodedoctor" Area="DataArea" AreaIndex="3" FieldName="saf_code_doctor" Visible="False" Caption="SAF CODE(Doctor)">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrname" Area="DataArea" AreaIndex="3" FieldName="dr_name" Visible="False" Caption="DR NAME">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspec" Area="DataArea" AreaIndex="3" FieldName="spec" Visible="False" Caption="SPEC">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsubspec" Area="DataArea" AreaIndex="3" FieldName="sub_spec" Visible="False" Caption="SUB SPEC">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmonitoring" Area="DataArea" AreaIndex="3" FieldName="monitoring" Visible="False" Caption="MONITORING">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsbo" Area="DataArea" AreaIndex="3" FieldName="sbo" Visible="False" Caption="SBO">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldmr" Area="DataArea" AreaIndex="3" FieldName="mr" Visible="False" Caption="MR">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldbo" Area="DataArea" AreaIndex="3" FieldName="bo" Visible="False" Caption="BO">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldam" Area="DataArea" AreaIndex="3" FieldName="am" Visible="False" Caption="AM">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldregion" Area="DataArea" AreaIndex="3" FieldName="region" Visible="False" Caption="REGION">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrm" Area="DataArea" AreaIndex="3" FieldName="rm" Visible="False" Caption="RM">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_EXPENSE_PIVOT" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="PlasticBlue" Visible="False">
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="fy" ReadOnly="True" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="year" ReadOnly="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="month" ReadOnly="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="posting_date" ReadOnly="True" VisibleIndex="4">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="spr_no" ReadOnly="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cost_center_code" ReadOnly="True" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cost_center" ReadOnly="True" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="gl_account" ReadOnly="True" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description" ReadOnly="True" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description_header" ReadOnly="True" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="description_detail" ReadOnly="True" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="budget" ReadOnly="True" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="actual" ReadOnly="True" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_structure" ReadOnly="True" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_code_adm_expense_type" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_description_adm_expense_type" ReadOnly="True" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_code_product" ReadOnly="True" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_description_product" ReadOnly="True" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nik_cost_center" ReadOnly="True" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nik_initiator" ReadOnly="True" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_code_employee" ReadOnly="True" VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="saf_code_doctor" ReadOnly="True" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_name" ReadOnly="True" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="spec" ReadOnly="True" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sub_spec" ReadOnly="True" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="monitoring" ReadOnly="True" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sbo" ReadOnly="True" VisibleIndex="27">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="mr" ReadOnly="True" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="bo" ReadOnly="True" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="am" ReadOnly="True" VisibleIndex="30">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="region" ReadOnly="True" VisibleIndex="31">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rm" ReadOnly="True" VisibleIndex="32">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
                    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Theme="PlasticBlue">
                    </dx:ASPxLoadingPanel>
                </td>
                <td style="width: 15%">
                    <dx:ASPxPivotCustomizationControl ID="ASPxPivotCustomizationControl1" runat="server" AllowFilter="True" AllowSort="True" ASPxPivotGridID="ASPxPivotGrid1" Height="480px" Layout="StackedSideBySide" Theme="PlasticBlue" Width="300px">
                    </dx:ASPxPivotCustomizationControl>
                </td>
            </tr>
        </table>
        <asp:TextBox runat="server" Text="<%$appSettings:host%>" ID="baseUrl" style="display: none;"/>
    
</form>
</html>
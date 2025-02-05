﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesPivot.aspx.cs" Inherits="SF_WebApi.Report.SalesPivot" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="DevExpress.CodeParser" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

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
    <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: SALES PIVOT ASO::</td></tr>
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
            <dx:ASPxDateEdit ID="TextStartDate" runat="server" Caption="Date Start" CaptionSettings="left" ClientInstanceName="dateStart" EditFormatString="dd/MM/yyyy" Theme="PlasticBlue" Visible="False">
            </dx:ASPxDateEdit>
            <dx:ASPxDateEdit ID="TextEndDate" runat="server" Caption="Date Start" CaptionSettings="left" ClientInstanceName="dateStart" EditFormatString="dd/MM/yyyy" Theme="PlasticBlue" Visible="False">
            </dx:ASPxDateEdit>
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
            <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" Theme="PlasticBlue"
                OnCustomCellDisplayText="ASPxPivotGrid1_CustomCellDisplayText">
                <Fields>
                    <dx:PivotGridField ID="fieldDATA" AreaIndex="0" Caption="1A.DATA" FieldName="DATA">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="planValue" Area="DataArea" AreaIndex="0" Caption="4K.PLAN VALUE" FieldName="PLAN_VALUE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="salesValue" Area="DataArea" AreaIndex="1" Caption="4H.SALES VALUE" FieldName="SALES_VALUE">
                    </dx:PivotGridField>
                    
                    <dx:PivotGridField ID="incAchiev" Area="DataArea" AreaIndex="2" Caption="INC ACHIEV" UnboundExpression="Iif([PLAN_VALUE] &lt;&gt; 0, [INC_VALUE] / [PLAN_VALUE], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([PLAN_VALUE] &lt;&gt; 0, [INC_VALUE] / [PLAN_VALUE], 0)" UnboundType="Decimal">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="incValue" Area="DataArea" AreaIndex="3" Caption="4I.INC VALUE" FieldName="INC_VALUE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="achievement" Area="DataArea" AreaIndex="4" Caption="ACHIEVEMENT" UnboundExpression="Iif([PLAN_VALUE] &lt;&gt; 0, [SALES_VALUE] / [PLAN_VALUE], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([PLAN_VALUE] &lt;&gt; 0, [SALES_VALUE] / [PLAN_VALUE], 0)" UnboundType="Decimal">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldSBOTAN" Area="RowArea" AreaIndex="2" Caption="2J.SBO TAN" FieldName="SBO_TAN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldBOTAN" Area="RowArea" AreaIndex="1" Caption="2Q.BO TAN" FieldName="BO_TAN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldREGION" Area="RowArea" AreaIndex="0" Caption="2R.REGION" FieldName="REGION">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldYEAR" Area="DataArea" Caption="1B.YEAR" FieldName="YEAR" Visible="False">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldMonthName" Area="DataArea" Caption="1C.Month Name" FieldName="Month_Name" Visible="False">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldfulldate" Area="DataArea" AreaIndex="4" Caption="1D.Full Date" FieldName="full_date" Visible="False">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldexpireddate" Area="DataArea" AreaIndex="4" Caption="1E.EXPIRED DATE" FieldName="expired_date" Visible="False">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldBranchSalesDesc" Area="DataArea" AreaIndex="4" FieldName="Branch_Sales_Desc" Caption="2A.Branch Sales Desc" Visible="False">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldChannelOutletGroup" Area="DataArea" AreaIndex="4" FieldName="Channel_Outlet_Group" Visible="False" Caption="2B.Channel Outlet Group">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldChannelOutlet" Area="DataArea" AreaIndex="4" FieldName="Channel_Outlet" Visible="False" Caption="2C.Channel Outlet">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCustomerId" Area="DataArea" AreaIndex="4" FieldName="Customer_Id" Visible="False" Caption="2D.Customer Id">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCustomerName" Area="DataArea" AreaIndex="4" FieldName="Customer_Name" Visible="False" Caption="2E.Customer Name">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCity" Area="DataArea" AreaIndex="4" FieldName="City" Visible="False" Caption="2F.City">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldTerritoryGeoName" Area="DataArea" AreaIndex="4" FieldName="Territory_Geo_Name" Visible="False" Caption="2G.Teritory Geo Name">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldSalesCategoryName" Area="DataArea" AreaIndex="4" FieldName="Sales_Category_Name" Visible="False" Caption="2H.Sales Category Name">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCATEGORYOUTLET" Area="DataArea" AreaIndex="4" FieldName="CATEGORY_OUTLET" Visible="False" Caption="2I.CATEGORY OUTLET">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldNIKMR" Area="DataArea" AreaIndex="4" FieldName="NIK_MR" Visible="False" Caption="2K.NIK MR">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldMR" Area="DataArea" AreaIndex="4" FieldName="MR" Visible="False" Caption="2L.Mr">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldNIKAM" Area="DataArea" AreaIndex="4" FieldName="NIK_AM" Visible="False" Caption="2M.NIK AM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldAM" Area="DataArea" AreaIndex="4" FieldName="AM" Visible="False" Caption="2N. AM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldNIKRM" Area="DataArea" AreaIndex="4" FieldName="NIK_RM" Visible="False" Caption="2O.NIK RM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldRM" Area="DataArea" AreaIndex="4" FieldName="RM" Visible="False" Caption="2P.RM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldKAE" Area="DataArea" AreaIndex="4" FieldName="KAE" Visible="False" Caption="2S.KAE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldKAM" Area="DataArea" AreaIndex="4" FieldName="KAM" Visible="False" Caption="2T.KAM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldKAMCATEGORY" Area="DataArea" AreaIndex="4" FieldName="KAM_CATEGORY" Visible="False" Caption="2U.KAM CATEGORY">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCLASSOUTLET" Area="DataArea" AreaIndex="4" FieldName="CLASS_OUTLET" Visible="False" Caption="2V.CLASS OUTLET">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldAREAOSE" Area="DataArea" AreaIndex="4" FieldName="AREA_OSE" Visible="False" Caption="2W.AREA OSE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPPM" Area="DataArea" AreaIndex="4" FieldName="PPM" Visible="False" Caption="2X.PPM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPM" Area="DataArea" AreaIndex="4" FieldName="PM" Visible="False" Caption="2Y.PM">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldProductCode" Area="DataArea" AreaIndex="4" FieldName="Product_Code" Visible="False" Caption="3A.Product Code">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldProductDesc" Area="DataArea" AreaIndex="4" FieldName="Product_Desc" Visible="False" Caption="3B.Product Desc">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldInvoiceNo" Area="DataArea" AreaIndex="4" FieldName="Invoice_No" Visible="False" Caption="3C.Invoice No">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldCATEGORYPRODUCTTAN" Area="DataArea" AreaIndex="4" FieldName="CATEGORY_PRODUCT_TAN" Visible="False" Caption="3D.CATEGORY PRODUCT TAN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldGP" Area="DataArea" AreaIndex="4" FieldName="GP" Visible="False" Caption="3E.GP">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPRD" Area="DataArea" AreaIndex="4" FieldName="PRD" Visible="False" Caption="3F.PRD">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldOSE" Area="DataArea" AreaIndex="4" FieldName="OSE" Visible="False" Caption="3G.OSE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldGROUPBYFIN" Area="DataArea" AreaIndex="4" FieldName="GROUP_BY_FIN" Visible="False" Caption="3H.GROUP BY FIN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldTHEURAPTICS" Area="DataArea" AreaIndex="4" FieldName="THEURAPTICS" Visible="False" Caption="3I.THEURAPTICS">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldHna" Area="DataArea" AreaIndex="4" FieldName="Hna" Visible="False" Caption="4A.Hna">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldSalesQuantity" Area="DataArea" AreaIndex="4" FieldName="Sales_Quantity" Visible="False" Caption="4B.Sales Quantity">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldSalesGross" Area="DataArea" AreaIndex="4" FieldName="Sales_Gross" Visible="False" Caption="4C.Sales Gross">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldDiscountRp" Area="DataArea" AreaIndex="4" FieldName="Discount_Rp" Visible="False" Caption="4D.Discount Rp">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldpersenDisc" Area="DataArea" AreaIndex="4" FieldName="persen_Disc" Visible="False" Caption="4E.% Disc">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldSALESGROSSTAN" Area="DataArea" AreaIndex="4" FieldName="SALES_GROSS_TAN" Visible="False" Caption="4F.SALES GROSS TAN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldDISCOUNTRPTAN" Area="DataArea" AreaIndex="4" FieldName="DISCOUNT_RP_TAN" Visible="False" Caption="4G.DISCOUNT RP TAN">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPLANQTY" Area="DataArea" AreaIndex="4" FieldName="PLAN_QTY" Visible="False" Caption="4J.PLAN QTY">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPATIENTMONTH" Area="DataArea" AreaIndex="4" FieldName="PATIENT_MONTH" Visible="False" Caption="4L.PATIENT MONTH">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPATIENTQUARTER" Area="DataArea" AreaIndex="4" FieldName="PATIENT_QUARTER" Visible="False" Caption="4M.PATIENT QUARTER">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldPATIENTYEAR" Area="DataArea" AreaIndex="4" FieldName="PATIENT_YEAR" Visible="False" Caption="4N.PATIENT YEAR">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldUNITPRICE" Area="DataArea" AreaIndex="4" FieldName="UNIT_PRICE" Visible="False" Caption="4O.UNIT PRICE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldTABLET" Area="DataArea" AreaIndex="4" FieldName="TABLET" Visible="False" Caption="4P.TABLET">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldDOSSAGE" Area="DataArea" AreaIndex="4" FieldName="DOSSAGE" Visible="False" Caption="4Q.DOSSAGE">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldFOCUS" Area="DataArea" AreaIndex="4" FieldName="FOCUS" Visible="False" Caption="4Q.FOCUS">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="fieldlotnumber" Area="DataArea" AreaIndex="4" FieldName="lot_number" Visible="False" Caption="4R.LOT NUMBER">
                    </dx:PivotGridField>
                    <dx:PivotGridField ID="field2" Caption="AVG DISCOUNT" UnboundExpression="[DISCOUNT_RP_TAN] / [SALES_GROSS_TAN] * 100" UnboundExpressionMode="UseSummaryValues" Visible="False">
                    </dx:PivotGridField>
                </Fields>
            </dx:ASPxPivotGrid>
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False">
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Month_Name" ReadOnly="True" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="YEAR" ReadOnly="True" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="full_date" ReadOnly="True" VisibleIndex="2">
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="Branch_Sales_Desc" ReadOnly="True" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Channel_Outlet_Group" ReadOnly="True" VisibleIndex="4">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Channel_Outlet" ReadOnly="True" VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Customer_Id" ReadOnly="True" VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Customer_Name" ReadOnly="True" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="City" ReadOnly="True" VisibleIndex="8">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Territory_Geo_Name" ReadOnly="True" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Sales_Category_Name" ReadOnly="True" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Product_Code" ReadOnly="True" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Product_Desc" ReadOnly="True" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Invoice_No" ReadOnly="True" VisibleIndex="13">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Hna" ReadOnly="True" VisibleIndex="14">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Sales_Quantity" ReadOnly="True" VisibleIndex="15">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Sales_Gross" ReadOnly="True" VisibleIndex="16">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Discount_Rp" ReadOnly="True" VisibleIndex="17">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="persen_Disc" ReadOnly="True" VisibleIndex="18">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DATA" ReadOnly="True" VisibleIndex="19">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SALES_GROSS_TAN" ReadOnly="True" VisibleIndex="20">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DISCOUNT_RP_TAN" ReadOnly="True" VisibleIndex="21">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CATEGORY_PRODUCT_TAN" ReadOnly="True" VisibleIndex="22">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SBO_TAN" ReadOnly="True" VisibleIndex="23">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SALES_VALUE" ReadOnly="True" VisibleIndex="24">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="INC_VALUE" ReadOnly="True" VisibleIndex="25">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PLAN_QTY" ReadOnly="True" VisibleIndex="26">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PLAN_VALUE" ReadOnly="True" VisibleIndex="27">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PATIENT_MONTH" ReadOnly="True" VisibleIndex="28">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PATIENT_QUARTER" ReadOnly="True" VisibleIndex="29">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PATIENT_YEAR" ReadOnly="True" VisibleIndex="30">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CATEGORY_OUTLET" ReadOnly="True" VisibleIndex="31">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="GP" ReadOnly="True" VisibleIndex="32">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PRD" ReadOnly="True" VisibleIndex="33">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NIK_MR" ReadOnly="True" VisibleIndex="34">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MR" ReadOnly="True" VisibleIndex="35">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NIK_AM" ReadOnly="True" VisibleIndex="36">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AM" ReadOnly="True" VisibleIndex="37">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NIK_RM" ReadOnly="True" VisibleIndex="38">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NIK_PPM" ReadOnly="True" VisibleIndex="39">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PPM" ReadOnly="True" VisibleIndex="40">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="RM" ReadOnly="True" VisibleIndex="41">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BO_TAN" ReadOnly="True" VisibleIndex="42">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="REGION" ReadOnly="True" VisibleIndex="43">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="UNIT_PRICE" ReadOnly="True" VisibleIndex="44">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OSE" ReadOnly="True" VisibleIndex="45">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="KAE" ReadOnly="True" VisibleIndex="46">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="KAM" ReadOnly="True" VisibleIndex="47">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="KAM_CATEGORY" ReadOnly="True" VisibleIndex="48">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CLASS_OUTLET" ReadOnly="True" VisibleIndex="49">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="AREA_OSE" ReadOnly="True" VisibleIndex="50">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="GROUP_BY_FIN" ReadOnly="True" VisibleIndex="51">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="TABLET" ReadOnly="True" VisibleIndex="52">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DOSSAGE" ReadOnly="True" VisibleIndex="53">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FOCUS" ReadOnly="True" VisibleIndex="54">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="THEURAPTICS" ReadOnly="True" VisibleIndex="55">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PM" ReadOnly="True" VisibleIndex="56">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="t_aso_id" ReadOnly="True" VisibleIndex="57">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="lot_number" ReadOnly="True" VisibleIndex="58">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="expired_date" ReadOnly="True" VisibleIndex="59">
                    </dx:GridViewDataDateColumn>
                </Columns>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_PIVOT_ASO_REPORT_MOBILE" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <%--<asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                    <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>--%>
                    <asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value"  DefaultValue="" DbType="DateTime"/>
                    <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value"  DefaultValue="" DbType="DateTime"/>
                    <asp:ControlParameter ControlID="txtPosition" Name="position" PropertyName="Text" Type="String" DefaultValue=""/>
                    <asp:ControlParameter ControlID="txtNik" Name="nik" PropertyName="Text" Type="String" DefaultValue=""/>
                </SelectParameters>
            </asp:SqlDataSource>
            <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
            </dx:ASPxPivotGridExporter>
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server">
            </dx:ASPxGridViewExporter>
        </td>
        <td style="width:20%">
            <dx:ASPxPivotCustomizationControl ID="ASPxPivotCustomizationControl1" runat="server" AllowFilter="True" AllowSort="True" ASPxPivotGridID="ASPxPivotGrid1" Height="480px" Layout="StackedSideBySide" Theme="PlasticBlue" Width="300px">
            </dx:ASPxPivotCustomizationControl>
        </td>
    </tr>
</table>
</form>


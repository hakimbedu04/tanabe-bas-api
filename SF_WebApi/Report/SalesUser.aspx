<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesUser.aspx.cs" Inherits="SF_WebApi.Report.SalesUser" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: USER PIVOT ::</td></tr>
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
                    <dx:ASPxButton ID="btRetrieve" runat="server" Text="Retrieve" ClientInstanceName="btnRetrieve" CssClass="button" ToolTip="Retrieve" Theme="PlasticBlue" AutoPostBack="False">
                        <Paddings PaddingLeft="1px" />
                        <ClientSideEvents Click="do_retrieve"></ClientSideEvents>
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
                            <dx:PivotGridField ID="fieldrepregion" Area="RowArea" AreaIndex="0" Caption="REGION" FieldName="rep_region">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsptargetvalue" Area="DataArea" AreaIndex="0" Caption="TARGET VALUE" FieldName="sp_target_value" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldspsalesvalue" Area="DataArea" AreaIndex="1" Caption="SALES VALUE" FieldName="sp_sales_value" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsalesplan" Area="DataArea" AreaIndex="2" Caption="USER PLAN" FieldName="sales_plan">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsalesrealization" Area="DataArea" AreaIndex="3" Caption="USER REAL" FieldName="sales_realization">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldtargetvalue" Area="DataArea" AreaIndex="0" Caption="TARGET VALUE" UnboundExpression="Round([fieldsptargetvalue])" UnboundExpressionMode="UseSummaryValues" UnboundType="Integer" UnboundFieldName="fieldtargetvalue">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="Percent" AreaIndex="4" Caption="USER ACHV" Area="DataArea" UnboundExpression="Iif([fieldsalesplan] &lt;&gt; 0, [fieldsalesrealization] / [fieldsalesplan], 0)" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="Iif([fieldsalesplan] &lt;&gt; 0, [fieldsalesrealization] / [fieldsalesplan], 0)" UnboundType="Decimal">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldprdname" AreaIndex="1" Caption="PRODUCT" FieldName="prd_name" Area="RowArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldnamaam" AreaIndex="3" Caption="AM" FieldName="nama_am" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrareamis" AreaIndex="3" Caption="AREA MISC" FieldName="dr_area_mis" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepbo" AreaIndex="3" Caption="BO" FieldName="rep_bo" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcategory" AreaIndex="3" Caption="CATEGORY" FieldName="dr_category" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrchanel" AreaIndex="3" Caption="CHANEL" FieldName="dr_chanel" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepdivision" AreaIndex="3" Caption="DIVISION" FieldName="rep_division" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrdklk" AreaIndex="3" Caption="DK/LK" FieldName="dr_dk_lk" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldprdprice" AreaIndex="3" Caption="PRICE" FieldName="prd_price" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsalesdateplan" AreaIndex="3" Caption="MONTH" FieldName="sales_date_plan" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsalesyearplan" AreaIndex="3" Caption="YEAR" FieldName="sales_year_plan" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcode" AreaIndex="3" Caption="DR CODE" FieldName="dr_code" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldprdcode" AreaIndex="3" Caption="PROD CODE" FieldName="prd_code" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrspec" AreaIndex="3" Caption="SPEC" FieldName="dr_spec" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsubspec" AreaIndex="3" Caption="SUB SPEC" FieldName="dr_sub_spec" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrquadrant" AreaIndex="3" Caption="QUADRANT" FieldName="dr_quadrant" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrmonitoring" AreaIndex="3" Caption="MONITORING" FieldName="dr_monitoring" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldprdcategory" AreaIndex="3" Caption="PRO CTGY" FieldName="prd_category" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepname" AreaIndex="3" Caption="TI NAME" FieldName="rep_name" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsalesvalue" Area="DataArea" AreaIndex="1" Caption="SALES VALUE" UnboundExpression="Round([fieldspsalesvalue])" UnboundExpressionMode="UseSummaryValues" UnboundFieldName="fieldsalesvalue" UnboundType="Integer">
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_SALES_PIVOT_MOBILE" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="startDate" Name="dateStart" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="endDate" Name="dateEnd" PropertyName="Value"  DefaultValue="2019-01-01" DbType="DateTime"/>
                            <asp:ControlParameter ControlID="txtPosition" DefaultValue="" Name="position" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtNik" Name="rep_id" PropertyName="Text" Type="String" DefaultValue="0"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="False">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="sp_id" VisibleIndex="0" ReadOnly="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_id" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="prd_code" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="prd_name" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="prd_price" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="prd_price_bpjs" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="prd_category" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_target_qty" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_target_value" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_sales_qty" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_sales_value" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_note" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_id" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_date_plan" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_year_plan" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_plan" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_realization" VisibleIndex="16" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_info" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_code" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_plan_verification_status" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_plan_verification_by" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="sales_plan_verification_date" VisibleIndex="21">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_real_verification_status" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sales_real_verification_by" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="sales_real_verification_date" VisibleIndex="24">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="sales_date_plan_saved" VisibleIndex="25">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="sales_date_plan_updated" VisibleIndex="26">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="sales_date_realization_saved" VisibleIndex="27">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_name" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_quadrant" VisibleIndex="29">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_monitoring" VisibleIndex="30">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_spec" VisibleIndex="31">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sub_spec" VisibleIndex="32">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_area_mis" VisibleIndex="33">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_chanel" VisibleIndex="34">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_category" VisibleIndex="35">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_sub_category" VisibleIndex="36">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="dr_dk_lk" VisibleIndex="37">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_plan" VisibleIndex="38">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="sp_real" VisibleIndex="39">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_name" VisibleIndex="40">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_am" VisibleIndex="41">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_region" VisibleIndex="42">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_bo" VisibleIndex="43">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_sbo" ReadOnly="True" VisibleIndex="44">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_position" VisibleIndex="45">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_division" VisibleIndex="46">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_rm" VisibleIndex="47">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rep_ppm" VisibleIndex="48">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="nama_ppm" VisibleIndex="49">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="target_class_user" ReadOnly="True" VisibleIndex="50">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="real_class_user" ReadOnly="True" VisibleIndex="51">
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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoctorPivot.aspx.cs" Inherits="SF_WebApi.Report.DoctorPivot" %>

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
            <tr><td class="title-form" style="text-align:center; font-weight:bold;" colspan="11">:: DOCTOR PIVOT ::</td></tr>
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
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientIDMode="AutoID" DataSourceID="SqlDataSource1" Theme="PlasticBlue">
                        <Fields>
                            <dx:PivotGridField ID="fielddrsum" Area="DataArea" AreaIndex="0" FieldName="dr_sum" Caption="SUM">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrspec" Area="RowArea" AreaIndex="0" FieldName="dr_spec" Caption="SPEC">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrregion" Area="ColumnArea" AreaIndex="0" FieldName="dr_region" Caption="REGION">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddraddress" Area="DataArea" AreaIndex="0" Caption="ADDRESS" FieldName="dr_address" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcode" AreaIndex="0" FieldName="dr_code" Caption="DR CODE" Visible="False" Area="DataArea">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrbo" Area="DataArea" AreaIndex="0" Caption="BO" FieldName="dr_bo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldamname" Area="DataArea" AreaIndex="0" Caption="AM NAME" FieldName="am_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrname" Area="DataArea" AreaIndex="0" Caption="DR NAME" FieldName="dr_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrmonitoring" Area="DataArea" AreaIndex="0" Caption="MONITORING" FieldName="dr_monitoring" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrareamis" Area="DataArea" AreaIndex="0" Caption="AREA MIS" FieldName="dr_area_mis" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrcategory" Area="DataArea" AreaIndex="0" Caption="CATEGORY" FieldName="dr_category" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrchanel" Area="DataArea" AreaIndex="0" Caption="CHANNEL" FieldName="dr_chanel" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrdayvisit" Area="DataArea" AreaIndex="0" Caption="DAY VISIT" FieldName="dr_day_visit" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrkolnot" Area="DataArea" AreaIndex="0" Caption="KOL / NOT" FieldName="dr_kol_not" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrgender" Area="DataArea" AreaIndex="0" Caption="GENDER" FieldName="dr_gender" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddremail" Area="DataArea" AreaIndex="0" Caption="EMAIL" FieldName="dr_email" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrbirthday" Area="DataArea" AreaIndex="0" Caption="BIRTHDAY" FieldName="dr_birthday" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrdklk" Area="DataArea" AreaIndex="0" Caption="DK/LK" FieldName="dr_dk_lk" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldsboid" Area="DataArea" AreaIndex="0" FieldName="sbo_id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsbo" Area="DataArea" AreaIndex="0" Caption="SBO" FieldName="dr_sbo" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrrep" Area="DataArea" AreaIndex="0" FieldName="dr_rep" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepname" AreaIndex="0" FieldName="rep_name" Visible="False" Area="DataArea" Caption="TI NAME">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddram" Area="DataArea" AreaIndex="0" FieldName="dr_am" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldrepposition" Area="DataArea" AreaIndex="0" FieldName="rep_position" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsubspec" Area="DataArea" AreaIndex="0" FieldName="dr_sub_spec" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrquadrant" Area="DataArea" AreaIndex="0" FieldName="dr_quadrant" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsubcategory" Area="DataArea" AreaIndex="0" FieldName="dr_sub_category" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrvisitinghour" Area="DataArea" AreaIndex="0" Caption="VISITING HOUR" FieldName="dr_visiting_hour" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrnumberpatient" Area="DataArea" AreaIndex="0" Caption="PATIENT NUMBER" FieldName="dr_number_patient" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrphone" Area="DataArea" AreaIndex="0" FieldName="dr_phone" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrusedsession" Area="DataArea" AreaIndex="0" FieldName="dr_used_session" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldisused" Area="DataArea" AreaIndex="0" FieldName="is_used" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrusedremaining" Area="DataArea" AreaIndex="0" FieldName="dr_used_remaining" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrusedmonthsession" Area="DataArea" AreaIndex="0" FieldName="dr_used_month_session" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrstatus" Area="DataArea" AreaIndex="0" FieldName="dr_status" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldisusedonsales" Area="DataArea" AreaIndex="0" FieldName="is_used_on_sales" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsalessession" Area="DataArea" AreaIndex="0" FieldName="dr_sales_session" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrsalesmonthsession" Area="DataArea" AreaIndex="0" FieldName="dr_sales_month_session" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrppm" Area="DataArea" AreaIndex="0" FieldName="dr_ppm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldppmname" Area="DataArea" AreaIndex="0" FieldName="ppm_name" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldcustid" Area="DataArea" AreaIndex="0" FieldName="cust_id" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrrm" Area="DataArea" AreaIndex="0" FieldName="dr_rm" Visible="False">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fielddrrmname" Area="DataArea" AreaIndex="0" FieldName="dr_rm_name" Visible="False">
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
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter2" runat="server"></dx:ASPxPivotGridExporter>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:basConnectionString %>" SelectCommand="SP_SELECT_MASTER_DOCTOR_PIVOT" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="department" QueryStringField="d" Type="String" />
                            <asp:QueryStringParameter Name="rep_position" QueryStringField="p" Type="String" />
                            <asp:QueryStringParameter Name="rep_id" QueryStringField="i" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server">
                    </dx:ASPxPivotGridExporter>
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Visible="false">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="DR CODE" FieldName="dr_code" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DR NAME" FieldName="dr_name" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SBO" FieldName="dr_sbo" ReadOnly="True" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="BO" FieldName="dr_bo" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REGION" FieldName="dr_region" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP ID" FieldName="dr_rep" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TI NAME" FieldName="rep_name" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP AM" FieldName="dr_am" VisibleIndex="7">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AM NAME" FieldName="am_name" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="REP POSITION" FieldName="rep_position" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SPEC" FieldName="dr_spec" VisibleIndex="10">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB SPEC" FieldName="dr_sub_spec" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="QUADRANT" FieldName="dr_quadrant" VisibleIndex="12">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MONITORING" FieldName="dr_monitoring" ReadOnly="True" VisibleIndex="13">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="ADDRESS" FieldName="dr_address" ReadOnly="True" VisibleIndex="14">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="AREA MIS" FieldName="dr_area_mis" ReadOnly="True" VisibleIndex="15">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUM" FieldName="dr_sum" VisibleIndex="16">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CATEGORY" FieldName="dr_category" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUB CATEGORY" FieldName="dr_sub_category" VisibleIndex="18">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CHANNEL" FieldName="dr_chanel" VisibleIndex="19">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="DAY VISIT" FieldName="dr_day_visit" VisibleIndex="20">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="VISITING HOUR" FieldName="dr_visiting_hour" VisibleIndex="21">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="NUMBER PATIENT" FieldName="dr_number_patient" VisibleIndex="22">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="KOL/NOT" FieldName="dr_kol_not" VisibleIndex="23">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="GENDER" FieldName="dr_gender" ReadOnly="True" VisibleIndex="24">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PHONE" FieldName="dr_phone" VisibleIndex="25">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="EMAIL" FieldName="dr_email" VisibleIndex="26">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn Caption="BIRTHDAY" FieldName="dr_birthday" VisibleIndex="27">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn Caption="DK/LK" FieldName="dr_dk_lk" ReadOnly="True" VisibleIndex="28">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="STATUS" FieldName="is_used" ReadOnly="True" VisibleIndex="29">
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

    </script>
</html>

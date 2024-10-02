using DevExpress.Utils;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Customization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SF_WebApi.PivotGridLayout
{
    public class CoveragePivotGridSettings
    {
        private static PivotGridSettings compactCoveragePivotGridSettings;

        public static PivotGridSettings GetSetting
        {
            get
            {
                if (compactCoveragePivotGridSettings == null)
                    compactCoveragePivotGridSettings = CreateLayoutCoveragePivotGridSettings();
                return compactCoveragePivotGridSettings;
            }
        }

        private static PivotGridSettings CreateLayoutCoveragePivotGridSettings()
        {
            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = "CoveragePivot";
            settings.Width = Unit.Percentage(100);
            settings.CallbackRouteValues = new
            {
                Controller = "CoveragePivot",
                Action = "CoveragePivotPartialView"
            };
            settings.CustomActionRouteValues = new
            {
                Controller = "CoveragePivot",
                Action = "CoveragePivotCustomCallback"
            };
            settings.OptionsView.HorizontalScrollBarMode = global::DevExpress.Web.ScrollBarMode.Auto;
            settings.OptionsView.RowTotalsLocation = PivotRowTotalsLocation.Far;
            settings.OptionsView.ShowGrandTotalsForSingleValues = true;
            settings.OptionsView.ShowRowGrandTotals = true;
            settings.OptionsView.ShowRowTotals = false;
            settings.OptionsView.ShowTotalsForSingleValues = false;
            settings.OptionsView.ShowFilterHeaders = true;

            settings.Styles.FieldValueStyle.Wrap = DefaultBoolean.False;
            settings.Styles.HeaderStyle.Font.Size = 7;
            settings.Styles.CellStyle.Font.Size = 7;
            settings.Styles.ColumnAreaStyle.Font.Size = 7;
            settings.Styles.RowAreaStyle.Font.Size = 7;
            settings.Styles.FieldValueGrandTotalStyle.Font.Size = 7;
            settings.Styles.GrandTotalCellStyle.Font.Size = 7;



            settings.PivotCustomizationExtensionSettings.Name = "pivotCustomization";
            settings.PivotCustomizationExtensionSettings.AllowedLayouts = CustomizationFormAllowedLayouts.BottomPanelOnly1by4 | CustomizationFormAllowedLayouts.BottomPanelOnly2by2 | CustomizationFormAllowedLayouts.StackedDefault | CustomizationFormAllowedLayouts.StackedSideBySide;
            settings.PivotCustomizationExtensionSettings.Layout = CustomizationFormLayout.StackedSideBySide;
            settings.PivotCustomizationExtensionSettings.AllowSort = true;
            settings.PivotCustomizationExtensionSettings.AllowFilter = true;
            settings.PivotCustomizationExtensionSettings.Height = Unit.Pixel(480);
            settings.PivotCustomizationExtensionSettings.Width = Unit.Pixel(250);

            settings.BeforePerformDataSelect = (sender, e) =>
            {
                MVCxPivotGrid PivotGrid = (MVCxPivotGrid)sender;
                string layout = (string)System.Web.HttpContext.Current.Session["Layout"];
                // Debug.WriteLine("Is layout empty? " & String.IsNullOrEmpty(layout))
                if (!string.IsNullOrEmpty(layout))
                    PivotGrid.LoadLayoutFromString(layout, DevExpress.Web.ASPxPivotGrid.PivotGridWebOptionsLayout.DefaultLayout);
            };

            settings.Fields.Add(field =>
            {
                field.ID = "1";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_code";
                field.Caption = "DR CODE";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "2";
                field.Area = PivotArea.RowArea;
                field.FieldName = "rep_region";
                field.Caption = "REGION";
                field.AreaIndex = 0;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "3";
                field.Area = PivotArea.RowArea;
                field.FieldName = "nama_am";
                field.Caption = "AM";
                field.AreaIndex = 1;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "4";
                field.Area = PivotArea.RowArea;
                field.FieldName = "rep_name";
                field.Caption = "TI NAME";
                field.AreaIndex = 2;
            });

            settings.Fields.Add(field =>
            {
                field.ID = "433";
                field.Area = PivotArea.DataArea;
                field.FieldName = "dr_plan";
                field.Caption = "PLAN";
                field.AreaIndex = 0;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "434";
                field.Area = PivotArea.DataArea;
                field.FieldName = "dr_real";
                field.Caption = "REAL";
                field.AreaIndex = 1;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "Percent";
                field.Area = PivotArea.DataArea;
                field.FieldName = "";
                field.Caption = "SUM COVERAGE IN %";
                field.AreaIndex = 2;
                field.CellFormat.FormatString = "p";
            });
            settings.Fields.Add(field =>
            {
                field.ID = "Remaining";
                field.Area = PivotArea.DataArea;
                field.FieldName = "";
                field.Caption = "REM";
                field.AreaIndex = 3;
            });

            settings.Fields.Add(field =>
            {
                field.ID = "5";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "nama_rm";
                field.Caption = "RM";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "6";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "rep_bo";
                field.Caption = "BO";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "7";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "rep_sbo";
                field.Caption = "SBO";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "8";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "rep_position";
                field.Caption = "POSITION";
                field.Visible = false;
            });

            settings.Fields.Add(field =>
            {
                field.ID = "9";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "rep_division";
                field.Caption = "DIVISION";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "10";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_name";
                field.Caption = "DR NAME";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "11";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_spec";
                field.Caption = "SPEC";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "12";
                field.Area = PivotArea.RowArea;
                field.FieldName = "dr_sub_spec";
                field.Caption = "SUB SPEC";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "13";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_quadrant";
                field.Caption = "QUADRANT";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "14";
                field.Area = PivotArea.DataArea;
                field.FieldName = "dr_monitoring";
                field.Caption = "MONITORING";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "15";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_address";
                field.Caption = "ADDRESS";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "16";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_area_mis";
                field.Caption = "AREA MIS";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "17";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_category";
                field.Caption = "CATEGORY";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "18";
                field.Area = PivotArea.DataArea;
                field.FieldName = "dr_sub_category";
                field.Caption = "SUB CTGY";
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.ID = "19";
                field.Area = PivotArea.FilterArea;
                field.FieldName = "dr_chanel";
                field.Caption = "CHANNEL";
                field.Visible = false;
            });

            settings.CustomCellDisplayText = (sender, e) =>
            {
                MVCxPivotGrid pivotGrid = (MVCxPivotGrid)sender;
                DevExpress.Web.ASPxPivotGrid.PivotGridField planField = pivotGrid.Fields["dr_plan"];
                object plan = e.GetCellValue(planField);
                DevExpress.Web.ASPxPivotGrid.PivotGridField realField = pivotGrid.Fields["dr_real"];
                object real = e.GetCellValue(realField);
                decimal grandTotalPlan = System.Convert.ToDecimal(e.GetRowGrandTotal(planField));
                decimal grandTotalReal = System.Convert.ToDecimal(e.GetRowGrandTotal(realField));

                if (object.ReferenceEquals(e.DataField, pivotGrid.Fields["Percent"]))
                {
                    if (plan == null || plan.Equals(0))
                        return;
                    if (real == null || real.Equals(0))
                        return;
                    if (grandTotalPlan == 0)
                        return;
                    if (grandTotalReal == 0)
                        return;
                    decimal perc = Decimal.Parse(real.ToString()) / Decimal.Parse(plan.ToString());
                    e.DisplayText = string.Format("{0:p}", perc);
                }
                if (object.ReferenceEquals(e.DataField, pivotGrid.Fields["Remaining"]))
                {
                    int remaining = System.Convert.ToInt32(real) - System.Convert.ToInt32(plan);
                    e.DisplayText = remaining.ToString();
                }
            };

            settings.PreRender = (sender, e) =>
            {
                MVCxPivotGrid PivotGrid = (MVCxPivotGrid)sender;
                if (System.Web.HttpContext.Current.Session["Layout"] != null)
                    PivotGrid.LoadLayoutFromString((string)System.Web.HttpContext.Current.Session["Layout"], PivotGridWebOptionsLayout.DefaultLayout);
                PivotGrid.ExpandAll();
            };

            settings.GridLayout = (sender, e) =>
            {
                MVCxPivotGrid PivotGrid = (MVCxPivotGrid)sender;
                System.Web.HttpContext.Current.Session["Layout"] = PivotGrid.SaveLayoutToString(PivotGridWebOptionsLayout.DefaultLayout);
            };

            return settings;
        }
    }
}
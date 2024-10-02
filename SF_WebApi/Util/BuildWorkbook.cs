using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace SF_WebApi.Util
{
    public class BuildWorkbook
    {
        public static void BuildWorkbooks(DataTable datas, string filename)
        {
            try
            {
                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart wbp = xl.AddWorkbookPart();
                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                    Workbook wb = new Workbook();
                    FileVersion fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";
                    Worksheet ws = new Worksheet();
                    SheetData sd = new SheetData();

                    WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                    wbsp.Stylesheet = CreateStylesheet();
                    wbsp.Stylesheet.Save();

                    Columns columns = new Columns();
                    #region width
                    DataRow contentRowTemp = default(DataRow);
                    
                    var tempText = "";
                    double fSimpleWidth = 0.0f;
                    double fWidthOfZero = 0.0f;
                    double fDigitWidth = 0.0f;
                    double fMaxDigitWidth = 0.0f;
                    double fTruncWidth = 0.0f;

                    
                    // I just need a Graphics object. Any reasonable bitmap size will do.
                    Graphics g = Graphics.FromImage(new Bitmap(200, 200));
                    
                    //for (int j = 0; j <= datas.Columns.Count - 1; j++)
                    //{
                        //contentRowTemp = datas.Rows[j];
                        //for (int k = 0; k <= contentRowTemp.ItemArray.Count() - 1; k++)
                        //{
                        //    tempText = contentRowTemp.ItemArray[k].ToString();
                            //tempText = datas.Columns[j].ToString();
                            for (int i = 0; i < 10; ++i)
                            {
                                System.Drawing.Font drawfont = new System.Drawing.Font("Calibri", 11);
                                fDigitWidth = g.MeasureString(i.ToString(), drawfont).Width;
                                if (fDigitWidth > fMaxDigitWidth)
                                {
                                    fMaxDigitWidth = fDigitWidth;
                                }
                            }
                            g.Dispose();
                            // Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}] / {Maximum Digit Width} * 256) / 256
                            fTruncWidth = Math.Truncate((tempText.ToCharArray().Count()*fMaxDigitWidth + 5.0)/fMaxDigitWidth*256.0)/ 256.0;
                            columns.Append(CreateColumnData(1, (uint)datas.Columns.Count, 27.86));
                        //}
                    //}
                    ws.Append(columns);
                    #endregion
                    
                    

                    #region header
                    Row header = new Row();
                    header.RowIndex = (int)1;
                    Row r;
                    Cell c;
                    r = new Row();
                    foreach (DataColumn column in datas.Columns)
                    {
                        Cell headerCell = CreateTextCell(datas.Columns.IndexOf(column) + 1, 1, column.ColumnName);
                        header.AppendChild(headerCell);
                        c = CreateTextCell(datas.Columns.IndexOf(column) + 1, 1, column.ColumnName);
                        r.Append(c);
                    }
                    sd.Append(r);
                    #endregion

                    #region content
                    //data.AppendChild(header);
                    DataRow contentRow = default(DataRow);
                    for (int i = 0; i <= datas.Rows.Count - 1; i++)
                    {
                        contentRow = datas.Rows[i];
                        sd.AppendChild(CreateContentRow(contentRow, i + 2));
                    }
                    #endregion
                    ws.Append(sd);
                    wsp.Worksheet = ws;
                    wsp.Worksheet.Save();
                    Sheets sheets = new Sheets();
                    Sheet sheet = new Sheet();
                    sheet.Name = "Sheet1";
                    sheet.SheetId = 1;
                    sheet.Id = wbp.GetIdOfPart(wsp);
                    sheets.Append(sheet);
                    wb.Append(fv);
                    wb.Append(sheets);

                    xl.WorkbookPart.Workbook = wb;
                    xl.WorkbookPart.Workbook.Save();
                    xl.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        private static string GetColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            int modifier = 0;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = Convert.ToInt32((dividend - modifier) / 26);
            }

            return columnName;
        }

        private static Cell CreateTextCell(int columnIndex, int rowIndex, object cellValue)
        {
            Cell cell = new Cell();
            cell.CellReference = GetColumnName(columnIndex) + rowIndex;

            if (cellValue is int || cellValue is Decimal)
            {
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(cellValue.ToString());
            }
            else
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(cellValue.ToString());
            }
            return cell;
        }

        private static Row CreateContentRow(DataRow dataRow, int rowIndex)
        {
            Row row = new Row { RowIndex = (UInt32)rowIndex };

            for (int i = 0; i <= dataRow.Table.Columns.Count - 1; i++)
            {
                Cell dataCell = CreateTextCell(i + 1, rowIndex, dataRow[i]);
                row.AppendChild(dataCell);
            }
            return row;
        }

        private static Column CreateColumnData(UInt32 startColumnIndex, UInt32 endColumnIndex, double columnWidth)
        {
            Column column;
            column = new Column();
            column.Min = startColumnIndex;
            column.Max = endColumnIndex;
            column.Width = columnWidth;
            column.CustomWidth = true;
            return column;
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = "Calibri";
            FontSize ftsz = new FontSize();
            ftsz.Val = 11;
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);
            fts.Count = (uint)fts.ChildElements.Count;

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fills.Count = (uint)fills.ChildElements.Count;

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = (uint)borders.ChildElements.Count;

            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = (uint)csfs.ChildElements.Count;

            uint iExcelIndex = 164;
            NumberingFormats nfs = new NumberingFormats();
            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.ApplyBorder = true;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);

            NumberingFormat nf;
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "dd/mm/yyyy hh:mm:ss";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "#,##0.0000";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            // #,##0.00 is also Excel style index 4
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "#,##0.00";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            // @ is also Excel style index 49
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = "@";
            nfs.Append(nf);
            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);

            nfs.Count = (uint)nfs.ChildElements.Count;
            cfs.Count = (uint)cfs.ChildElements.Count;

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = "Normal";
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = (uint)css.ChildElements.Count;
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = "TableStyleMedium9";
            tss.DefaultPivotStyle = "PivotStyleLight16";
            ss.Append(tss);

            return ss;
        }
    }
}
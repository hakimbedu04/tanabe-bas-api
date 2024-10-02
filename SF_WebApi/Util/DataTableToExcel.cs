using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
﻿using DocumentFormat.OpenXml.Packaging;
﻿using DocumentFormat.OpenXml;
using System.Linq;
using SF_Utils;

namespace SF_WebApi
{
    public class DataTableToExcel
    {
        public void createExcel(string YourExcelfileName, string sheetName)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(YourExcelfileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = (string.IsNullOrEmpty(sheetName)) ? "Export" : sheetName
                };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();
            }
        }

        public void copyExcel(string YourExcelfileName, string sheetName)
        {
            MemoryStream ms = new MemoryStream();

            string strTempFileName = Path.GetTempFileName();
            string strFileName = strTempFileName.Replace(".tmp", string.Empty) + ".xlsx";
            if (System.IO.File.Exists(strFileName))
            {
                System.IO.File.Delete(strFileName);
            }

            var templateFile = Enums.ExcelTemplate.SP_Plan + ".xlsx";
            var templateFileName = HttpContext.Current.Server.MapPath(Constants.SPPlan + templateFile);
            if (System.IO.File.Exists(templateFileName))
            {
                System.IO.File.Copy(templateFileName, strFileName);
            }

        }

        public void generateExcels(DataTable YoutdTName, string YourExcelfileName, string sheetName = null)
        {
            createExcel(YourExcelfileName, sheetName);

            //populate the data into the spreadsheet
            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(YourExcelfileName, true))
            {
                WorkbookPart workbook = spreadsheet.WorkbookPart;
                //create a reference to Sheet1

                //var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();

                //SheetData data = new SheetData();

                WorksheetPart worksheet = workbook.WorksheetParts.Last();
                
                SheetData data = worksheet.Worksheet.GetFirstChild<SheetData>();

                //add column names to the first row
                Row header = new Row();
                
                header.RowIndex = (int)1;

                foreach (DataColumn column in YoutdTName.Columns)
                {
                    Cell headerCell = createTextCell(YoutdTName.Columns.IndexOf(column) + 1, 1, column.ColumnName);
                    header.AppendChild(headerCell);
                }
                
                data.AppendChild(header);

                //loop through each data row
                DataRow contentRow = default(DataRow);
                for (int i = 0; i <= YoutdTName.Rows.Count - 1; i++)
                {
                    contentRow = YoutdTName.Rows[i];
                    data.AppendChild(createContentRow(contentRow, i + 2));
                }
            }
        }

        private static string getColumnName(int columnIndex)
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

        private static Cell createTextCell(int columnIndex, int rowIndex, object cellValue)
        {
            Cell cell = new Cell();
            cell.CellReference = getColumnName(columnIndex) + rowIndex;

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

        private static Row createContentRow(DataRow dataRow, int rowIndex)
        {
            Row row = new Row { RowIndex = (UInt32)rowIndex };

            for (int i = 0; i <= dataRow.Table.Columns.Count - 1; i++)
            {
                Cell dataCell = createTextCell(i + 1, rowIndex, dataRow[i]);
                row.AppendChild(dataCell);
            }
            return row;
        }
    }
}
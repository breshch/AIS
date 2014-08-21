using AIS_Enterprise_Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Reports
{
    public static class Helpers
    {
        public static string CreationNewFileReport(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            string newPath = path;
            int indexExcelNewFile = 0;
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    newPath = newPath.Substring(0, newPath.Length - 5) + "_new.xlsx";

                    while (true)
                    {
                        indexExcelNewFile++;
                        newPath = newPath.Substring(0, newPath.Length - 5) + "_" + indexExcelNewFile + ".xlsx";

                        if (File.Exists(newPath))
                        {
                            try
                            {
                                File.Delete(newPath);
                                break;
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return newPath;
        }

        public static ExcelPackage CreationNewBook(string path)
        {
            return new ExcelPackage(new FileInfo(path));
        }


        public static void CompletedReport(string path, List<Action<ExcelPackage>> methods)
        {
            string newPath = Helpers.CreationNewFileReport(path);
            var ep = Helpers.CreationNewBook(newPath);

            foreach (var method in methods)
            {
                method.Invoke(ep);
            }

            ep.Save();
            Process.Start(newPath);
        }

        public static ExcelWorksheet GetSheet(ExcelPackage ep, string name)
        {
            if (ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(name))
            {
                ep.Workbook.Worksheets.Delete(name);
            }
            ep.Workbook.Worksheets.Add(name);

            return ep.Workbook.Worksheets.First(ws => ws.Name == name);
        }

        public static double PixelsToInches(double pixels)
        {
            return (pixels - 7) / 7d + 1;
        }

        public static void CreateCell(ExcelWorksheet sheet, int fromRow, int fromColumn, int toRow, int toColumn, string value, Color color,
            float size = 11, bool isFontBold = false,
            OfficeOpenXml.Style.ExcelHorizontalAlignment alignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
            OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Medium)
        {
            var cell = sheet.Cells[fromRow, fromColumn, toRow, toColumn];
            cell.Merge = true;
            cell.Style.Font.Size = size;
            cell.Style.Font.Bold = isFontBold;
            cell.Style.HorizontalAlignment = alignment;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Border.BorderAround(borderStyle);
            cell.Style.WrapText = true;
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
            cell.Style.Font.Name = "Courier New";
            cell.Value = value;
        }

        public static void CreateCell(ExcelWorksheet sheet, int fromRow, int fromColumn, int toRow, int toColumn, double value, Color color,
            float size = 11, bool isFontBold = false,
            OfficeOpenXml.Style.ExcelHorizontalAlignment alignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
            OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Medium)
        {
            var cell = sheet.Cells[fromRow, fromColumn, toRow, toColumn];
            cell.Merge = true;
            cell.Style.Font.Size = size;
            cell.Style.Font.Bold = isFontBold;
            cell.Style.HorizontalAlignment = alignment;
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            cell.Style.Border.BorderAround(borderStyle);
            cell.Style.WrapText = true;
            cell.Style.Numberformat.Format = value % 1 == 0 ? "#,##0" : "#,##0.0";
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
            cell.Style.Font.Name = "Courier New";
            cell.Value = value;
        }

        public static void CreateCell(ExcelWorksheet sheet, int row, int column, string value, Color color, float size = 11, bool isFontBold = false,
            OfficeOpenXml.Style.ExcelHorizontalAlignment alignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
            OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Medium)
        {
            CreateCell(sheet, row, column, row, column, value, color, size, isFontBold, alignment, borderStyle);
        }

        public static void CreateCell(ExcelWorksheet sheet, int row, int column, double value, Color color, float size = 11, bool isFontBold = false,
            OfficeOpenXml.Style.ExcelHorizontalAlignment alignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
            OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Medium)
        {
            CreateCell(sheet, row, column, row, column, value, color, size, isFontBold, alignment, borderStyle);
        }
    }
}

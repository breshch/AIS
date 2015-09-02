﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ReportTool.Reports
{
    public static class Helpers
    {
        private static object _syncLock = new object();

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
            string newPath = CreationNewFileReport(path);
            var ep = CreationNewBook(newPath);

            methods.ForEach(method => method.Invoke(ep));

            ep.Save();
            Process.Start(newPath);
        }

        public static ExcelWorksheet GetSheet(ExcelPackage ep, string name)
        {
            lock (_syncLock)
            {
                if (ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(name))
                {
                    ep.Workbook.Worksheets.Delete(name);
                }
                ep.Workbook.Worksheets.Add(name);
            }

            return ep.Workbook.Worksheets.First(ws => ws.Name == name);
        }

        public static double PixelsToInches(double pixels)
        {
            return (pixels - 7) / 7d + 1;
        }

        public static void CreateCell(ExcelWorksheet sheet, int fromRow, int fromColumn, int toRow, int toColumn, string value, Color? color,
            float size = 11, bool isFontBold = false,
            ExcelHorizontalAlignment alignment = ExcelHorizontalAlignment.Center,
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Medium)
        {
            var cell = sheet.Cells[fromRow, fromColumn, toRow, toColumn];
            cell.Merge = true;
            cell.Style.Font.Size = size;
            cell.Style.Font.Bold = isFontBold;
            cell.Style.HorizontalAlignment = alignment;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.BorderAround(borderStyle);
            cell.Style.WrapText = true;
            
			if (color != null)
			{
				cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
				cell.Style.Fill.BackgroundColor.SetColor(color.Value);
			}
            cell.Style.Font.Name = "Segoe UI Light";
            cell.Value = value;
        }

        public static void CreateCell(ExcelWorksheet sheet, int fromRow, int fromColumn, int toRow, int toColumn, double value, Color? color,
            float size = 11, bool isFontBold = false,
            ExcelHorizontalAlignment alignment = ExcelHorizontalAlignment.Center,
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Medium)
        {
            var cell = sheet.Cells[fromRow, fromColumn, toRow, toColumn];
            cell.Merge = true;
            cell.Style.Font.Size = size;
            cell.Style.Font.Bold = isFontBold;
            cell.Style.HorizontalAlignment = alignment;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.BorderAround(borderStyle);
            cell.Style.WrapText = true;
            cell.Style.Numberformat.Format = value % 1 == 0 ? "#,##0" : "#,##0.0";
            
	        if (color != null)
	        {
				cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
				cell.Style.Fill.BackgroundColor.SetColor(color.Value);    
	        }

			cell.Style.Font.Name = "Segoe UI Light";
            cell.Value = value;
        }

        public static void CreateCell(ExcelWorksheet sheet, int row, int column, string value, Color? color, float size = 11, bool isFontBold = false,
            ExcelHorizontalAlignment alignment = ExcelHorizontalAlignment.Center,
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Medium)
        {
            CreateCell(sheet, row, column, row, column, value, color, size, isFontBold, alignment, borderStyle);
        }

        public static void CreateCell(ExcelWorksheet sheet, int row, int column, double value, Color? color, float size = 11, bool isFontBold = false,
            ExcelHorizontalAlignment alignment = ExcelHorizontalAlignment.Center,
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Medium)
        {
            CreateCell(sheet, row, column, row, column, value, color, size, isFontBold, alignment, borderStyle);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;
using OfficeOpenXml;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public static class ConvertingCostsExcelToDB
    {
		private const string PATH_COSTS = @"C:\Users\bresh\Desktop\AutoCosts (1) — копия.xlsx";

        public static void ConvertExcelToDB(BusinessContext bc)
        {
            var existingFile = new FileInfo(PATH_COSTS);

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First();

                        var costItems = bc.GetDirectoryCostItems().ToList();
                        var rcs = bc.GetDirectoryRCs().ToList();

                        int indexRow = 5;
                        while (sheet.Cells[indexRow, 1].Value != null)
                        {
                            //long serialDate = long.Parse(sheet.Cells[indexRow, 1].Value.ToString());
							//var date = DateTime.ParseExact(sheet.Cells[indexRow, 1].Value.ToString(),
							//	"dd.MM.yyyy hh.mm.ss", new CultureInfo("ru-Ru")); 
							// DateTime.FromOADate(serialDate);

	                        DateTime date;
	                        if (!DateTime.TryParseExact(sheet.Cells[indexRow, 1].Text.ToString(), "dd.MM.yyyy",
		                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
	                        {
								date = DateTime.ParseExact(sheet.Cells[indexRow, 1].Text.ToString(), "dd.MM.yy",
			                        CultureInfo.InvariantCulture, DateTimeStyles.None);
	                        }
							
                            string costItemName = sheet.Cells[indexRow, 4].Value.ToString().Trim();
                            var costItem = costItems.First(i => i.Name.ToLower() == costItemName.ToLower());

	                        //string rcName = sheet.Cells[indexRow, 5].Value.ToString().Trim();
                            
							string rcName = "Логистикон";
							var rc = rcs.First(r => r.Name == rcName);

                            double summ = 0;
                            bool isIncoming = false;
                            if (sheet.Cells[indexRow, 5].Value != null)//was 6
	                        {
                                isIncoming = true;
                                summ = double.Parse(sheet.Cells[indexRow, 5].Value.ToString().Trim());//was 7
	                        }
                            else
                            {
                                isIncoming = false;
                                summ = double.Parse(sheet.Cells[indexRow, 6].Value.ToString().Trim());
                            }

	                        var noteDescription = sheet.Cells[indexRow, 7].Value;//was 8
                            var note = bc.AddDirectoryNote(noteDescription != null ? noteDescription.ToString().Trim() : null);

                            var transports = new List<Transport>
                            {
                                new Transport
                                {
                                    DirectoryNote = note,
                                    DirectoryRC = rc,
                                    Weight = 0
                                }
                            };
//                            bc.AddInfoCosts(date, costItem, isIncoming, null, summ, Currency.RUR, null);
							bc.AddInfoCosts(date, costItem, isIncoming, null, summ, Currency.RUR, transports);

                            indexRow++;
							Debug.WriteLine(indexRow);

	                        if (indexRow == 459)
		                        break;
                        }
                    }
                }
            }
        }
    }
}

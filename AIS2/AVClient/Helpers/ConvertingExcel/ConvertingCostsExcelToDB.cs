using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using AVClient.AVServiceReference;
using OfficeOpenXml;

namespace AVClient.Helpers.ConvertingExcel
{
    public static class ConvertingCostsExcelToDB
    {
		private const string PATH_COSTS = @"C:\Users\Alexey\Desktop\затраты Храпуново OLD.xlsx";
		private static AVBusinessLayerClient bc = ServerConnector.GetInstanse;
        public static void ConvertExcelToDB()
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

                        int indexRow = 915;
                        while (sheet.Cells[indexRow, 1].Value != null)
                        {
                            //long serialDate = long.Parse(sheet.Cells[indexRow, 1].Value.ToString());
							//var date = DateTime.ParseExact(sheet.Cells[indexRow, 1].Value.ToString(),
							//	"dd.MM.yyyy hh.mm.ss", new CultureInfo("ru-Ru")); 
							// DateTime.FromOADate(serialDate);

	                        DateTime date;
	                        if (!DateTime.TryParseExact(sheet.Cells[indexRow, 1].Value.ToString(), "dd.MM.yyyy H:mm:ss",
		                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
	                        {
								date = DateTime.ParseExact(sheet.Cells[indexRow, 1].Value.ToString(), "dd.MM.yy H:mm:ss",
			                        CultureInfo.InvariantCulture, DateTimeStyles.None);
	                        }
							
                            string costItemName = sheet.Cells[indexRow, 4].Value.ToString().Trim();
                            var costItem = costItems.First(i => i.Name == costItemName);

	                        string rcName = sheet.Cells[indexRow, 5].Value.ToString().Trim();
                            var rc = rcs.First(r => r.Name == rcName);

                            double summ = 0;
                            bool isIncoming = false;
                            if (sheet.Cells[indexRow, 6].Value != null)
	                        {
                                isIncoming = true;
                                summ = double.Parse(sheet.Cells[indexRow, 6].Value.ToString().Trim());
	                        }
                            else
                            {
                                isIncoming = false;
                                summ = double.Parse(sheet.Cells[indexRow, 7].Value.ToString().Trim());
                            }

	                        var noteDescription = sheet.Cells[indexRow, 8].Value;
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
                            bc.AddInfoCosts(date, costItem, isIncoming, null, summ, Currency.RUR, transports.ToArray());

                            indexRow++;
							Debug.WriteLine(indexRow);

	                        if (indexRow == 1545)
		                        break;
                        }
                    }
                }
            }
        }
    }
}

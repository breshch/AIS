using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public static class ConvertingCostsExcelToDB
    {
        private const string PATH_COSTS = "Files/Costs.xlsx";

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

                        int indexRow = 25;
                        while (sheet.Cells[indexRow, 1].Value != null)
                        {
                            long serialDate = long.Parse(sheet.Cells[indexRow, 1].Value.ToString());
                            DateTime date = DateTime.FromOADate(serialDate);
                            
                            string costItemName = sheet.Cells[indexRow, 4].Value.ToString();
                            var costItem = costItems.First(i => i.Name == costItemName);
                            
                            string rcName = sheet.Cells[indexRow, 5].Value.ToString().Split(' ').Last();
                            var rc = rcs.First(r => r.Name == rcName);

                            double summ = 0;
                            bool isIncoming = false;
                            if (sheet.Cells[indexRow, 6].Value != null)
	                        {
                                isIncoming = true;
                                summ = double.Parse(sheet.Cells[indexRow, 6].Value.ToString());
	                        }
                            else
                            {
                                isIncoming = false;
                                summ = double.Parse(sheet.Cells[indexRow, 7].Value.ToString());
                            }

                            string noteDescription = sheet.Cells[indexRow, 8].Value.ToString();
                            var note = bc.AddDirectoryNote(noteDescription);

                            var transports = new List<Transport>
                            {
                                new Transport
                                {
                                    DirectoryNote = note,
                                    DirectoryRC = rc,
                                    Weight = 0
                                }
                            };
                            bc.AddInfoCosts(date, costItem, isIncoming, null, summ, transports);

                            indexRow++;
                        }
                    }
                }
            }
        }
    }
}

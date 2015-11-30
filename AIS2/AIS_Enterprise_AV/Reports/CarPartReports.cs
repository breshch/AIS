using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AIS_Enterprise_Data;
using OfficeOpenXml;

namespace AIS_Enterprise_AV.Reports
{
    public static class CarPartReports
    {
        public static void ComplitedLoanRemainsToDate(BusinessContext bc, DateTime date)
        {
            string path = Path.Combine(Environment.SpecialFolder.Desktop.ToString(), "Залог остатки" + date.ToShortDateString() +".xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) => LoanRemainsToDate(ep, bc, date)
                });
        }

        private static void LoanRemainsToDate(ExcelPackage ep, BusinessContext bc, DateTime date)
        {
            string name = date.ToShortDateString();
            var sheet = Helpers.GetSheet(ep, name);
            Helpers.CreateCell(sheet, 1, 1, "Артикул", Color.Transparent);
            Helpers.CreateCell(sheet, 1, 2, "Описание", Color.Transparent);
            Helpers.CreateCell(sheet, 1, 3, "Остаток на дату", Color.Transparent);
            Helpers.CreateCell(sheet, 1, 4, "Цена RUR", Color.Transparent);
            Helpers.CreateCell(sheet, 1, 5, "Сумма RUR", Color.Transparent);

            var carPartRemains = bc.GetRemainsToDate(date);
	        double totalSumm = 0;
            int indexRow = 2;
            foreach (var carPartRemain in carPartRemains)
            {
                Helpers.CreateCell(sheet, indexRow, 1, carPartRemain.Article, Color.Transparent);
                Helpers.CreateCell(sheet, indexRow, 2, carPartRemain.Description, Color.Transparent);
                Helpers.CreateCell(sheet, indexRow, 3, carPartRemain.Remain, Color.Transparent);
                Helpers.CreateCell(sheet, indexRow, 4, carPartRemain.PriceRUR, Color.Transparent);
				Helpers.CreateCell(sheet, indexRow, 5, carPartRemain.Remain * carPartRemain.PriceRUR, Color.Transparent);
	            totalSumm += carPartRemain.Remain * carPartRemain.PriceRUR;
                
                indexRow++;
            }
			Helpers.CreateCell(sheet, indexRow, 1, "Итого", Color.Transparent,11,true);
			Helpers.CreateCell(sheet, indexRow, 5, totalSumm + " RUR", Color.Transparent,11,true);
        }


    }
}

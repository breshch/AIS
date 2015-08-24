using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AVClient.AVServiceReference;
using OfficeOpenXml;

namespace AVClient.Reports
{
	public static class SafeReports
	{
		private static AVBusinessLayerClient bc = ServerConnector.GetInstanse;
		private const string PATH_DIRECTORY_CARS_REPORTS = "Reports\\Safe";

		public static void SafeToMinsk(DateTime from, DateTime to)
		{
			string path = Path.Combine(PATH_DIRECTORY_CARS_REPORTS, "SafeToMinsk.xlsx");
			Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    ep =>
                    {
						CashReport(ep, from, to);    
                    }
                });
		}

		private static void CashReport(ExcelPackage ep, DateTime from, DateTime to)
		{

			string name = "SafeToMinsk";
			var sheet = Helpers.GetSheet(ep, name);

			var colorGray = Color.LightGray;
			var colorCell = Color.Transparent;

			Helpers.CreateCell(sheet, 1, 1, "Дата", colorGray);
			Helpers.CreateCell(sheet, 1, 2, "Сумма Минск", colorGray);
			Helpers.CreateCell(sheet, 1, 3, "Сумма сейф", colorGray);
			Helpers.CreateCell(sheet, 1, 4, "Разница", colorGray);

				int indexRow = 2;
				var totalCash = bc.GetTotalEqualCashSafeToMinsks(from, to);
				foreach (var cash in totalCash)
				{
					Helpers.CreateCell(sheet, indexRow, 1, cash.Date.ToString("MM.yyyy"), colorCell);
					Helpers.CreateCell(sheet, indexRow, 2, cash.MinskCash != null ? cash.MinskCash.Value.ToString("c") : "", colorCell);
					Helpers.CreateCell(sheet, indexRow, 3, cash.SafeCash.ToString("c"), colorCell);
					Helpers.CreateCell(sheet, indexRow, 4, cash.MinskCash != null ? (cash.MinskCash.Value - cash.SafeCash).ToString("c") : "", colorCell);

					indexRow++;
				}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ReportTool.Models;
using Color = System.Drawing.Color;

namespace ReportTool.Reports
{
	public class CashReports
	{
		private static string _pathCashReport = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AVReports", "CashReports");

		public static void MonthCashReportMinsk()
		{
			using (var bc = new BusinessContext())
			{

				var minDate = bc.GetParameterValue<DateTime?>(ParameterType.MinDateCostChange);
				var maxDate = bc.GetParameterValue<DateTime?>(ParameterType.MaxDateCostChange);

				if (!minDate.HasValue || !maxDate.HasValue)
				{
					return;
				}

				DateTime prevDate = DateTime.MinValue;
				DirectoryRCPercentage[] rcPercentages = null;
				for (DateTime date = minDate.Value.Date; date <= maxDate.Value.Date; date = date.AddDays(1))
				{
					using (var ep = new ExcelPackage())
					{
						var name = "Итого";


						if (!ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(name))
						{
							ep.Workbook.Worksheets.Add(name);
						}

						var sheet = ep.Workbook.Worksheets.First(ws => ws.Name == name);
						var colorTransparent = Color.Transparent;
						if (sheet.Cells[1, 1].Value == null)
						{
							Helpers.CreateCell(sheet, 1, 1, "ЦО", null, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, 1, 2, "Валюта", null, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, 1, 3, "Приход", null, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, 1, 4, "Расход", null, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
						}

						sheet.Cells[1, 1, 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

						int indexRow = 3;

						var costs = bc.GetInfoCosts(date).ToList();
						if (!costs.Any())
						{
							continue;
						}

						var totalSums = new Dictionary<Currency, Balance>();
						var currencies =
							Enum.GetNames(typeof (Currency)).Select(x => (Currency) Enum.Parse(typeof (Currency), x)).ToArray();
						foreach (var currency in currencies)
						{
							totalSums[currency] = new Balance();
						}

						if (prevDate.Month != date.Month)
						{
							rcPercentages = bc.GetRCPercentages(date.Year, date.Month)
								.OrderByDescending(x => x.Percentage).ToArray();
						}

						prevDate = date;


						var distinctedRCs = costs.Select(c => c.DirectoryRC).Distinct().ToArray();
						var sortedRCs = new List<DirectoryRC>();
						foreach (var rcPercentage in rcPercentages)
						{
							var rc = distinctedRCs.FirstOrDefault(x => x.Id == rcPercentage.DirectoryRCId);
							if (rc != null)
							{
								sortedRCs.Add(rc);
							}
						}

						foreach (var rc in sortedRCs)
						{
							var costsRcCurrencies = costs.Where(c => c.DirectoryRC.Id == rc.Id).GroupBy(x => x.Currency);

							bool isFirst = false;
							int countCurrenciesPerRC = 0;
							foreach (var costsCurrency in costsRcCurrencies)
							{
								countCurrenciesPerRC++;

								double summIncoming = 0;
								double summExpence = 0;

								foreach (var cost in costsCurrency)
								{
									summIncoming += cost.IsIncoming ? cost.Summ : 0;
									summExpence += !cost.IsIncoming ? cost.Summ : 0;
								}

								totalSums[costsCurrency.Key].Expence += summExpence;
								totalSums[costsCurrency.Key].Incoming += summIncoming;

								if (!isFirst)
								{
									isFirst = true;
									Helpers.CreateCell(sheet, indexRow, 1, rc.Name, null, 12, true, ExcelHorizontalAlignment.Center,
										ExcelBorderStyle.None);
								}

								Helpers.CreateCell(sheet, indexRow, 2, costsCurrency.Key.ToString(), null, 12, true,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 3, summIncoming.ToString("N2"), null, 12, true,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 4, summExpence.ToString("N2"), null, 12, true,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);

								indexRow++;
							}

							sheet.Cells[indexRow - countCurrenciesPerRC, 1, indexRow - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

							indexRow++;
						}

						bool isFirstTotal = false;
						int countTotals = 0;
						foreach (var totals in totalSums.Where(x => x.Value.Expence > 0 || x.Value.Incoming > 0))
						{
							countTotals++;

							if (!isFirstTotal)
							{
								isFirstTotal = true;
								Helpers.CreateCell(sheet, indexRow, 1, "Итого", null, 12, true, ExcelHorizontalAlignment.Center,
									ExcelBorderStyle.None);
							}

							Helpers.CreateCell(sheet, indexRow, 2, totals.Key.ToString(), null, 12, true, ExcelHorizontalAlignment.Center,
								ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 3, totals.Value.Incoming.ToString("N2"), null, 12, true,
								ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 4, totals.Value.Expence.ToString("N2"), null, 12, true,
								ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);

							indexRow++;
						}

						sheet.Cells[indexRow - countTotals, 1, indexRow - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

						indexRow += 2;

						DateTime startDate = date;

						int counter = 0;
						double? minskSum = null;
						while (!minskSum.HasValue)
						{
							counter++;
							minskSum = bc.GetTotalEqualCashSafeToMinsks(startDate);
							if (!minskSum.HasValue)
							{
								startDate = startDate.AddMonths(-1);
							}

							if (counter > 12)
							{
								break;
							}
						}

						if (minskSum.HasValue)
						{
							var costsToPeriod = bc.GetInfoCosts(new DateTime(startDate.Year, startDate.Month, 1), date).ToArray();
							var costsSum = costsToPeriod
								.Where(x => x.Currency == Currency.RUR)
								.Sum(x => x.IsIncoming ? x.Summ : -x.Summ);

							Helpers.CreateCell(sheet, indexRow, 1, indexRow, 2, "Итого касса на " + date.ToShortDateString(), null, 12, true,
								ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 3, (minskSum + costsSum) + " RUR", null, 12, true,
								ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);

							indexRow++;
							foreach (
								var totals in totalSums.Where(x => x.Key != Currency.RUR && (x.Value.Expence > 0 || x.Value.Incoming > 0)))
							{
								double sum = costsToPeriod.Where(x => x.Currency == totals.Key)
									.Sum(x => x.IsIncoming ? x.Summ : -x.Summ);
								if (sum == 0)
								{
									continue;
								}

								Helpers.CreateCell(sheet, indexRow, 3, sum + " " + totals.Key, null, 12, true, ExcelHorizontalAlignment.Center,
									ExcelBorderStyle.None);
								indexRow++;
							}
						}

						sheet.Column(1).Width = Helpers.PixelsToInches(200);
						sheet.Column(2).Width = Helpers.PixelsToInches(200);
						sheet.Column(3).Width = Helpers.PixelsToInches(200);
						sheet.Column(4).Width = Helpers.PixelsToInches(200);


						name = "Касса за " + date.ToShortDateString();
						sheet = Helpers.GetSheet(ep, name);

						Helpers.CreateCell(sheet, 1, 1, "Статья затрат", colorTransparent, 12, true, ExcelHorizontalAlignment.Center,
							ExcelBorderStyle.Thick);
						Helpers.CreateCell(sheet, 1, 2, "ЦО", colorTransparent, 12, true, ExcelHorizontalAlignment.Center,
							ExcelBorderStyle.Thick);
						Helpers.CreateCell(sheet, 1, 3, "Приход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center,
							ExcelBorderStyle.Thick);
						Helpers.CreateCell(sheet, 1, 4, "Расход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center,
							ExcelBorderStyle.Thick);
						Helpers.CreateCell(sheet, 1, 5, "Описание", colorTransparent, 12, true, ExcelHorizontalAlignment.Center,
							ExcelBorderStyle.Thick);

						indexRow = 2;

						var colorGray = Color.LightGray;

						double maxLengthNote = 0;

						foreach (var rc in sortedRCs)
						{
							double summIncoming = 0;
							double summExpence = 0;

							int firstIndexRow = indexRow;
							foreach (var cost in costs.Where(c => c.DirectoryRC.Id == rc.Id).OrderBy(c => c.Date))
							{
								Helpers.CreateCell(sheet, indexRow, 1, cost.DirectoryCostItem.Name, colorTransparent, 11, false,
									ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 2, cost.DirectoryRC.Name, colorTransparent, 11, false,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 3, cost.IsIncoming ? cost.Incoming : null, colorTransparent, 11, false,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 4, !cost.IsIncoming ? cost.Expense : null, colorTransparent, 11, false,
									ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
								Helpers.CreateCell(sheet, indexRow, 5, cost.ConcatNotes, colorTransparent, 11, false,
									ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);

								FormattedText formattedText = new FormattedText(cost.ConcatNotes, CultureInfo.CurrentCulture,
									FlowDirection.LeftToRight,
									new Typeface("Courier New"), 11, Brushes.Black);

								if (maxLengthNote < formattedText.Width)
								{
									maxLengthNote = formattedText.Width;
								}

								summIncoming += cost.IsIncoming ? cost.Summ : 0;
								summExpence += !cost.IsIncoming ? cost.Summ : 0;

								indexRow++;
							}

							Helpers.CreateCell(sheet, indexRow, 1, "Итого", colorGray, 12, true, ExcelHorizontalAlignment.Center,
								ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 2, null, colorGray, 12, true, ExcelHorizontalAlignment.Center,
								ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 3, summIncoming.ToString("c"), colorGray, 12, true,
								ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 4, summExpence.ToString("c"), colorGray, 12, true,
								ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
							Helpers.CreateCell(sheet, indexRow, 5, null, colorGray, 12, true, ExcelHorizontalAlignment.Center,
								ExcelBorderStyle.None);

							sheet.Cells[firstIndexRow, 1, indexRow - 1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thick);
							sheet.Cells[firstIndexRow, 2, indexRow - 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thick);
							sheet.Cells[firstIndexRow, 3, indexRow - 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thick);
							sheet.Cells[firstIndexRow, 4, indexRow - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thick);
							sheet.Cells[firstIndexRow, 5, indexRow - 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);

							indexRow++;
						}

						sheet.Column(1).Width = Helpers.PixelsToInches(100);
						sheet.Column(2).Width = Helpers.PixelsToInches(150);
						sheet.Column(3).Width = Helpers.PixelsToInches(100);
						sheet.Column(4).Width = Helpers.PixelsToInches(100);
						sheet.Column(5).Width = Helpers.PixelsToInches(maxLengthNote*1.5);

						string pathDirectoryMonth = date.ToString("MM.yyyy");

						string fileName = Path.Combine(_pathCashReport, pathDirectoryMonth, "Касса за " + date.ToString("dd.MM.yyyy HH-mm-ss") + ".xlsx");
						if (!Directory.Exists(Path.GetDirectoryName(fileName)))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(fileName));
						}
						ep.SaveAs(new FileInfo(fileName));
					}
				}

				bc.EditParameter<DateTime?>(ParameterType.MinDateCostChange, null);
				bc.EditParameter<DateTime?>(ParameterType.MaxDateCostChange, null);
			}
		}
	}

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Color = System.Drawing.Color;

namespace AIS_Enterprise_AV.Reports
{
    public static class WorkerSalaryReports
    {
        private const string COMPANY_NAME_AV = "АВ";
        private const string PATH_REPORT_MINSK = "Reports\\Minsk\\Зарплата.xlsx";
        private const string PATH_DIRECTORY_REPORT_SALARY_WORKERS = "Reports\\Print\\Зарплата.xlsx";
        private const string PATH_CASH_REPORT = "Reports\\Касса.xlsx";

        private const int INDEX_HEADER_ROW_OVERTIME = 4;
        private const int COUNT_HEADER_ROW_OVERTIME = 3;

        private const int INDEX_HEADER_ROW_DATE_OVERTIME = 4;
        private const int INDEX_HEADER_ROW_DESCRIPTION_OVERTIME = 5;
        private const int INDEX_HEADER_ROW_RCS_OVERTIME = 6;

        private const int COUNT_HEADER_ROW_SUMM_OVERTIME = 2;

        private const int INDEX_HEADER_COLUMN_FULL_NAME_OVERTIME = 1;
        private const int INDEX_HEADER_COLUMN_POST_NAME_OVERTIME = 2;

        private const int INDEX_HEADER_ROW_DATE_MINSK = 1;
        private const int COUNT_HEADER_ROW_DATE_MINSK = 2;

        private const int INDEX_HEADER_ROW_MINSK = 3;
        private const int COUNT_HEADER_ROW_MINSK = 3;

        private const int INDEX_HEADER_ROW_AV_FENOX_MINSK = 4;
        private const int INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK = 5;
        private const int COUNT_HEADER_ROW_AV_FENOX_MINSK = 2;

        private const int INDEX_HEADER_COLUMN_PP_MINSK = 1;
        private const int INDEX_HEADER_COLUMN_FULL_NAME_MINSK = 2;
        private const int INDEX_HEADER_COLUMN_POST_NAME_MINSK = 3;
        private const int INDEX_HEADER_COLUMN_SALARY_AV_MINSK = 4;
        private const int INDEX_HEADER_COLUMN_CARD_AV_MINSK = 5;
        private const int INDEX_HEADER_COLUMN_PREPAYMENT_AV_MINSK = 6;
        private const int INDEX_HEADER_COLUMN_COMPENSATION_AV_MINSK = 7;
        private const int INDEX_HEADER_COLUMN_OVERTIME_KO5_AV_MINSK = 8;
        private const int INDEX_HEADER_COLUMN_OVERTIME_PAM16_AV_MINSK = 9;
        private const int INDEX_HEADER_COLUMN_CASH_AV_MINSK = 10;
        private const int INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK = 11;
        private const int INDEX_HEADER_COLUMN_CARD_FENOX_MINSK = 12;
        private const int INDEX_HEADER_COLUMN_OVERTIME_MO5_FENOX_MINSK = 13;
        private const int INDEX_HEADER_COLUMN_OVERTIME_PAM1_FENOX_MINSK = 14;
        private const int INDEX_HEADER_COLUMN_OVERTIME_MO2_FENOX_MINSK = 15;
        private const int INDEX_HEADER_COLUMN_CASH_FENOX_MINSK = 16;

        private const int INDEX_HEADER_COLUMN_OFFICE_MINSK = 17;
        private const int INDEX_HEADER_COLUMN_ISSUE_SALARY_MINSK = 18;
        private const int INDEX_HEADER_COLUMN_TOTAL_SALARY_MINSK = 19;
        private const int INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK = 20;

        private const int COUNT_COLUMNS_AV_MINSK = 7;
        private const int COUNT_COLUMNS_FENOX_MINSK = 6;


        public static void ComplitedReportSalaryOvertimeTransportMinsk(int year, int month)
        {
            var sw = Stopwatch.StartNew();
            Helpers.CompletedReport(PATH_REPORT_MINSK, new List<Action<ExcelPackage>>
                {
                    (ep) => SalaryReportMinsk(ep, year, month),
                    (ep) => OverTimeReportMinsk(ep, year, month),
                    (ep) => TransportReportMinsk(ep, year, month),
                    
                });
            Debug.WriteLine("Total time : " + sw.ElapsedMilliseconds);
        }

        public static void ComplitedReportSalaryWorkers(int year, int month)
        {
            Helpers.CompletedReport(PATH_DIRECTORY_REPORT_SALARY_WORKERS, new List<Action<ExcelPackage>>
                {
                    (ep) => SalaryReportWorkers(ep, year, month),
                });
        }

        public static void ComplitedMonthCashReportMinsk(int year, int month)
        {
            Helpers.CompletedReport(PATH_CASH_REPORT, new List<Action<ExcelPackage>>
                {
                    (ep) => MonthCashReportMinsk(ep, year, month)
                });
        }

        private static void TransportReportMinsk(ExcelPackage ep, int year, int month)
        {
            using (var bc = new BusinessContext())
            {
                var sw = Stopwatch.StartNew();
                string name = "Транспорт";
                var sheet = Helpers.GetSheet(ep, name);

                var colorTransparent = Color.Transparent;

                Helpers.CreateCell(sheet, 1, 1, "Дата", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 2, "ЦО", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 3, "Приход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 4, "Расход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 5, "Описание", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);

                int indexRow = 2;

                double maxLengthNote = 0;
                var costs = bc.GetInfoCosts(year, month).Where(c => c.DirectoryCostItem.Name == "Транспорт (5031)" && c.DirectoryRC.Name != "26А" && c.DirectoryRC.Name != "ВСЕ").ToList();
                foreach (var rc in costs.Select(c => c.DirectoryRC).Distinct().OrderByDescending(r => r.Percentes))
                {
                    double summIncoming = 0;
                    double summExpence = 0;

                    int firstIndexRow = indexRow;
                    foreach (var cost in costs.Where(c => c.DirectoryRC.Id == rc.Id).OrderBy(c => c.Date))
                    {
                        Helpers.CreateCell(sheet, indexRow, 1, cost.Date.ToShortDateString(), colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 2, cost.DirectoryRC.Name, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 3, cost.IsIncoming ? cost.Incoming : null, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 4, !cost.IsIncoming ? cost.Expense : null, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 5, cost.ConcatNotes, colorTransparent, 11, false, ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);

                        var formattedText = new FormattedText(cost.ConcatNotes, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface("Courier New"), 11, Brushes.Black);

                        if (maxLengthNote < formattedText.Width)
                        {
                            maxLengthNote = formattedText.Width;
                        }

                        summIncoming += cost.IsIncoming ? cost.Summ : 0;
                        summExpence += !cost.IsIncoming ? cost.Summ : 0;

                        indexRow++;
                    }

                    var colorGray = Color.LightGray;
                    Helpers.CreateCell(sheet, indexRow, 1, "Итого", colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 2, null, colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 3, summIncoming.ToString("c"), colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 4, summExpence.ToString("c"), colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 5, null, colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);

                    sheet.Cells[firstIndexRow, 1, indexRow - 1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 2, indexRow - 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 3, indexRow - 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 4, indexRow - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 5, indexRow - 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                    indexRow++;
                }


                sheet.Column(1).Width = Helpers.PixelsToInches(100);
                sheet.Column(2).Width = Helpers.PixelsToInches(100);
                sheet.Column(3).Width = Helpers.PixelsToInches(150);
                sheet.Column(4).Width = Helpers.PixelsToInches(150);
                sheet.Column(5).Width = Helpers.PixelsToInches(maxLengthNote * 1.5);

                Debug.WriteLine("Transport : " + sw.ElapsedMilliseconds);
            }
        }

        private static void MonthCashReportMinsk(ExcelPackage ep, int year, int month)
        {
            using (var bc = new BusinessContext())
            {
                var sw = Stopwatch.StartNew();
                string name = "Касса";
                var sheet = Helpers.GetSheet(ep, name);

                var colorTransparent = Color.Transparent;

                Helpers.CreateCell(sheet, 1, 1, "Дата", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 2, "Статья затрат", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 3, "ЦО", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 4, "Приход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 5, "Расход", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);
                Helpers.CreateCell(sheet, 1, 6, "Описание", colorTransparent, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.Thick);

                int indexRow = 2;

                double maxLengthNote = 0;
                var costs = bc.GetInfoCosts(year, month).ToList();
                foreach (var rc in costs.Select(c => c.DirectoryRC).Distinct().OrderByDescending(r => r.Percentes))
                {
                    double summIncoming = 0;
                    double summExpence = 0;

                    int firstIndexRow = indexRow;
                    foreach (var cost in costs.Where(c => c.DirectoryRC.Id == rc.Id).OrderBy(c => c.Date))
                    {
                        Helpers.CreateCell(sheet, indexRow, 1, cost.Date.ToShortDateString(), colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 2, cost.DirectoryCostItem.Name, colorTransparent, 11, false, ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 3, cost.DirectoryRC.Name, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 4, cost.IsIncoming ? cost.Incoming : null, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 5, !cost.IsIncoming ? cost.Expense : null, colorTransparent, 11, false, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                        Helpers.CreateCell(sheet, indexRow, 6, cost.ConcatNotes, colorTransparent, 11, false, ExcelHorizontalAlignment.Left, ExcelBorderStyle.None);

                        FormattedText formattedText = new FormattedText(cost.ConcatNotes, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface("Courier New"), 11, Brushes.Black);

                        if (maxLengthNote < formattedText.Width)
                        {
                            maxLengthNote = formattedText.Width;
                        }

                        summIncoming += cost.IsIncoming ? cost.Summ : 0;
                        summExpence += !cost.IsIncoming ? cost.Summ : 0;

                        indexRow++;
                    }

                    var colorGray = Color.LightGray;
                    Helpers.CreateCell(sheet, indexRow, 1, "Итого", colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 2, null, colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 3, null, colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 4, summIncoming.ToString("c"), colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 5, summExpence.ToString("c"), colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);
                    Helpers.CreateCell(sheet, indexRow, 6, null, colorGray, 12, true, ExcelHorizontalAlignment.Center, ExcelBorderStyle.None);

                    sheet.Cells[firstIndexRow, 1, indexRow - 1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 2, indexRow - 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 3, indexRow - 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 4, indexRow - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 5, indexRow - 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[firstIndexRow, 6, indexRow - 1, 6].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                    indexRow++;
                }


                sheet.Column(1).Width = Helpers.PixelsToInches(100);
                sheet.Column(2).Width = Helpers.PixelsToInches(150);
                sheet.Column(3).Width = Helpers.PixelsToInches(100);
                sheet.Column(4).Width = Helpers.PixelsToInches(100);
                sheet.Column(5).Width = Helpers.PixelsToInches(150);
                sheet.Column(6).Width = Helpers.PixelsToInches(maxLengthNote * 1.5);

                Debug.WriteLine("Transport : " + sw.ElapsedMilliseconds);
            }
        }

        private static void OverTimeReportMinsk(ExcelPackage ep, int year, int month)
        {
            using (var bc = new BusinessContext())
            {
                var sw = Stopwatch.StartNew();
                var overTimes = bc.GetInfoOverTimes(year, month).ToList();

                if (!overTimes.Any())
                {
                    return;
                }

                string name = "Переработка";
                var sheet = Helpers.GetSheet(ep, name);

                var colorTransparent = Color.Transparent;

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_OVERTIME, INDEX_HEADER_COLUMN_FULL_NAME_OVERTIME, INDEX_HEADER_ROW_OVERTIME + COUNT_HEADER_ROW_OVERTIME - 1, INDEX_HEADER_COLUMN_FULL_NAME_OVERTIME, "ФИО", colorTransparent);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_OVERTIME, INDEX_HEADER_COLUMN_POST_NAME_OVERTIME, INDEX_HEADER_ROW_OVERTIME + COUNT_HEADER_ROW_OVERTIME - 1, INDEX_HEADER_COLUMN_POST_NAME_OVERTIME, "Должность", colorTransparent);

                var workers = bc.GetDirectoryWorkers(year, month, false).ToList();

                var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);
                int indexRowWorker = INDEX_HEADER_ROW_OVERTIME + COUNT_HEADER_ROW_OVERTIME;

                foreach (var worker in workers)
                {
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_FULL_NAME_OVERTIME, worker.FullName, colorTransparent, 11, false, ExcelHorizontalAlignment.Left);

                    string postName = bc.GetCurrentPost(worker.Id, lastDateInMonth).DirectoryPost.Name;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_POST_NAME_OVERTIME, postName, colorTransparent);

                    indexRowWorker++;
                }

                var weekEndsInMonth = bc.GetHolidays(year, month).ToList();


                var workerSumms = new List<WorkerSummForReport>();

                int countWorkDayInMonth = bc.GetCountWorkDaysInMonth(year, month);

                var maxRCs = new List<DirectoryRC>();
                int indexColumnOverTime = INDEX_HEADER_COLUMN_POST_NAME_OVERTIME + 1;
                foreach (var overTime in overTimes)
                {
                    int countRCs = overTime.CurrentRCs.ToList().Count();

                    Helpers.CreateCell(sheet, INDEX_HEADER_ROW_DATE_OVERTIME, indexColumnOverTime, INDEX_HEADER_ROW_DATE_OVERTIME, indexColumnOverTime + countRCs - 1, overTime.StartDate.ToShortDateString(), colorTransparent);
                    Helpers.CreateCell(sheet, INDEX_HEADER_ROW_DESCRIPTION_OVERTIME, indexColumnOverTime, INDEX_HEADER_ROW_DESCRIPTION_OVERTIME, indexColumnOverTime + countRCs - 1, overTime.Description, colorTransparent);

                    var currentRCs = overTime.CurrentRCs.ToList();
                    int currentPercentage = currentRCs.Sum(r => r.DirectoryRC.Percentes);
                    for (int i = 0; i < countRCs; i++)
                    {
                        if (!maxRCs.Select(r => r.Name).Contains(currentRCs[i].DirectoryRC.Name))
                        {
                            maxRCs.Add(currentRCs[i].DirectoryRC);
                        }

                        Helpers.CreateCell(sheet, INDEX_HEADER_ROW_RCS_OVERTIME, indexColumnOverTime + i, currentRCs[i].DirectoryRC.Name, colorTransparent);

                        if (countRCs < 3)
                        {
                            sheet.Column(indexColumnOverTime + i).Width = Helpers.PixelsToInches(192 / countRCs);
                        }

                        int indexRowWorkerRC = 0;
                        foreach (var worker in workers)
                        {
                            var workerSummForReport = workerSumms.FirstOrDefault(w => w.WorkerId == worker.Id);
                            if (workerSummForReport == null)
                            {
                                workerSummForReport = new WorkerSummForReport { WorkerId = worker.Id };
                                workerSumms.Add(workerSummForReport);
                            }

                            indexRowWorkerRC++;
                            double valueRC = 0;
                            var infoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == overTime.StartDate.Date);
                            if (infoDate != null)
                            {
                                var overTimeHours = bc.IsOverTime(infoDate, weekEndsInMonth);
                                if (overTimeHours != null)
                                {
                                    valueRC = overTimeHours.Value * 1.3 * currentRCs[i].DirectoryRC.Percentes / currentPercentage;

                                    var workerRCSummForReport = workerSummForReport.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == currentRCs[i].DirectoryRC.Name);
                                    if (workerRCSummForReport == null)
                                    {
                                        workerRCSummForReport = new WorkerRCSummForReport { RCName = currentRCs[i].DirectoryRC.Name };
                                        workerSummForReport.WorkerRCSummForReports.Add(workerRCSummForReport);
                                    }

                                    var post = bc.GetCurrentPost(worker.Id, infoDate.Date).DirectoryPost;
                                    var postSalary = bc.GetDirectoryPostSalaryByDate(post.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

                                    double salaryInHour = postSalary.AdminWorkerSalary.Value / 8 / countWorkDayInMonth;
                                    workerRCSummForReport.Summ += valueRC * 2 * salaryInHour;
                                }
                            }

                            Helpers.CreateCell(sheet, INDEX_HEADER_ROW_RCS_OVERTIME + indexRowWorkerRC, indexColumnOverTime + i, valueRC, colorTransparent);
                        }
                    }

                    indexColumnOverTime += countRCs;
                }

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_OVERTIME, indexColumnOverTime, INDEX_HEADER_ROW_OVERTIME + COUNT_HEADER_ROW_SUMM_OVERTIME - 1, indexColumnOverTime + maxRCs.Count - 1, "Итого", colorTransparent);

                int indexCurrentRC = indexColumnOverTime;
                foreach (var rc in maxRCs.OrderByDescending(r => r.Percentes))
                {
                    Helpers.CreateCell(sheet, INDEX_HEADER_ROW_RCS_OVERTIME, indexCurrentRC, rc.Name, colorTransparent);

                    int indexRowWorkerSum = 0;
                    foreach (var workerSumm in workerSumms)
                    {
                        indexRowWorkerSum++;

                        double valueSumm = 0;
                        if (workerSumm.WorkerRCSummForReports.Select(w => w.RCName).Contains(rc.Name))
                        {
                            valueSumm = workerSumm.WorkerRCSummForReports.First(w => w.RCName == rc.Name).Summ;
                        }

                        Helpers.CreateCell(sheet, INDEX_HEADER_ROW_RCS_OVERTIME + indexRowWorkerSum, indexCurrentRC, valueSumm, colorTransparent);
                        sheet.Column(indexCurrentRC).Width = Helpers.PixelsToInches(80);
                    }


                    indexCurrentRC++;
                }

                Helpers.CreateCell(sheet, 1, 1, INDEX_HEADER_ROW_OVERTIME - 1, indexCurrentRC - 1, "Переработка", colorTransparent, 26, true);

                sheet.Column(INDEX_HEADER_COLUMN_FULL_NAME_OVERTIME).Width = Helpers.PixelsToInches(250);
                sheet.Column(INDEX_HEADER_COLUMN_POST_NAME_OVERTIME).Width = Helpers.PixelsToInches(100);

                sheet.Row(INDEX_HEADER_ROW_DESCRIPTION_OVERTIME).Height = 90;
                Debug.WriteLine("Overtime : " + sw.ElapsedMilliseconds);
            }
        }

        private static void SalaryReportMinsk(ExcelPackage ep, int year, int month)
        {
            using (var bc = new BusinessContext())
            {
                string name = month.ToString() + "'" + year.ToString();

                var sheet = Helpers.GetSheet(ep, name);

                var colorHeaderDate = Color.FromArgb(237, 237, 237);
                var colorAVCash = Color.FromArgb(91, 155, 213);
                var colorAVSalary = Color.FromArgb(221, 235, 247);
                var colorAVKO5 = Color.FromArgb(189, 215, 238);
                var colorAVPam16 = Color.FromArgb(198, 224, 180);

                var colorFenoxCash = Color.FromArgb(237, 125, 49);
                var colorFenoxSalary = Color.FromArgb(252, 228, 214);
                var colorFenoxOverTime = Color.FromArgb(248, 203, 173);

                var color26 = Color.FromArgb(255, 230, 153);


                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_DATE_MINSK, INDEX_HEADER_COLUMN_PP_MINSK, INDEX_HEADER_ROW_DATE_MINSK + COUNT_HEADER_ROW_DATE_MINSK - 1, INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK, "Зарплатный табель за " + month + "." + year, colorHeaderDate, 20, true);

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_PP_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_PP_MINSK, "№ ПП", colorHeaderDate, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_FULL_NAME_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_FULL_NAME_MINSK, "ФИО", colorHeaderDate, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_POST_NAME_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_POST_NAME_MINSK, "Должность", colorHeaderDate, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_SALARY_AV_MINSK, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_SALARY_AV_MINSK + COUNT_COLUMNS_AV_MINSK - 1, "АВ-Автотехник", colorAVCash, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK + COUNT_COLUMNS_FENOX_MINSK - 1, "Фенокс Автомотив РУС", colorFenoxCash, 11, true);

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_SALARY_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_SALARY_AV_MINSK, "Оклад", colorAVSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_CARD_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_CARD_AV_MINSK, "Безнал", colorAVSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_PREPAYMENT_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_PREPAYMENT_AV_MINSK, "Аванс", colorAVSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_COMPENSATION_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_COMPENSATION_AV_MINSK, "Ком-ия\\ Отпуск", colorAVSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_OVERTIME_KO5_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_OVERTIME_PAM16_AV_MINSK, "Переработка", colorAVSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK, INDEX_HEADER_COLUMN_OVERTIME_KO5_AV_MINSK, "КО-5", colorAVKO5, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK, INDEX_HEADER_COLUMN_OVERTIME_PAM16_AV_MINSK, "ПАМ-16", colorAVPam16, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_CASH_AV_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_CASH_AV_MINSK, "Касса", colorAVCash, 11, true);

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK, "Оклад", colorFenoxSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_CARD_FENOX_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_CARD_FENOX_MINSK, "Безнал", colorFenoxSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_OVERTIME_MO5_FENOX_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_OVERTIME_MO2_FENOX_MINSK, "Переработка", colorFenoxSalary, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK, INDEX_HEADER_COLUMN_OVERTIME_MO5_FENOX_MINSK, "МО-5", colorFenoxOverTime, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK, INDEX_HEADER_COLUMN_OVERTIME_PAM1_FENOX_MINSK, "ПАМ-1", colorFenoxOverTime, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_OVERTIME_MINSK, INDEX_HEADER_COLUMN_OVERTIME_MO2_FENOX_MINSK, "МО-2", colorFenoxOverTime, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_AV_FENOX_MINSK, INDEX_HEADER_COLUMN_CASH_FENOX_MINSK, INDEX_HEADER_ROW_AV_FENOX_MINSK + COUNT_HEADER_ROW_AV_FENOX_MINSK - 1, INDEX_HEADER_COLUMN_CASH_FENOX_MINSK, "Касса", colorFenoxCash, 11, true);

                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_OFFICE_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_OFFICE_MINSK, "26А", color26, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_ISSUE_SALARY_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_ISSUE_SALARY_MINSK, "Итого к выдаче", colorHeaderDate, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_TOTAL_SALARY_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_TOTAL_SALARY_MINSK, "Итого З/П", colorHeaderDate, 11, true);
                Helpers.CreateCell(sheet, INDEX_HEADER_ROW_MINSK, INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK, INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1, INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK, "Касса + переработка", colorHeaderDate, 11, true);



                var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);

                var warehouseWorkers = bc.GetDirectoryWorkers(year, month, false).ToList();

                var overTimes = bc.GetInfoOverTimes(year, month).ToList();
                var weekEndsInMonth = bc.GetHolidays(year, month).ToList();
                var workerSumms = new List<WorkerSummForReport>();

                var currentRCs = bc.GetCurrentRCs(overTimes.Select(o => o.Id)).ToList();

                int countWorkDayInMonth = bc.GetCountWorkDaysInMonth(year, month);


                var sw2 = Stopwatch.StartNew();
                foreach (var overTime in overTimes)
                {
                    var overTimeRCs = currentRCs.Where(r => r.InfoOverTimeId == overTime.Id).ToList();
                    int countRCs = overTimeRCs.Count();

                    int currentPercentage = overTimeRCs.Sum(r => r.DirectoryRC.Percentes);
                    for (int i = 0; i < countRCs; i++)
                    {
                        foreach (var worker in warehouseWorkers)
                        {
                            var workerSummForReport = workerSumms.FirstOrDefault(w => w.WorkerId == worker.Id);
                            if (workerSummForReport == null)
                            {
                                workerSummForReport = new WorkerSummForReport { WorkerId = worker.Id };
                                workerSumms.Add(workerSummForReport);
                            }

                            var infoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == overTime.StartDate.Date);
                            if (infoDate != null)
                            {
                                var overTimeHours = bc.IsOverTime(infoDate, weekEndsInMonth);
                                if (overTimeHours != null)
                                {
                                    double percentage = overTimeHours.Value * 1.3 * overTimeRCs[i].DirectoryRC.Percentes / currentPercentage;

                                    var workerRCSummForReport = workerSummForReport.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == overTimeRCs[i].DirectoryRC.Name);
                                    if (workerRCSummForReport == null)
                                    {
                                        workerRCSummForReport = new WorkerRCSummForReport { RCName = overTimeRCs[i].DirectoryRC.Name };
                                        workerSummForReport.WorkerRCSummForReports.Add(workerRCSummForReport);
                                    }

                                    var post = bc.GetCurrentPost(worker.Id, infoDate.Date).DirectoryPost;
                                    var postSalary = bc.GetDirectoryPostSalaryByDate(post.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

                                    double salaryInHour = postSalary.AdminWorkerSalary.Value / 8 / countWorkDayInMonth;
                                    workerRCSummForReport.Summ += percentage * 2 * salaryInHour;
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine("Before : " + sw2.ElapsedMilliseconds);

                double totalCardAV = 0;
                double totalPrepaymentBankTransactionAV = 0;
                double totalCompensationAV = 0;
                double totalOverTimeKO5AV = 0;
                double totalOverTimePAM16AV = 0;
                double totalCashAV = 0;
                double totalCardFenox = 0;
                double totalOverTimeMO5Fenox = 0;
                double totalOverTimePAM1Fenox = 0;
                double totalOverTimeMO2Fenox = 0;
                double totalCashFenox = 0;
                double totalOffice = 0;
                double totalIssueSalary = 0;
                double totalTotalSalary = 0;
                double totalCashPlusOverTimes = 0;

                var currentMainWorkerPosts = bc.GetCurrentMainPosts(lastDateInMonth).ToList();

                int indexWorker = 0;
                foreach (var worker in warehouseWorkers)
                {
                    indexWorker++;

                    int indexRowWorker = (INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1) + indexWorker;

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_PP_MINSK, indexWorker, colorHeaderDate);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_FULL_NAME_MINSK, worker.FullName, colorHeaderDate, 11, false, ExcelHorizontalAlignment.Left);

                    var currentWorkerPost = currentMainWorkerPosts.First(p => p.DirectoryWorkerId == worker.Id);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_POST_NAME_MINSK, currentWorkerPost.DirectoryPost.Name, colorHeaderDate);

                    var infoMonth = bc.GetInfoMonth(worker.Id, year, month);

                    double salaryAV = 0;
                    double salaryFenox = 0;

                    var postSalary = bc.GetDirectoryPostSalaryByDate(currentWorkerPost.DirectoryPost.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

                    if (!currentWorkerPost.IsTwoCompanies)
                    {
                        if (currentWorkerPost.DirectoryPost.DirectoryCompany.Name == "АВ")
                        {
                            salaryAV = postSalary.AdminWorkerSalary.Value;
                        }
                        else
                        {
                            salaryFenox = postSalary.AdminWorkerSalary.Value;
                        }
                    }
                    else
                    {
                        salaryAV = postSalary.AdminWorkerSalary.Value - postSalary.UserWorkerHalfSalary.Value;
                        salaryFenox = postSalary.UserWorkerHalfSalary.Value;
                    }

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_SALARY_AV_MINSK, salaryAV, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CARD_AV_MINSK, infoMonth.CardAV, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_PREPAYMENT_AV_MINSK, infoMonth.PrepaymentBankTransaction, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_COMPENSATION_AV_MINSK, infoMonth.Compensation + infoMonth.VocationPayment, colorAVSalary);

                    double totalOverTimeAV = 0;

                    var workerSumm = workerSumms.FirstOrDefault(w => w.WorkerId == worker.Id);
                    WorkerRCSummForReport rc = null;

                    double rcValue = 0;
                    if (workerSumm != null)
                    {
                        rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "КО-5");

                        if (rc != null)
                        {
                            rcValue = rc.Summ;
                        }
                    }

                    totalOverTimeAV += rcValue;
                    totalOverTimeKO5AV += rcValue;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_KO5_AV_MINSK, rcValue, colorAVKO5);

                    rcValue = 0;
                    if (workerSumm != null)
                    {
                        rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "ПАМ-16");

                        if (rc != null)
                        {
                            rcValue = rc.Summ;
                        }
                    }

                    totalOverTimeAV += rcValue;
                    totalOverTimePAM16AV += rcValue;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_PAM16_AV_MINSK, rcValue, colorAVPam16);

                    double cashAV = salaryAV - infoMonth.CardAV - infoMonth.PrepaymentBankTransaction;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CASH_AV_MINSK, cashAV, colorAVCash);


                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK, salaryFenox, colorFenoxSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CARD_FENOX_MINSK, infoMonth.CardFenox, colorFenoxSalary);

                    double totalOverTimeFenox = 0;
                    rcValue = 0;
                    if (workerSumm != null)
                    {
                        rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "МО-5");
                        if (rc != null)
                        {
                            rcValue = rc.Summ;
                        }
                    }
                    totalOverTimeFenox += rcValue;
                    totalOverTimeMO5Fenox += rcValue;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_MO5_FENOX_MINSK, rcValue, colorFenoxOverTime);

                    rcValue = 0;
                    if (workerSumm != null)
                    {
                        rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "ПАМ-1");
                        if (rc != null)
                        {
                            rcValue = rc.Summ;
                        }
                    }
                    totalOverTimeFenox += rcValue;
                    totalOverTimePAM1Fenox += rcValue;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_PAM1_FENOX_MINSK, rcValue, colorFenoxOverTime);

                    rcValue = 0;
                    if (workerSumm != null)
                    {
                        rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "МО-2");
                        if (rc != null)
                        {
                            rcValue = rc.Summ;
                        }
                    }
                    totalOverTimeFenox += rcValue;
                    totalOverTimeMO2Fenox += rcValue;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_MO2_FENOX_MINSK, rcValue, colorFenoxOverTime);

                    double cashFenox = salaryFenox - infoMonth.CardFenox;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CASH_FENOX_MINSK, cashFenox, colorFenoxCash);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OFFICE_MINSK, null, color26);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_ISSUE_SALARY_MINSK, salaryAV + salaryFenox - infoMonth.CardAV -
                        infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment - infoMonth.CardFenox, colorHeaderDate);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_TOTAL_SALARY_MINSK, infoMonth.CardAV + infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment + totalOverTimeAV + cashAV +
                        infoMonth.CardFenox + totalOverTimeFenox + cashFenox, colorHeaderDate);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK, totalOverTimeAV + cashAV + totalOverTimeFenox + cashFenox, colorHeaderDate);


                    totalCardAV += infoMonth.CardAV;
                    totalPrepaymentBankTransactionAV += infoMonth.PrepaymentBankTransaction;
                    totalCompensationAV += infoMonth.Compensation + infoMonth.VocationPayment;
                    totalCashAV += cashAV;

                    totalCardFenox += infoMonth.CardFenox;
                    totalCashFenox += cashFenox;

                    totalIssueSalary += salaryAV + salaryFenox - infoMonth.CardAV - infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment - infoMonth.CardFenox;
                    totalTotalSalary += infoMonth.CardAV + infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment + totalOverTimeAV + cashAV +
                        infoMonth.CardFenox + totalOverTimeFenox + cashFenox;
                    totalCashPlusOverTimes += totalOverTimeAV + cashAV + totalOverTimeFenox + cashFenox;
                }


                var officeWorkers = bc.GetDirectoryWorkers(year, month, true).ToList();

                foreach (var worker in officeWorkers)
                {
                    indexWorker++;

                    int indexRowWorker = (INDEX_HEADER_ROW_MINSK + COUNT_HEADER_ROW_MINSK - 1) + indexWorker;

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_PP_MINSK, indexWorker, colorHeaderDate);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_FULL_NAME_MINSK, worker.FullName, colorHeaderDate, 11, false, ExcelHorizontalAlignment.Left);

                    var currentWorkerPost = currentMainWorkerPosts.First(p => p.DirectoryWorkerId == worker.Id);
                    string postName = currentWorkerPost.DirectoryPost.Name;
                    if (postName.Contains("_"))
                    {
                        postName = postName.Substring(0, postName.IndexOf("_"));
                    }

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_POST_NAME_MINSK, postName, colorHeaderDate);

                    var infoMonth = bc.GetInfoMonth(worker.Id, year, month);

                    double salaryAV = 0;
                    double salaryFenox = 0;

                    var postSalary = bc.GetDirectoryPostSalaryByDate(currentWorkerPost.DirectoryPost.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

                    if (!currentWorkerPost.IsTwoCompanies)
                    {
                        if (currentWorkerPost.DirectoryPost.DirectoryCompany.Name == "АВ")
                        {
                            salaryAV = postSalary.AdminWorkerSalary.Value;
                        }
                        else
                        {
                            salaryFenox = postSalary.AdminWorkerSalary.Value;
                        }
                    }
                    else
                    {
                        salaryAV = postSalary.AdminWorkerSalary.Value;
                        salaryFenox = postSalary.UserWorkerHalfSalary.Value;
                    }

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_SALARY_AV_MINSK, salaryAV, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CARD_AV_MINSK, infoMonth.CardAV, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_PREPAYMENT_AV_MINSK, 0, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_COMPENSATION_AV_MINSK, infoMonth.Compensation + infoMonth.VocationPayment, colorAVSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_KO5_AV_MINSK, 0, colorAVKO5);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_PAM16_AV_MINSK, 0, colorAVPam16);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CASH_AV_MINSK, 0, colorAVCash);


                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_SALARY_FENOX_MINSK, salaryFenox, colorFenoxSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CARD_FENOX_MINSK, infoMonth.CardFenox, colorFenoxSalary);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_MO5_FENOX_MINSK, 0, colorFenoxOverTime);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_PAM1_FENOX_MINSK, 0, colorFenoxOverTime);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OVERTIME_MO2_FENOX_MINSK, 0, colorFenoxOverTime);
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_CASH_FENOX_MINSK, 0, colorFenoxCash);

                    double officeCash = salaryAV - infoMonth.CardAV + salaryFenox - infoMonth.CardFenox;
                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_OFFICE_MINSK, officeCash, color26);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_ISSUE_SALARY_MINSK, salaryAV + salaryFenox - infoMonth.CardAV - infoMonth.PrepaymentBankTransaction +
                        infoMonth.Compensation + infoMonth.VocationPayment - infoMonth.CardFenox, colorHeaderDate);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_TOTAL_SALARY_MINSK, infoMonth.CardAV + infoMonth.PrepaymentBankTransaction + infoMonth.Compensation +
                         +infoMonth.VocationPayment + infoMonth.CardFenox + officeCash, colorHeaderDate);

                    Helpers.CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK, 0, colorHeaderDate);


                    totalCardAV += infoMonth.CardAV;
                    totalCompensationAV += infoMonth.Compensation + infoMonth.VocationPayment;

                    totalCardFenox += infoMonth.CardFenox;

                    totalOffice += officeCash;
                    totalIssueSalary += salaryAV + salaryFenox - infoMonth.CardAV - infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment - infoMonth.CardFenox;
                    totalTotalSalary += infoMonth.CardAV + infoMonth.PrepaymentBankTransaction + infoMonth.Compensation + infoMonth.VocationPayment + infoMonth.CardFenox + officeCash;
                }

                indexWorker++;

                Helpers.CreateCell(sheet, indexWorker + 5, 1, indexWorker + 1 + 5, 4, "Итого", colorHeaderDate);

                Helpers.CreateCell(sheet, indexWorker + 5, 5, totalCardAV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 6, totalPrepaymentBankTransactionAV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 7, totalCompensationAV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 1 + 5, 5, indexWorker + 1 + 5, 7, (totalCardAV + totalPrepaymentBankTransactionAV + totalCompensationAV), colorAVCash);

                Helpers.CreateCell(sheet, indexWorker + 5, 8, totalOverTimeKO5AV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 9, totalOverTimePAM16AV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 10, totalCashAV, colorAVCash);
                Helpers.CreateCell(sheet, indexWorker + 1 + 5, 8, indexWorker + 1 + 5, 10, (totalOverTimeKO5AV + totalOverTimePAM16AV + totalCashAV), colorAVCash);

                Helpers.CreateCell(sheet, indexWorker + 5, 11, indexWorker + 1 + 5, 11, null, colorFenoxSalary);
                Helpers.CreateCell(sheet, indexWorker + 5, 12, indexWorker + 1 + 5, 12, totalCardFenox, colorFenoxCash);

                Helpers.CreateCell(sheet, indexWorker + 5, 13, totalOverTimeMO5Fenox, colorFenoxCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 14, totalOverTimePAM1Fenox, colorFenoxCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 15, totalOverTimeMO2Fenox, colorFenoxCash);
                Helpers.CreateCell(sheet, indexWorker + 5, 16, totalCashFenox, colorFenoxCash);
                Helpers.CreateCell(sheet, indexWorker + 1 + 5, 13, indexWorker + 1 + 5, 16, (totalOverTimeMO5Fenox + totalOverTimePAM1Fenox + totalOverTimeMO2Fenox + totalCashFenox), colorFenoxCash);

                Helpers.CreateCell(sheet, indexWorker + 5, 17, indexWorker + 1 + 5, 17, totalOffice, color26);
                Helpers.CreateCell(sheet, indexWorker + 5, 18, indexWorker + 1 + 5, 18, totalIssueSalary, colorHeaderDate);
                Helpers.CreateCell(sheet, indexWorker + 5, 19, indexWorker + 1 + 5, 19, totalTotalSalary, colorHeaderDate);
                Helpers.CreateCell(sheet, indexWorker + 5, 20, indexWorker + 1 + 5, 20, totalCashPlusOverTimes, colorHeaderDate);


                sheet.Column(INDEX_HEADER_COLUMN_PP_MINSK).Width = Helpers.PixelsToInches(42);
                sheet.Column(INDEX_HEADER_COLUMN_FULL_NAME_MINSK).Width = Helpers.PixelsToInches(250);
                sheet.Column(INDEX_HEADER_COLUMN_POST_NAME_MINSK).Width = Helpers.PixelsToInches(110);

                for (int i = 4; i <= INDEX_HEADER_COLUMN_TOTAL_CASH_PLUS_OVERTIME_MINSK; i++)
                {
                    sheet.Column(i).Width = Helpers.PixelsToInches(90);
                }

                if (DateTime.Now.Year > year || (DateTime.Now.Year == year && DateTime.Now.Month > month))
                {
                    var date = new DateTime(year, month, 5).AddMonths(1);

                    var costItem = bc.GetDirectoryCostItem("З/п (701)");
                    var rcs = bc.GetDirectoryRCs();
                    var noteSalary = bc.GetDirectoryNote("Зарплата");
                    var noteOverTime = bc.GetDirectoryNote("Переработка");

                    if (totalCashAV != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "ВСЕ"), noteSalary, false, Math.Round(totalCashAV, 2), Currency.RUR, 0);
                    }

                    if (totalOverTimeKO5AV != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "КО-5"), noteOverTime, false, Math.Round(totalOverTimeKO5AV, 2), Currency.RUR, 0);
                    }

                    if (totalOverTimePAM16AV != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "ПАМ-16"), noteOverTime, false, Math.Round(totalOverTimePAM16AV, 2), Currency.RUR, 0);
                    }

                    if (totalCashFenox != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "МО-5"), noteSalary, false, Math.Round(totalCashFenox, 2), Currency.RUR, 0);
                    }

                    if (totalOverTimeMO5Fenox != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "МО-5"), noteOverTime, false, Math.Round(totalOverTimeMO5Fenox, 2), Currency.RUR, 0);
                    }

                    if (totalOverTimePAM1Fenox != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "ПАМ-1"), noteOverTime, false, Math.Round(totalOverTimePAM1Fenox, 2), Currency.RUR, 0);
                    }

                    if (totalOverTimeMO2Fenox != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "МО-2"), noteOverTime, false, Math.Round(totalOverTimeMO2Fenox, 2), Currency.RUR, 0);
                    }

                    if (totalOffice != 0)
                    {
                        bc.EditInfoCost(date, costItem, rcs.First(r => r.Name == "26А"), noteSalary, false, Math.Round(totalOffice, 2), Currency.RUR, 0);
                    }
                }
            }
        }

        private static void SalaryReportWorkers(ExcelPackage ep, int year, int month)
        {
            using (var bc = new BusinessContext())
            {
                var sw = Stopwatch.StartNew();

                int countDaysInMonth = DateTime.DaysInMonth(year, month);
                int countWorkDays = bc.GetCountWorkDaysInMonth(year, month);
                var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);

                var weekendsInMonth = bc.GetHolidays(year, month).ToList();

                var workers = bc.GetDirectoryWorkersWithInfoDatesAndPanalties(year, month, false).ToList();

                foreach (var worker in workers)
                {
                    string name = worker.FullName;
                    var sheet = Helpers.GetSheet(ep, name);

                    sheet.PrinterSettings.PaperSize = ePaperSize.A5;
                    sheet.PrinterSettings.Orientation = eOrientation.Landscape;

                    sheet.Cells.Style.Font.Size = 8;

                    //Header
                    ExcelRange headerName = sheet.Cells[1, 1, 1, countDaysInMonth];
                    headerName.Merge = true;
                    headerName.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    headerName.Value = worker.LastName + " " + worker.FirstName + " " + worker.MidName;

                    ExcelRange headerDate = sheet.Cells[2, 1, 2, countDaysInMonth];
                    headerDate.Merge = true;
                    headerDate.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    headerDate.Value = month + "." + year;

                    ExcelRange headerPost = sheet.Cells[3, 1, 3, countDaysInMonth];
                    headerPost.Merge = true;
                    headerPost.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    headerPost.Value = countWorkDays + " рабочих дня(-ей)";

                    sheet.Cells[1, 1, 3, countDaysInMonth].Style.Border.BorderAround(ExcelBorderStyle.Thick);


                    int count = 0;


                    for (int i = 1; i <= countDaysInMonth; i++)
                    {
                        DateTime workerDay = new DateTime(year, month, i);
                        count++;
                        ExcelRange headerDay = sheet.Cells[5, count];

                        sheet.Column(i).Width = 2.7;

                        if (i > lastDateInMonth.Day)
                        {
                            continue;
                        }

                        if (weekendsInMonth.Any(w => w.Date == workerDay.Date))
                        {
                            headerDay.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerDay.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                        }
                        headerDay.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        headerDay.Value = i;
                    }

                    for (int i = 1; i <= count; i++)
                    {
                        sheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //Body

                    double prevSum = 0;
                    int countMissimgDays = 0;
                    int countVocationDays = 0;
                    int countSickDays = 0;
                    int countPanalty = 0;
                    double workerPanalty = 0;
                    double totalVocations = 0;
                    double totalSickDays = 0;

                    var workerPostReportSalaries = new List<WorkerPostReportSalary>();

                    var infoDates = bc.GetInfoDates(worker.Id, year, month).ToList();

                    foreach (var infoDate in infoDates)
                    {
                        ExcelRange bodyDay = sheet.Cells[6, infoDate.Date.Day];
                        bodyDay.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        var currentWorkerPost = bc.GetCurrentPost(worker.Id, infoDate.Date);
                        var postSalary = bc.GetDirectoryPostSalaryByDate(currentWorkerPost.DirectoryPost.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));
                        double workerSalaryInHour = (double)((postSalary.AdminWorkerSalary) / countWorkDays / 8);

                        var workerPostReportSalary = workerPostReportSalaries.FirstOrDefault(w => w.PostId == currentWorkerPost.Id);

                        if (workerPostReportSalary == null)
                        {
                            workerPostReportSalary = new WorkerPostReportSalary
                            {
                                PostId = currentWorkerPost.Id,
                                PostName = currentWorkerPost.DirectoryPost.Name,
                                AdminWorkerSalary = postSalary.AdminWorkerSalary.Value,
                                ChangePostDay = currentWorkerPost.ChangeDate.Date >= new DateTime(year, month, 1).Date ? currentWorkerPost.ChangeDate.Day : 1
                            };

                            workerPostReportSalaries.Add(workerPostReportSalary);
                        }

                        if (!weekendsInMonth.Any(w => w.Date == infoDate.Date.Date))
                        {
                            workerPostReportSalary.CountWorkDays++;
                        }

                        switch (infoDate.DescriptionDay)
                        {
                            case DescriptionDay.Был:
                                break;
                            case DescriptionDay.Б:
                                if (countSickDays < 5)
                                {
                                    countSickDays++;
                                    totalSickDays += workerSalaryInHour * 8;
                                }
                                else
                                {
                                    countMissimgDays++;
                                }
                                break;
                            case DescriptionDay.О:
                                countVocationDays++;
                                totalVocations += workerSalaryInHour * 8;
                                break;
                            case DescriptionDay.ДО:
                                countVocationDays++;
                                break;
                            case DescriptionDay.П:
                            case DescriptionDay.С:
                                countMissimgDays++;
                                break;
                        }

                        if (infoDate.InfoPanalty != null)
                        {
                            countPanalty++;
                            workerPanalty += infoDate.InfoPanalty.Summ;
                        }

                        if (weekendsInMonth.Any(w => w.Date == infoDate.Date.Date))
                        {
                            if (infoDate.CountHours != null)
                            {
                                bodyDay.Value = infoDate.CountHours;
                            }
                            else
                            {
                                bodyDay.Style.Font.Bold = true;
                                bodyDay.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                bodyDay.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                                bodyDay.Value = "В";
                            }
                        }
                        else
                        {
                            if (infoDate.DescriptionDay == DescriptionDay.Был)
                            {
                                bodyDay.Value = infoDate.CountHours;
                            }
                            else
                            {
                                bodyDay.Value = infoDate.DescriptionDay;
                            }
                        }

                        if (infoDate.CountHours != null)
                        {
                            if (weekendsInMonth.Any(w => w.Date.Date == infoDate.Date.Date))
                            {
                                prevSum += infoDate.CountHours.Value * 2 * workerSalaryInHour;
                                workerPostReportSalary.CountWorkOverTimeHours += infoDate.CountHours.Value;
                            }
                            else
                            {
                                if (infoDate.CountHours > 0 && infoDate.CountHours <= 8)
                                {
                                    prevSum += infoDate.CountHours.Value * workerSalaryInHour;
                                    workerPostReportSalary.CountWorkHours += infoDate.CountHours.Value;
                                }
                                else if (infoDate.CountHours > 8)
                                {
                                    prevSum += 8 * workerSalaryInHour + (infoDate.CountHours.Value - 8) * 2 * workerSalaryInHour;
                                    workerPostReportSalary.CountWorkHours += 8;
                                    workerPostReportSalary.CountWorkOverTimeHours += infoDate.CountHours.Value - 8;
                                }
                            }
                        }
                    }

                    sheet.Cells[5, 1, 6, countDaysInMonth].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                    //downheader

                    ExcelRange downHeaderTypeOfCharge = sheet.Cells[8, 1, 8, 8];
                    downHeaderTypeOfCharge.Merge = true;
                    downHeaderTypeOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderTypeOfCharge.Style.Font.Bold = true;
                    downHeaderTypeOfCharge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderTypeOfCharge.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderTypeOfCharge.Value = "Начисления";

                    ExcelRange downHeaderPeriod = sheet.Cells[8, 9, 8, 10];
                    downHeaderPeriod.Merge = true;
                    downHeaderPeriod.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderPeriod.Style.Font.Bold = true;
                    downHeaderPeriod.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderPeriod.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderPeriod.Value = "Дата";

                    ExcelRange downHeaderDays = sheet.Cells[8, 11, 8, 12];
                    downHeaderDays.Merge = true;
                    downHeaderDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderDays.Style.Font.Bold = true;
                    downHeaderDays.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderDays.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderDays.Value = "Дни";

                    ExcelRange downHeaderHours = sheet.Cells[8, 13, 8, 14];
                    downHeaderHours.Merge = true;
                    downHeaderHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderHours.Style.Font.Bold = true;
                    downHeaderHours.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderHours.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderHours.Value = "Часы";

                    ExcelRange downHeaderSalaryInHour = sheet.Cells[8, 15, 8, 17];
                    downHeaderSalaryInHour.Merge = true;
                    downHeaderSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderSalaryInHour.Style.Font.Bold = true;
                    downHeaderSalaryInHour.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderSalaryInHour.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderSalaryInHour.Value = "Зп/Час";

                    ExcelRange downHeaderSummOfCharge = sheet.Cells[8, 18, 8, 20];
                    downHeaderSummOfCharge.Merge = true;
                    downHeaderSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderSummOfCharge.Style.Font.Bold = true;
                    downHeaderSummOfCharge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderSummOfCharge.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderSummOfCharge.Value = "Сумма";

                    ExcelRange downHeaderTypeOfHolding = sheet.Cells[8, 21, 8, 25];
                    downHeaderTypeOfHolding.Merge = true;
                    downHeaderTypeOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderTypeOfHolding.Style.Font.Bold = true;
                    downHeaderTypeOfHolding.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderTypeOfHolding.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderTypeOfHolding.Value = "Удержания";

                    ExcelRange downHeaderCount = sheet.Cells[8, 26, 8, 27];
                    downHeaderCount.Merge = true;
                    downHeaderCount.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderCount.Style.Font.Bold = true;
                    downHeaderCount.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderCount.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderCount.Value = "К-во";

                    ExcelRange downHeaderSummOfHolding = sheet.Cells[8, 28, 8, countDaysInMonth];
                    downHeaderSummOfHolding.Merge = true;
                    downHeaderSummOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downHeaderSummOfHolding.Style.Font.Bold = true;
                    downHeaderSummOfHolding.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    downHeaderSummOfHolding.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    downHeaderSummOfHolding.Value = "Сумма";

                    ExcelRange downLeftHeaderSalary = sheet.Cells[9, 1, 9, 3];
                    downLeftHeaderSalary.Merge = true;
                    downLeftHeaderSalary.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderSalary.Value = "Оклад";


                    //Salary
                    int countRow = 9;
                    for (int i = 0; i < workerPostReportSalaries.Count - 1; i++)
                    {
                        ExcelRange downLeftBodyPost = sheet.Cells[countRow, 4, countRow, 8];
                        downLeftBodyPost.Merge = true;
                        downLeftBodyPost.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyPost.Value = workerPostReportSalaries[i].PostName;

                        ExcelRange downLeftBodyPeriod = sheet.Cells[countRow, 9, countRow, 10];
                        downLeftBodyPeriod.Merge = true;
                        downLeftBodyPeriod.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyPeriod.Value = workerPostReportSalaries[i].ChangePostDay + "-" + (workerPostReportSalaries[i + 1].ChangePostDay - 1);

                        ExcelRange downLeftBodyDays = sheet.Cells[countRow, 11, countRow, 12];
                        downLeftBodyDays.Merge = true;
                        downLeftBodyDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyDays.Value = workerPostReportSalaries[i].CountWorkDays;

                        ExcelRange downLeftBodyHours = sheet.Cells[countRow, 13, countRow, 14];
                        downLeftBodyHours.Merge = true;
                        downLeftBodyHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyHours.Value = workerPostReportSalaries[i].CountWorkHours;

                        ExcelRange downLeftBodySalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                        downLeftBodySalaryInHour.Merge = true;
                        downLeftBodySalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodySalaryInHour.Value = Math.Round(((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8), 2) + " р.";

                        ExcelRange downLeftBodySummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                        downLeftBodySummOfCharge.Merge = true;
                        downLeftBodySummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodySummOfCharge.Value = Math.Round(((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) * workerPostReportSalaries[i].CountWorkHours, 2) + " р.";

                        countRow++;
                    }

                    ExcelRange downLeftBodyLastPost = sheet.Cells[countRow, 4, countRow, 8];
                    downLeftBodyLastPost.Merge = true;
                    downLeftBodyLastPost.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastPost.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].PostName;

                    ExcelRange downLeftBodyLastPeriod = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyLastPeriod.Merge = true;
                    downLeftBodyLastPeriod.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastPeriod.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].ChangePostDay + "-" + "-" + lastDateInMonth.Day;

                    ExcelRange downLeftBodyLastDays = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkDays;

                    ExcelRange downLeftBodyLastHours = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyLastHours.Merge = true;
                    downLeftBodyLastHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastHours.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkHours;

                    ExcelRange downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodyLastSalaryInHour.Merge = true;
                    downLeftBodyLastSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSalaryInHour.Value = Math.Round(((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8), 2) + " р.";

                    ExcelRange downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodyLastSummOfCharge.Merge = true;
                    downLeftBodyLastSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSummOfCharge.Value = Math.Round(((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) *
                        workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkHours, 2) + " р.";

                    countRow++;

                    if (workerPostReportSalaries.Any(w => w.CountWorkOverTimeHours > 0))
                    {
                        ExcelRange downLeftHeaderOverTime = sheet.Cells[countRow, 1, countRow, 3];
                        downLeftHeaderOverTime.Merge = true;
                        downLeftHeaderOverTime.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftHeaderOverTime.Value = "Переработка";

                        for (int i = 0; i < workerPostReportSalaries.Count - 1; i++)
                        {
                            ExcelRange downLeftBodyPost = sheet.Cells[countRow, 4, countRow, 8];
                            downLeftBodyPost.Merge = true;
                            downLeftBodyPost.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodyPost.Value = workerPostReportSalaries[i].PostName;

                            ExcelRange downLeftBodyPeriod = sheet.Cells[countRow, 9, countRow, 10];
                            downLeftBodyPeriod.Merge = true;
                            downLeftBodyPeriod.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodyPeriod.Value = "—";

                            ExcelRange downLeftBodyDays = sheet.Cells[countRow, 11, countRow, 12];
                            downLeftBodyDays.Merge = true;
                            downLeftBodyDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodyDays.Value = "—";

                            ExcelRange downLeftBodyHours = sheet.Cells[countRow, 13, countRow, 14];
                            downLeftBodyHours.Merge = true;
                            downLeftBodyHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodyHours.Value = workerPostReportSalaries[i].CountWorkOverTimeHours;

                            ExcelRange downLeftBodySalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                            downLeftBodySalaryInHour.Merge = true;
                            downLeftBodySalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodySalaryInHour.Value = Math.Round((((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) * 2), 2) + " р.";

                            ExcelRange downLeftBodySummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                            downLeftBodySummOfCharge.Merge = true;
                            downLeftBodySummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            downLeftBodySummOfCharge.Value = Math.Round((((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) *
                                workerPostReportSalaries[i].CountWorkOverTimeHours) * 2, 2) + " р.";

                            countRow++;
                        }

                        ExcelRange downLeftBodyOvertimeLastPost = sheet.Cells[countRow, 4, countRow, 8];
                        downLeftBodyOvertimeLastPost.Merge = true;
                        downLeftBodyOvertimeLastPost.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastPost.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].PostName;

                        ExcelRange downLeftBodyOvertimeLastPeriod = sheet.Cells[countRow, 9, countRow, 10];
                        downLeftBodyOvertimeLastPeriod.Merge = true;
                        downLeftBodyOvertimeLastPeriod.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastPeriod.Value = "—";

                        ExcelRange downLeftBodyOvertimeLastDays = sheet.Cells[countRow, 11, countRow, 12];
                        downLeftBodyOvertimeLastDays.Merge = true;
                        downLeftBodyOvertimeLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastDays.Value = "—";

                        ExcelRange downLeftBodyOvertimeLastHours = sheet.Cells[countRow, 13, countRow, 14];
                        downLeftBodyOvertimeLastHours.Merge = true;
                        downLeftBodyOvertimeLastHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastHours.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkOverTimeHours;

                        ExcelRange downLeftBodyOvertimeLastSalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                        downLeftBodyOvertimeLastSalaryInHour.Merge = true;
                        downLeftBodyOvertimeLastSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastSalaryInHour.Value = Math.Round((((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) * 2), 2) + " р.";

                        ExcelRange downLeftBodyOvertimeLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                        downLeftBodyOvertimeLastSummOfCharge.Merge = true;
                        downLeftBodyOvertimeLastSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        downLeftBodyOvertimeLastSummOfCharge.Value = Math.Round((((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) *
                                workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkOverTimeHours) * 2, 2) + " р.";

                        countRow++;
                    }

                    var infoMonth = bc.GetInfoMonth(worker.Id, year, month);

                    ExcelRange downLeftHeaderBonus = sheet.Cells[countRow, 1, countRow, 8];
                    downLeftHeaderBonus.Merge = true;
                    downLeftHeaderBonus.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderBonus.Value = "Премия";

                    downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyLastHours.Merge = true;
                    downLeftBodyLastHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastHours.Value = "—";

                    downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyLastSalaryInHour.Merge = true;
                    downLeftBodyLastSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSalaryInHour.Value = "—";

                    downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodyLastSummOfCharge.Merge = true;
                    downLeftBodyLastSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSummOfCharge.Value = infoMonth.Bonus + " р.";

                    countRow++;

                    ExcelRange downLeftHeaderVocation = sheet.Cells[countRow, 1, countRow, 8];
                    downLeftHeaderVocation.Merge = true;
                    downLeftHeaderVocation.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderVocation.Value = "Отпускные";

                    downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyLastHours.Merge = true;
                    downLeftBodyLastHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastHours.Value = countVocationDays;

                    downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyLastSalaryInHour.Merge = true;
                    downLeftBodyLastSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSalaryInHour.Value = countVocationDays * 8;

                    downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodyLastSummOfCharge.Merge = true;
                    downLeftBodyLastSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSummOfCharge.Value = Math.Round(totalVocations, 2) + " р.";

                    countRow++;

                    ExcelRange downLeftHeaderSickDays = sheet.Cells[countRow, 1, countRow, 8];
                    downLeftHeaderSickDays.Merge = true;
                    downLeftHeaderSickDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderSickDays.Value = "Больничные";

                    downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyLastHours.Merge = true;
                    downLeftBodyLastHours.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastHours.Value = countSickDays;

                    downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyLastSalaryInHour.Merge = true;
                    downLeftBodyLastSalaryInHour.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSalaryInHour.Value = countSickDays * 8;

                    downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodyLastDays.Merge = true;
                    downLeftBodyLastDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastDays.Value = "—";

                    downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodyLastSummOfCharge.Merge = true;
                    downLeftBodyLastSummOfCharge.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftBodyLastSummOfCharge.Value = Math.Round(totalSickDays, 2) + " р.";

                    countRow++;

                    ExcelRange downLeftHeaderPanalty = sheet.Cells[9, 21, 9, 25];
                    downLeftHeaderPanalty.Merge = true;
                    downLeftHeaderPanalty.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderPanalty.Value = "Штраф";

                    ExcelRange downLeftHeaderMissDays = sheet.Cells[10, 21, 10, 25];
                    downLeftHeaderMissDays.Merge = true;
                    downLeftHeaderMissDays.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderMissDays.Value = "Прогул";

                    ExcelRange downLeftHeaderBirthDay = sheet.Cells[11, 21, 11, 25];
                    downLeftHeaderBirthDay.Merge = true;
                    downLeftHeaderBirthDay.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderBirthDay.Value = "Дни рождения";

                    ExcelRange downLeftHeaderPrePayment = sheet.Cells[12, 21, 12, 25];
                    downLeftHeaderPrePayment.Merge = true;
                    downLeftHeaderPrePayment.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downLeftHeaderPrePayment.Value = "Аванс";

                    ExcelRange downBodyCount = sheet.Cells[9, 26, 9, 27];
                    downBodyCount.Merge = true;
                    downBodyCount.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodyCount.Value = countPanalty;

                    ExcelRange downBodySummOfHolding = sheet.Cells[9, 28, 9, countDaysInMonth];
                    downBodySummOfHolding.Merge = true;
                    downBodySummOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodySummOfHolding.Value = Math.Round(workerPanalty, 2) + " р.";

                    //downbody//Progul

                    downBodyCount = sheet.Cells[10, 26, 10, 27];
                    downBodyCount.Merge = true;
                    downBodyCount.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodyCount.Value = countMissimgDays;

                    downBodySummOfHolding = sheet.Cells[10, 28, 10, countDaysInMonth];
                    downBodySummOfHolding.Merge = true;
                    downBodySummOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodySummOfHolding.Value = "—";

                    //downbody//Birthday

                    downBodyCount = sheet.Cells[11, 26, 11, 27];
                    downBodyCount.Merge = true;
                    downBodyCount.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodyCount.Value = "—";

                    downBodySummOfHolding = sheet.Cells[11, 28, 11, countDaysInMonth];
                    downBodySummOfHolding.Merge = true;
                    downBodySummOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodySummOfHolding.Value = infoMonth.BirthDays + " р.";

                    //downbody//Prepayment

                    downBodyCount = sheet.Cells[12, 26, 12, 27];
                    downBodyCount.Merge = true;
                    downBodyCount.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodyCount.Value = "—";

                    downBodySummOfHolding = sheet.Cells[12, 28, 12, countDaysInMonth];
                    downBodySummOfHolding.Merge = true;
                    downBodySummOfHolding.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    downBodySummOfHolding.Value = infoMonth.PrepaymentCash + " р.";


                    sheet.Cells[8, 1, countRow - 1, 20].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    sheet.Cells[8, 21, countRow - 1, countDaysInMonth].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    //footerHeader

                    ExcelRange footerHeaderSummOfPayments = sheet.Cells[countRow + 1, 1, countRow + 1, 7];
                    footerHeaderSummOfPayments.Merge = true;
                    footerHeaderSummOfPayments.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerHeaderSummOfPayments.Style.Font.Bold = true;
                    footerHeaderSummOfPayments.Value = "Всего начислено";

                    ExcelRange footerHeaderAllHolds = sheet.Cells[countRow + 1, 12, countRow + 1, 17];
                    footerHeaderAllHolds.Merge = true;
                    footerHeaderAllHolds.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerHeaderAllHolds.Style.Font.Bold = true;
                    footerHeaderAllHolds.Value = "Всего удержано";

                    ExcelRange footerHeaderCleanPayments = sheet.Cells[countRow + 1, 22, countRow + 1, 27];
                    footerHeaderCleanPayments.Merge = true;
                    footerHeaderCleanPayments.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerHeaderCleanPayments.Style.Font.Bold = true;
                    footerHeaderCleanPayments.Value = "Всего выплачено";

                    // footerBody

                    double allSalary = 0;
                    double allOverTimeSalary = 0;
                    foreach (var workerPostReportSalary in workerPostReportSalaries)
                    {
                        allSalary += Math.Round(((double)workerPostReportSalary.AdminWorkerSalary / countWorkDays / 8) * workerPostReportSalary.CountWorkHours, 2);
                        allOverTimeSalary += Math.Round((((double)workerPostReportSalary.AdminWorkerSalary / countWorkDays / 8) * workerPostReportSalary.CountWorkOverTimeHours) * 2, 2);
                    }

                    double summOfPayments = allSalary + allOverTimeSalary + infoMonth.Bonus + totalVocations + totalSickDays;
                    double summOfHoldings = workerPanalty + infoMonth.BirthDays + infoMonth.PrepaymentCash;

                    ExcelRange footerBodySummOfPayments = sheet.Cells[countRow + 1, 8, countRow + 1, 11];
                    footerBodySummOfPayments.Merge = true;
                    footerBodySummOfPayments.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerBodySummOfPayments.Value = Math.Round(summOfPayments, 2) + " р.";

                    ExcelRange footerBodyAllHolds = sheet.Cells[countRow + 1, 18, countRow + 1, 21];
                    footerBodyAllHolds.Merge = true;
                    footerBodyAllHolds.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerBodyAllHolds.Value = Math.Round(summOfHoldings, 2) + " р.";

                    ExcelRange footerBodyCleanPayments = sheet.Cells[countRow + 1, 28, countRow + 1, countDaysInMonth];
                    footerBodyCleanPayments.Merge = true;
                    footerBodyCleanPayments.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    footerBodyCleanPayments.Value = Math.Round(summOfPayments - summOfHoldings, 2) + " р.";

                    countRow++;

                    sheet.Cells[countRow, 1, countRow, countDaysInMonth].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                    countRow += 2;

                    //footerPanalty
                    int enterValueOfCountRow = countRow;
                    bool isDrowed = false;
                    foreach (var infoDate in infoDates)
                    {
                        if (infoDate.InfoPanalty != null)
                        {
                            if (isDrowed == false)
                            {
                                ExcelRange footerPanaltyReportHeader = sheet.Cells[countRow, 1, countRow, countDaysInMonth];
                                footerPanaltyReportHeader.Merge = true;
                                footerPanaltyReportHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                footerPanaltyReportHeader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                footerPanaltyReportHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                footerPanaltyReportHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                footerPanaltyReportHeader.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                                footerPanaltyReportHeader.Style.Font.Bold = true;
                                footerPanaltyReportHeader.Value = "Расшифровка штрафов";
                                countRow++;
                                isDrowed = true;
                            }

                            if (isDrowed == true)
                            {
                                ExcelRange footerPanaltyReportDate = sheet.Cells[countRow, 1, countRow, 3];
                                footerPanaltyReportDate.Merge = true;
                                footerPanaltyReportDate.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                footerPanaltyReportDate.Value = infoDate.Date.ToShortDateString();

                                ExcelRange footerPanaltyReportSumm = sheet.Cells[countRow, 4, countRow, 8];
                                footerPanaltyReportSumm.Merge = true;
                                footerPanaltyReportSumm.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                footerPanaltyReportSumm.Value = infoDate.InfoPanalty.Summ + " р.";

                                ExcelRange footerPanaltyReportDescription = sheet.Cells[countRow, 9, countRow, countDaysInMonth];
                                footerPanaltyReportDescription.Merge = true;
                                footerPanaltyReportDescription.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                footerPanaltyReportDescription.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                footerPanaltyReportDescription.Value = infoDate.InfoPanalty.Description;
                                countRow++;
                            }
                        }
                    }

                    if (enterValueOfCountRow < countRow)
                    {
                        sheet.Cells[enterValueOfCountRow, 1, countRow - 1, countDaysInMonth].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    }

                    ExcelRange nextSideFio = sheet.Cells[33, 3, 34, countDaysInMonth - 3];
                    nextSideFio.Merge = true;
                    nextSideFio.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    nextSideFio.Style.Font.Bold = true;
                    nextSideFio.Style.Font.Size = 17;
                    nextSideFio.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    nextSideFio.Value = worker.LastName + " " + worker.FirstName + " " + worker.MidName;
                }

                Debug.WriteLine("Workers : " + sw.ElapsedMilliseconds);
            }
        }
    }
}

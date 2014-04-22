using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_AV.Models;
using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_Global.Helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers
{
    public static class FormingSalaryReport
    {
        private const string PATH_REPORT_MINSK = "Зарплата\\Minsk\\Зарплата.xlsx";
        private const string PATH_DIRECTORY_REPORT_SALARY_WORKERS = "Зарплата\\Print\\";

        private const int INDEX_HEADER_ROW = 4;
        private const int COUNT_HEADER_ROW = 3;

        private const int INDEX_HEADER_ROW_DATE = 4;
        private const int INDEX_HEADER_ROW_DESCRIPTION = 5;
        private const int INDEX_HEADER_ROW_RCS = 6;

        private const int COUNT_HEADER_ROW_SUMM = 2;

        private const int INDEX_HEADER_COLUMN_FULL_NAME = 1;
        private const int INDEX_HEADER_COLUMN_POST_NAME = 2;

        public static void CompletedReportMinsk(BusinessContextAV bc, int year, int month)
        {
            string path = CreationNewFileReport(PATH_REPORT_MINSK);
            var ep = CreationNewBook(path);

            SalaryReportMinsk(ep, bc, year, month);
            OverTimeReportMinsk(ep, bc, year, month);

            ep.Save();
            Process.Start(path);
        }

        public static void ComplitedReportSalaryWorkers(BusinessContextAV bc, int year, int month)
        {
            string path = CreationNewFileReport(Path.Combine(PATH_DIRECTORY_REPORT_SALARY_WORKERS, month + "." + year + ".xlsx"));
            var ep = CreationNewBook(path);

            SalaryReportWorkers(ep, bc, year, month);

            ep.Save();
            Process.Start(path);
        }

        private static string CreationNewFileReport(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            string newPath = path;
            int indexExcelNewFile = 0;
            if (File.Exists(PATH_REPORT_MINSK))
            {
                try
                {
                    File.Delete(PATH_REPORT_MINSK);
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

        private static ExcelPackage CreationNewBook(string path)
        {
            return new ExcelPackage(new FileInfo(path));
        }

        private static void CreateCell(ExcelWorksheet sheet, int fromRow, int fromColumn, int toRow, int toColumn, string value,
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
            cell.Value = value;
        }

        private static void CreateCell(ExcelWorksheet sheet, int row, int column, string value, float size = 11, bool isFontBold = false,
            OfficeOpenXml.Style.ExcelHorizontalAlignment alignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
            OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Medium)
        {
            CreateCell(sheet, row, column, row, column, value, size, isFontBold, alignment, borderStyle);
        }

        private static void OverTimeReportMinsk(ExcelPackage ep, BusinessContextAV bc, int year, int month)
        {
            string name = "Переработка";
            if (ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(name))
            {
                ep.Workbook.Worksheets.Delete(name);
            }
            ep.Workbook.Worksheets.Add(name);

            ExcelWorksheet sheet = ep.Workbook.Worksheets.First(ws => ws.Name == name);

            CreateCell(sheet, INDEX_HEADER_ROW, INDEX_HEADER_COLUMN_FULL_NAME, INDEX_HEADER_ROW + COUNT_HEADER_ROW - 1, INDEX_HEADER_COLUMN_FULL_NAME, "ФИО");
            CreateCell(sheet, INDEX_HEADER_ROW, INDEX_HEADER_COLUMN_POST_NAME, INDEX_HEADER_ROW + COUNT_HEADER_ROW - 1, INDEX_HEADER_COLUMN_POST_NAME, "Должность");

            var workers = bc.GetDirectoryWorkers(year, month);

            var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);
            int indexRowWorker = INDEX_HEADER_ROW + COUNT_HEADER_ROW;

            foreach (var worker in workers)
            {
                CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_FULL_NAME, worker.FullName, 11, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left);

                string postName = bc.GetCurrentPost(worker.Id, lastDateInMonth).DirectoryPost.Name;
                CreateCell(sheet, indexRowWorker, INDEX_HEADER_COLUMN_POST_NAME, postName);

                indexRowWorker++;
            }

            var weekEndsInMonth = bc.GetWeekendsInMonth(year, month).ToList();

            var overTimes = bc.GetInfoOverTimes(year, month).ToList();
            var workerSumms = new List<WorkerSummForReport>();

            int countWorkDayInMonth = bc.GetCountWorkDaysInMonth(year, month);

            var maxRCs = new List<DirectoryRC>();
            int indexColumnOverTime = INDEX_HEADER_COLUMN_POST_NAME + 1;
            foreach (var overTime in overTimes)
            {
                int countRCs = overTime.CurrentRCs.ToList().Count();

                CreateCell(sheet, INDEX_HEADER_ROW_DATE, indexColumnOverTime, INDEX_HEADER_ROW_DATE, indexColumnOverTime + countRCs - 1, overTime.StartDate.ToShortDateString());
                CreateCell(sheet, INDEX_HEADER_ROW_DESCRIPTION, indexColumnOverTime, INDEX_HEADER_ROW_DESCRIPTION, indexColumnOverTime + countRCs - 1, overTime.Description);


                var currentRCs = overTime.CurrentRCs.ToList();
                int currentPercentage = currentRCs.Sum(r => r.DirectoryRC.Percentes);
                for (int i = 0; i < countRCs; i++)
                {
                    if (!maxRCs.Select(r => r.Name).Contains(currentRCs[i].DirectoryRC.Name))
                    {
                        maxRCs.Add(currentRCs[i].DirectoryRC);
                    }

                    CreateCell(sheet, INDEX_HEADER_ROW_RCS, indexColumnOverTime + i, currentRCs[i].DirectoryRC.Name);

                    if (countRCs < 3)
                    {
                        sheet.Column(indexColumnOverTime + i).Width = PixelsToInches(192 / countRCs);
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
                        string valueRC = null;
                        var infoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == overTime.StartDate.Date);
                        if (infoDate != null)
                        {
                            var overTimeHours = bc.IsOverTime(infoDate, weekEndsInMonth);
                            if (overTimeHours != null)
                            {
                                double percentage = Math.Round(overTimeHours.Value * 1.3 * currentRCs[i].DirectoryRC.Percentes / currentPercentage, 1);
                                valueRC = percentage.ToString();

                                var workerRCSummForReport = workerSummForReport.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == currentRCs[i].DirectoryRC.Name);
                                if (workerRCSummForReport == null)
                                {
                                    workerRCSummForReport = new WorkerRCSummForReport { RCName = currentRCs[i].DirectoryRC.Name };
                                    workerSummForReport.WorkerRCSummForReports.Add(workerRCSummForReport);
                                }

                                double salaryInHour = bc.GetCurrentPost(worker.Id, infoDate.Date).DirectoryPost.UserWorkerSalary / 8 / countWorkDayInMonth;
                                workerRCSummForReport.Summ += percentage * 2 * salaryInHour;
                            }
                        }

                        CreateCell(sheet, INDEX_HEADER_ROW_RCS + indexRowWorkerRC, indexColumnOverTime + i, valueRC);
                    }
                }

                indexColumnOverTime += countRCs;
            }

            CreateCell(sheet, INDEX_HEADER_ROW, indexColumnOverTime, INDEX_HEADER_ROW + COUNT_HEADER_ROW_SUMM - 1, indexColumnOverTime + maxRCs.Count - 1, "Итого");

            int indexCurrentRC = indexColumnOverTime;
            foreach (var rc in maxRCs.OrderByDescending(r => r.Percentes))
            {
                CreateCell(sheet, INDEX_HEADER_ROW_RCS, indexCurrentRC, rc.Name);

                int indexRowWorkerSum = 0;
                foreach (var workerSumm in workerSumms)
                {
                    indexRowWorkerSum++;

                    string valueSumm = null;
                    if (workerSumm.WorkerRCSummForReports.Select(w => w.RCName).Contains(rc.Name))
                    {
                        valueSumm = workerSumm.WorkerRCSummForReports.First(w => w.RCName == rc.Name).Summ.ToString();
                    }


                    CreateCell(sheet, INDEX_HEADER_ROW_RCS + indexRowWorkerSum, indexCurrentRC, valueSumm);
                }


                indexCurrentRC++;
            }

            CreateCell(sheet, 1, 1, INDEX_HEADER_ROW - 1, indexCurrentRC - 1, "Переработка", 26, true);

            sheet.Column(1).Width = PixelsToInches(250);
            sheet.Column(2).Width = PixelsToInches(100);

            sheet.Row(INDEX_HEADER_ROW_DESCRIPTION).Height = 90;
        }

        private static double PixelsToInches(double pixels)
        {
            return (pixels - 7) / 7d + 1;
        }

        private static void SalaryReportMinsk(ExcelPackage ep, BusinessContextAV bc, int year, int month)
        {
            string stringDate = month.ToString() + "'" + year.ToString();

            if (ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(stringDate))
            {
                ep.Workbook.Worksheets.Delete(stringDate);
            }
            ep.Workbook.Worksheets.Add(stringDate);

            ExcelWorksheet sheet = ep.Workbook.Worksheets.First(ws => ws.Name == stringDate);

            sheet.Cells[1, 1, 4, 16].Style.Font.Bold = true;
            sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            sheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            ExcelRange headerAV = sheet.Cells[3, 4, 3, 9];
            headerAV.Merge = true;
            headerAV.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerAV.Style.Fill.BackgroundColor.SetColor(Color.Black);
            headerAV.Style.Font.Color.SetColor(Color.Red);
            headerAV.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.White);
            headerAV.Value = "АВ-Автотехник";

            ExcelRange headerFenox = sheet.Cells[3, 10, 3, 13];
            headerFenox.Merge = true;
            headerFenox.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerFenox.Style.Fill.BackgroundColor.SetColor(Color.Black);
            headerFenox.Style.Font.Color.SetColor(Color.Red);
            headerFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.White);
            headerFenox.Value = "Фенокс Автомотив Рус";

            ExcelRange headerDate = sheet.Cells[1, 1, 2, 16];
            headerDate.Merge = true;
            headerDate.Style.Font.Size = 25;
            headerDate.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerDate.Value = "З/п за " + month + " " + year + " г.";

            ExcelRange headerNumber = sheet.Cells[3, 1, 4, 1];
            headerNumber.Merge = true;
            headerNumber.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerNumber.Value = "№ пп";

            ExcelRange headerFIO = sheet.Cells[3, 2, 4, 2];
            headerFIO.Merge = true;
            headerFIO.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            headerFIO.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerFIO.Value = "ФИО";

            ExcelRange headerPost = sheet.Cells[3, 3, 4, 3];
            headerPost.Merge = true;
            headerPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerPost.Value = "Должность";

            ExcelRange header26 = sheet.Cells[3, 14, 4, 14];
            header26.Merge = true;
            header26.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            header26.Value = "26";

            ExcelRange headerTotalPayment = sheet.Cells[3, 15, 4, 15];
            headerTotalPayment.Merge = true;
            headerTotalPayment.Style.WrapText = true;
            headerTotalPayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerTotalPayment.Value = "Всего к выдаче";

            ExcelRange headerTotalSalary = sheet.Cells[3, 16, 4, 16];
            headerTotalSalary.Merge = true;
            headerTotalSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerTotalSalary.Value = "Всего з/п";

            ExcelRange headerSalaryAV = sheet.Cells[4, 4];
            headerSalaryAV.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerSalaryAV.Value = "Оклад";

            ExcelRange headerCardAV = sheet.Cells[4, 5];
            headerCardAV.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerCardAV.Value = "Карточка";

            ExcelRange headerPrepayment = sheet.Cells[4, 6];
            headerPrepayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerPrepayment.Value = "Аванс";

            ExcelRange headerCompensation = sheet.Cells[4, 7];
            headerCompensation.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerCompensation.Value = "Компенсация";

            ExcelRange headerDoublePayTimeAV = sheet.Cells[4, 8];
            headerDoublePayTimeAV.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerDoublePayTimeAV.Value = "Переработка";

            ExcelRange headerCashAV = sheet.Cells[4, 9];
            headerCashAV.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerCashAV.Value = "Касса";

            ExcelRange headerSalaryFenox = sheet.Cells[4, 10];
            headerSalaryFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerSalaryFenox.Value = "Оклад";

            ExcelRange headerCardFenox = sheet.Cells[4, 11];
            headerCardFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerCardFenox.Value = "Карточка";

            ExcelRange headerDoublePayTimeFenox = sheet.Cells[4, 12];
            headerDoublePayTimeFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerDoublePayTimeFenox.Value = "Переработка";

            ExcelRange headerCashFenox = sheet.Cells[4, 13];
            headerCashFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            headerCashFenox.Value = "Касса";

            // Body

            double totalSalaryAVCard = 0;
            double totalAVPrepayment = 0;
            double totalAVCompensation = 0;
            double totalOverTimeAV = 0;
            double totalSalaryFenoxCard = 0;
            double totalFenoxOverTime = 0;
            double totalAVB = 0;
            double totalAVCash = 0;
            double totalAVN = 0;
            double totalFenoxCash = 0;
            double totalFenoxN = 0;
            double total26 = 0;
            double totalPayment = 0;
            double totalSalary = 0;

            var workers = bc.GetDirectoryWorkers(year, month).ToList();

            int count = 0;

            foreach (var worker in workers)
            {
                var lastDateInMonth = DateTime.Now.Year == year && DateTime.Now.Month == month ? DateTime.Now : new DateTime(year, month, DateTime.DaysInMonth(year, month));
                count++;

                for (int j = 1; j < 4; j++)
                {
                    ExcelRange bodyNumberNamePost = sheet.Cells[count + 4, j];
                    bodyNumberNamePost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                }

                var currentPost = bc.GetCurrentPost(worker.Id, lastDateInMonth);

                sheet.Cells[count + 4, 1].Value = count;
                sheet.Cells[count + 4, 2].Value = worker.LastName + " " + worker.FirstName;
                sheet.Cells[count + 4, 3].Value = currentPost.DirectoryPost.Name;

                ExcelRange bodyAVSalary = sheet.Cells[count + 4, 4, count + 4, 4];
                bodyAVSalary.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVSalary.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(83, 141, 213));
                bodyAVSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyAVCard = sheet.Cells[count + 4, 5, count + 4, 5];
                bodyAVCard.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVCard.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
                bodyAVCard.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyAVPrepayment = sheet.Cells[count + 4, 6, count + 4, 6];
                bodyAVPrepayment.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVPrepayment.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
                bodyAVPrepayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyAVCompensation = sheet.Cells[count + 4, 7, count + 4, 7];
                bodyAVCompensation.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVCompensation.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
                bodyAVCompensation.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyAVOverTime = sheet.Cells[count + 4, 8, count + 4, 8];
                bodyAVOverTime.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVOverTime.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
                bodyAVOverTime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyAVCash = sheet.Cells[count + 4, 9, count + 4, 9];
                bodyAVCash.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyAVCash.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(149, 179, 215));
                bodyAVCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyFenoxSalary = sheet.Cells[count + 4, 10, count + 4, 10];
                bodyFenoxSalary.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyFenoxSalary.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 54, 52));
                bodyFenoxSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyFenoxCard = sheet.Cells[count + 4, 11, count + 4, 11];
                bodyFenoxCard.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyFenoxCard.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 184, 183));
                bodyFenoxCard.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyFenoxOverTime = sheet.Cells[count + 4, 12, count + 4, 12];
                bodyFenoxOverTime.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyFenoxOverTime.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 184, 183));
                bodyFenoxOverTime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyFenoxCash = sheet.Cells[count + 4, 13, count + 4, 13];
                bodyFenoxCash.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                bodyFenoxCash.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(218, 150, 148));
                bodyFenoxCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange body26 = sheet.Cells[count + 4, 14, count + 4, 14];
                body26.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                body26.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                body26.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ExcelRange bodyTotalPayment = sheet.Cells[count + 4, 15, count + 4, 15];
                bodyTotalPayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);


                ExcelRange bodyTotalSalary = sheet.Cells[count + 4, 16, count + 4, 16];
                bodyTotalSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                var infoMonth = bc.GetInfoMonth(worker.Id, year, month);
                bodyAVCard.Value = infoMonth.SalaryAV;
                bodyAVPrepayment.Value = infoMonth.PrepaymentBankTransaction;
                bodyAVCompensation.Value = 0;
                bodyFenoxCard.Value = infoMonth.SalaryFenox;

                int countOfWorkDays = bc.GetCountWorkDaysInMonth(year, month);

                var infoDates = bc.GetInfoDates(worker.Id, year, month);

                var weekends = bc.GetWeekendsInMonth(year, month).ToList();

                double totalOverTime = 0;
                foreach (var infoDate in infoDates)
                {
                    var currentPostOfInfoDate = bc.GetCurrentPost(worker.Id, infoDate.Date);
                    double currentSalaryInHour = Math.Round(((double)currentPostOfInfoDate.DirectoryPost.UserWorkerSalary / countOfWorkDays / 8), 2);

                    double overTime = 0;
                    if (weekends.Any(w => w.Date == infoDate.Date))
                    {
                        overTime += infoDate.CountHours != null ? infoDate.CountHours.Value : 0;
                    }
                    else
                    {
                        overTime += infoDate.CountHours != null && infoDate.CountHours > 8 ? infoDate.CountHours.Value - 8 : 0;
                    }

                    totalOverTime += overTime * currentSalaryInHour * 2;
                }

                double overTimeAv = 0;
                double overTimeFenox = 0;
                double cashAv = 0;
                double cashFenox = 0;
                double workerTotalPayment = 0;
                double workerTotalSalary = 0;

                //if (worker.Companies.Count(c => c.FireCompanyDate == null) == 1)
                //{
                //    string workerCompanyName = worker.Companies.First(c => c.FireCompanyDate == null).CompanyName;
                //    if (workerCompanyName == "AV")
                //    {
                //        bodyAVSalary.Value = currentPost.AdminWorkerSalary;
                //        overTimeAv = totalOverTime;
                //        cashAv = currentPost.AdminWorkerSalary - salary.SalaryAVBr - salary.PrePaymentBr;
                //    }
                //    else
                //    {
                //        bodyFenoxSalary.Value = currentPost.AdminWorkerSalary;
                //        overTimeFenox = totalOverTime;
                //        cashFenox = currentPost.AdminWorkerSalary - salary.SalaryFenoxBr;
                //    }
                //}
                //else
                //{
                //    bodyAVSalary.Value = currentPost.AdminWorkerSalary - currentPost.AdminHalfWorkerSalary;

                //    bodyFenoxSalary.Value = currentPost.AdminHalfWorkerSalary;
                //    cashAv = currentPost.AdminWorkerSalary - currentPost.AdminHalfWorkerSalary - salary.SalaryAVBr - salary.PrePaymentBr;
                //    cashFenox = currentPost.AdminHalfWorkerSalary - salary.SalaryFenoxBr;
                //}

                overTimeAv = Math.Round((totalOverTime * 0.4), 2);
                overTimeFenox = Math.Round((totalOverTime * 0.6), 2);
                workerTotalPayment = cashAv + cashFenox;
                workerTotalSalary = infoMonth.SalaryAV + infoMonth.PrepaymentBankTransaction + 0 + cashAv + infoMonth.SalaryFenox + cashFenox;

                bodyAVOverTime.Value = overTimeAv;
                bodyFenoxOverTime.Value = overTimeFenox;
                bodyAVCash.Value = cashAv;
                bodyFenoxCash.Value = cashFenox;
                bodyTotalPayment.Value = workerTotalPayment;
                bodyTotalSalary.Value = workerTotalSalary;

                if (overTimeAv == 0 && infoMonth.SalaryAV == 0 && infoMonth.PrepaymentBankTransaction == 0 && 0 == 0 && cashAv == 0)
                {
                    bodyAVSalary.Value = "---->";
                    bodyAVCard.Value = "---->";
                    bodyAVOverTime.Value = "---->";
                    bodyAVCompensation.Value = "---->";
                    bodyAVPrepayment.Value = "---->";
                    bodyAVCash.Value = "---->";
                }

                if (overTimeFenox == 0 && infoMonth.SalaryFenox == 0 && cashFenox == 0)
                {
                    bodyFenoxSalary.Value = "<----";
                    bodyFenoxCard.Value = "<----";
                    bodyFenoxOverTime.Value = "<----";
                    bodyFenoxCash.Value = "<----";
                }

                totalSalaryAVCard += infoMonth.SalaryAV;
                totalAVPrepayment += infoMonth.PrepaymentBankTransaction;
                totalAVCompensation += 0;
                totalOverTimeAV += overTimeAv;
                totalAVCash += cashAv;
                totalSalaryFenoxCard += infoMonth.SalaryFenox;
                totalFenoxCash += cashFenox;
                totalFenoxOverTime += overTimeFenox;
                totalPayment += cashAv + cashFenox;
                totalSalary += infoMonth.SalaryAV + infoMonth.PrepaymentBankTransaction + 0 + cashAv + infoMonth.SalaryFenox + cashFenox;
            }

            //double cash26 = 0;
            //double workerTotalPayment26 = 0;
            //double workerTotalSalary26 = 0;

            //foreach (var worker26 in workers26)
            //{
            //    DateTime lastDayInMonthDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            //    count++;

            //    for (int j = 1; j < 4; j++)
            //    {
            //        ExcelRange bodyNumberNamePost = sheet.Cells[count + 4, j];
            //        bodyNumberNamePost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            //    }

            //    PostOffice workerPost26 = worker26.PostOffice;

            //    sheet.Cells[count + 4, 1].Value = count;
            //    sheet.Cells[count + 4, 2].Value = worker26.LastName + " " + worker26.FirstName;
            //    sheet.Cells[count + 4, 3].Value = workerPost26.PostName;

            //    ExcelRange bodyAVSalary = sheet.Cells[count + 4, 4, count + 4, 4];
            //    bodyAVSalary.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVSalary.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(83, 141, 213));
            //    bodyAVSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyAVCard = sheet.Cells[count + 4, 5, count + 4, 5];
            //    bodyAVCard.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVCard.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
            //    bodyAVCard.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyAVPrepayment = sheet.Cells[count + 4, 6, count + 4, 6];
            //    bodyAVPrepayment.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVPrepayment.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
            //    bodyAVPrepayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyAVCompensation = sheet.Cells[count + 4, 7, count + 4, 7];
            //    bodyAVCompensation.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVCompensation.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
            //    bodyAVCompensation.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyAVOverTime = sheet.Cells[count + 4, 8, count + 4, 8];
            //    bodyAVOverTime.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVOverTime.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(197, 217, 241));
            //    bodyAVOverTime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyAVCash = sheet.Cells[count + 4, 9, count + 4, 9];
            //    bodyAVCash.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyAVCash.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(149, 179, 215));
            //    bodyAVCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyFenoxSalary = sheet.Cells[count + 4, 10, count + 4, 10];
            //    bodyFenoxSalary.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyFenoxSalary.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(150, 54, 52));
            //    bodyFenoxSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyFenoxCard = sheet.Cells[count + 4, 11, count + 4, 11];
            //    bodyFenoxCard.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyFenoxCard.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 184, 183));
            //    bodyFenoxCard.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyFenoxOverTime = sheet.Cells[count + 4, 12, count + 4, 12];
            //    bodyFenoxOverTime.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyFenoxOverTime.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 184, 183));
            //    bodyFenoxOverTime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyFenoxCash = sheet.Cells[count + 4, 13, count + 4, 13];
            //    bodyFenoxCash.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    bodyFenoxCash.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(218, 150, 148));
            //    bodyFenoxCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange body26 = sheet.Cells[count + 4, 14, count + 4, 14];
            //    body26.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    body26.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            //    body26.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    ExcelRange bodyTotalPayment = sheet.Cells[count + 4, 15, count + 4, 15];
            //    bodyTotalPayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);


            //    ExcelRange bodyTotalSalary = sheet.Cells[count + 4, 16, count + 4, 16];
            //    bodyTotalSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            //    MonthOffice workerMonth26 = worker26.YearsOffice.First(y => y.YearValue == year).MonthesOffice.First(m => m.MonthValue == month);
            //    SalaryOffice salary26 = workerMonth26.SalaryOffice;
            //    bodyAVCard.Value = salary26.SalaryAVBr;
            //    bodyAVPrepayment.Value = salary26.PrePaymentBr;
            //    bodyAVCompensation.Value = salary26.CompensationAVBr;
            //    bodyFenoxCard.Value = salary26.SalaryFenoxBr;

            //    cash26 = (salary26.SalaryAV - salary26.SalaryAVBr - salary26.PrePaymentBr - salary26.CompensationAVBr) + (salary26.SalaryFenox - salary26.SalaryFenoxBr);
            //    workerTotalPayment26 = cash26;
            //    workerTotalSalary26 = salary26.SalaryAVBr + salary26.PrePaymentBr + salary26.CompensationAVBr + salary26.SalaryFenoxBr + cash26;

            //    int countOfWorkDays = Helper.GetCountWorkDayInMonth(year, month);

            //    if (worker26.Companies.Count(c => c.FireCompanyDate == null) == 1)
            //    {
            //        string workerCompanyName = worker26.Companies.First(c => c.FireCompanyDate == null).CompanyName;
            //        if (workerCompanyName == "AV")
            //        {
            //            bodyAVSalary.Value = salary26.SalaryAV;
            //        }
            //        else
            //        {
            //            bodyFenoxSalary.Value = salary26.SalaryFenox;
            //        }
            //    }
            //    else
            //    {
            //        bodyAVSalary.Value = salary26.SalaryAV;
            //        bodyFenoxSalary.Value = salary26.SalaryFenox;
            //    }

            //    bodyTotalPayment.Value = workerTotalPayment26;
            //    bodyTotalSalary.Value = workerTotalSalary26;
            //    body26.Value = cash26;

            //    totalSalaryAVCard += salary26.SalaryAVBr;
            //    totalAVPrepayment += salary26.PrePaymentBr;
            //    totalAVCompensation += salary26.CompensationAVBr;
            //    totalSalaryFenoxCard += salary26.SalaryFenoxBr;
            //    total26 += cash26;
            //    totalPayment += workerTotalPayment26;
            //    totalSalary += workerTotalSalary26;

            //    if (salary26.SalaryAV == 0)
            //    {
            //        bodyAVSalary.Value = "---->";
            //        bodyAVCard.Value = "---->";
            //        bodyAVOverTime.Value = "---->";
            //        bodyAVCompensation.Value = "---->";
            //        bodyAVPrepayment.Value = "---->";
            //        bodyAVCash.Value = "---->";
            //    }
            //    else if (salary26.SalaryFenox == 0)
            //    {
            //        bodyFenoxSalary.Value = "---->";
            //        bodyFenoxCard.Value = "---->";
            //        bodyFenoxOverTime.Value = "---->";
            //        bodyFenoxCash.Value = "---->";
            //    }
            //    bodyAVCash.Value = "---->";
            //    bodyAVOverTime.Value = "---->";
            //    bodyFenoxOverTime.Value = "---->";
            //    bodyFenoxCash.Value = "---->";
            //}

            totalAVB = Math.Round(totalSalaryAVCard + totalAVCompensation + totalAVPrepayment);
            totalAVN = Math.Round(totalAVCash + totalOverTimeAV);
            totalFenoxN = Math.Round(totalFenoxCash + totalFenoxOverTime);

            // Footer

            ExcelRange footerTotal = sheet.Cells[count + 5, 1, count + 6, 4];
            footerTotal.Merge = true;
            footerTotal.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotal.Value = "Итого";

            ExcelRange footerTotalAVB = sheet.Cells[count + 6, 5, count + 6, 7];
            footerTotalAVB.Merge = true;
            footerTotalAVB.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalAVB.Value = totalAVB;

            ExcelRange footerTotalAVCash = sheet.Cells[count + 5, 9, count + 5, 9];
            footerTotalAVCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalAVCash.Value = totalAVCash;

            ExcelRange footerTotalAVN = sheet.Cells[count + 6, 8, count + 6, 9];
            footerTotalAVN.Merge = true;
            footerTotalAVN.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalAVN.Value = totalAVN;

            ExcelRange footerTotalFenoxB = sheet.Cells[count + 5, 11, count + 6, 11];
            footerTotalFenoxB.Merge = true;
            footerTotalFenoxB.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalFenoxB.Value = totalSalaryFenoxCard;

            ExcelRange footerTotalFenoxCash = sheet.Cells[count + 5, 13, count + 5, 13];
            footerTotalFenoxCash.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalFenoxCash.Value = totalFenoxCash;

            ExcelRange footerTotalFenoxN = sheet.Cells[count + 6, 12, count + 6, 13];
            footerTotalFenoxN.Merge = true;
            footerTotalFenoxN.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalFenoxN.Value = totalFenoxN;

            ExcelRange footerTotal26 = sheet.Cells[count + 5, 14, count + 6, 14];
            footerTotal26.Merge = true;
            footerTotal26.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotal26.Value = total26;

            ExcelRange footerTotalPayment = sheet.Cells[count + 5, 15, count + 6, 15];
            footerTotalPayment.Merge = true;
            footerTotalPayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalPayment.Value = totalPayment;

            ExcelRange footerTotalSalary = sheet.Cells[count + 5, 16, count + 6, 16];
            footerTotalSalary.Merge = true;
            footerTotalSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerTotalSalary.Value = totalSalary;

            ExcelRange footerSalaryAVCard = sheet.Cells[count + 5, 5];
            footerSalaryAVCard.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerSalaryAVCard.Value = totalSalaryAVCard;

            ExcelRange footerSalaryAVPrepayment = sheet.Cells[count + 5, 6];
            footerSalaryAVPrepayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerSalaryAVPrepayment.Value = totalAVPrepayment;

            ExcelRange footerSalaryAVCompensation = sheet.Cells[count + 5, 7];
            footerSalaryAVCompensation.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerSalaryAVCompensation.Value = totalAVCompensation;

            ExcelRange footerSalaryAVOvertime = sheet.Cells[count + 5, 8];
            footerSalaryAVOvertime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerSalaryAVOvertime.Value = totalOverTimeAV;

            ExcelRange footerSalaryFenoxOvertime = sheet.Cells[count + 5, 12];
            footerSalaryFenoxOvertime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            footerSalaryFenoxOvertime.Value = totalFenoxOverTime;

            ExcelRange footerSalaryFenox = sheet.Cells[count + 5, 10, count + 6, 10];
            footerSalaryFenox.Merge = true;
            footerSalaryFenox.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

            for (int i = 1; i < 17; i++)
            {
                sheet.Column(i).AutoFit();
            }
        }

        private static void SalaryReportWorkers(ExcelPackage ep, BusinessContextAV bc, int year, int month)
        {
            int countDaysInMonth = DateTime.DaysInMonth(year, month);
            int countWorkDays = bc.GetCountWorkDaysInMonth(year, month);
            var lastDayInMonth = HelperMethods.GetLastDateInMonth(year, month);

            var weekendsInMonth = bc.GetWeekendsInMonth(year, month).ToList();

            var workers = bc.GetDirectoryWorkers(year, month);

            foreach (var worker in workers)
            {
                if (ep.Workbook.Worksheets.Select(ws => ws.Name).Contains(worker.FullName))
                {
                    ep.Workbook.Worksheets.Delete(worker.FullName);
                }
                ep.Workbook.Worksheets.Add(worker.FullName);

                ExcelWorksheet sheet = ep.Workbook.Worksheets.First(ws => ws.Name == worker.FullName);
                sheet.Cells.Style.Font.Size = 8;

                var currentPost = bc.GetCurrentPost(worker.Id, lastDayInMonth);
                double workerSalaryInHour = Math.Round(((double)(currentPost.DirectoryPost.UserWorkerSalary) / countWorkDays / 8), 2);

                //Header
                ExcelRange headerName = sheet.Cells[1, 1, 1, countDaysInMonth];
                headerName.Merge = true;
                headerName.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                headerName.Value = worker.LastName + " " + worker.FirstName + " " + worker.MidName;

                ExcelRange headerDate = sheet.Cells[2, 1, 2, countDaysInMonth];
                headerDate.Merge = true;
                headerDate.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                headerDate.Value = month + "." + year;

                ExcelRange headerPost = sheet.Cells[3, 1, 3, countDaysInMonth];
                headerPost.Merge = true;
                headerPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                headerPost.Value = countWorkDays + " рабочих дня(-ей)";

                sheet.Cells[1, 1, 3, countDaysInMonth].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);


                int count = 0;


                for (int i = 1; i <= countDaysInMonth; i++)
                {
                    DateTime workerDay = new DateTime(year, month, i);
                    count++;
                    ExcelRange headerDay = sheet.Cells[5, count];

                    if (weekendsInMonth.Any(w => w.Date == workerDay.Date))
                    {
                        headerDay.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        headerDay.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                    }
                    headerDay.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    headerDay.Value = i;
                    sheet.Column(i).Width = 2.7;
                }

                for (int i = 1; i <= count; i++)
                {
                    sheet.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                //Body

                double prevSum = 0;
                int countMissimgDays = 0;
                int countVocationDays = 0;
                int countSickDays = 0;
                int countPanalty = 0;
                double workerPanalty = 0;

                var workerPostReportSalaries = new List<WorkerPostReportSalary>();
                //List<Post> workerPosts = new List<Post>();
                //List<int> startDays = new List<int>();
                //List<int> countOfWorkDaysInPeriod = new List<int>();
                //List<double> countOfWorkHoursInPeriod = new List<double>();
                //List<double> countOfWorkOvertimeHoursInPeriod = new List<double>();

                var infoDates = bc.GetInfoDates(worker.Id, year, month).ToList();

                foreach (var infoDate in infoDates)
                {
                    ExcelRange bodyDay = sheet.Cells[6, infoDate.Date.Day];
                    bodyDay.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    var currentWorkerPost = bc.GetCurrentPost(worker.Id, infoDate.Date);

                    var workerPostReportSalary = workerPostReportSalaries.FirstOrDefault(w => w.PostId == currentPost.Id);

                    if (workerPostReportSalary == null)
                    {
                        workerPostReportSalary = new WorkerPostReportSalary
                        {
                            PostId = currentPost.Id,
                            PostName = currentPost.DirectoryPost.Name,
                            AdminWorkerSalary = currentPost.DirectoryPost.UserWorkerSalary,
                            ChangePostDay = currentPost.ChangeDate.Day,
                        };

                        workerPostReportSalaries.Add(workerPostReportSalary);
                    }

                    if (!weekendsInMonth.Any(w => w.Date == infoDate.Date))
                    {
                        workerPostReportSalary.CountWorkDays++;
                    }

                    switch (infoDate.DescriptionDay)
                    {
                        case DescriptionDay.Был:
                            break;
                        case DescriptionDay.Б:
                            countSickDays++;
                            break;
                        case DescriptionDay.О:
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

                    if (weekendsInMonth.Any(w => w.Date == infoDate.Date))
                    {
                        if (infoDate.CountHours != null)
                        {
                            bodyDay.Value = infoDate.CountHours;
                        }
                        else
                        {
                            bodyDay.Style.Font.Bold = true;
                            bodyDay.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
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
                        if (weekendsInMonth.Any(w => w.Date == infoDate.Date))
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
                                workerPostReportSalary.CountWorkOverTimeHours += infoDate.CountHours.Value - 8;
                            }
                        }
                    }
                }

                sheet.Cells[5, 1, 6, countDaysInMonth].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

                //downheader

                ExcelRange downHeaderTypeOfCharge = sheet.Cells[8, 1, 8, 8];
                downHeaderTypeOfCharge.Merge = true;
                downHeaderTypeOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderTypeOfCharge.Style.Font.Bold = true;
                downHeaderTypeOfCharge.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderTypeOfCharge.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderTypeOfCharge.Value = "Начисления";

                ExcelRange downHeaderPeriod = sheet.Cells[8, 9, 8, 10];
                downHeaderPeriod.Merge = true;
                downHeaderPeriod.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderPeriod.Style.Font.Bold = true;
                downHeaderPeriod.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderPeriod.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderPeriod.Value = "Дата";

                ExcelRange downHeaderDays = sheet.Cells[8, 11, 8, 12];
                downHeaderDays.Merge = true;
                downHeaderDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderDays.Style.Font.Bold = true;
                downHeaderDays.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderDays.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderDays.Value = "Дни";

                ExcelRange downHeaderHours = sheet.Cells[8, 13, 8, 14];
                downHeaderHours.Merge = true;
                downHeaderHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderHours.Style.Font.Bold = true;
                downHeaderHours.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderHours.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderHours.Value = "Часы";

                ExcelRange downHeaderSalaryInHour = sheet.Cells[8, 15, 8, 17];
                downHeaderSalaryInHour.Merge = true;
                downHeaderSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderSalaryInHour.Style.Font.Bold = true;
                downHeaderSalaryInHour.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderSalaryInHour.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderSalaryInHour.Value = "Зп/Час";

                ExcelRange downHeaderSummOfCharge = sheet.Cells[8, 18, 8, 20];
                downHeaderSummOfCharge.Merge = true;
                downHeaderSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderSummOfCharge.Style.Font.Bold = true;
                downHeaderSummOfCharge.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderSummOfCharge.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderSummOfCharge.Value = "Сумма";

                ExcelRange downHeaderTypeOfHolding = sheet.Cells[8, 21, 8, 25];
                downHeaderTypeOfHolding.Merge = true;
                downHeaderTypeOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderTypeOfHolding.Style.Font.Bold = true;
                downHeaderTypeOfHolding.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderTypeOfHolding.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderTypeOfHolding.Value = "Удержания";

                ExcelRange downHeaderCount = sheet.Cells[8, 26, 8, 27];
                downHeaderCount.Merge = true;
                downHeaderCount.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderCount.Style.Font.Bold = true;
                downHeaderCount.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderCount.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderCount.Value = "К-во";

                ExcelRange downHeaderSummOfHolding = sheet.Cells[8, 28, 8, countDaysInMonth];
                downHeaderSummOfHolding.Merge = true;
                downHeaderSummOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downHeaderSummOfHolding.Style.Font.Bold = true;
                downHeaderSummOfHolding.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                downHeaderSummOfHolding.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 230, 230));
                downHeaderSummOfHolding.Value = "Сумма";

                ExcelRange downLeftHeaderSalary = sheet.Cells[9, 1, 9, 3];
                downLeftHeaderSalary.Merge = true;
                downLeftHeaderSalary.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderSalary.Value = "Оклад";

                int countRow = 9;
                for (int i = 0; i < workerPostReportSalaries.Count - 1; i++)
                {
                    ExcelRange downLeftBodyPost = sheet.Cells[countRow, 4, countRow, 8];
                    downLeftBodyPost.Merge = true;
                    downLeftBodyPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyPost.Value = workerPostReportSalaries[i].PostName;

                    ExcelRange downLeftBodyPeriod = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyPeriod.Merge = true;
                    downLeftBodyPeriod.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyPeriod.Value = workerPostReportSalaries[i].ChangePostDay + "-" + (workerPostReportSalaries[i + 1].ChangePostDay - 1);

                    ExcelRange downLeftBodyDays = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyDays.Merge = true;
                    downLeftBodyDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyDays.Value = workerPostReportSalaries[i].CountWorkDays;

                    ExcelRange downLeftBodyHours = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyHours.Merge = true;
                    downLeftBodyHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyHours.Value = workerPostReportSalaries[i].CountWorkHours;

                    ExcelRange downLeftBodySalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodySalaryInHour.Merge = true;
                    downLeftBodySalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodySalaryInHour.Value = Math.Round(((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8), 2) + " р.";

                    ExcelRange downLeftBodySummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodySummOfCharge.Merge = true;
                    downLeftBodySummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodySummOfCharge.Value = Math.Round(((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) * workerPostReportSalaries[i].CountWorkHours, 2) + " р.";

                    countRow++;
                }

                ExcelRange downLeftBodyLastPost = sheet.Cells[countRow, 4, countRow, 8];
                downLeftBodyLastPost.Merge = true;
                downLeftBodyLastPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastPost.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].PostName;

                ExcelRange downLeftBodyLastPeriod = sheet.Cells[countRow, 9, countRow, 10];
                downLeftBodyLastPeriod.Merge = true;
                downLeftBodyLastPeriod.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastPeriod.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].ChangePostDay + "-" + "-" + lastDayInMonth.Day;

                ExcelRange downLeftBodyLastDays = sheet.Cells[countRow, 11, countRow, 12];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkDays;

                ExcelRange downLeftBodyLastHours = sheet.Cells[countRow, 13, countRow, 14];
                downLeftBodyLastHours.Merge = true;
                downLeftBodyLastHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastHours.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkHours;

                ExcelRange downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                downLeftBodyLastSalaryInHour.Merge = true;
                downLeftBodyLastSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSalaryInHour.Value = Math.Round(((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8), 2) + " р.";

                ExcelRange downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                downLeftBodyLastSummOfCharge.Merge = true;
                downLeftBodyLastSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSummOfCharge.Value = Math.Round(((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) * 
                    workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkHours, 2) + " р.";

                countRow++;

                if (workerPostReportSalaries[0].CountWorkOverTimeHours > 0)
                {
                    ExcelRange downLeftHeaderOverTime = sheet.Cells[countRow, 1, countRow, 3];
                    downLeftHeaderOverTime.Merge = true;
                    downLeftHeaderOverTime.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftHeaderOverTime.Value = "Переработка";

                    for (int i = 0; i < workerPostReportSalaries.Count - 1; i++)
                    {
                        ExcelRange downLeftBodyPost = sheet.Cells[countRow, 4, countRow, 8];
                        downLeftBodyPost.Merge = true;
                        downLeftBodyPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodyPost.Value = workerPostReportSalaries[i].PostName;

                        ExcelRange downLeftBodyPeriod = sheet.Cells[countRow, 9, countRow, 10];
                        downLeftBodyPeriod.Merge = true;
                        downLeftBodyPeriod.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodyPeriod.Value = "—";

                        ExcelRange downLeftBodyDays = sheet.Cells[countRow, 11, countRow, 12];
                        downLeftBodyDays.Merge = true;
                        downLeftBodyDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodyDays.Value = "—";

                        ExcelRange downLeftBodyHours = sheet.Cells[countRow, 13, countRow, 14];
                        downLeftBodyHours.Merge = true;
                        downLeftBodyHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodyHours.Value = workerPostReportSalaries[i].CountWorkOverTimeHours;

                        ExcelRange downLeftBodySalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                        downLeftBodySalaryInHour.Merge = true;
                        downLeftBodySalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodySalaryInHour.Value = Math.Round((((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) * 2), 2) + " р.";

                        ExcelRange downLeftBodySummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                        downLeftBodySummOfCharge.Merge = true;
                        downLeftBodySummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        downLeftBodySummOfCharge.Value = Math.Round((((double)workerPostReportSalaries[i].AdminWorkerSalary / countWorkDays / 8) * 
                            workerPostReportSalaries[i].CountWorkOverTimeHours) * 2, 2) + " р.";

                        countRow++;
                    }

                    ExcelRange downLeftBodyOvertimeLastPost = sheet.Cells[countRow, 4, countRow, 8];
                    downLeftBodyOvertimeLastPost.Merge = true;
                    downLeftBodyOvertimeLastPost.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastPost.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].PostName;

                    ExcelRange downLeftBodyOvertimeLastPeriod = sheet.Cells[countRow, 9, countRow, 10];
                    downLeftBodyOvertimeLastPeriod.Merge = true;
                    downLeftBodyOvertimeLastPeriod.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastPeriod.Value = "—";

                    ExcelRange downLeftBodyOvertimeLastDays = sheet.Cells[countRow, 11, countRow, 12];
                    downLeftBodyOvertimeLastDays.Merge = true;
                    downLeftBodyOvertimeLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastDays.Value = "—";

                    ExcelRange downLeftBodyOvertimeLastHours = sheet.Cells[countRow, 13, countRow, 14];
                    downLeftBodyOvertimeLastHours.Merge = true;
                    downLeftBodyOvertimeLastHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastHours.Value = workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkOverTimeHours;

                    ExcelRange downLeftBodyOvertimeLastSalaryInHour = sheet.Cells[countRow, 15, countRow, 17];
                    downLeftBodyOvertimeLastSalaryInHour.Merge = true;
                    downLeftBodyOvertimeLastSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastSalaryInHour.Value = Math.Round((((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) * 2), 2) + " р.";

                    ExcelRange downLeftBodyOvertimeLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                    downLeftBodyOvertimeLastSummOfCharge.Merge = true;
                    downLeftBodyOvertimeLastSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    downLeftBodyOvertimeLastSummOfCharge.Value = Math.Round((((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) *
                            workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkOverTimeHours) * 2, 2) + " р.";

                    countRow++;
                }

                var infoMonth = bc.GetInfoMonth(worker.Id, year, month);

                ExcelRange downLeftHeaderBonus = sheet.Cells[countRow, 1, countRow, 8];
                downLeftHeaderBonus.Merge = true;
                downLeftHeaderBonus.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderBonus.Value = "Премия";

                downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = "—";

                downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                downLeftBodyLastHours.Merge = true;
                downLeftBodyLastHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastHours.Value = "—";

                downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                downLeftBodyLastSalaryInHour.Merge = true;
                downLeftBodyLastSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSalaryInHour.Value = "—";

                downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = "—";

                downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                downLeftBodyLastSummOfCharge.Merge = true;
                downLeftBodyLastSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSummOfCharge.Value = infoMonth.Bonus + " р.";

                countRow++;

                ExcelRange downLeftHeaderVocation = sheet.Cells[countRow, 1, countRow, 8];
                downLeftHeaderVocation.Merge = true;
                downLeftHeaderVocation.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderVocation.Value = "Отпускные";

                downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = "—";

                downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                downLeftBodyLastHours.Merge = true;
                downLeftBodyLastHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastHours.Value = countVocationDays;

                downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                downLeftBodyLastSalaryInHour.Merge = true;
                downLeftBodyLastSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSalaryInHour.Value = countVocationDays * 8;

                downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = workerSalaryInHour + " р.";

                downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                downLeftBodyLastSummOfCharge.Merge = true;
                downLeftBodyLastSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSummOfCharge.Value = workerSalaryInHour * 8 * countVocationDays + " р.";

                countRow++;

                ExcelRange downLeftHeaderSickDays = sheet.Cells[countRow, 1, countRow, 8];
                downLeftHeaderSickDays.Merge = true;
                downLeftHeaderSickDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderSickDays.Value = "Больничные";

                downLeftBodyLastDays = sheet.Cells[countRow, 9, countRow, 10];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = "—";

                downLeftBodyLastHours = sheet.Cells[countRow, 11, countRow, 12];
                downLeftBodyLastHours.Merge = true;
                downLeftBodyLastHours.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastHours.Value = countSickDays;

                downLeftBodyLastSalaryInHour = sheet.Cells[countRow, 13, countRow, 14];
                downLeftBodyLastSalaryInHour.Merge = true;
                downLeftBodyLastSalaryInHour.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSalaryInHour.Value = countSickDays * 8;

                downLeftBodyLastDays = sheet.Cells[countRow, 15, countRow, 17];
                downLeftBodyLastDays.Merge = true;
                downLeftBodyLastDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastDays.Value = workerSalaryInHour + " р.";

                downLeftBodyLastSummOfCharge = sheet.Cells[countRow, 18, countRow, 20];
                downLeftBodyLastSummOfCharge.Merge = true;
                downLeftBodyLastSummOfCharge.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftBodyLastSummOfCharge.Value = workerSalaryInHour * 8 * countSickDays + " р.";

                countRow++;

                ExcelRange downLeftHeaderPanalty = sheet.Cells[9, 21, 9, 25];
                downLeftHeaderPanalty.Merge = true;
                downLeftHeaderPanalty.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderPanalty.Value = "Штраф";

                ExcelRange downLeftHeaderMissDays = sheet.Cells[10, 21, 10, 25];
                downLeftHeaderMissDays.Merge = true;
                downLeftHeaderMissDays.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderMissDays.Value = "Прогул";

                ExcelRange downLeftHeaderBirthDay = sheet.Cells[11, 21, 11, 25];
                downLeftHeaderBirthDay.Merge = true;
                downLeftHeaderBirthDay.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderBirthDay.Value = "Дни рождения";

                ExcelRange downLeftHeaderPrePayment = sheet.Cells[12, 21, 12, 25];
                downLeftHeaderPrePayment.Merge = true;
                downLeftHeaderPrePayment.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downLeftHeaderPrePayment.Value = "Аванс";

                ExcelRange downBodyCount = sheet.Cells[9, 26, 9, 27];
                downBodyCount.Merge = true;
                downBodyCount.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodyCount.Value = countPanalty;

                ExcelRange downBodySummOfHolding = sheet.Cells[9, 28, 9, countDaysInMonth];
                downBodySummOfHolding.Merge = true;
                downBodySummOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodySummOfHolding.Value = workerPanalty + " р.";

                //downbody//Progul

                downBodyCount = sheet.Cells[10, 26, 10, 27];
                downBodyCount.Merge = true;
                downBodyCount.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodyCount.Value = countMissimgDays;

                downBodySummOfHolding = sheet.Cells[10, 28, 10, countDaysInMonth];
                downBodySummOfHolding.Merge = true;
                downBodySummOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodySummOfHolding.Value = countMissimgDays * 8 * workerSalaryInHour + " р.";

                //downbody//Birthday

                downBodyCount = sheet.Cells[11, 26, 11, 27];
                downBodyCount.Merge = true;
                downBodyCount.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodyCount.Value = "—";

                downBodySummOfHolding = sheet.Cells[11, 28, 11, countDaysInMonth];
                downBodySummOfHolding.Merge = true;
                downBodySummOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodySummOfHolding.Value = infoMonth.BirthDays + " р.";

                //downbody//Prepayment

                downBodyCount = sheet.Cells[12, 26, 12, 27];
                downBodyCount.Merge = true;
                downBodyCount.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodyCount.Value = "—";

                downBodySummOfHolding = sheet.Cells[12, 28, 12, countDaysInMonth];
                downBodySummOfHolding.Merge = true;
                downBodySummOfHolding.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                downBodySummOfHolding.Value = infoMonth.PrepaymentCash + " р.";


                sheet.Cells[8, 1, countRow - 1, 20].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                sheet.Cells[8, 21, countRow - 1, countDaysInMonth].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                //footerHeader

                ExcelRange footerHeaderSummOfPayments = sheet.Cells[countRow + 1, 1, countRow + 1, 7];
                footerHeaderSummOfPayments.Merge = true;
                footerHeaderSummOfPayments.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerHeaderSummOfPayments.Style.Font.Bold = true;
                footerHeaderSummOfPayments.Value = "Всего начислено";

                ExcelRange footerHeaderAllHolds = sheet.Cells[countRow + 1, 12, countRow + 1, 17];
                footerHeaderAllHolds.Merge = true;
                footerHeaderAllHolds.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerHeaderAllHolds.Style.Font.Bold = true;
                footerHeaderAllHolds.Value = "Всего удержано";

                ExcelRange footerHeaderCleanPayments = sheet.Cells[countRow + 1, 22, countRow + 1, 27];
                footerHeaderCleanPayments.Merge = true;
                footerHeaderCleanPayments.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerHeaderCleanPayments.Style.Font.Bold = true;
                footerHeaderCleanPayments.Value = "Всего выплачено";

                // footerBody

                double allSalary = Math.Round(((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) *
                        workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkHours, 2);

                double allOvertimeSalary = Math.Round((((double)workerPostReportSalaries[workerPostReportSalaries.Count - 1].AdminWorkerSalary / countWorkDays / 8) *
                        workerPostReportSalaries[workerPostReportSalaries.Count - 1].CountWorkOverTimeHours) * 2, 2);

                double SummOfPayments = allSalary + allOvertimeSalary + infoMonth.Bonus + (workerSalaryInHour * 8 * countVocationDays) + (workerSalaryInHour * 8 * countSickDays);

                double SummOfHoldings = (workerPanalty) + (countMissimgDays * 8 * workerSalaryInHour) + (infoMonth.BirthDays) + (infoMonth.PrepaymentCash);

                ExcelRange footerBodySummOfPayments = sheet.Cells[countRow + 1, 8, countRow + 1, 11];
                footerBodySummOfPayments.Merge = true;
                footerBodySummOfPayments.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerBodySummOfPayments.Value = SummOfPayments + " р.";

                ExcelRange footerBodyAllHolds = sheet.Cells[countRow + 1, 18, countRow + 1, 21];
                footerBodyAllHolds.Merge = true;
                footerBodyAllHolds.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerBodyAllHolds.Value = SummOfHoldings + " р.";

                ExcelRange footerBodyCleanPayments = sheet.Cells[countRow + 1, 28, countRow + 1, countDaysInMonth];
                footerBodyCleanPayments.Merge = true;
                footerBodyCleanPayments.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                footerBodyCleanPayments.Value = SummOfPayments - SummOfHoldings + " р.";

                countRow++;

                sheet.Cells[countRow, 1, countRow, countDaysInMonth].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);

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
                            footerPanaltyReportHeader.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            footerPanaltyReportHeader.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            footerPanaltyReportHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            footerPanaltyReportHeader.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
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
                            footerPanaltyReportDate.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            footerPanaltyReportDate.Value = infoDate.Date.ToShortDateString();

                            ExcelRange footerPanaltyReportSumm = sheet.Cells[countRow, 4, countRow, 8];
                            footerPanaltyReportSumm.Merge = true;
                            footerPanaltyReportSumm.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            footerPanaltyReportSumm.Value = infoDate.InfoPanalty.Summ + " р.";

                            ExcelRange footerPanaltyReportDescription = sheet.Cells[countRow, 9, countRow, countDaysInMonth];
                            footerPanaltyReportDescription.Merge = true;
                            footerPanaltyReportDescription.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            footerPanaltyReportDescription.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            footerPanaltyReportDescription.Value = infoDate.InfoPanalty.Description;
                            countRow++;
                        }
                    }
                }

                if (enterValueOfCountRow < countRow)
                {
                    sheet.Cells[enterValueOfCountRow, 1, countRow - 1, countDaysInMonth].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                }

                ExcelRange nextSideFio = sheet.Cells[33, 3, 34, countDaysInMonth - 3];
                nextSideFio.Merge = true;
                nextSideFio.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                nextSideFio.Style.Font.Bold = true;
                nextSideFio.Style.Font.Size = 17;
                nextSideFio.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                nextSideFio.Value = worker.LastName + " " + worker.FirstName + " " + worker.MidName;
            }
        }
    }
}

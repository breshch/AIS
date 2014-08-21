using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Reports
{
    public static class CashReports
    {
        private const string PATH_DIRECTORY_COST_REPORTS = "Reports\\Costs";
        private const string PATH_INCOMING_REPORT = "Приход";
        private const string PATH_EXPENSE_REPORT = "Расход";
        private const string PATH_EXPENSES_REPORT = "Расходы";

        private const string APPEAL = "Начальнику финансового отдела";
        private const string APPEAL_NAME = "Паращенко А.Н.";
        private const string MEMORANDUM = "СЛУЖЕБНАЯ ЗАПИСКА";
        private const string MEMORANDUM_INCOMING_RC_CONTEXT = "Прошу Вас поставить в приход сумму и уменьшить задолженность по фирмам:";
        private const string MEMORANDUM_INCOMING_26_CONTEXT_FIRST_PART = "Прошу сумму в размере ";
        private const string MEMORANDUM_INCOMING_26_CONTEXT_SECOND_PART = " RUB, поставить в приход кассы «АВ», и снять из затрат по статье Содержание ФК.";
        private const string TABLE_HEADER_NAME = "Фирма";
        private const string TABLE_HEADER_SUMM = "Сумма";
        private const string TABLE_FOOTER_NAME = "ИТОГО:";
        private const string FOOTER_MEMORANDUM_FIRTST_PART = "И отнести общую сумму ";
        private const string FOOTER_MEMORANDUM_SECOND_PART = " RUB на ПРИХОД в кассу «АВ-Автотехник»";
        private const string FOOTER_RC_APEL = "Руководитель ";
        private const string FOOTER_RC_SU_APEL = "Зам. руководителя ";
        private const string FOOTER_26_APEL = "Заместитель директора по Маркетингу";
        private const string OFFICE_NAME = "(26А)";
        private const string OFFICE_HEADER_INCOMING_FULL_NAME = "26А/5013";

        private const string FOOTER_RC_KO2_APEL_NAME = "Бобров.А.Г";
        private const string FOOTER_RC_ALL_APEL_NAME = "Голоколенко.В";
        private const string FOOTER_26_APEL_NAME = "Жилинский Е.В.";
        private const string FOOTER_RC_PAM16_APEL_NAME = "Михеенко А.";


        public static void IncomingRC(DirectoryRC rc, BusinessContext bc, int year, int month)
        {
            string path = Path.Combine(PATH_DIRECTORY_COST_REPORTS, PATH_INCOMING_REPORT + " " + rc.Name + " " + month + "." + year + ".xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) => IncomingRCReport(ep, bc, year, month, rc),
                });
        }

        public static void Incoming26(BusinessContext bc, int year, int month)
        {
            string path = Path.Combine(PATH_DIRECTORY_COST_REPORTS, PATH_INCOMING_REPORT + " " + OFFICE_NAME + " " + month + "." + year + ".xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) => Incoming26Report(ep, bc, year, month),
                });
        }

        public static void ExpenseRCs(BusinessContext bc, int year, int month)
        {
            string path = Path.Combine(PATH_DIRECTORY_COST_REPORTS, PATH_EXPENSES_REPORT + " " + month + "." + year + ".xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) => ExpenseRCsReport(ep, bc, year, month),
                });
        }

        public static void Expense26(BusinessContext bc, int year, int month)
        {
            string path = Path.Combine(PATH_DIRECTORY_COST_REPORTS, PATH_EXPENSE_REPORT + " " + OFFICE_NAME + " " + month + "." + year + ".xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) => Expense26Report(ep, bc, year, month),
                });
        }

        private static void IncomingRCReport(ExcelPackage ep, BusinessContext bc, int year, int month, DirectoryRC rc)
        {
            string name = PATH_INCOMING_REPORT + " " + rc.Name;
            var sheet = Helpers.GetSheet(ep, name);

            string rcAdvancedName = "ОТ. " + rc.Name + "/" + rc.Name[0] + " " + rc.Name.Substring(1);
            rcAdvancedName = rcAdvancedName.Replace("-", "");

            int indexRow = 1;
            CreationHeader(sheet, ref indexRow, MEMORANDUM_INCOMING_RC_CONTEXT, 2);

            Helpers.CreateCell(sheet, indexRow, 1, rcAdvancedName, Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;

            Helpers.CreateCell(sheet, indexRow, 1, TABLE_HEADER_NAME, Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
            Helpers.CreateCell(sheet, indexRow, 2, TABLE_HEADER_SUMM, Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);
            indexRow++;

            var infoCosts = bc.GetInfoCostsRCIncoming(year, month, rc.Name).ToList();
            foreach (var infoCost in infoCosts)
            {
                Helpers.CreateCell(sheet, indexRow, 1, infoCost.ConcatNotes.ToString(), Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 2, infoCost.Summ + " RUB", Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                indexRow++;
            }

            Helpers.CreateCell(sheet, indexRow, 1, TABLE_FOOTER_NAME, Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left);
            Helpers.CreateCell(sheet, indexRow, 2, infoCosts.Sum(c => c.Summ) + " RUB", Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, indexRow, 2, FOOTER_MEMORANDUM_FIRTST_PART + infoCosts.Sum(c => c.Summ) + FOOTER_MEMORANDUM_SECOND_PART,
                Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_RC_APEL + rcAdvancedName, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);

            if (rc.Name != "КО-2")
            {
                Helpers.CreateCell(sheet, indexRow, 2, FOOTER_RC_ALL_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);    
            }
            else
            {
                Helpers.CreateCell(sheet, indexRow, 2, FOOTER_RC_KO2_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);    
            }
        }

        private static void Incoming26Report(ExcelPackage ep, BusinessContext bc, int year, int month)
        {
            string name = PATH_INCOMING_REPORT + " " + OFFICE_NAME;
            var sheet = Helpers.GetSheet(ep, name);

            int indexRow = 1;
            Helpers.CreateCell(sheet, indexRow, 2, OFFICE_HEADER_INCOMING_FULL_NAME, Color.Transparent, 14, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;

            double summ = bc.GetInfoCost26Summ(year, month);
            CreationHeader(sheet, ref indexRow, MEMORANDUM_INCOMING_26_CONTEXT_FIRST_PART + summ + MEMORANDUM_INCOMING_26_CONTEXT_SECOND_PART, 2);

            indexRow += 10;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_26_APEL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 2, FOOTER_26_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
        }

        private static void Expense26Report(ExcelPackage ep, BusinessContext bc, int year, int month)
        {
            string name = PATH_EXPENSE_REPORT + " " + OFFICE_NAME;
            var sheet = Helpers.GetSheet(ep, name);

            int indexRow = 1;
            var infoCosts = bc.GetInfoCosts26Expense(year, month).ToList();

            int countRows = 0;
            string context = "Прошу снять из кассы АВ следующие суммы:" + Environment.NewLine;
            countRows++;

            foreach (var infoCost in infoCosts)
            {
                context += "Сумму в размере " + infoCost.Summ + " " + infoCost.ConcatNotes + " – распределение по проектам согласно служебной записке." + Environment.NewLine;
                countRows += 2;
            }

            CreationHeader(sheet, ref indexRow, context, countRows);

            indexRow += 6;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_26_APEL, Color.Transparent, 14, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 2, FOOTER_26_APEL_NAME, Color.Transparent, 14, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
        }

        private static void ExpenseRCsReport(ExcelPackage ep, BusinessContext bc, int year, int month)
        {
            string name = PATH_EXPENSES_REPORT;
            var sheet = Helpers.GetSheet(ep, name);

            sheet.View.ShowGridLines = false;

            sheet.Column(1).Width = Helpers.PixelsToInches(498);
            sheet.Column(2).Width = Helpers.PixelsToInches(110);
            sheet.Column(3).Width = Helpers.PixelsToInches(110);
            sheet.Column(4).Width = Helpers.PixelsToInches(145);
            sheet.Column(5).Width = Helpers.PixelsToInches(110);

            int indexRow = 1;

            Helpers.CreateCell(sheet, indexRow, 2, indexRow, 4, APPEAL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;

            Helpers.CreateCell(sheet, indexRow, 2,  indexRow, 4, APPEAL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, indexRow, 4, MEMORANDUM, Color.Transparent, 14, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            int indexContextRow = indexRow;
            double commonSumm = 0;
            
            sheet.Row(indexRow).Height = 2 * 20;
            indexRow += 2;


            var rcs = bc.GetDirectoryRCsMonthExpense(year, month).Where(r => r.Name != "26А" && r.Name != "ВСЕ").ToList();

            if (bc.GetInfoCosts(year, month).Any(c => c.DirectoryRC.Name == "ВСЕ"))
            {
                rcs = new List<DirectoryRC>(rcs.Union(bc.GetDirectoryRCs().Where(r => r.Percentes > 0).ToList()));
            }

            foreach (var rc in rcs)
            {
                Helpers.CreateCell(sheet, indexRow, 1,rc.ReportName, Color.Transparent, 12, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
                indexRow++;

                Helpers.CreateCell(sheet, indexRow, 1, "Статья затрат", Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 2, rc.ReportName, Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 3, "ВСЕ", Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 4, "Сумма, RUB", Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                
                indexRow++;

                double commonRCSumm = 0;
                double commonAllSumm = 0;
                double commonRCAndAllSumm = 0;

                var infoCosts = bc.GetInfoCostsRCAndAll(year, month, rc.Name).ToList();
                foreach (var infoCostsCostItem in infoCosts.Where(c => !c.IsIncoming).GroupBy(c => c.DirectoryCostItem.Name))
                {
                    string costItemName = infoCostsCostItem.First().DirectoryCostItem.Name;

                    Helpers.CreateCell(sheet, indexRow, 1, costItemName, Color.Transparent, 10, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                 
                    double costItemExpenseSumm = infoCostsCostItem.Where(c => c.DirectoryRC.Name != "ВСЕ").Sum(c => c.Summ);
                    double costItemIncomingSumm = infoCosts.Where(c => c.IsIncoming && c.DirectoryCostItem.Name == costItemName && c.DirectoryRC.Name != "ВСЕ").Sum(c => c.Summ);
                    double costItemRCSumm = costItemExpenseSumm - costItemIncomingSumm;
                    string costItemRCSummString = costItemRCSumm != 0 ? costItemRCSumm + " RUB" : "";

                    Helpers.CreateCell(sheet, indexRow, 2, costItemRCSummString, Color.Transparent, 10, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    double costItemExpenseSummAll = Math.Round(infoCostsCostItem.Where(c => c.DirectoryRC.Name == "ВСЕ").Sum(c => c.Summ) * rc.Percentes / 100, 0);
                    double costItemIncomingSummAll = Math.Round(infoCosts.Where(c => c.IsIncoming && c.DirectoryCostItem.Name == costItemName && c.DirectoryRC.Name == "ВСЕ").Sum(c => c.Summ) * rc.Percentes / 100, 0);
                    double costItemRCSummAll = costItemExpenseSummAll - costItemIncomingSummAll;
                    string costItemRCSummAllString = costItemRCSummAll != 0 ? costItemRCSummAll + " RUB" : "";

                    Helpers.CreateCell(sheet, indexRow, 3, costItemRCSummAllString, Color.Transparent, 10, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    double commonCostItemSumm = costItemRCSumm + costItemRCSummAll;
                    string commonCostItemSummString = commonCostItemSumm != 0 ? commonCostItemSumm + " RUB" : "";


                    Helpers.CreateCell(sheet, indexRow, 4, commonCostItemSummString, Color.Transparent, 10, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    commonRCSumm += costItemRCSumm;
                    commonAllSumm += costItemRCSummAll;
                    commonRCAndAllSumm += commonCostItemSumm;
                    
                    indexRow++;               
                }
                string commonRCSummString = commonRCSumm != 0 ? commonRCSumm + " RUB" : "";
                string commonAllSummString = commonAllSumm != 0 ? commonAllSumm + " RUB" : "";
                string commonRCAndAllSummString = commonRCAndAllSumm != 0 ? commonRCAndAllSumm + " RUB" : "";

                Helpers.CreateCell(sheet, indexRow, 1, "Итого", Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 2, commonRCSummString, Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 3, commonAllSummString, Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                Helpers.CreateCell(sheet, indexRow, 4, commonRCAndAllSummString, Color.LightGray, 11, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                commonSumm += commonRCAndAllSumm;

                indexRow += 2;
            }

            string context = "Просим Вас снять с кассы «АВ» сумму в размере " + commonSumm + " RUB, и отнести на затраты по следующим центрам ответственности и статьям затрат:";
            Helpers.CreateCell(sheet, indexContextRow, 1, indexContextRow, 4, context, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_RC_APEL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;
            string rcPAM1 = bc.GetDirectoryRC("ПАМ-1").ReportName;
            Helpers.CreateCell(sheet, indexRow, 1, rcPAM1, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 4, FOOTER_26_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_RC_SU_APEL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;
            string rcKO1 = bc.GetDirectoryRC("КО-1").ReportName;
            string rcKO2 = bc.GetDirectoryRC("КО-2").ReportName;
            Helpers.CreateCell(sheet, indexRow, 1, rcKO1 + " / " + rcKO2, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 4, FOOTER_RC_KO2_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_RC_APEL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;
            string rcMO2 = bc.GetDirectoryRC("МО-2").ReportName;
            string rcMO5 = bc.GetDirectoryRC("МО-5").ReportName;
            string rcKO5 = bc.GetDirectoryRC("КО-5").ReportName;
            Helpers.CreateCell(sheet, indexRow, 1, rcMO2 + " / " + rcMO5 + " / " + rcKO5, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 4, FOOTER_RC_ALL_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, FOOTER_RC_APEL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;
            string rcPAM16 = bc.GetDirectoryRC("ПАМ-16").ReportName;
            Helpers.CreateCell(sheet, indexRow, 1, rcPAM16, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            Helpers.CreateCell(sheet, indexRow, 4, FOOTER_RC_PAM16_APEL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;
        }

        private static void CreationHeader(ExcelWorksheet sheet, ref int indexRow, string context, int countRows)
        {
            sheet.View.ShowGridLines = false;

            sheet.Column(1).Width = Helpers.PixelsToInches(330);
            sheet.Column(2).Width = Helpers.PixelsToInches(250);

            Helpers.CreateCell(sheet, indexRow, 1, indexRow, 2, APPEAL, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow++;

            Helpers.CreateCell(sheet, indexRow, 2, APPEAL_NAME, Color.Transparent, 12, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Right, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 7;

            Helpers.CreateCell(sheet, indexRow, 1, indexRow, 2, MEMORANDUM, Color.Transparent, 14, true, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelBorderStyle.None);
            indexRow += 2;

            Helpers.CreateCell(sheet, indexRow, 1, indexRow, 2, context, Color.Transparent, 14, false, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelBorderStyle.None);
            sheet.Row(indexRow).Height = (countRows + 2) * 20;
            indexRow += 2;
        }
    }
}

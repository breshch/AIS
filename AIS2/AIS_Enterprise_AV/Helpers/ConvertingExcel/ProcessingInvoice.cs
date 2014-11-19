﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using OfficeOpenXml;

namespace AIS_Enterprise_AV.Helpers.ConvertingExcel
{
    public static class ProcessingInvoice
    {
        public static void Procesing(BusinessContext bc, string path, int percentageRus, int percentageImport)
        {
            var ep = new ExcelPackage(new FileInfo(path));
            var sheet = ep.Workbook.Worksheets.First(s => s.Name == "АВ ТТН");
            int i = 1;
            while (sheet.Cells[i, 13].Value == null || sheet.Cells[i, 13].Value.ToString() != "Дата\nсоставления")
            {
                i++;
            }
            var date = DateTime.Parse(sheet.Cells[i + 1, 13].Value.ToString());

            var articlePrices = bc.GetArticlePrices(date).ToList();

            while (sheet.Cells[i, 1].Value == null || sheet.Cells[i, 1].Value.ToString() != "2")
            {
                i++;
            }

            i--;
            var invoices = new List<Invoice>();
            while (sheet.Cells[i, 11].Value == null || sheet.Cells[i, 11].Value.ToString() != "0")
            {
                if (sheet.Cells[i, 3].Value != null)
                {
                    var article = sheet.Cells[i, 3].Value.ToString().Replace(" ", "");
                    var prevArticle = article;
                    int j;

                    bool isFound = false;
                    for (j = article.Length; j > 0; j--)
                    {
                        article = article.Substring(0, j);

                        var equalArticle = articlePrices.FirstOrDefault(a => (a.Article + a.Mark) == article);

                        if (equalArticle != null)
                        {
                            int count = int.Parse(sheet.Cells[i, 11].Value.ToString());
                            var invoice = new Invoice
                            {
                                Article = equalArticle.Article + equalArticle.Mark,
                                Count = count,
                                Description = equalArticle.Description,
                                PriceBase = equalArticle.Price,
                                IsRus = equalArticle.Mark != null
                            };
                            invoices.Add(invoice);
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        article = prevArticle;
                        for (j = article.Length; j > 0; j--)
                        {
                            article = article.Substring(0, j);

                            var equalArticle = articlePrices.FirstOrDefault(a => a.Article == article);

                            if (equalArticle != null)
                            {
                                int count = int.Parse(sheet.Cells[i, 11].Value.ToString());
                                var invoice = new Invoice
                                {
                                    Article = equalArticle.Article + equalArticle.Mark,
                                    Count = count,
                                    Description = equalArticle.Description,
                                    PriceBase = equalArticle.Price,
                                    IsRus = equalArticle.Mark != null
                                };
                                invoices.Add(invoice);
                                break;
                            }
                        }
                    }

                    if (j == 0)
                    {
                        using (var sw = new StreamWriter("DebugArticles.txt", true))
                        {
                            sw.WriteLine(prevArticle);
                        }
                    }
                }
                i++;
            }

            using (var sw = new StreamWriter("DebugArticles.txt", true))
            {
                sw.WriteLine("#############################################");
            }

            ComplitedCompliteInvoice(path, percentageRus, percentageImport, invoices);
        }

        private static void ComplitedCompliteInvoice(string path, int percentageRus, int percentageImport, List<Invoice> invoices)
        {
            Reports.Helpers.CompletedReport(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CompliteInvoice_" + Path.GetFileName(path)),
               new List<Action<ExcelPackage>>
                {
                    ep => CreatingCompliteInvoice(ep, percentageRus, percentageImport, path, invoices)
                });
        }

        private static void CreatingCompliteInvoice(ExcelPackage ep, int percentageRus, int percentageImport, string path, List<Invoice> invoices)
        {
            string name = "АВ ТТН";
            var sheet = Reports.Helpers.GetSheet(ep, name);

            string header = Path.GetFileNameWithoutExtension(path);

            Reports.Helpers.CreateCell(sheet, 1, 1, 1, 6, header, Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 1, "Артикул", Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 2, "Описание", Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 3, "Базовая цена", Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 4, "Проценты", Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 5, "Количество", Color.Transparent);
            Reports.Helpers.CreateCell(sheet, 3, 6, "Итого", Color.Transparent);

            int index = 4;
            foreach (var invoice in invoices)
            {
                double pricePercentage = Math.Round(invoice.PriceBase *
                                         ((100 - (invoice.IsRus ? percentageRus : percentageImport)) / 100.0), 2);

                Reports.Helpers.CreateCell(sheet, index, 1, invoice.Article, Color.Transparent);
                Reports.Helpers.CreateCell(sheet, index, 2, invoice.Description, Color.Transparent);
                Reports.Helpers.CreateCell(sheet, index, 3, invoice.PriceBase.ToString("N2"), Color.Transparent);
                Reports.Helpers.CreateCell(sheet, index, 4, pricePercentage.ToString("N2"), Color.Transparent);
                Reports.Helpers.CreateCell(sheet, index, 5, invoice.Count, Color.Transparent);
                Reports.Helpers.CreateCell(sheet, index, 6, (invoice.Count * pricePercentage).ToString("N2"), Color.Transparent);

                index++;
            }
        }

    }
}

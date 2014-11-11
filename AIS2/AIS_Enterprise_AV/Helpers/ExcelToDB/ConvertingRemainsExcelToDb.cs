using System.Diagnostics;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public class ConvertingRemainsExcelToDb
    {
        public static void ConvertRemains(BusinessContext bc, string path)
        {
            var existingFile = new FileInfo(path);

            using (var package = new ExcelPackage(existingFile))
            {
                var carParts = bc.GetDirectoryCarParts().ToList();
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First(w => w.Name == "Ноябрь 2014");
                        int indexRow = 3;

                        using (var sw = new StreamWriter("articles.txt"))
                        {
                            sw.WriteLine();
                        }

                        var date = new DateTime(2014, 11, 01);
                        string article = GetValue(sheet.Cells[indexRow, 1].Value);
                        while (!string.IsNullOrWhiteSpace(article))
                        {
                            int indexDigit = -1;
                            for (int i = 0; i < article.Length - 1; i++)
                            {
                                if (char.IsDigit(article[i]))
                                {
                                    indexDigit = i;
                                }
                                else if (char.IsLetter(article[i]) && indexDigit != -1)
                                {
                                    indexDigit = i - 1;
                                    break;
                                }

                            }

                            string newArticle = article.Substring(0, indexDigit + 1);
                            string mark = indexDigit != (article.Length - 1)
                                ? article.Substring(indexDigit + 1)
                                : null;


                            var equalCarPart = carParts.FirstOrDefault(p => ((p.Article + p.Mark) == article) || (p.Article == newArticle));
                            if (equalCarPart == null)
                            {
                                equalCarPart = new DirectoryCarPart
                                {
                                    Article = newArticle,
                                    Mark = mark,
                                    Note = new CarPartNote(),
                                    FactoryAndCross = new CarPartFactoryAndCross(),
                                    Description = GetValue(sheet.Cells[indexRow, 5].Value)
                                };

                                using (var sw = new StreamWriter("articles.txt", true))
                                {
                                    sw.WriteLine(equalCarPart.Article + equalCarPart.Mark);
                                }
                            }

                            int remain = int.Parse(GetValue(sheet.Cells[indexRow, 6].Value));

                            bc.AddInfoLastMonthDayRemain(equalCarPart, date, remain);

                            indexRow++;
                            article = GetValue(sheet.Cells[indexRow, 1].Value);
                        }
                    }
                }
            }
        }


        private static string GetValue(object parameter)
        {
            return parameter != null ? parameter.ToString() : null;
        }
    }
}

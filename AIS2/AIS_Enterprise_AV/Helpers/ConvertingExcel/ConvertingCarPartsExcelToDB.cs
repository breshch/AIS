using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public class ConvertingCarPartsExcelToDB
    {
        public static void ConvertImport(BusinessContext bc, string path)
        {
            var existingFile = new FileInfo(path);

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First();
                        int indexRow = 6;

                        string number =  GetValue(sheet.Cells[indexRow, 1].Value);
                        while (!string.IsNullOrWhiteSpace(number))
                        {
                            string article =  GetValue(sheet.Cells[indexRow, 2].Value);

                            if (string.IsNullOrWhiteSpace(article) || article == "№ Fenox")
                            {
                                indexRow++;
                                number = GetValue(sheet.Cells[indexRow, 1].Value);
                                continue;   
                            }

                            string description = GetValue(sheet.Cells[indexRow, 3].Value);
                            string originalNumber =  GetValue(sheet.Cells[indexRow, 4].Value);
                            string material =  GetValue(sheet.Cells[indexRow, 5].Value);
                            string attachment =  GetValue(sheet.Cells[indexRow, 6].Value);
                            string factoryNumber =  GetValue(sheet.Cells[indexRow, 7].Value);
                            string crossNumber =  GetValue(sheet.Cells[indexRow, 8].Value);
                            string countInBox = GetValue(sheet.Cells[indexRow, 12].Value);

                            if (bc.GetDirectoryCarPart(article, null) == null)
                            {
                                bc.AddDirectoryCarPart(article, null, description, originalNumber, factoryNumber,
                                    crossNumber, material, attachment, countInBox);
                            }

                            indexRow++;
                        }
                    }
                }
            }
        }

        public static void ConvertRussian(BusinessContext bc, string path)
        {
            var existingFile = new FileInfo(path);

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First(s => s.Name == "Печать цен Отечественные");
                        int indexRow = 8;

                        string name = GetValue(sheet.Cells[indexRow, 2].Value);

                        string number = GetValue(sheet.Cells[indexRow, 1].Value);
                        string description = GetValue(sheet.Cells[indexRow, 2].Value);
                        string originalNumber = GetValue(sheet.Cells[indexRow, 3].Value);
                        string article = GetValue(sheet.Cells[indexRow, 4].Value);
                        string mark = GetValue(sheet.Cells[indexRow, 5].Value);
                        string material = GetValue(sheet.Cells[indexRow, 7].Value);
                        string attachment = GetValue(sheet.Cells[indexRow, 8].Value);
                        string factoryNumber = null;
                        string crossNumber = null;
                        string countInBox = GetValue(sheet.Cells[indexRow, 11].Value);

                       
                        while (!(string.IsNullOrWhiteSpace(number) && string.IsNullOrWhiteSpace(name)))
                        {
                            if (string.IsNullOrWhiteSpace(number) && !string.IsNullOrWhiteSpace(name))
                            {
                                indexRow++;
                                name = GetValue(sheet.Cells[indexRow, 2].Value);
                                number = GetValue(sheet.Cells[indexRow, 1].Value);
                                continue;
                            }

                            description = GetValue(sheet.Cells[indexRow, 2].Value) ?? description;
                            originalNumber = GetValue(sheet.Cells[indexRow, 3].Value) ?? originalNumber;
                            article = GetValue(sheet.Cells[indexRow, 4].Value) ?? article;
                            mark = GetValue(sheet.Cells[indexRow, 5].Value) ?? mark;
                            material = GetValue(sheet.Cells[indexRow, 7].Value) ?? material;
                            attachment = GetValue(sheet.Cells[indexRow, 8].Value) ?? attachment;
                            countInBox = GetValue(sheet.Cells[indexRow, 11].Value) ?? countInBox;

                            if (bc.GetDirectoryCarPart(article, mark) == null)
                            {
                                bc.AddDirectoryCarPart(article, mark, description, originalNumber, factoryNumber,
                                    crossNumber, material, attachment, countInBox);
                            }

                            indexRow++;

                            name = GetValue(sheet.Cells[indexRow, 2].Value);
                            number = GetValue(sheet.Cells[indexRow, 1].Value);
                        }
                    }
                }
            }
        }

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
                            for (int i = 0; i < article.Length; i++)
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


                            var equalCarPart = carParts.FirstOrDefault(p => ((p.Article + p.Mark) == article) || (p.Mark == null && p.Article == newArticle));
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


        public static void ConvertPriceRus(BusinessContext bc, string path)
        {
            var existingFile = new FileInfo(path);

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First(s => s.Name == "Печать цен Отечественные");

                        string date = GetValue(sheet.Cells[3, 8].Value);
                        date = date.Substring(date.IndexOf("от") + 3);
                        DateTime priceDate = DateTime.Parse(date);

                        int indexRow = 8;

                        string fullName = GetValue(sheet.Cells[indexRow, 18].Value);

                        string name = GetValue(sheet.Cells[indexRow, 2].Value);
                        string number = GetValue(sheet.Cells[indexRow, 1].Value);
                        string description = GetValue(sheet.Cells[indexRow, 2].Value);
                        string originalNumber = GetValue(sheet.Cells[indexRow, 3].Value);
                        string article = GetValue(sheet.Cells[indexRow, 4].Value);
                        string mark = GetValue(sheet.Cells[indexRow, 5].Value);
                        string material = GetValue(sheet.Cells[indexRow, 7].Value);
                        string attachment = GetValue(sheet.Cells[indexRow, 8].Value);
                        string factoryNumber = null;
                        string crossNumber = null;
                        string countInBox = GetValue(sheet.Cells[indexRow, 11].Value);

                        var carParts = bc.GetDirectoryCarParts().ToList();

                        while (!(string.IsNullOrWhiteSpace(number) && string.IsNullOrWhiteSpace(name)))
                        {
                            if (string.IsNullOrWhiteSpace(number) && !string.IsNullOrWhiteSpace(name))
                            {
                                indexRow++;
                                name = GetValue(sheet.Cells[indexRow, 2].Value);
                                number = GetValue(sheet.Cells[indexRow, 1].Value);
                                continue;
                            }

                            fullName = GetValue(sheet.Cells[indexRow, 18].Value);

                            description = GetValue(sheet.Cells[indexRow, 2].Value) ?? description;
                            originalNumber = GetValue(sheet.Cells[indexRow, 3].Value) ?? originalNumber;
                            article = GetValue(sheet.Cells[indexRow, 4].Value) ?? article;
                            mark = GetValue(sheet.Cells[indexRow, 5].Value) ?? mark;
                            material = GetValue(sheet.Cells[indexRow, 7].Value) ?? material;
                            attachment = GetValue(sheet.Cells[indexRow, 8].Value) ?? attachment;
                            countInBox = GetValue(sheet.Cells[indexRow, 11].Value) ?? countInBox;

                            var equalCarPart = carParts.FirstOrDefault(p => (p.Article + p.Mark) == fullName);
                            if (equalCarPart == null)
                            {
                                equalCarPart = bc.AddDirectoryCarPart(article, mark, description, originalNumber, factoryNumber,
                                    crossNumber, material, attachment, countInBox);
                            }

                            if (equalCarPart.Description == null)
                            {
                                equalCarPart.Description = description;
                                equalCarPart.OriginalNumber = originalNumber;
                                equalCarPart.Note.Material = material;
                                equalCarPart.Note.Attachment = attachment;
                                equalCarPart.CountInBox = countInBox;
                                bc.SaveChanges();
                            }

                            double priceBase = double.Parse( GetValue(sheet.Cells[indexRow, 9].Value));
                            double priceBigWholesale = double.Parse(GetValue(sheet.Cells[indexRow, 14].Value));
                            double priceSmallWholesale = double.Parse(GetValue(sheet.Cells[indexRow, 15].Value));

                            var lastCurrentCarPart = bc.GetCurrentCarPart(equalCarPart.Id, priceDate);

                            if (lastCurrentCarPart == null ||
                                (lastCurrentCarPart.PriceBase != priceBase ||
                                 lastCurrentCarPart.PriceBigWholesale != priceBigWholesale ||
                                 lastCurrentCarPart.PriceSmallWholesale != priceSmallWholesale))
                            {
                                bc.AddCurrentCarPart(equalCarPart, priceDate, priceBase, priceBigWholesale,
                                    priceSmallWholesale);
                            }

                            indexRow++;

                            name = GetValue(sheet.Cells[indexRow, 2].Value);
                            number = GetValue(sheet.Cells[indexRow, 1].Value);
                        }
                    }
                }
            }
        }

        public static void ConvertPriceImport(BusinessContext bc, string path)
        {
            var existingFile = new FileInfo(path);

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var sheet = workBook.Worksheets.First();

                        string date = path.Substring(path.LastIndexOf(" ") + 1, path.LastIndexOf(".") - (path.LastIndexOf(" ") + 1));
                        DateTime priceDate = DateTime.Parse(date);

                        int indexRow = 6;

                        var carParts = bc.GetDirectoryCarParts().ToList();

                        string number = GetValue(sheet.Cells[indexRow, 1].Value);
                        while (!string.IsNullOrWhiteSpace(number))
                        {
                            string article = GetValue(sheet.Cells[indexRow, 2].Value);

                            if (string.IsNullOrWhiteSpace(article) || article == "№ Fenox")
                            {
                                indexRow++;
                                number = GetValue(sheet.Cells[indexRow, 1].Value);
                                continue;
                            }

                            string description = GetValue(sheet.Cells[indexRow, 3].Value);
                            string originalNumber = GetValue(sheet.Cells[indexRow, 4].Value);
                            string material = GetValue(sheet.Cells[indexRow, 5].Value);
                            string attachment = GetValue(sheet.Cells[indexRow, 6].Value);
                            string factoryNumber = GetValue(sheet.Cells[indexRow, 7].Value);
                            string crossNumber = GetValue(sheet.Cells[indexRow, 8].Value);
                            string countInBox = GetValue(sheet.Cells[indexRow, 12].Value);

                            var equalCarPart = carParts.FirstOrDefault(p => p.FullCarPartName == article);
                            if (equalCarPart == null)
                            {
                                equalCarPart = bc.AddDirectoryCarPart(article, null, description, originalNumber,
                                    factoryNumber,
                                    crossNumber, material, attachment, countInBox);
                            }

                            if (equalCarPart.Description == null)
                            {
                                equalCarPart.Description = description;
                                equalCarPart.OriginalNumber = originalNumber;
                                equalCarPart.Note.Material = material;
                                equalCarPart.Note.Attachment = attachment;
                                equalCarPart.FactoryAndCross.FactoryNumber = factoryNumber;
                                equalCarPart.FactoryAndCross.CrossNumber = crossNumber;
                                equalCarPart.CountInBox = countInBox;
                                bc.SaveChanges();
                            }

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 9].Value));

                            var lastCurrentCarPart = bc.GetCurrentCarPart(equalCarPart.Id, priceDate);

                            if (lastCurrentCarPart == null || lastCurrentCarPart.PriceBase != priceBase)
                            {
                                bc.AddCurrentCarPart(equalCarPart, priceDate, priceBase, null, null);
                            }

                            indexRow++;
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

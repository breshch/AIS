using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EntityFramework.BulkInsert.Extensions;

namespace AIS_Enterprise_AV.Helpers.ExcelToDB
{
    public class ConvertingCarPartsExcelToDB
    {
        public static void ConvertImport(BusinessContext bc, string path)
        {
            path = Reports.Helpers.ConvertXlsToXlsx(path);

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

                            if (bc.GetDirectoryCarPart(article, null) == null)
                            {
                                bc.AddDirectoryCarPart(article, null, description, originalNumber, factoryNumber,
                                    crossNumber, material, attachment, countInBox, true);
                            }

                            indexRow++;
                        }
                    }
                }
            }
        }

        public static void ConvertRussian(BusinessContext bc, string path)
        {
            path = Reports.Helpers.ConvertXlsToXlsx(path);

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
                                    crossNumber, material, attachment, countInBox, false);
                            }

                            indexRow++;

                            name = GetValue(sheet.Cells[indexRow, 2].Value);
                            number = GetValue(sheet.Cells[indexRow, 1].Value);
                        }
                    }
                }
            }
        }

        public static DateTime ConvertPriceRus(BusinessContext bc, string path, Currency currency)
        {
            path = Reports.Helpers.ConvertXlsToXlsx(path);

            DateTime priceDate = DateTime.Now;

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
                        priceDate = DateTime.Parse(date);

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

                        string fullName = article + mark;

                        var carParts = bc.GetDirectoryCarParts().ToList();
                        var currentCarParts = bc.GetCurrentCarParts().ToList();

                        var carPartsMemory = new List<DirectoryCarPart>();
                        var currentCarPartsMemory = new List<CurrentCarPart>();

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

                            fullName = article + mark;

                            var equalCarPart = carParts.FirstOrDefault(p => p.FullCarPartName == fullName);
                            if (equalCarPart == null)
                            {
                                //equalCarPart = bc.AddDirectoryCarPart(article, mark, description, originalNumber, factoryNumber,
                                //    crossNumber, material, attachment, countInBox, false);
                                equalCarPart = new DirectoryCarPart
                                {
                                    Article = article,
                                    Mark = mark,
                                    Description = description,
                                    OriginalNumber = originalNumber,
                                    Material = material,
                                    Attachment = attachment,
                                    FactoryNumber = factoryNumber,
                                    CrossNumber = crossNumber,
                                    CountInBox = countInBox,
                                    IsImport = false
                                };

                                carPartsMemory.Add(equalCarPart);
                            }

                            if (equalCarPart.Description == null)
                            {
                                equalCarPart.Description = description;
                                equalCarPart.OriginalNumber = originalNumber;
                                equalCarPart.Material = material;
                                equalCarPart.Attachment = attachment;
                                equalCarPart.CountInBox = countInBox;
                            }

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 9].Value));
                            double? priceBigWholesale = GetValue(sheet.Cells[indexRow, 14].Value) != null
                                ? double.Parse(GetValue(sheet.Cells[indexRow, 14].Value))
                                : default(double?);
                            double? priceSmallWholesale = GetValue(sheet.Cells[indexRow, 15].Value) != null
                                ? double.Parse(GetValue(sheet.Cells[indexRow, 15].Value))
                                : default(double?);

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null ||
                                (lastCurrentCarPart.PriceBase != priceBase ||
                                 lastCurrentCarPart.PriceBigWholesale != priceBigWholesale ||
                                 lastCurrentCarPart.PriceSmallWholesale != priceSmallWholesale))
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(priceDate, priceBase, priceBigWholesale,
                                    priceSmallWholesale, currency, fullName);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;

                            name = GetValue(sheet.Cells[indexRow, 2].Value);
                            number = GetValue(sheet.Cells[indexRow, 1].Value);
                        }

                        bc.DataContext.BulkInsert(carPartsMemory);

                        carParts = bc.GetDirectoryCarParts().ToList();

                        foreach (var currentCarPart in currentCarPartsMemory)
                        {
                            currentCarPart.DirectoryCarPartId = carParts.First(p => p.FullCarPartName == currentCarPart.FullName).Id;
                        }

                        bc.DataContext.BulkInsert(currentCarPartsMemory);
                    }
                }
            }

            return priceDate;
        }

        public static DateTime ConvertPriceImport(BusinessContext bc, string path, Currency currency)
        {
            path = Reports.Helpers.ConvertXlsToXlsx(path);

            DateTime priceDate = DateTime.Now;

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
                        if (!DateTime.TryParse(date, out priceDate))
                        {
                            priceDate = DateTime.Now;
                        }

                        var carParts = bc.GetDirectoryCarParts().ToList();
                        var currentCarParts = bc.GetCurrentCarParts().ToList();

                        var carPartsMemory = new List<DirectoryCarPart>();
                        var currentCarPartsMemory = new List<CurrentCarPart>();


                        int indexRow = 1;
                        int indexBaseColumn = 1;
                        while (true)
                        {
                            bool isFound = false;
                            for (int i = 1; i <= 50; i++)
                            {
                                var value = GetValue(sheet.Cells[i, indexBaseColumn].Value);
                                if (value != null && value.ToLower() == "№ fenox")
                                {
                                    indexRow = i;
                                    isFound = true;
                                    break;
                                }
                            }

                            if (isFound)
                            {
                                break;
                            }

                            indexBaseColumn++;
                        }

                        int factoryNumberColumn = 0;
                        int crossNumberColumn = 0;
                        int countInBoxColumn = 0;
                        int priceBaseColumn = 0;

                        int indexColumn = 3;

                        while (factoryNumberColumn == 0 || crossNumberColumn == 0 || countInBoxColumn == 0 || priceBaseColumn == 0)
                        {
                            var value = GetValue(sheet.Cells[indexRow, indexColumn].Value);
                            if (value != null)
                            {
                                value = value.Trim().ToLower();
                                switch (value)
                                {
                                    case "оригинальные номера":
                                        factoryNumberColumn = indexColumn;
                                        break;
                                    case "аналоги":
                                        crossNumberColumn = indexColumn;
                                        break;
                                    case "групп.упак.шт":
                                    case "групп. упак. шт":
                                        countInBoxColumn = indexColumn;
                                        break;
                                    case "цена rub":
                                    case "цена rub базовая":
                                    case "цена базовая":
                                    case "цена usd":
                                        priceBaseColumn = indexColumn;
                                        break;
                                }
                            }

                            indexColumn++;
                        }

                        indexRow++;
                        if (GetValue(sheet.Cells[indexRow, indexBaseColumn].Value) == null)
                        {
                            indexRow += 2;
                        }

                        string number1 = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
                        string number2 = GetValue(sheet.Cells[indexRow + 1, indexBaseColumn].Value);

                        while (!(string.IsNullOrWhiteSpace(number1) || string.IsNullOrWhiteSpace(number2)))
                        {
                            string checkLine = GetValue(sheet.Cells[indexRow, indexBaseColumn + 1].Value);

                            if (string.IsNullOrWhiteSpace(checkLine) || checkLine == "Наименование")
                            {
                                indexRow++;
                                number1 = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
                                number2 = GetValue(sheet.Cells[indexRow + 1, indexBaseColumn].Value);
                                continue;
                            }

                            string article = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
                            string description = GetValue(sheet.Cells[indexRow, indexBaseColumn + 1].Value);
                            string originalNumber = GetValue(sheet.Cells[indexRow, indexBaseColumn + 2].Value);
                            string material = GetValue(sheet.Cells[indexRow, indexBaseColumn + 3].Value);
                            string attachment = GetValue(sheet.Cells[indexRow, indexBaseColumn + 4].Value);

                            string factoryNumber = GetValue(sheet.Cells[indexRow, factoryNumberColumn].Value);
                            string crossNumber = GetValue(sheet.Cells[indexRow, crossNumberColumn].Value);
                            string countInBox = GetValue(sheet.Cells[indexRow, countInBoxColumn].Value);

                            var equalCarPart = carParts.FirstOrDefault(p => p.FullCarPartName == article);
                            if (equalCarPart == null)
                            {
                                equalCarPart = new DirectoryCarPart
                                {
                                    Article = article,
                                    Mark = null,
                                    Description = description,
                                    OriginalNumber = originalNumber,
                                    Material = material,
                                    Attachment = attachment,
                                    FactoryNumber = factoryNumber,
                                    CrossNumber = crossNumber,
                                    CountInBox = countInBox,
                                    IsImport = true
                                };

                                carPartsMemory.Add(equalCarPart);
                            }

                            if (equalCarPart.Description == null)
                            {
                                equalCarPart.Description = description;
                                equalCarPart.OriginalNumber = originalNumber;
                                equalCarPart.Material = material;
                                equalCarPart.Attachment = attachment;
                                equalCarPart.FactoryNumber = factoryNumber;
                                equalCarPart.CrossNumber = crossNumber;
                                equalCarPart.CountInBox = countInBox;
                                bc.SaveChanges();
                            }

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, priceBaseColumn].Value));

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null || lastCurrentCarPart.PriceBase != priceBase)
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(priceDate, priceBase, null, null, currency, article);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;
                        }

                        bc.DataContext.BulkInsert(carPartsMemory);

                        carParts = bc.GetDirectoryCarParts().ToList();

                        foreach (var currentCarPart in currentCarPartsMemory)
                        {
                            currentCarPart.DirectoryCarPartId = carParts.First(p => p.FullCarPartName == currentCarPart.FullName).Id;
                        }

                        bc.DataContext.BulkInsert(currentCarPartsMemory);
                    }
                }
            }
            return priceDate;
        }

        public static void ConvertingCarPartRemainsToDb(BusinessContext bc, string path)
        {
            path = Reports.Helpers.ConvertXlsToXlsx(path);

            var existingFile = new FileInfo(path);

            var monthes = new Dictionary<string, int>
            {
                { "Январь 2014", 1 },
                { "Февраль 2014", 2 },
                { "Март 2014", 3 },
                { "Апрель 2014", 4 },
                { "Май 2014", 5 },
                { "Июнь 2014", 6 },
                { "Июль 2014", 7 },
                { "Август 2014", 8 },
                { "Сентябрь 2014", 9 },
                { "Октябрь 2014", 10 },
                { "Ноябрь 2014", 11 },
                { "Декабрь 2014", 12 },
            };

            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        foreach (var month in monthes.Where(m => m.Value >= 12))
                        {
                            var sheet = workBook.Worksheets.First(s => s.Name == month.Key);

                            var carParts = bc.GetDirectoryCarParts().ToList();
                            var excelCarParts = new List<DirectoryCarPart>();
                            var excelRemains = new List<InfoLastMonthDayRemain>();

                            int indexRow = 3;

                            using (var sw = new StreamWriter("articles.txt"))
                            {
                                sw.WriteLine();
                            }

                            var date = new DateTime(2014, month.Value, 01);
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


                                var equalCarPart = carParts.FirstOrDefault(p => ((p.Article + p.Mark) == article) ||
                                                                                (p.Mark == null &&
                                                                                 p.Article == newArticle));
                                if (equalCarPart == null)
                                {
                                    equalCarPart = bc.AddDirectoryCarPart(newArticle, mark,
                                        GetValue(sheet.Cells[indexRow, 5].Value),
                                        null, null, null, null, null, null, string.IsNullOrWhiteSpace(mark));

                                    using (var sw = new StreamWriter("articles.txt", true))
                                    {
                                        sw.WriteLine(equalCarPart.Article + equalCarPart.Mark);
                                    }
                                }

                                int remain = int.Parse(GetValue(sheet.Cells[indexRow, 6].Value) ?? "0");

                                excelCarParts.Add(equalCarPart);
                                excelRemains.Add(new InfoLastMonthDayRemain
                                {
                                    Count = remain,
                                    DirectoryCarPartId = equalCarPart.Id,
                                    Date = date
                                });

                                indexRow++;
                                article = GetValue(sheet.Cells[indexRow, 1].Value);
                            }

                            int indexColumn = 7;

                            var containers = new List<InfoContainer>();

                            while (GetValue(sheet.Cells[2, indexColumn].Value) != "И того приход")
                            {
                                var containerName = GetValue(sheet.Cells[2, indexColumn].Value);
                                if (containerName != null)
                                {
                                    if (!containerName.Contains("от"))
                                    {
                                        indexColumn++;
                                        continue;
                                    }
                                    string name = containerName.Substring(0, containerName.IndexOf("от")).Trim();
                                    DateTime dateContainer = DateTime.Parse(containerName.Substring(containerName.IndexOf("от") + 3,
                                        containerName.IndexOf(" ", containerName.IndexOf("от") + 3) - (containerName.IndexOf("от") + 3))
                                        .Replace(",", ".").Trim(new[] { ' ', '.' }));
                                    string description = containerName.Substring(containerName.IndexOf("от") + 11).Replace(",", " ").Trim();

                                    var containerCarParts = new List<CurrentContainerCarPart>();
                                    for (int row = 3; row < 1227; row++)
                                    {
                                        var countCarPart = GetValue(sheet.Cells[row, indexColumn].Value);
                                        if (countCarPart != null)
                                        {
                                            containerCarParts.Add(new CurrentContainerCarPart
                                            {
                                                CountCarParts = int.Parse(countCarPart),
                                                DirectoryCarPartId = excelCarParts[row - 3].Id
                                            });
                                        }
                                    }

                                    var container = new InfoContainer
                                    {
                                        DatePhysical = dateContainer,
                                        DateOrder = dateContainer,
                                        IsIncoming = true,
                                        Name = name,
                                        Description = description,
                                        CarParts = containerCarParts
                                    };
                                    containers.Add(container);
                                }
                                indexColumn++;
                            }

                            indexColumn++;

                            while (GetValue(sheet.Cells[2, indexColumn].Value) != "Расход")
                            {
                                var containerName = GetValue(sheet.Cells[2, indexColumn].Value);
                                if (containerName != null)
                                {
                                    if (!containerName.Contains("от"))
                                    {
                                        indexColumn++;
                                        continue;
                                    }
                                    string name = containerName.Substring(0, containerName.IndexOf("от")).Trim();
                                    DateTime dateContainer =
                                        DateTime.Parse(containerName.Substring(containerName.IndexOf("от") + 3,
                                            containerName.IndexOf(" ", containerName.IndexOf("от") + 3) -
                                            (containerName.IndexOf("от") + 3))
                                            .Replace(",", ".").Trim(new[] { ' ', '.' }));
                                    string description =
                                        containerName.Substring(containerName.IndexOf("от") + 11)
                                            .Replace(",", " ")
                                            .Trim();

                                    var containerCarParts = new List<CurrentContainerCarPart>();
                                    for (int row = 3; row < 1227; row++)
                                    {
                                        var countCarPart = GetValue(sheet.Cells[row, indexColumn].Value);
                                        if (countCarPart != null)
                                        {
                                            containerCarParts.Add(new CurrentContainerCarPart
                                            {
                                                CountCarParts = int.Parse(countCarPart),
                                                DirectoryCarPartId = excelCarParts[row - 3].Id,
                                            });
                                        }
                                    }


                                    var container = new InfoContainer
                                    {
                                        DatePhysical = dateContainer,
                                        DateOrder = dateContainer,
                                        IsIncoming = false,
                                        Name = name,
                                        Description = description,
                                        CarParts = containerCarParts
                                    };
                                    containers.Add(container);
                                }
                                indexColumn++;
                            }

                            bc.DataContext.BulkInsert(excelRemains);
                            bc.DataContext.BulkInsert(containers);

                            var containersDB = bc.GetInfoContainers(containers).ToList();

                            foreach (var container in containersDB)
                            {
                                var c = container;

                                bc.DataContext.BulkInsert(c.CarParts.Select(p => new CurrentContainerCarPart
                                {
                                    CountCarParts = p.CountCarParts,
                                    DirectoryCarPartId = p.DirectoryCarPartId,
                                    InfoContainerId = c.Id
                                }));
                            }
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


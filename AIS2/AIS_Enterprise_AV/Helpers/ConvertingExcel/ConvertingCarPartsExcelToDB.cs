using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using DocumentFormat.OpenXml.Drawing.Charts;
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

        private const int WM_CLOSE = 16;
        private const uint WM_COMMAND = 0x0111;
        private const int BN_CLICKED = 245;
        private const int IDOK = 1;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //[DllImport("coredll.dll")]
        //private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void PushButtons()
        {
            Task.Factory.StartNew(() =>
            {
                bool isFirst = false;
                while (true)
                {
                    IntPtr hwnd;
                    IntPtr hwndChild = IntPtr.Zero;

                    //Get a handle for the Calculator Application main window
                    hwnd = FindWindow(null, "Microsoft Excel");
                    if (hwnd != IntPtr.Zero)
                    {
                        SetForegroundWindow(hwnd);
                        SendKeys.SendWait("{ENTER}");

                        if (isFirst)
                        {
                            break;
                        }
                        isFirst = true;
                    }

                    Thread.Sleep(500);
                }
            });
        }


        public static DateTime ConvertPriceRus(BusinessContext bc, string path)
        {
            DateTime priceDate = DateTime.Now;

            if (Path.GetExtension(path).Count() == 4)
            {
                PushButtons();

                var app = new Microsoft.Office.Interop.Excel.Application();
                var wb = app.Workbooks.Open(path);
                
                wb.SaveAs(
                    Filename: path + "x",
                    FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                wb.Close();
                app.Quit();
                File.Delete(path);
                path += "x";
            }

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
                        var currentCarParts = bc.GetCurrentCarParts().ToList();

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
                            }

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 9].Value));
                            double priceBigWholesale = double.Parse(GetValue(sheet.Cells[indexRow, 14].Value));
                            double priceSmallWholesale = double.Parse(GetValue(sheet.Cells[indexRow, 15].Value));

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null ||
                                (lastCurrentCarPart.PriceBase != priceBase ||
                                 lastCurrentCarPart.PriceBigWholesale != priceBigWholesale ||
                                 lastCurrentCarPart.PriceSmallWholesale != priceSmallWholesale))
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(equalCarPart.Id, priceDate, priceBase, priceBigWholesale,
                                    priceSmallWholesale);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;

                            name = GetValue(sheet.Cells[indexRow, 2].Value);
                            number = GetValue(sheet.Cells[indexRow, 1].Value);
                        }

                        bc.DataContext.BulkInsert(currentCarPartsMemory);
                        bc.SaveChanges();
                    }
                }
            }

            return priceDate;
        }

        public static DateTime ConvertPriceImport(BusinessContext bc, string path)
        {
            DateTime priceDate = DateTime.Now;

            if (Path.GetExtension(path).Count() == 4)
            {
                PushButtons();

                var app = new Microsoft.Office.Interop.Excel.Application();
                var wb = app.Workbooks.Open(path);

                wb.SaveAs(
                    Filename: path + "x",
                    FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                wb.Close();
                app.Quit();
                File.Delete(path);
                path += "x";
            }

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

                        var currentCarPartsMemory = new List<CurrentCarPart>();


                        int indexRow = 1;
                        while (true)
                        {
                            var value = GetValue(sheet.Cells[indexRow, 2].Value);
                            if (value != null && value.ToLower() == "№ fenox")
                            {
                                break;
                            }
                            indexRow++;
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
                                        priceBaseColumn = indexColumn;
                                        break;
                                }
                            }

                            indexColumn++;
                        }

                        indexRow++;
                        if (GetValue(sheet.Cells[indexRow,2].Value) == null)
                        {
                            indexRow += 2;
                        }

                        string number1 = GetValue(sheet.Cells[indexRow, 2].Value);
                        string number2 = GetValue(sheet.Cells[indexRow + 1, 2].Value);

                        while (!(string.IsNullOrWhiteSpace(number1) || string.IsNullOrWhiteSpace(number2)))
                        {
                            string checkLine = GetValue(sheet.Cells[indexRow, 3].Value);

                            if (string.IsNullOrWhiteSpace(checkLine) || checkLine == "Наименование")
                            {
                                indexRow++;
                                number1 = GetValue(sheet.Cells[indexRow, 2].Value);
                                number2 = GetValue(sheet.Cells[indexRow + 1, 2].Value);
                                continue;
                            }

                            string article = GetValue(sheet.Cells[indexRow, 2].Value);
                            string description = GetValue(sheet.Cells[indexRow, 3].Value);
                            string originalNumber = GetValue(sheet.Cells[indexRow, 4].Value);
                            string material = GetValue(sheet.Cells[indexRow, 5].Value);
                            string attachment = GetValue(sheet.Cells[indexRow, 6].Value);

                            string factoryNumber = GetValue(sheet.Cells[indexRow, factoryNumberColumn].Value);
                            string crossNumber = GetValue(sheet.Cells[indexRow, crossNumberColumn].Value);
                            string countInBox = GetValue(sheet.Cells[indexRow, countInBoxColumn].Value);

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

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, priceBaseColumn].Value));

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null || lastCurrentCarPart.PriceBase != priceBase)
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(equalCarPart.Id, priceDate, priceBase, null, null);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;
                        }

                        bc.DataContext.BulkInsert(currentCarPartsMemory);
                        bc.SaveChanges();
                    }
                }
            }
            return priceDate;
        }


        private static string GetValue(object parameter)
        {
            return parameter != null ? parameter.ToString() : null;
        }
    }
}

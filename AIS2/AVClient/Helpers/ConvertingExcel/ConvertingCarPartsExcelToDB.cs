using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AVClient.AVServiceReference;
using OfficeOpenXml;

namespace AVClient.Helpers.ConvertingExcel
{
    public static class ConvertingCarPartsExcelToDB
    {
	    private static AVBusinessLayerClient bc = ServerConnector.GetInstanse;

        public static void ConvertImport(string path)
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

        public static void ConvertRussian(string path)
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

        public static DateTime ConvertPriceRus(string path, Currency currency)
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
						var sheet = workBook.Worksheets.First(s => s.Name == "Отечка_RUB");

						string date = path.Substring(path.LastIndexOf("(") + 1, 10);
						if (!DateTime.TryParse(date, out priceDate))
						{
							throw new Exception();
						}

                        int indexRow = 7;

                        string header = GetValue(sheet.Cells[indexRow, 4].Value);
                        string carPartName = GetValue(sheet.Cells[indexRow, 1].Value);

                        string article = GetValue(sheet.Cells[indexRow, 2].Value);
                        string mark = GetValue(sheet.Cells[indexRow, 3].Value);
						string description = GetValue(sheet.Cells[indexRow, 5].Value);
                        string material = GetValue(sheet.Cells[indexRow, 8].Value);
                        string attachment = GetValue(sheet.Cells[indexRow, 9].Value);
                        string factoryNumber = null;
                        string crossNumber = null;
                        string countInBox = GetValue(sheet.Cells[indexRow, 13].Value);
						string originalNumber = GetValue(sheet.Cells[indexRow, 18].Value);

                        string fullName = article + mark;

                        var carParts = bc.GetDirectoryCarParts().ToList();
                        var currentCarParts = bc.GetCurrentCarParts().ToList();

                        var carPartsMemory = new List<DirectoryCarPart>();
                        var currentCarPartsMemory = new List<CurrentCarPart>();

                        while (!(string.IsNullOrWhiteSpace(carPartName) && string.IsNullOrWhiteSpace(header)))
                        {
                            if (string.IsNullOrWhiteSpace(carPartName) && !string.IsNullOrWhiteSpace(header))
                            {
                                indexRow++;
								header = GetValue(sheet.Cells[indexRow, 4].Value);
								carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
                                continue;
                            }

                            description = GetValue(sheet.Cells[indexRow, 5].Value) ?? description;
                            originalNumber = GetValue(sheet.Cells[indexRow, 18].Value) ?? originalNumber;
                            article = GetValue(sheet.Cells[indexRow, 2].Value) ?? article;
                            mark = GetValue(sheet.Cells[indexRow, 3].Value) ?? mark;
                            material = GetValue(sheet.Cells[indexRow, 8].Value) ?? material;
                            attachment = GetValue(sheet.Cells[indexRow, 9].Value) ?? attachment;
                            countInBox = GetValue(sheet.Cells[indexRow, 13].Value) ?? countInBox;

                            fullName = article + mark;

                            var equalCarPart = carParts.FirstOrDefault(p => p.FullCarPartName == fullName);
                            if (equalCarPart == null)
                            {
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

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 10].Value));
							//double? priceBigWholesale = GetValue(sheet.Cells[indexRow, 16].Value) != null
							//	? double.Parse(GetValue(sheet.Cells[indexRow, 16].Value))
							//	: default(double?);
							//double? priceSmallWholesale = GetValue(sheet.Cells[indexRow, 17].Value) != null
							//	? double.Parse(GetValue(sheet.Cells[indexRow, 17].Value))
							//	: default(double?);

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null ||
                                (lastCurrentCarPart.PriceBase != priceBase ))
								// lastCurrentCarPart.PriceBigWholesale != priceBigWholesale ||
								// lastCurrentCarPart.PriceSmallWholesale != priceSmallWholesale))
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(priceDate, priceBase, null,
                                    null, currency, fullName);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;

							header = GetValue(sheet.Cells[indexRow, 4].Value);
							carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
                        }

                        bc.AddDirectoryCarParts(carPartsMemory.ToArray());

                        carParts = bc.GetDirectoryCarParts().ToList();

                        foreach (var currentCarPart in currentCarPartsMemory)
                        {
                            currentCarPart.DirectoryCarPartId = carParts.First(p => p.FullCarPartName == currentCarPart.FullName).Id;
                        }

                        bc.AddCurrentCarParts(currentCarPartsMemory.ToArray());
                    }
                }
            }

            return priceDate;
        }

        public static DateTime ConvertPriceImport(string path, Currency currency)
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
						var sheet = workBook.Worksheets.First(s => s.Name == "Иномарка_RUB");

						string date = path.Substring(path.LastIndexOf("(") + 1, 10);
						if (!DateTime.TryParse(date, out priceDate))
						{
							throw new Exception();
						}

						int indexRow = 7;

						string header = GetValue(sheet.Cells[indexRow, 2].Value);
						string carPartName = GetValue(sheet.Cells[indexRow, 1].Value);

						string article = GetValue(sheet.Cells[indexRow, 2].Value);
						string description = GetValue(sheet.Cells[indexRow, 3].Value);
						string material = GetValue(sheet.Cells[indexRow, 6].Value);
						//string factoryNumber = GetValue(sheet.Cells[indexRow, 9].Value);
						string crossNumber = GetValue(sheet.Cells[indexRow, 14].Value);
						string countInBox = GetValue(sheet.Cells[indexRow, 10].Value);
						string originalNumber = GetValue(sheet.Cells[indexRow, 13].Value);

                        var carParts = bc.GetDirectoryCarParts().ToList();
                        var currentCarParts = bc.GetCurrentCarParts().ToList();

                        var carPartsMemory = new List<DirectoryCarPart>();
                        var currentCarPartsMemory = new List<CurrentCarPart>();


						//int indexRow = 1;
						//int indexBaseColumn = 1;
						//while (true)
						//{
						//	bool isFound = false;
						//	for (int i = 1; i <= 50; i++)
						//	{
						//		var value = GetValue(sheet.Cells[i, indexBaseColumn].Value);
						//		if (value != null && value.ToLower() == "№ fenox")
						//		{
						//			indexRow = i;
						//			isFound = true;
						//			break;
						//		}
						//	}

						//	if (isFound)
						//	{
						//		break;
						//	}

						//	indexBaseColumn++;
						//}

						//int factoryNumberColumn = 0;
						//int crossNumberColumn = 0;
						//int countInBoxColumn = 0;
						//int priceBaseColumn = 0;

						//int indexColumn = 3;

						//while (factoryNumberColumn == 0 || crossNumberColumn == 0 || countInBoxColumn == 0 || priceBaseColumn == 0)
						//{
						//	var value = GetValue(sheet.Cells[indexRow, indexColumn].Value);
						//	if (value != null)
						//	{
						//		value = value.Trim().ToLower();
						//		switch (value)
						//		{
						//			case "оригинальные номера":
						//				factoryNumberColumn = indexColumn;
						//				break;
						//			case "аналоги":
						//				crossNumberColumn = indexColumn;
						//				break;
						//			case "групп.упак.шт":
						//			case "групп. упак. шт":
						//				countInBoxColumn = indexColumn;
						//				break;
						//			case "цена rub":
						//			case "цена rub базовая":
						//			case "цена базовая":
						//			case "цена usd":
						//				priceBaseColumn = indexColumn;
						//				break;
						//		}
						//	}

						//	indexColumn++;
						//}

						//indexRow++;
						//if (GetValue(sheet.Cells[indexRow, indexBaseColumn].Value) == null)
						//{
						//	indexRow += 2;
						//}

						//string number1 = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
						//string number2 = GetValue(sheet.Cells[indexRow + 1, indexBaseColumn].Value);

						while (!(string.IsNullOrWhiteSpace(carPartName) && string.IsNullOrWhiteSpace(header)))
						{
							if (string.IsNullOrWhiteSpace(carPartName) && !string.IsNullOrWhiteSpace(header))
							{
								indexRow++;
								header = GetValue(sheet.Cells[indexRow, 2].Value);
								carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
								continue;
							}

							description = GetValue(sheet.Cells[indexRow, 3].Value) ?? description;
							originalNumber = GetValue(sheet.Cells[indexRow, 13].Value) ?? originalNumber;
							article = GetValue(sheet.Cells[indexRow, 2].Value) ?? article;
							material = GetValue(sheet.Cells[indexRow, 6].Value) ?? material;
							countInBox = GetValue(sheet.Cells[indexRow, 10].Value) ?? countInBox;
							crossNumber = GetValue(sheet.Cells[indexRow, 14].Value);

							//string article = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
							//string description = GetValue(sheet.Cells[indexRow, indexBaseColumn + 1].Value);
							//string originalNumber = GetValue(sheet.Cells[indexRow, indexBaseColumn + 2].Value);
							//string material = GetValue(sheet.Cells[indexRow, indexBaseColumn + 3].Value);
							//string attachment = GetValue(sheet.Cells[indexRow, indexBaseColumn + 4].Value);

							//string factoryNumber = GetValue(sheet.Cells[indexRow, factoryNumberColumn].Value);
							//string crossNumber = GetValue(sheet.Cells[indexRow, crossNumberColumn].Value);
							//string countInBox = GetValue(sheet.Cells[indexRow, countInBoxColumn].Value);

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
                                equalCarPart.CrossNumber = crossNumber;
                                equalCarPart.CountInBox = countInBox;
                                //bc.SaveChanges(); TODO Refactor
                            }

                            double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 7].Value));

                            var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
                                .OrderByDescending(c => c.Date)
                                .FirstOrDefault(c => priceDate.Date >= c.Date);

                            if (lastCurrentCarPart == null || lastCurrentCarPart.PriceBase != priceBase)
                            {
                                var currentCarPart = bc.AddCurrentCarPartNoSave(priceDate, priceBase, null, null, currency, article);

                                currentCarPartsMemory.Add(currentCarPart);
                            }

                            indexRow++;

							header = GetValue(sheet.Cells[indexRow, 2].Value);
							carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
                        }

                        bc.AddDirectoryCarParts(carPartsMemory.ToArray());

                        carParts = bc.GetDirectoryCarParts().ToList();

                        foreach (var currentCarPart in currentCarPartsMemory)
                        {
                            currentCarPart.DirectoryCarPartId = carParts.First(p => p.FullCarPartName == currentCarPart.FullName).Id;
                        }

                        bc.AddCurrentCarParts(currentCarPartsMemory.ToArray());
                    }
                }
            }
            return priceDate;
		}

		public static DateTime ConvertPriceLiquides(string path, Currency currency)
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
						var sheet = workBook.Worksheets.First(s => s.Name == "Жидкости, смазки, щетки");

						string date = path.Substring(path.LastIndexOf("(") + 1, 10);
						if (!DateTime.TryParse(date, out priceDate))
						{
							throw new Exception();
						}

						int indexRow = 7;

						string header = GetValue(sheet.Cells[indexRow, 2].Value);
						string carPartName = GetValue(sheet.Cells[indexRow, 1].Value);

						string article = GetValue(sheet.Cells[indexRow, 2].Value);
						string description = GetValue(sheet.Cells[indexRow, 3].Value);
						string material = GetValue(sheet.Cells[indexRow, 6].Value);
						//string factoryNumber = GetValue(sheet.Cells[indexRow, 9].Value);
						string crossNumber = GetValue(sheet.Cells[indexRow, 14].Value);
						string countInBox = GetValue(sheet.Cells[indexRow, 10].Value);
						string originalNumber = GetValue(sheet.Cells[indexRow, 13].Value);

						var carParts = bc.GetDirectoryCarParts().ToList();
						var currentCarParts = bc.GetCurrentCarParts().ToList();

						var carPartsMemory = new List<DirectoryCarPart>();
						var currentCarPartsMemory = new List<CurrentCarPart>();


						//int indexRow = 1;
						//int indexBaseColumn = 1;
						//while (true)
						//{
						//	bool isFound = false;
						//	for (int i = 1; i <= 50; i++)
						//	{
						//		var value = GetValue(sheet.Cells[i, indexBaseColumn].Value);
						//		if (value != null && value.ToLower() == "№ fenox")
						//		{
						//			indexRow = i;
						//			isFound = true;
						//			break;
						//		}
						//	}

						//	if (isFound)
						//	{
						//		break;
						//	}

						//	indexBaseColumn++;
						//}

						//int factoryNumberColumn = 0;
						//int crossNumberColumn = 0;
						//int countInBoxColumn = 0;
						//int priceBaseColumn = 0;

						//int indexColumn = 3;

						//while (factoryNumberColumn == 0 || crossNumberColumn == 0 || countInBoxColumn == 0 || priceBaseColumn == 0)
						//{
						//	var value = GetValue(sheet.Cells[indexRow, indexColumn].Value);
						//	if (value != null)
						//	{
						//		value = value.Trim().ToLower();
						//		switch (value)
						//		{
						//			case "оригинальные номера":
						//				factoryNumberColumn = indexColumn;
						//				break;
						//			case "аналоги":
						//				crossNumberColumn = indexColumn;
						//				break;
						//			case "групп.упак.шт":
						//			case "групп. упак. шт":
						//				countInBoxColumn = indexColumn;
						//				break;
						//			case "цена rub":
						//			case "цена rub базовая":
						//			case "цена базовая":
						//			case "цена usd":
						//				priceBaseColumn = indexColumn;
						//				break;
						//		}
						//	}

						//	indexColumn++;
						//}

						//indexRow++;
						//if (GetValue(sheet.Cells[indexRow, indexBaseColumn].Value) == null)
						//{
						//	indexRow += 2;
						//}

						//string number1 = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
						//string number2 = GetValue(sheet.Cells[indexRow + 1, indexBaseColumn].Value);

						while (!(string.IsNullOrWhiteSpace(carPartName) && string.IsNullOrWhiteSpace(header)))
						{
							if (string.IsNullOrWhiteSpace(carPartName) && !string.IsNullOrWhiteSpace(header))
							{
								indexRow++;
								header = GetValue(sheet.Cells[indexRow, 2].Value);
								carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
								continue;
							}

							description = GetValue(sheet.Cells[indexRow, 3].Value) ?? description;
							originalNumber = GetValue(sheet.Cells[indexRow, 13].Value) ?? originalNumber;
							article = GetValue(sheet.Cells[indexRow, 2].Value) ?? article;
							material = GetValue(sheet.Cells[indexRow, 6].Value) ?? material;
							countInBox = GetValue(sheet.Cells[indexRow, 10].Value) ?? countInBox;
							crossNumber = GetValue(sheet.Cells[indexRow, 14].Value);

							//string article = GetValue(sheet.Cells[indexRow, indexBaseColumn].Value);
							//string description = GetValue(sheet.Cells[indexRow, indexBaseColumn + 1].Value);
							//string originalNumber = GetValue(sheet.Cells[indexRow, indexBaseColumn + 2].Value);
							//string material = GetValue(sheet.Cells[indexRow, indexBaseColumn + 3].Value);
							//string attachment = GetValue(sheet.Cells[indexRow, indexBaseColumn + 4].Value);

							//string factoryNumber = GetValue(sheet.Cells[indexRow, factoryNumberColumn].Value);
							//string crossNumber = GetValue(sheet.Cells[indexRow, crossNumberColumn].Value);
							//string countInBox = GetValue(sheet.Cells[indexRow, countInBoxColumn].Value);

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
								equalCarPart.CrossNumber = crossNumber;
								equalCarPart.CountInBox = countInBox;
								//bc.SaveChanges(); TODO Refactor
							}

							double priceBase = double.Parse(GetValue(sheet.Cells[indexRow, 7].Value));

							var lastCurrentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == equalCarPart.Id)
								.OrderByDescending(c => c.Date)
								.FirstOrDefault(c => priceDate.Date >= c.Date);

							if (lastCurrentCarPart == null || lastCurrentCarPart.PriceBase != priceBase)
							{
								var currentCarPart = bc.AddCurrentCarPartNoSave(priceDate, priceBase, null, null, currency, article);

								currentCarPartsMemory.Add(currentCarPart);
							}

							indexRow++;

							header = GetValue(sheet.Cells[indexRow, 2].Value);
							carPartName = GetValue(sheet.Cells[indexRow, 1].Value);
						}

						bc.AddDirectoryCarParts(carPartsMemory.ToArray());

						carParts = bc.GetDirectoryCarParts().ToList();

						foreach (var currentCarPart in currentCarPartsMemory)
						{
							currentCarPart.DirectoryCarPartId = carParts.First(p => p.FullCarPartName == currentCarPart.FullName).Id;
						}

						bc.AddCurrentCarParts(currentCarPartsMemory.ToArray());
					}
				}
			}
			return priceDate;
		}

        public static void ConvertingCarPartRemainsToDb(string path)
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
                        for (var date = new DateTime(2015, 3, 1); date <= new DateTime(2015, 6, 1); date = date.AddMonths(1))
                        {
                            var sheet = workBook.Worksheets.First(s => s.Name == date.ToString("MMMM yyyy"));

                            var carParts = bc.GetDirectoryCarParts().ToList();
                            var excelCarParts = new List<DirectoryCarPart>();
                            var excelRemains = new List<InfoLastMonthDayRemain>();

                            int indexRow = 3;

                            using (var sw = new StreamWriter("articles.txt"))
                            {
                                sw.WriteLine();
                            }

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
                                        .Replace(",", ".").Trim(' ', '.'));
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
                                        CarParts = containerCarParts.ToArray()
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
                                            .Replace(",", ".").Trim(' ', '.'));
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
                                                DirectoryCarPartId = excelCarParts[row - 3].Id
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
                                        CarParts = containerCarParts.ToArray()
                                    };
                                    containers.Add(container);
                                }
                                indexColumn++;
                            }

                            bc.RemoveInfoLastMonthDayRemains(date.Year, date.Month);
                            bc.AddInfoLastMonthDayRemains(excelRemains.ToArray());

                            bc.RemoveContainers(date.Year, date.Month);
                            bc.AddInfoContainers(containers.ToArray());

                            var containersDB = bc.GetInfoContainers(containers.ToArray()).ToList();

                            foreach (var container in containersDB)
                            {
                                var c = container;

                                bc.AddCurrentContainerCarParts(c.CarParts.Select(p => new CurrentContainerCarPart
                                {
                                    CountCarParts = p.CountCarParts,
                                    DirectoryCarPartId = p.DirectoryCarPartId,
                                    InfoContainerId = c.Id
                                }).ToArray());
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


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AIS_Enterprise_Data;
using Numerizr;
using OfficeOpenXml;

namespace AIS_Enterprise_AV.Reports
{
    public static class CarsReports
    {
        private const string PATH_DIRECTORY_CARS_REPORTS = "Reports\\Cars";

        public static void Cars(BusinessContext bc, DateTime from, DateTime to)
        {
            string path = Path.Combine(PATH_DIRECTORY_CARS_REPORTS, "Cars.xlsx");
            Helpers.CompletedReport(path, new List<Action<ExcelPackage>>
                {
                    (ep) =>
                    {
                        for (DateTime date = from; date.Date <= to.Date; date = date.AddMonths(1))
                        {
                            CarsReport(ep, bc, date.Year, date.Month);    
                        }
                    }
                });
        }

        private static void CarsReport(ExcelPackage ep, BusinessContext bc, int year, int month)
        {
            string name = month + "'" + year;
            var sheet = Helpers.GetSheet(ep, name);
           
            var colorGray = Color.LightGray;


            Helpers.CreateCell(sheet, 1, 1, "Дата", colorGray);
            Helpers.CreateCell(sheet, 1, 2, "Логистикон", colorGray);
            Helpers.CreateCell(sheet, 1, 3, "Кузин", colorGray);
            Helpers.CreateCell(sheet, 1, 4, "Павловский Посад", colorGray);


            var costs = bc.GetInfoCostsTransportAndNoAllAndExpenseOnly(year, month).ToList();
            var costsGroups = costs.GroupBy(c => c.GroupId).Select(g => new { Summ = g.Sum(c => c.Summ), 
                TransportCompany = g.First().DirectoryTransportCompany != null ? g.First().DirectoryTransportCompany.Name : "", Date = g.First().Date });

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                var date = new DateTime(year, month, i);
                Helpers.CreateCell(sheet, i + 1, 1, date.ToShortDateString(),colorGray);
                
                int countGazelL = 0;
                int countValdayL = 0;

                int countGazelK = 0;
                int countFotonK = 0;

                int countGazelP = 0;
                int countValdayP = 0;


                var costsDate = costsGroups.Where(c => c.Date.Date == new DateTime(year, month, i).Date);


                foreach (var cost in costsDate)
                {
                    switch (cost.TransportCompany)
                    {
                        case "Логистикон":
                            if (cost.Summ == 4000)
                            {
                                countGazelL++;
                            }
                            else if (cost.Summ == 7000)
                            {
                                countValdayL++;
                            }
                            break;
                        case "Кузин":
                            if (cost.Summ == 4000)
                            {
                                countGazelK++;
                            }
                            else if (cost.Summ == 7000)
                            {
                                countFotonK++;
                            }
                            break;
                        case "Павловский Посад":
                            if (cost.Summ == 4000)
                            {
                                countGazelP++;
                            }
                            else if (cost.Summ == 7000)
                            {
                                countValdayP++;
                            }
                            break;
                    }
                }


                string valueL = "";
                string valueK = "";
                string valueP = "";

                if (countGazelL > 0)
                {
                    valueL += GetNumerizedGazel(countGazelL);
                }

                if (countValdayL > 0)
                {
                    if (countGazelL > 0)
	                {
                        valueL += " + ";
	                }

                    valueL += GetNumerizedValday(countValdayL);
                }

                if (countGazelK > 0)
                {
                    valueK += GetNumerizedGazel(countGazelK);
                }

                if (countFotonK > 0)
                {
                    if (countGazelK > 0)
                    {
                        valueK += " + ";
                    }

                    valueK += GetNumerizedFoton(countFotonK);
                }

                if (countGazelP > 0)
                {
                    valueP += GetNumerizedGazel(countGazelP);
                }

                if (countValdayP > 0)
                {
                    if (countGazelP > 0)
                    {
                        valueP += " + ";
                    }

                    valueP += GetNumerizedValday(countValdayP);
                }


                Helpers.CreateCell(sheet, i + 1, 2, valueL, colorGray);
                Helpers.CreateCell(sheet, i + 1, 3, valueK, colorGray);
                Helpers.CreateCell(sheet, i + 1, 4, valueP, colorGray);

            }


            for (int i = 1 ; i <= 4; i++)
			{
                sheet.Column(i).Width = Helpers.PixelsToInches(200);
			}


            
        }

        private static string GetNumerizedGazel(int count)
        {
            return count + " " + NumerizrFactory.Numerize("ru", count, "газель", "газели", "газелей");
        }

        private static string GetNumerizedValday(int count)
        {
            return count + " " + NumerizrFactory.Numerize("ru", count, "валдай", "валдая", "валдаев");
        }

        private static string GetNumerizedFoton(int count)
        {
            return count + " " + NumerizrFactory.Numerize("ru", count, "фотон", "фотона", "фотонов");
        }
    }
}

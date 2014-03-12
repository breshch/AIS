using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    public static class HelperCalendar
    {
        public static void InputDateToDataBase(int year)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Calendar", year.ToString() + ".txt");

            using (var dc = new DataContext())
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line[11] == 'в')
                        {
                            var date = DateTime.Parse(line.Substring(0, 10));


                            if (!dc.DirectoryHolidays.Select(h => h.Date).Contains(date))
                            {
                                var directoryHoliday = new DirectoryHoliday { Date = date };
                                dc.DirectoryHolidays.Add(directoryHoliday);
                            }
                        }
                    }
                }
                dc.SaveChanges();
            }
        }

        public static int GetCountWorkDaysInMonth(int year, int month)
        {
            using (var dc = new DataContext())
            {
                int holiDays = dc.DirectoryHolidays.Where(h => h.Date.Year == year && h.Date.Month == month).Count();
                return DateTime.DaysInMonth(year, month) - holiDays;
            }
        }
    }
}

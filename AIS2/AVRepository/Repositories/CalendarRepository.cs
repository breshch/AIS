using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Directories;

namespace AVRepository.Repositories
{
	public class CalendarRepository :BaseRepository
	{
		public int GetCountWorkDaysInMonth(int year, int month)
		{
			using (var db = GetContext())
			{
				int holiDays = db.DirectoryHolidays.Count(h => h.Date.Year == year && h.Date.Month == month);
				return DateTime.DaysInMonth(year, month) - holiDays;
			}
		}

		public DateTime[] GetHolidays(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.DirectoryHolidays
					.Where(h => h.Date.Year == year && h.Date.Month == month)
					.Select(h => h.Date)
					.ToArray();
			}
		}

		public DateTime[] GetHolidays(int year)
		{
			using (var db = GetContext())
			{
				return db.DirectoryHolidays
					.Where(h => h.Date.Year == year)
					.Select(h => h.Date)
					.ToArray();
			}
		}

		public DateTime[] GetHolidays(DateTime fromDate, DateTime toDate)
		{
			using (var db = GetContext())
			{
				return db.DirectoryHolidays
					.Where(h => DbFunctions.DiffDays(h.Date, fromDate) <= 0 &&
								DbFunctions.DiffDays(h.Date, toDate) >= 0)
					.Select(h => h.Date)
					.ToArray();
			}
		}

		public bool IsWeekend(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.DirectoryHolidays
					.Any(w => DbFunctions.DiffDays(w.Date, date) == 0);
			}
		}

		public void SetHolidays(int year, List<DateTime> holidays)
		{
			var holidaysInDB = GetHolidays(year).ToList();

			using (var db = GetContext())
			{
				foreach (var holiday in holidays)
				{
					if (holidaysInDB.All(h => h.Date != holiday.Date))
					{
						db.DirectoryHolidays.Add(new DirectoryHoliday { Date = holiday });
					}
				}

				db.SaveChanges();
			}
		}

	}
}

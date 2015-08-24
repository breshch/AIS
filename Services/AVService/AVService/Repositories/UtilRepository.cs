using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Helpers;
using Shared.Enums;

namespace AVService.Repositories
{
	public class UtilRepository : BaseRepository
	{
		public void EditParameter(ParameterType parameterType, object value)
		{
			using (var db = GetContext())
			{
				db.Parameters.First(p => p.Name == parameterType.ToString()).Value = value.ToString();
				db.SaveChanges();
			}
		}

		public T GetParameterValue<T>(ParameterType parameterType)
		{
			using (var db = GetContext())
			{
				var parameter = db.Parameters.FirstOrDefault(p => p.Name == parameterType.ToString());
				if (parameter == null)
				{
					parameter = db.Parameters.Add(new Parameter
					{
						Name = parameterType.ToString(),
						Value = default(T).ToString()
					});
					db.SaveChanges();
				}
				db.Entry(parameter).Reload();

				string value = parameter.Value;

				return (T)Convert.ChangeType(value, typeof(T));
			}
		}

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

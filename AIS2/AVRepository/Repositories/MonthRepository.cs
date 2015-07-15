using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Infos;

namespace AVRepository.Repositories
{
	public class MonthRepository : BaseRepository
	{
		public void EditInfoMonthPayment(int workerId, DateTime date, string propertyName, double propertyValue)
		{
			using (var db = GetContext())
			{
				var worker = GetDirectoryWorker(workerId);
				var infoMonth = worker.InfoMonthes.First(m => m.Date.Year == date.Year && m.Date.Month == date.Month);

				infoMonth.GetType().GetProperty(propertyName).SetValue(infoMonth, propertyValue);
				db.SaveChanges();
			}
		}
		public int[] GetYears()
		{
			using (var db = GetContext())
			{
				return db.InfoMonthes
					.Select(m => m.Date.Year)
					.Distinct()
					.ToArray();
			}
		}
		public int[] GetMonthes(int year)
		{
			using (var db = GetContext())
			{
				return db.InfoMonthes
					.Where(m => m.Date.Year == year)
					.Select(m => m.Date.Month)
					.Distinct()
					.ToArray();
			}
		}
		public InfoMonth GetInfoMonth(int workerId, int year, int month)
		{
			var worker = GetDirectoryWorker(workerId);
			return worker.InfoMonthes.First(m => m.Date.Year == year && m.Date.Month == month);
		}
		public InfoMonth[] GetInfoMonthes(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoMonthes
					.Where(m => m.Date.Year == year && m.Date.Month == month)
					.ToArray();
			}
		}
	}
}

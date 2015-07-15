using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AVRepository.Repositories
{
	public class DateRepository : BaseRepository
	{
		public InfoDate[] GetInfoDatePanalties(int workerId, int year, int month)
		{
			var worker = GetDirectoryWorker(workerId);
			return worker.InfoDates
				.Where(d => d.Date.Year == year && d.Date.Month == month && d.InfoPanalty != null)
				.ToArray();
		}
		public InfoDate[] GetInfoDatePanaltiesWithoutCash(int workerId, int year, int month)
		{
			var worker = GetDirectoryWorker(workerId);
			return worker.InfoDates
				.Where(d => d.Date.Year == year && d.Date.Month == month && d.InfoPanalty != null)
				.ToArray();
		}
		public void EditInfoDateHour(int workerId, DateTime date, string hour)
		{
			using (var db = GetContext())
			{
				var worker = GetDirectoryWorker(workerId);
				var infoDate = worker.InfoDates.First(d => d.Date.Date == date.Date);

				if (hour != "В")
				{
					if (Enum.IsDefined(typeof(DescriptionDay), hour))
					{
						infoDate.CountHours = null;
						infoDate.DescriptionDay = (DescriptionDay)Enum.Parse(typeof(DescriptionDay), hour);
					}
					else
					{
						infoDate.CountHours = double.Parse(hour);
						infoDate.DescriptionDay = DescriptionDay.Был;
					}
				}
				else
				{
					infoDate.CountHours = null;
					infoDate.DescriptionDay = DescriptionDay.Был;
				}

				db.SaveChanges();
			}
		}
		public InfoDate[] GetInfoDates(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.InfoDates
					.Where(d => DbFunctions.DiffDays(d.Date, date) == 0)
					.ToArray();
			}
		}
		public InfoDate[] GetInfoDates(int workerId, int year, int month)
		{
			var worker = GetDirectoryWorker(workerId);
			return
				worker.InfoDates.AsQueryable()
					.Include(d => d.InfoPanalty)
					.Where(d => d.Date.Year == year && d.Date.Month == month)
					.ToArray();

		}
		public InfoDate[] GetInfoDates(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoDates
					.Where(d => d.Date.Year == year && d.Date.Month == month)
					.Include(d => d.InfoPanalty)
					.ToArray();
			}
		}
		public double? IsOverTime(InfoDate infoDate, List<DateTime> weekEnds)
		{
			if (weekEnds.Any(w => w.Date == infoDate.Date.Date))
			{
				return infoDate.CountHours != null ? infoDate.CountHours : null;
			}
			else
			{
				if (infoDate.CountHours != null)
				{
					return infoDate.CountHours > 8 ? infoDate.CountHours - 8 : null;
				}
				else
				{
					return null;
				}
			}
		}
		public void EditDeadSpiritHours(int deadSpiritWorkerId, DateTime date, double hoursSpiritWorker)
		{
			using (var db = GetContext())
			{
				var deadSpiritWorker = GetDirectoryWorker(deadSpiritWorkerId);

				var infoDateDeadSpirit = deadSpiritWorker.InfoDates.First(d => d.Date.Date == date.Date);
				infoDateDeadSpirit.CountHours = hoursSpiritWorker;
				db.SaveChanges();
			}
		}
		public DateTime GetLastWorkDay(int workerId)
		{
			var worker = GetDirectoryWorker(workerId);
			var infoDate = worker.InfoDates.OrderByDescending(d => d.Date).FirstOrDefault(d => d.CountHours != null);
			return infoDate != null ? infoDate.Date : DateTime.Now;
		}
	}
}

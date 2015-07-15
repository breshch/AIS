using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Infos;

namespace AVRepository.Repositories
{
	public class PanaltyRepository : BaseRepository
	{
		public InfoPanalty GetInfoPanalty(int workerId, DateTime date)
		{
			var worker = GetDirectoryWorker(workerId);
			return worker.InfoDates.AsQueryable().First(d => d.Date.Date == date.Date).InfoPanalty;
		}

		public bool IsInfoPanalty(int workerId, DateTime date)
		{
			var worker = GetDirectoryWorker(workerId);
			return worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty != null;
		}

		public InfoPanalty AddInfoPanalty(int workerId, DateTime date, double summ, string description)
		{
			using (var db = GetContext())
			{
				var worker = GetDirectoryWorker(workerId);
				var infoPanalty = new InfoPanalty
				{
					Summ = summ,
					Description = description
				};

				worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty = infoPanalty;

				db.SaveChanges();

				return infoPanalty;
			}
		}


		public InfoPanalty EditInfoPanalty(int workerId, DateTime date, double summ, string description)
		{
			using (var db = GetContext())
			{
				var worker = GetDirectoryWorker(workerId);

				var infoPanalty = worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty;
				infoPanalty.Summ = summ;
				infoPanalty.Description = description;

				db.SaveChanges();
				return infoPanalty;
			}
		}

		public void RemoveInfoPanalty(int workerId, DateTime date)
		{
			using (var db = GetContext())
			{
				var worker = GetDirectoryWorker(workerId);

				var infoPanalty = worker.InfoDates.First(d => d.Date.Date == date.Date).InfoPanalty;
				db.InfoPanalties.Remove(infoPanalty);
				db.SaveChanges();
			}
		}

	}
}

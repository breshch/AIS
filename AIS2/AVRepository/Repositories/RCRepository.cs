using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;

namespace AVRepository.Repositories
{
	public class RCRepository : BaseRepository
	{
		public DirectoryRC[] GetDirectoryRCs()
		{
			using (var db = GetContext())
			{
				return db.DirectoryRCs.ToArray();
			}
		}

		public DirectoryRC[] GetDirectoryRCsByPercentage()
		{
			using (var db = GetContext())
			{
				return db.DirectoryRCs
					.Where(r => r.Percentes > 0 || r.Name == "ПАМ-16")
					.ToArray();
			}
		}

		public DirectoryRC GetDirectoryRC(string name)
		{
			return GetDirectoryRCs().First(r => r.Name == name);
		}

		public DirectoryRC AddDirectoryRC(string directoryRCName, string descriptionName, int percentes)
		{
			using (var db = GetContext())
			{
				var directoryRC = new DirectoryRC
				{
					Name = directoryRCName,
					DescriptionName = descriptionName,
					Percentes = percentes
				};

				db.DirectoryRCs.Add(directoryRC);

				db.SaveChanges();

				return directoryRC;
			}
		}

		public void RemoveDirectoryRC(int directoryRCId)
		{
			using (var db = GetContext())
			{
				var directoryRC = db.DirectoryRCs.Find(directoryRCId);
				db.DirectoryRCs.Remove(directoryRC);

				db.SaveChanges();
			}
		}

		public DirectoryRC[] GetDirectoryRCsMonthIncoming(int year, int month)
		{
			return GetInfoCosts(year, month)
				.Where(c => c.DirectoryCostItem.Name == "Приход")
				.Select(c => c.DirectoryRC)
				.Distinct()
				.ToArray();
		}

		public DirectoryRC[] GetDirectoryRCsMonthExpense(int year, int month)
		{
			return GetInfoCosts(year, month)
				.Where(c => !c.IsIncoming && c.Currency == Currency.RUR)
				.Select(c => c.DirectoryRC)
				.Distinct()
				.ToArray();
		}
		public CurrentRC[] GetCurrentRCs(IEnumerable<int> ids)
		{
			using (var db = GetContext())
			{
				return db.CurrentRCs.Include(r => r.DirectoryRC).Where(r => ids.Contains(r.InfoOverTimeId)).ToArray();
			}
		}
	}
}

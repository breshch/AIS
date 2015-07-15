using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Directories;


namespace AVRepository.Repositories
{
	public class CompanyRepository : BaseRepository
	{
		public string[] GetDirectoryCompanies(int workerId, int year, int month, int lastDayInMonth)
		{
			var currentPosts = GetCurrentPosts(workerId, year, month, lastDayInMonth);
			var directoryPosts = currentPosts.Select(p => p.DirectoryPost);

			return directoryPosts
				.Select(p => p.DirectoryCompany.Name)
				.Distinct()
				.ToArray();
		}
		public DirectoryCompany[] GetDirectoryCompanies()
		{
			using (var db = GetContext())
			{
				return db.DirectoryCompanies.ToArray();
			}
		}
		public DirectoryCompany AddDirectoryCompany(string directoryCompanyName)
		{
			using (var db = GetContext())
			{
				var directoryCompany = new DirectoryCompany
				{
					Name = directoryCompanyName,
				};

				db.DirectoryCompanies.Add(directoryCompany);
				db.SaveChanges();

				return directoryCompany;
			}
		}
		public void RemoveDirectoryCompany(int directoryCompanyId)
		{
			using (var db = GetContext())
			{
				var directoryCompany = db.DirectoryCompanies.Find(directoryCompanyId);
				db.DirectoryCompanies.Remove(directoryCompany);

				db.SaveChanges();
			}
		}

	}
}

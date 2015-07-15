using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;

namespace AVRepository.Repositories
{
	public class PostRepository : BaseRepository
	{
		public DirectoryTypeOfPost[] GetDirectoryTypeOfPosts()
		{
			using (var db = GetContext())
			{
				return db.DirectoryTypeOfPosts.ToArray();
			}
		}
		public DirectoryTypeOfPost AddDirectoryTypeOfPost(string directoryTypeOfPostName)
		{
			using (var db = GetContext())
			{
				var directoryTypeOfPost = new DirectoryTypeOfPost
				{
					Name = directoryTypeOfPostName
				};

				db.DirectoryTypeOfPosts.Add(directoryTypeOfPost);
				db.SaveChanges();

				return directoryTypeOfPost;
			}
		}
		public void RemoveDirectoryTypeOfPost(int directoryTypeOfPostId)
		{
			using (var db = GetContext())
			{
				var directoryTypeOfPost = db.DirectoryTypeOfPosts.Find(directoryTypeOfPostId);
				db.DirectoryTypeOfPosts.Remove(directoryTypeOfPost);

				db.SaveChanges();
			}
		}
		public DirectoryTypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date)
		{
			return GetCurrentPost(workerId, date).DirectoryPost.DirectoryTypeOfPost;
		}

		public DirectoryPost[] GetDirectoryPosts()
		{
			using (var db = GetContext())
			{
				return db.DirectoryPosts.ToArray();
			}
		}
		public DirectoryPost[] GetDirectoryPosts(DirectoryCompany company)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPosts
					.Where(p => p.DirectoryCompanyId == company.Id)
					.ToArray();
			}
		}
		public DirectoryPost GetDirectoryPost(string postName)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPosts.FirstOrDefault(p => p.Name == postName);
			}
		}
		public DirectoryPost AddDirectoryPost(string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company,
			List<DirectoryPostSalary> postSalaries)
		{
			using (var db = GetContext())
			{
				var directoryPost = new DirectoryPost
				{
					Name = name,
					DirectoryTypeOfPost = typeOfPost,
					DirectoryCompany = company,
					DirectoryPostSalaries = postSalaries
				};

				db.DirectoryPosts.Add(directoryPost);
				db.SaveChanges();

				return directoryPost;
			}
		}
		public DirectoryPost EditDirectoryPost(int postId, string name, DirectoryTypeOfPost typeOfPost,
			DirectoryCompany company, List<DirectoryPostSalary> postSalaries)
		{
			using (var db = GetContext())
			{
				var directoryPost = db.DirectoryPosts.Find(postId);
				directoryPost.Name = name;
				directoryPost.DirectoryTypeOfPost = typeOfPost;
				directoryPost.DirectoryCompany = company;
				db.DirectoryPostSalaries.RemoveRange(directoryPost.DirectoryPostSalaries);
				db.SaveChanges();
				directoryPost.DirectoryPostSalaries = postSalaries;
				db.SaveChanges();

				return directoryPost;
			}
		}
		public void RemoveDirectoryPost(DirectoryPost post)
		{
			using (var db = GetContext())
			{
				db.DirectoryPosts.Remove(post);
				db.SaveChanges();
			}
		}
		public bool ExistsDirectoryPost(string name)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPosts.Any(p => p.Name == name);
			}
		}

		public void AddCurrentPost(int workerId, CurrentCompanyAndPost currentCompanyAndPost)
		{
			using (var db = GetContext())
			{
				var worker = db.DirectoryWorkers.Find(workerId);

				if (worker.CurrentCompaniesAndPosts.Any())
				{
					var lastCurrentPost = worker.CurrentCompaniesAndPosts.OrderByDescending(c => c.ChangeDate).First();
					lastCurrentPost.FireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);
				}

				var currentPost = new CurrentPost
				{
					ChangeDate = currentCompanyAndPost.PostChangeDate,
					DirectoryPost = db.DirectoryPosts.First(p => p.Name == currentCompanyAndPost.DirectoryPost.Name &&
																 p.DirectoryCompany.Name ==
																 currentCompanyAndPost.DirectoryPost.DirectoryCompany.Name),
					IsTwoCompanies = currentCompanyAndPost.IsTwoCompanies,
					IsTemporaryPost = currentCompanyAndPost.IsTemporaryPost
				};
				worker.CurrentCompaniesAndPosts.Add(currentPost);

				db.SaveChanges();
			}
		}

		public CurrentPost[] GetCurrentPosts(DateTime lastDateInMonth)
		{
			using (var db = GetContext())
			{
				var firstDateInMonth = new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1);

				return db.CurrentPosts
					.Where(p => DbFunctions.DiffDays(p.ChangeDate, lastDateInMonth) > 0 &&
								p.FireDate == null ||
								p.FireDate != null &&
								DbFunctions.DiffDays(p.FireDate.Value, firstDateInMonth) < 0 &&
								DbFunctions.DiffDays(p.ChangeDate, lastDateInMonth) > 0)
					.ToArray();
			}
		}

		public CurrentPost[] GetCurrentPosts(int workerId, int year, int month, int lastDayInMonth)
		{

			var lastDateInMonth = new DateTime(year, month, lastDayInMonth);
			var firstDateInMonth = new DateTime(year, month, 1);

			var worker = GetDirectoryWorker(workerId);

			return worker.CurrentCompaniesAndPosts
				.Where(p => p.ChangeDate.Date <= lastDateInMonth.Date &&
							p.FireDate == null ||
							p.FireDate != null &&
							p.FireDate.Value.Date >= firstDateInMonth.Date &&
							p.ChangeDate.Date <= lastDateInMonth.Date)
				.ToArray();

		}

		public CurrentPost GetCurrentPost(int workerId, DateTime date)
		{
			var worker = GetDirectoryWorker(workerId);

			return worker.CurrentCompaniesAndPosts
				.First(p => p.ChangeDate.Date <= date.Date &&
							p.FireDate == null ||
							p.FireDate != null &&
							p.FireDate.Value.Date >= date.Date &&
							p.ChangeDate.Date <= date.Date);
		}

		public CurrentPost GetMainPost(int workerId, DateTime date)
		{
			using (var db = GetContext())
			{
				return db.CurrentPosts.Where(
					p => p.DirectoryWorkerId == workerId &&
						 p.IsTemporaryPost != true &&
						 DbFunctions.DiffDays(date, p.ChangeDate) <= 0)
					.OrderByDescending(p => p.ChangeDate)
					.First();
			}
		}

		public CurrentPost[] GetCurrentMainPosts(DateTime lastDateInMonth)
		{
			using (var db = GetContext())
			{
				var workers = GetDirectoryWorkers(lastDateInMonth.Year, lastDateInMonth.Month);

				var allMainPosts = db.CurrentPosts
					.Include(p => p.DirectoryPost.DirectoryCompany)
					.Where(p => p.IsTemporaryPost != true &&
								DbFunctions.DiffDays(lastDateInMonth, p.ChangeDate) <= 0)
					.OrderByDescending(p => p.ChangeDate)
					.ToArray();

				return workers
					.Select(worker => allMainPosts
						.First(p => p.DirectoryWorkerId == worker.Id))
					.ToArray();
			}
		}

	}
}

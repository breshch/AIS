using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AVRepository.Repositories
{
	public class WorkerRepository : BaseRepository
	{
		public DirectoryWorker[] GetDeadSpiritDirectoryWorkers(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Where(w => w.IsDeadSpirit &&
								(DbFunctions.DiffDays(w.StartDate, date) >= 0 &&
								 (w.FireDate == null ||
								  (w.FireDate != null &&
								   DbFunctions.DiffDays(w.FireDate.Value, date) <= 0))))
					.ToArray();
			}
		}
		public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate, BitmapImage photo,
			DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
		{
			byte[] dataPhoto = null;
			if (photo != null)
			{
				var encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(photo));
				using (MemoryStream ms = new MemoryStream())
				{
					encoder.Save(ms);
					dataPhoto = ms.ToArray();
				}
			}

			using (var db = GetContext())
			{
				var worker = new DirectoryWorker
				{
					LastName = lastName,
					FirstName = firstName,
					MidName = midName,
					Gender = gender,
					BirthDay = birthDay,
					Address = address,
					HomePhone = homePhone,
					CellPhone = cellPhone,
					StartDate = startDate,
					DirectoryPhoto = new DirectoryPhoto { Photo = dataPhoto },
					FireDate = fireDate,
					CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts
						.Select(c => new CurrentPost
						{
							ChangeDate = c.PostChangeDate,
							FireDate = c.PostFireDate,
							DirectoryPostId = c.DirectoryPost.Id,
							IsTwoCompanies = c.IsTwoCompanies,
							IsTemporaryPost = c.IsTemporaryPost
						})),
					IsDeadSpirit = isDeadSpirit
				};

				db.DirectoryWorkers.Add(worker);
				db.SaveChanges();

				for (var date = startDate; date <= DateTime.Now; date = date.AddDays(1))
				{
					var infoDate = new InfoDate
					{
						Date = date,
						DescriptionDay = DescriptionDay.Был,
						CountHours = null
					};

					if (!IsWeekend(date))
					{
						infoDate.CountHours = 8;
					}

					worker.InfoDates.Add(infoDate);
				}

				db.SaveChanges();

				double birthday = GetParameterValue<double>(ParameterType.Birthday);
				for (var date = startDate; date <= DateTime.Now; date = date.AddMonths(1))
				{
					var infoMonth = new InfoMonth
					{
						Date = new DateTime(date.Year, date.Month, 1),
						BirthDays = !worker.IsDeadSpirit ? birthday : 0
					};

					worker.InfoMonthes.Add(infoMonth);
				}

				db.SaveChanges();

				return worker;
			}
		}
		public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
			DateTime? fireDate, CurrentPost currentPost)
		{
			using (var db = GetContext())
			{
				var worker = new DirectoryWorker
				{
					LastName = lastName,
					FirstName = firstName,
					MidName = midName,
					Gender = gender,
					BirthDay = birthDay,
					Address = address,
					HomePhone = homePhone,
					CellPhone = cellPhone,
					StartDate = startDate,
					FireDate = fireDate,
					CurrentCompaniesAndPosts = new List<CurrentPost> {currentPost}
				};

				db.DirectoryWorkers.Add(worker);
				db.SaveChanges();

				return worker;
			}
		}
		public DirectoryWorker[] GetDirectoryWorkers()
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Include(w => w.CurrentCompaniesAndPosts
						.Select(c => c.DirectoryPost.DirectoryTypeOfPost))
					.ToArray();
			}
		}
		public DirectoryWorker[] GetDirectoryWorkers(int year, int month)
		{
			var firstDateInMonth = new DateTime(year, month, 1);
			var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
								(w.FireDate == null ||
								 (w.FireDate != null &&
								  DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0)))
					.ToArray();
			}
		}
		public DirectoryWorker[] GetDirectoryWorkersMonthTimeSheet(int year, int month)
		{
			var firstDateInMonth = new DateTime(year, month, 1);
			var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
								(w.FireDate == null ||
								 (w.FireDate != null &&
								  DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0)))
					.Include(w => w.CurrentCompaniesAndPosts
						.Select(c => c.DirectoryPost.DirectoryTypeOfPost))
					.ToArray();
			}
		}
		public DirectoryWorker[] GetDirectoryWorkers(int year, int month, bool isOffice)
		{
			var firstDateInMonth = new DateTime(year, month, 1);
			var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var workers = GetDirectoryWorkersMonthTimeSheet(year, month);

			var newWorkers = new List<DirectoryWorker>();
			foreach (var worker in workers)
			{
				var post =
					worker.CurrentCompaniesAndPosts.First(p => p.ChangeDate.Date <= lastDateInMonth.Date && p.FireDate == null ||
															   p.FireDate != null && p.FireDate.Value.Date >= lastDateInMonth.Date &&
															   p.ChangeDate.Date <= lastDateInMonth.Date);

				if (isOffice)
				{
					if (post.DirectoryPost.DirectoryTypeOfPost.Name == TYPE_OF_POST_OFFICE &&
						worker.StartDate.Date <= lastDateInMonth.Date &&
						(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
					{
						newWorkers.Add(worker);
					}
				}
				else
				{
					if (post.DirectoryPost.DirectoryTypeOfPost.Name != TYPE_OF_POST_OFFICE &&
						worker.StartDate.Date <= lastDateInMonth.Date &&
						(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
					{
						newWorkers.Add(worker);
					}
				}
			}

			return newWorkers.ToArray();
		}
		public DirectoryWorker[] GetDirectoryWorkers(DateTime fromDate, DateTime toDate)
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Where(w => DbFunctions.DiffDays(w.StartDate, toDate) >= 0 &&
								(w.FireDate == null || w.FireDate != null &&
								 DbFunctions.DiffDays(w.FireDate.Value, fromDate) <= 0))
					.ToArray();
			}
		}
		public DirectoryWorker[] GetDirectoryWorkersWithInfoDatesAndPanalties(int year, int month, bool isOffice)
		{
			var firstDateInMonth = new DateTime(year, month, 1);
			var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			using (var db = GetContext())
			{
				var workers = db.DirectoryWorkers
					.Where(w => DbFunctions.DiffDays(w.StartDate, lastDateInMonth) >= 0 &&
								(w.FireDate == null ||
								 (w.FireDate != null &&
								  DbFunctions.DiffDays(w.FireDate.Value, firstDateInMonth) <= 0)))
					.Include(w => w.InfoDates).Include("InfoDates.InfoPanalty")
					.ToArray();

				var tmpWorkers = new List<DirectoryWorker>();
				foreach (var worker in workers)
				{
					var post = GetCurrentPost(worker.Id, lastDateInMonth);

					if (isOffice)
					{
						if (post.DirectoryPost.DirectoryTypeOfPost.Name == TYPE_OF_POST_OFFICE &&
							worker.StartDate.Date <= lastDateInMonth.Date &&
							(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
						{
							tmpWorkers.Add(worker);
						}
					}
					else
					{
						if (post.DirectoryPost.DirectoryTypeOfPost.Name != TYPE_OF_POST_OFFICE &&
							worker.StartDate.Date <= lastDateInMonth.Date &&
							(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
						{
							tmpWorkers.Add(worker);
						}
					}
				}

				return tmpWorkers.ToArray();
			}
		}
		public DirectoryWorker GetDirectoryWorker(int workerId)
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers.Find(workerId);
			}
		}
		public DirectoryWorker GetDirectoryWorkerWithPosts(int workerId)
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers
					.Include(w => w.CurrentCompaniesAndPosts
						.Select(p => p.DirectoryPost.DirectoryPostSalaries))
					.First(w => w.Id == workerId);
			}
		}
		public DirectoryWorker GetDirectoryWorker(string lastName, string firstName)
		{
			using (var db = GetContext())
			{
				return db.DirectoryWorkers.FirstOrDefault(w => w.LastName == lastName && w.FirstName == firstName);
			}
		}
		public DirectoryWorker EditDirectoryWorker(int id, string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone,
			string cellPhone, DateTime startDate, BitmapImage photo, DateTime? fireDate,
			ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
		{
			var directoryWorker = GetDirectoryWorker(id);

			byte[] dataPhoto = null;
			if (photo != null)
			{
				var encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(photo));
				using (MemoryStream ms = new MemoryStream())
				{
					encoder.Save(ms);
					dataPhoto = ms.ToArray();
				}
			}

			using (var db = GetContext())
			{
				directoryWorker.LastName = lastName;
				directoryWorker.FirstName = firstName;
				directoryWorker.MidName = midName;
				directoryWorker.Gender = gender;
				directoryWorker.BirthDay = birthDay;
				directoryWorker.Address = address;
				directoryWorker.HomePhone = homePhone;
				directoryWorker.CellPhone = cellPhone;
				directoryWorker.StartDate = startDate;

				if (directoryWorker.DirectoryPhoto == null)
				{
					directoryWorker.DirectoryPhoto = new DirectoryPhoto();
				}

				directoryWorker.DirectoryPhoto.Photo = dataPhoto;
				directoryWorker.FireDate = fireDate;

				db.CurrentPosts.RemoveRange(directoryWorker.CurrentCompaniesAndPosts);

				directoryWorker.CurrentCompaniesAndPosts = new List<CurrentPost>(currentCompaniesAndPosts
					.Select(c => new CurrentPost
					{
						ChangeDate = c.PostChangeDate,
						FireDate = c.PostFireDate,
						DirectoryPostId = c.DirectoryPost.Id,
						IsTwoCompanies = c.IsTwoCompanies,
						IsTemporaryPost = c.IsTemporaryPost
					}));
				directoryWorker.IsDeadSpirit = isDeadSpirit;

				db.SaveChanges();

				return directoryWorker;
			}
		}


	}

}

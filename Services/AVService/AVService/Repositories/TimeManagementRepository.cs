using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Infos;
using AVService.Models.Entities.Temps;
using AVService.Models.Enums;
using AVService.Models.Repositories;
using EntityFramework.BulkInsert.Extensions;
using Shared.Enums;

namespace AVService.Repositories
{
	public class TimeManagementRepository : BaseRepository
	{
		private readonly UtilRepository utilRepository;

		private const string TYPE_OF_POST_OFFICE = "Офис"; //TODO Refactoring Офис

		public TimeManagementRepository(UtilRepository utilRepository)
		{
			this.utilRepository = utilRepository;
		}

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

		public TypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date)
		{
			return GetCurrentPost(workerId, date).DirectoryPost.TypeOfPost;
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
		public DirectoryPost AddDirectoryPost(string name, TypeOfPost typeOfPost, DirectoryCompany company,
			List<DirectoryPostSalary> postSalaries)
		{
			using (var db = GetContext())
			{
				var directoryPost = new DirectoryPost
				{
					Name = name,
					TypeOfPost = typeOfPost,
					DirectoryCompany = company,
					DirectoryPostSalaries = postSalaries
				};

				db.DirectoryPosts.Add(directoryPost);
				db.SaveChanges();

				return directoryPost;
			}
		}
		public DirectoryPost EditDirectoryPost(int postId, string name, TypeOfPost typeOfPost,
			DirectoryCompany company, List<DirectoryPostSalary> postSalaries)
		{
			using (var db = GetContext())
			{
				var directoryPost = db.DirectoryPosts.Find(postId);
				directoryPost.Name = name;
				directoryPost.TypeOfPost = typeOfPost;
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

					if (!utilRepository.IsWeekend(date))
					{
						infoDate.CountHours = 8;
					}

					worker.InfoDates.Add(infoDate);
				}

				db.SaveChanges();

				double birthday = utilRepository.GetParameterValue<double>(ParameterType.MonthlyBirthdayPayment);
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
					CurrentCompaniesAndPosts = new List<CurrentPost> { currentPost }
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
						.Select(c => c.DirectoryPost.TypeOfPost))
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
						.Select(c => c.DirectoryPost.TypeOfPost))
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
					if (post.DirectoryPost.TypeOfPost == TypeOfPost.Office &&
						worker.StartDate.Date <= lastDateInMonth.Date &&
						(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
					{
						newWorkers.Add(worker);
					}
				}
				else
				{
					if (post.DirectoryPost.TypeOfPost == TypeOfPost.Office &&
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

		public WorkerModel[] GetWorkersModel(DateTime fromDate, DateTime toDate, WorkerModelQueryRule workerModelQueryRule)
		{
			using (var db = GetContext())
			{
				var workers = db.DirectoryWorkers
					.Where(x => DbFunctions.DiffDays(x.StartDate, toDate) >= 0 &&
								(x.FireDate == null || x.FireDate != null &&
								 DbFunctions.DiffDays(x.FireDate.Value, fromDate) <= 0))
					.ToArray();

				var workersId = workers.Select(x => x.Id).ToArray();

				var currentPosts = new CurrentPost[0];
				var directoryPosts = new DirectoryPost[0];
				var infoDates = new InfoDate[0];
				var infoMonths = new InfoMonth[0];

				if (workerModelQueryRule.HasFlag(WorkerModelQueryRule.CurrentPosts) || workerModelQueryRule.HasFlag(WorkerModelQueryRule.DirectoryPosts))
				{
					currentPosts = db.CurrentPosts
						.Where(x => workersId.Contains(x.DirectoryWorkerId) &&
						            (DbFunctions.DiffDays(x.ChangeDate, toDate) >= 0 &&
						             (x.FireDate == null || x.FireDate != null &&
						              DbFunctions.DiffDays(x.FireDate.Value, fromDate) <= 0)))
						.ToArray();

					var directoryPostId = currentPosts.Select(x => x.DirectoryPostId).ToArray();

					if (workerModelQueryRule.HasFlag(WorkerModelQueryRule.DirectoryPosts))
					{
						directoryPosts = db.DirectoryPosts
							.Where(x => directoryPostId.Contains(x.Id))
							.ToArray();
					}
				}

				if (workerModelQueryRule.HasFlag(WorkerModelQueryRule.InfoDates))
				{
					infoDates = db.InfoDates
						.Where(x => workersId.Contains(x.DirectoryWorkerId) &&
						            DbFunctions.DiffDays(x.Date, toDate) >= 0 &&
						            DbFunctions.DiffDays(x.Date, fromDate) <= 0)
						.ToArray();
				}

				if (workerModelQueryRule.HasFlag(WorkerModelQueryRule.InfoMonths))
				{
					infoMonths = db.InfoMonthes
						.Where(x => workersId.Contains(x.DirectoryWorkerId) &&
									DbFunctions.DiffDays(x.Date, toDate) >= 0 &&
									DbFunctions.DiffDays(x.Date, fromDate) <= 0)
						.ToArray();
				}

				var workersModel = new List<WorkerModel>();
				foreach (var worker in workers)
				{
					var workerModel = new WorkerModel();
					workerModel.Worker = worker;
					workerModel.CurrentPosts = currentPosts.Where(x => x.DirectoryWorkerId == worker.Id).ToArray();

					var workerDirectoryPostId = workerModel.CurrentPosts.Select(x => x.DirectoryPostId).ToArray();
					workerModel.DirectoryPosts = directoryPosts.Where(x => workerDirectoryPostId.Contains(x.Id)).ToArray();
					workerModel.InfoDates = infoDates.Where(x => x.DirectoryWorkerId == worker.Id).ToArray();
					workerModel.InfoMonthes = infoMonths.Where(x => x.DirectoryWorkerId == worker.Id).ToArray();

					workersModel.Add(workerModel);
				}

				return workersModel.ToArray();
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
						if (post.DirectoryPost.TypeOfPost == TypeOfPost.Office &&
							worker.StartDate.Date <= lastDateInMonth.Date &&
							(worker.FireDate == null || (worker.FireDate != null && worker.FireDate.Value.Date >= firstDateInMonth.Date)))
						{
							tmpWorkers.Add(worker);
						}
					}
					else
					{
						if (post.DirectoryPost.TypeOfPost == TypeOfPost.Office &&
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

		public void AddInfoOverTime(DateTime startDate, DateTime endDate, ICollection<DirectoryRC> directoryRCs,
			string description)
		{
			using (var db = GetContext())
			{
				var overTime = db.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, startDate) == 0);

				if (overTime == null)
				{
					overTime = new InfoOverTime
					{
						StartDate = startDate,
						EndDate = endDate,
						Description = description
					};

					foreach (var directoryRC in directoryRCs)
					{
						var currentRC = new CurrentRC
						{
							DirectoryRC = directoryRC
						};

						overTime.CurrentRCs.Add(currentRC);
					}


					db.InfoOverTimes.Add(overTime);

					db.SaveChanges();
				}
			}
		}

		public void EditInfoOverTime(DateTime startDate, DateTime endDate, List<DirectoryRC> directoryRCs, string description)
		{
			using (var db = GetContext())
			{
				var overTime = db.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, startDate) == 0);

				if (overTime != null)
				{
					overTime.StartDate = startDate;
					overTime.EndDate = endDate;
					overTime.Description = description;

					db.CurrentRCs.RemoveRange(overTime.CurrentRCs);

					db.SaveChanges();

					foreach (var directoryRC in directoryRCs)
					{
						var currentRC = new CurrentRC
						{
							DirectoryRC = directoryRC
						};

						overTime.CurrentRCs.Add(currentRC);
					}

					db.SaveChanges();
				}
			}
		}

		public void EditInfoOverTime(DateTime date, double hoursOverTime)
		{
			using (var db = GetContext())
			{
				var overTime = GetInfoOverTime(date);
				if (overTime != null)
				{
					overTime.EndDate = overTime.StartDate.AddHours(hoursOverTime);
					db.SaveChanges();
				}
			}
		}

		public InfoOverTime GetInfoOverTime(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.InfoOverTimes
					.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
			}
		}

		public InfoOverTime[] GetInfoOverTimes(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoOverTimes
					.Where(o => o.StartDate.Year == year && o.StartDate.Month == month)
					.ToArray();
			}
		}

		public DateTime[] GetInfoOverTimeDates(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoOverTimes
					.Where(o => o.StartDate.Year == year && o.StartDate.Month == month)
					.Select(o => o.StartDate)
					.ToArray();
			}
		}

		public void EditInfoOverTimeByDate(DateTime date, double hoursOverTime)
		{
			using (var db = GetContext())
			{
				var overTime = GetInfoOverTime(date);
				if (overTime != null)
				{
					overTime.EndDate = overTime.StartDate.AddHours(hoursOverTime);
					db.SaveChanges();
				}
			}
		}

		public bool IsInfoOverTimeDate(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.InfoOverTimes.Any(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
			}
		}

		public void RemoveInfoOverTime(DateTime date)
		{
			using (var db = GetContext())
			{
				var infoOverTime = db.InfoOverTimes.FirstOrDefault(o => DbFunctions.DiffDays(o.StartDate, date) == 0);
				if (infoOverTime != null)
				{
					db.InfoOverTimes.Remove(infoOverTime);
					db.SaveChanges();
				}
			}
		}

		#region DirectoryPostSalary

		public DirectoryPostSalary[] GetDirectoryPostSalaries(int postId)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPostSalaries.Where(s => s.DirectoryPostId == postId).ToArray();
			}
		}

		public DirectoryPostSalary GetDirectoryPostSalaryByDate(int postId, DateTime date)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPostSalaries.Where(s => s.DirectoryPostId == postId)
					.OrderByDescending(s => s.Date)
					.First(s => DbFunctions.DiffDays(date, s.Date) <= 0);
			}
		}

		public DirectoryPostSalary[] GetDirectoryPostSalaries(int year, int month)
		{
			using (var db = GetContext())
			{
				var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

				return db.DirectoryPostSalaries
					.GroupBy(s => s.DirectoryPostId)
					.Select(g => g.OrderByDescending(s => s.Date)
						.FirstOrDefault(s => DbFunctions.DiffDays(lastDateInMonth, s.Date) <= 0))
					.ToArray();
			}
		}

		#endregion

		public DirectoryPostSalary[] GetDirectoryPostSalariesByMonth(int year, int month)
		{
			using (var db = GetContext())
			{
				var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

				return db.DirectoryPostSalaries
					.GroupBy(s => s.DirectoryPostId)
					.Select(g => g.OrderByDescending(s => s.Date)
						.FirstOrDefault(s => DbFunctions.DiffDays(lastDateInMonth, s.Date) <= 0))
					.ToArray();
			}
		}

		#region PAM16Percentage

		public double GetPam16Percentage(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.DirectoryPam16Percentages
					.Where(d => DbFunctions.DiffDays(date, d.Date) <= 0)
					.OrderByDescending(d => d.Date).First().Percentage;
			}
		}

		public void SavePam16Percentage(DateTime date, double pam16Percentage)
		{
			using (var db = GetContext())
			{
				var pam16 = db.DirectoryPam16Percentages.FirstOrDefault(p => p.Date.Year == date.Year && p.Date.Month == date.Month);
				if (pam16 == null)
				{
					pam16 = new DirectoryPAM16Percentage
					{
						Date = date,
						Percentage = pam16Percentage
					};

					db.DirectoryPam16Percentages.Add(pam16);
				}
				else
				{
					pam16.Percentage = pam16Percentage;
				}
				db.SaveChanges();
			}
		}

		public void AddInfoDates(InfoDate[] infoDates)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(infoDates);
			}
		}

		public void AddInfoMonths(InfoMonth[] infoMonths)
		{
			using (var db = GetContext())
			{
				db.BulkInsert(infoMonths);
			}
		}

		#endregion

		private InfoCost[] GetInfoCosts(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Include(c => c.DirectoryCostItem)
					.Include(c => c.DirectoryRC)
					.Include(c => c.DirectoryTransportCompany)
					.Include(c => c.CurrentNotes.Select(n => n.DirectoryNote))
					.Where(c => c.Date.Year == year && c.Date.Month == month)
					.OrderBy(c => c.Date)
					.ToArray();
			}
		}
	}
}

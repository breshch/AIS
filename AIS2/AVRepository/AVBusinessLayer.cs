using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Media.Imaging;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Data.WareHouse;
using AIS_Enterprise_Global.Helpers;
using AVRepository.Repositories;

namespace AVRepository
{
	public class AVBusinessLayer : IAVBusinessLayer
	{
		private readonly UtilRepository utilRepository;
		private readonly RemainRepository remainRepository;
		private readonly AdministrationRepository administrationRepository;
		private readonly WarehouseRepository warehouseRepository;
		private readonly InitializationRepository initializationRepository;
		private readonly CostRepository costRepository;
		private readonly TimeManagementRepository timeManagementRepository;

		public AVBusinessLayer()
		{
			utilRepository = new UtilRepository();
			remainRepository = new RemainRepository(utilRepository);
			administrationRepository = new AdministrationRepository();
			warehouseRepository = new WarehouseRepository();
			costRepository = new CostRepository(utilRepository);
			timeManagementRepository = new TimeManagementRepository(utilRepository);
			initializationRepository = new InitializationRepository(utilRepository, timeManagementRepository, costRepository);
		}

		//done
		#region DirectoryCompany

		public string[] GetDirectoryCompaniesByWorker(int workerId, int year, int month, int lastDayInMonth)
		{
			return timeManagementRepository.GetDirectoryCompanies(workerId, year, month, lastDayInMonth);
		}

		public DirectoryCompany[] GetDirectoryCompanies()
		{
			return timeManagementRepository.GetDirectoryCompanies();
		}

		public DirectoryCompany AddDirectoryCompany(string directoryCompanyName)
		{
			return timeManagementRepository.AddDirectoryCompany(directoryCompanyName);
		}

		public void RemoveDirectoryCompany(int directoryCompanyId)
		{
			timeManagementRepository.RemoveDirectoryCompany(directoryCompanyId);
		}

		#endregion
		//**
		//done
		#region DirectoryTypeOfPost

		public DirectoryTypeOfPost[] GetDirectoryTypeOfPosts()
		{
			return timeManagementRepository.GetDirectoryTypeOfPosts();
		}

		public DirectoryTypeOfPost AddDirectoryTypeOfPost(string directoryTypeOfPostName)
		{
			return timeManagementRepository.AddDirectoryTypeOfPost(directoryTypeOfPostName);
		}

		public void RemoveDirectoryTypeOfPost(int directoryTypeOfPostId)
		{
			timeManagementRepository.RemoveDirectoryTypeOfPost(directoryTypeOfPostId);
		}

		public DirectoryTypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date)
		{
			return timeManagementRepository.GetDirectoryTypeOfPost(workerId, date);
		}

		#endregion
		//**
		//done
		#region DirectoryPost

		public DirectoryPost[] GetDirectoryPosts()
		{
			return timeManagementRepository.GetDirectoryPosts();
		}

		public DirectoryPost[] GetDirectoryPostsByCompany(DirectoryCompany company)
		{
			return timeManagementRepository.GetDirectoryPosts(company);
		}

		public DirectoryPost GetDirectoryPost(string postName)
		{
			return timeManagementRepository.GetDirectoryPost(postName);
		}

		public DirectoryPost AddDirectoryPost(string name, DirectoryTypeOfPost typeOfPost, DirectoryCompany company,
			List<DirectoryPostSalary> postSalaries)
		{
			return timeManagementRepository.AddDirectoryPost(name, typeOfPost, company, postSalaries);
		}

		public DirectoryPost EditDirectoryPost(int postId, string name, DirectoryTypeOfPost typeOfPost,
			DirectoryCompany company, List<DirectoryPostSalary> postSalaries)
		{
			return timeManagementRepository.EditDirectoryPost(postId, name, typeOfPost, company, postSalaries);
		}

		public void RemoveDirectoryPost(DirectoryPost post)
		{
			timeManagementRepository.RemoveDirectoryPost(post);
		}

		public bool ExistsDirectoryPost(string name)
		{
			return timeManagementRepository.ExistsDirectoryPost(name);
		}

		#endregion
		//**
		//done
		#region DirectoryWorker


		public DirectoryWorker[] GetDeadSpiritDirectoryWorkers(DateTime date)
		{
			return timeManagementRepository.GetDeadSpiritDirectoryWorkers(date);
		}

		public DirectoryWorker AddDirectoryWorkerWithMultiplyPosts(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate, BitmapImage photo,
			DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
		{
			return timeManagementRepository.AddDirectoryWorker(lastName, firstName, midName, gender, birthDay, address, cellPhone,
				homePhone, startDate, photo, fireDate, currentCompaniesAndPosts, isDeadSpirit);
		}

		public DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
			DateTime? fireDate, CurrentPost currentPost)
		{
			return timeManagementRepository.AddDirectoryWorker(lastName, midName, firstName, gender, birthDay, address
				, homePhone, cellPhone, startDate, fireDate, currentPost);
		}

		public DirectoryWorker[] GetDirectoryWorkers()
		{
			return timeManagementRepository.GetDirectoryWorkers();
		}

		public DirectoryWorker[] GetDirectoryWorkersByMonth(int year, int month)
		{
			return timeManagementRepository.GetDirectoryWorkers(year, month);
		}

		//TODO Refactoring DRY
		public DirectoryWorker[] GetDirectoryWorkersMonthTimeSheet(int year, int month)
		{
			return timeManagementRepository.GetDirectoryWorkersMonthTimeSheet(year, month);
		}

		public DirectoryWorker[] GetDirectoryWorkersByTypeOfPost(int year, int month, bool isOffice)
		{
			return timeManagementRepository.GetDirectoryWorkers(year, month, isOffice);
		}

		public DirectoryWorker[] GetDirectoryWorkersBetweenDates(DateTime fromDate, DateTime toDate)
		{
			return timeManagementRepository.GetDirectoryWorkers(fromDate, toDate);
		}

		public DirectoryWorker[] GetDirectoryWorkersWithInfoDatesAndPanalties(int year, int month, bool isOffice)
		{
			return timeManagementRepository.GetDirectoryWorkersWithInfoDatesAndPanalties(year, month, isOffice);
		}

		public DirectoryWorker GetDirectoryWorkerById(int workerId)
		{
			return timeManagementRepository.GetDirectoryWorker(workerId);
		}

		public DirectoryWorker GetDirectoryWorkerWithPosts(int workerId)
		{
			return timeManagementRepository.GetDirectoryWorkerWithPosts(workerId);
		}

		public DirectoryWorker GetDirectoryWorker(string lastName, string firstName)
		{
			return timeManagementRepository.GetDirectoryWorker(lastName, firstName);
		}

		public DirectoryWorker EditDirectoryWorker(int id, string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone,
			string cellPhone, DateTime startDate, BitmapImage photo, DateTime? fireDate,
			ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit)
		{
			return timeManagementRepository.EditDirectoryWorker(id, lastName, firstName, midName, gender, birthDay, address, homePhone,
				cellPhone, startDate, photo, fireDate, currentCompaniesAndPosts, isDeadSpirit);
		}

		#endregion
		//**
		//done
		#region InfoDates

		public InfoDate[] GetInfoDatePanalties(int workerId, int year, int month)
		{
			return timeManagementRepository.GetInfoDatePanalties(workerId, year, month);
		}

		public InfoDate[] GetInfoDatePanaltiesWithoutCash(int workerId, int year, int month)
		{
			return timeManagementRepository.GetInfoDatePanaltiesWithoutCash(workerId, year, month);
		}

		public void EditInfoDateHour(int workerId, DateTime date, string hour)
		{
			timeManagementRepository.EditInfoDateHour(workerId, date, hour);
		}

		public InfoDate[] GetInfoDates(DateTime date)
		{
			return timeManagementRepository.GetInfoDates(date);
		}

		public InfoDate[] GetInfoDatesByWorker(int workerId, int year, int month)
		{
			return timeManagementRepository.GetInfoDates(workerId, year, month);
		}

		public InfoDate[] GetInfoDatesByMonth(int year, int month)
		{
			return timeManagementRepository.GetInfoDates(year, month);
		}

		public double? IsOverTime(InfoDate infoDate, List<DateTime> weekEnds)
		{
			return timeManagementRepository.IsOverTime(infoDate, weekEnds);
		}

		public void EditDeadSpiritHours(int deadSpiritWorkerId, DateTime date, double hoursSpiritWorker)
		{
			timeManagementRepository.EditDeadSpiritHours(deadSpiritWorkerId, date, hoursSpiritWorker);
		}

		public DateTime GetLastWorkDay(int workerId)
		{
			return timeManagementRepository.GetLastWorkDay(workerId);
		}

		#endregion
		//**
		//done
		#region InfoMonth

		public void EditInfoMonthPayment(int workerId, DateTime date, string propertyName, double propertyValue)
		{
			timeManagementRepository.EditInfoMonthPayment(workerId, date, propertyName, propertyValue);
		}

		public int[] GetYears()
		{
			return timeManagementRepository.GetYears();
		}

		public int[] GetMonthes(int year)
		{
			return timeManagementRepository.GetMonthes(year);
		}

		public InfoMonth GetInfoMonth(int workerId, int year, int month)
		{
			return timeManagementRepository.GetInfoMonth(workerId, year, month);
		}

		public InfoMonth[] GetInfoMonthes(int year, int month)
		{
			return timeManagementRepository.GetInfoMonthes(year, month);
		}

		#endregion
		//**
		//done
		#region InfoPanalty

		public InfoPanalty GetInfoPanalty(int workerId, DateTime date)
		{
			return timeManagementRepository.GetInfoPanalty(workerId, date);
		}

		public bool IsInfoPanalty(int workerId, DateTime date)
		{
			return timeManagementRepository.IsInfoPanalty(workerId, date);
		}

		public InfoPanalty AddInfoPanalty(int workerId, DateTime date, double summ, string description)
		{
			return timeManagementRepository.AddInfoPanalty(workerId, date, summ, description);
		}


		public InfoPanalty EditInfoPanalty(int workerId, DateTime date, double summ, string description)
		{
			return timeManagementRepository.EditInfoPanalty(workerId, date, summ, description);
		}

		public void RemoveInfoPanalty(int workerId, DateTime date)
		{
			timeManagementRepository.RemoveInfoPanalty(workerId, date);
		}

		#endregion
		//**
		//done
		#region CurrentPost

		public void AddCurrentPost(int workerId, CurrentCompanyAndPost currentCompanyAndPost)
		{
			timeManagementRepository.AddCurrentPost(workerId, currentCompanyAndPost);
		}

		public CurrentPost[] GetCurrentPosts(DateTime lastDateInMonth)
		{
			return timeManagementRepository.GetCurrentPosts(lastDateInMonth);
		}

		public CurrentPost[] GetCurrentPosts(int workerId, int year, int month, int lastDayInMonth)
		{
			return timeManagementRepository.GetCurrentPosts(workerId, year, month, lastDayInMonth);
		}

		public CurrentPost GetCurrentPost(int workerId, DateTime date)
		{
			return timeManagementRepository.GetCurrentPost(workerId, date);
		}

		public CurrentPost GetMainPost(int workerId, DateTime date)
		{
			return timeManagementRepository.GetMainPost(workerId, date);
		}

		public CurrentPost[] GetCurrentMainPosts(DateTime lastDateInMonth)
		{
			return timeManagementRepository.GetCurrentMainPosts(lastDateInMonth);
		}

		#endregion
		//**
		//done
		#region Calendar

		public int GetCountWorkDaysInMonth(int year, int month)
		{
			return utilRepository.GetCountWorkDaysInMonth(year, month);
		}

		public DateTime[] GetHolidaysByMonth(int year, int month)
		{
			return utilRepository.GetHolidays(year, month);
		}

		public DateTime[] GetHolidaysByYear(int year)
		{
			return utilRepository.GetHolidays(year);
		}

		public DateTime[] GetHolidays(DateTime fromDate, DateTime toDate)
		{
			return utilRepository.GetHolidays(fromDate, toDate);
		}

		public bool IsWeekend(DateTime date)
		{
			return utilRepository.IsWeekend(date);
		}

		public void SetHolidays(int year, List<DateTime> holidays)
		{
			utilRepository.SetHolidays(year, holidays);
		}

		public void EditParameter(ParameterType parameterType, object value)
		{
			utilRepository.EditParameter(parameterType, value);
		}

		public int GetParameterValueByInt(ParameterType parameterType)
		{
			return utilRepository.GetParameterValue<int>(parameterType);
		}

		public double GetParameterValueByDouble(ParameterType parameterType)
		{
			return utilRepository.GetParameterValue<double>(parameterType);
		}

		public bool GetParameterValueByBool(ParameterType parameterType)
		{
			return utilRepository.GetParameterValue<bool>(parameterType);
		}

		public DateTime GetParameterValueByDateTime(ParameterType parameterType)
		{
			return utilRepository.GetParameterValue<DateTime>(parameterType);
		}

		#endregion
		//**
		//done
		#region Initialize

		public void InitializeAbsentDates()
		{
			initializationRepository.InitializeAbsentDates();
		}

		//private void InitializeWorkerLoanPayments(DirectoryWorker worker, InfoMonth infoMonth, DateTime salaryDate)
		//{
		//	initializationRepository.InitializeWorkerLoanPayments(worker, infoMonth, salaryDate);
			
		//}

		#endregion
		//**
		//done
		#region DirectoryUserStatus

		public DirectoryUserStatus[] GetDirectoryUserStatuses()
		{
			return administrationRepository.GetDirectoryUserStatuses();
		}

		public DirectoryUserStatus AddDirectoryUserStatus(string name, List<CurrentUserStatusPrivilege> privileges)
		{
			return administrationRepository.AddDirectoryUserStatus(name, privileges);
		}

		public void EditDirectoryUserStatus(int userStatusId, string userStatusName,
			List<CurrentUserStatusPrivilege> privileges)
		{
			administrationRepository.EditDirectoryUserStatus(userStatusId, userStatusName, privileges);
		}

		public void RemoveDirectoryUserStatus(int id)
		{
			administrationRepository.RemoveDirectoryUserStatus(id);
		}

		#endregion
		//**
		//done
		#region DirectoryUser

		public DirectoryUser[] GetDirectoryUsers()
		{
			return administrationRepository.GetDirectoryUsers();
		}

		public DirectoryUser GetDirectoryUser(int userId)
		{
			return administrationRepository.GetDirectoryUser(userId);
		}

		public DirectoryUser AddDirectoryUser(string userName, string password, DirectoryUserStatus userStatus)
		{
			return administrationRepository.AddDirectoryUser(userName, password, userStatus);
		}

		public DirectoryUser AddDirectoryUserAdmin(string userName, string password)
		{
			return administrationRepository.AddDirectoryUserAdmin(userName, password);
		}

		public void EditDirectoryUser(int userId, string userName, string password, DirectoryUserStatus userStatus)
		{
			administrationRepository.EditDirectoryUser(userId, userName, password, userStatus);
		}

		public void RemoveDirectoryUser(DirectoryUser user)
		{
			administrationRepository.RemoveDirectoryUser(user);
		}


		#endregion
		//**
		//done
		#region DirectoryUserStatusPrivilege

		public DirectoryUserStatusPrivilege GetDirectoryUserStatusPrivilege(string privilegeName)
		{
			return administrationRepository.GetDirectoryUserStatusPrivilege(privilegeName);
		}

		public string[] GetPrivileges(int userId)
		{
			return administrationRepository.GetPrivileges(userId);
		}

		#endregion
		//**
		//done
		#region DirectoryRC

		public DirectoryRC[] GetDirectoryRCs()
		{
			return timeManagementRepository.GetDirectoryRCs();
		}

		public DirectoryRC[] GetDirectoryRCsByPercentage()
		{
			return timeManagementRepository.GetDirectoryRCsByPercentage();
		}

		public DirectoryRC GetDirectoryRC(string name)
		{
			return timeManagementRepository.GetDirectoryRC(name);
		}

		public DirectoryRC AddDirectoryRC(string directoryRCName, string descriptionName, int percentes)
		{
			return timeManagementRepository.AddDirectoryRC(directoryRCName, descriptionName, percentes);
		}

		public void RemoveDirectoryRC(int directoryRCId)
		{
			timeManagementRepository.RemoveDirectoryRC(directoryRCId);
		}

		public DirectoryRC[] GetDirectoryRCsMonthIncoming(int year, int month)
		{
			return timeManagementRepository.GetDirectoryRCsMonthIncoming(year, month);
		}

		public DirectoryRC[] GetDirectoryRCsMonthExpense(int year, int month)
		{
			return timeManagementRepository.GetDirectoryRCsMonthExpense(year, month);
		}

		#endregion
		//**

		#region InfoOverTime

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

		#endregion

		//done
		#region InfoCost

		public InfoCost EditInfoCost(DateTime date, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note,
			bool isIncomming, double summ, Currency currency, double weight)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCostsByDate(date);
				var infoCost =
					infoCosts.FirstOrDefault(
						c =>
							c.DirectoryCostItem.Id == costItem.Id && c.DirectoryRCId == rc.Id &&
							c.CurrentNotes.First().DirectoryNoteId == note.Id);
				if (infoCost == null)
				{
					infoCost = new InfoCost
					{
						GroupId = Guid.NewGuid(),
						Date = date,
						DirectoryCostItemId = costItem.Id,
						DirectoryRC = rc,
						CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNote = note } },
						Summ = summ,
						Currency = currency,
						IsIncoming = isIncomming,
						Weight = weight,
					};

					db.InfoCosts.Add(infoCost);

					AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
						rc.Name + " " + infoCost.ConcatNotes);
				}
				else
				{
					double prevSumm = infoCost.Summ;

					infoCost.Summ = summ;
					infoCost.Currency = currency;
					infoCost.IsIncoming = isIncomming;
					infoCost.Weight = weight;

					if (infoCost.Summ - prevSumm != 0)
					{
						AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ - prevSumm, currency, CashType.Наличка,
							infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
					}
				}

				db.SaveChanges();
				return infoCost;
			}
		}

		public InfoCost[] GetInfoCostsByDate(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => DbFunctions.DiffDays(date, c.Date) == 0)
					.ToArray();
			}
		}

		public InfoCost GetInfoCost(int infoCostId)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts.Find(infoCostId);
			}
		}

		public InfoCost[] GetInfoCosts(int year, int month)
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

		public InfoCost[] GetInfoCostsRCIncoming(int year, int month, string rcName)
		{

			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year &&
								c.Date.Month == month &&
								c.DirectoryRC.Name == rcName &&
								c.DirectoryCostItem.Name == "Приход")
					.ToArray();
			}
		}

		public InfoCost[] GetInfoCosts26Expense(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year &&
								c.Date.Month == month &&
								c.DirectoryRC.Name == "26А" &&
								!c.IsIncoming)
					.ToArray();
			}
		}


		public InfoCost[] GetInfoCostsRCAndAll(int year, int month, string rcName)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCosts(year, month).ToList();
				var infoCostsRC = infoCosts.Where(c => c.DirectoryRC.Name == rcName && c.Currency == Currency.RUR).ToList();

				if (db.DirectoryRCs.First(r => r.Name == rcName).Percentes > 0)
				{
					infoCostsRC.AddRange(infoCosts.Where(c => c.DirectoryRC.Name == "ВСЕ" && c.Currency == Currency.RUR).ToList());
				}

				return infoCostsRC.ToArray();
			}
		}

		public InfoCost[] GetInfoCostsPAM16(int year, int month)
		{
			var infoCosts = GetInfoCosts(year, month).ToList();
			var infoCostsRC = infoCosts.Where(c => c.DirectoryRC.Name == "ПАМ-16" && c.Currency == Currency.RUR).ToList();

			infoCostsRC.AddRange(infoCosts.Where(c => c.DirectoryRC.Name == "ВСЕ" && c.Currency == Currency.RUR).ToList());

			return infoCostsRC.ToArray();
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnly(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Include(c => c.DirectoryCostItem)
					.Include(c => c.DirectoryRC)
					.Include(c => c.DirectoryTransportCompany)
					.Include(c => c.CurrentNotes.Select(n => n.DirectoryNote))
					.Where(c => !c.IsIncoming && c.DirectoryCostItem.Name == "Транспорт (5031)" && c.DirectoryRC.Name != "ВСЕ")
					.ToArray();
			}
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnlyByDate(DateTime date)
		{
			return GetInfoCostsByDate(date)
				.Where(c => !c.IsIncoming &&
					c.DirectoryCostItem.Name == "Транспорт (5031)" &&
					c.DirectoryRC.Name != "ВСЕ")
				.ToArray();
		}

		public void AddInfoCosts(DateTime date, DirectoryCostItem directoryCostItem, bool isIncoming,
			DirectoryTransportCompany transportCompany, double summ, Currency currency, List<Transport> transports)
		{
			using (var db = GetContext())
			{
				var groupId = Guid.NewGuid();
				if (directoryCostItem.Name == "Транспорт (5031)" && (transports[0].DirectoryRC.Name != "26А" || !isIncoming))
				{
					double commonWeight = transports.Sum(t => t.Weight);

					int indexCargo = 1;

					double totalSummTransport = 0;
					var cargos = transports.Select(t => t.DirectoryRC.Name).Distinct();

					foreach (var rc in cargos)
					{
						var currentNotes = new List<CurrentNote>();
						double weightRC = 0;

						foreach (var transport in transports.Where(t => t.DirectoryRC.Name == rc))
						{
							currentNotes.Add(new CurrentNote { DirectoryNote = db.DirectoryNotes.Find(transport.DirectoryNote.Id) });
							weightRC += transport.Weight;
						}

						double summTransport = 0;
						if (indexCargo < cargos.Count())
						{
							summTransport = weightRC != 0 ? Math.Round(summ / commonWeight * weightRC, 0) : summ;
							totalSummTransport += summTransport;
						}
						else
						{
							summTransport = summ - totalSummTransport;
						}

						var infoCost = new InfoCost
						{
							GroupId = groupId,
							Date = date,
							DirectoryCostItem = db.DirectoryCostItems.Find(directoryCostItem.Id),
							DirectoryRC = db.DirectoryRCs.First(r => r.Name == rc),
							IsIncoming = isIncoming,
							DirectoryTransportCompany =
								transportCompany != null ? db.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
							Summ = summTransport,
							Currency = currency,
							CurrentNotes = currentNotes,
							Weight = weightRC
						};

						db.InfoCosts.Add(infoCost);

						AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
							infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);

						indexCargo++;
					}
				}
				else
				{
					var infoCost = new InfoCost
					{
						GroupId = groupId,
						Date = date,
						DirectoryCostItem = db.DirectoryCostItems.Find(directoryCostItem.Id),
						DirectoryRC = db.DirectoryRCs.Find(transports.First().DirectoryRC.Id),
						IsIncoming = isIncoming,
						DirectoryTransportCompany =
							transportCompany != null ? db.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
						Summ = summ,
						Currency = currency,
						CurrentNotes =
							new List<CurrentNote>
							{
								new CurrentNote {DirectoryNote = db.DirectoryNotes.Find(transports.First().DirectoryNote.Id)}
							},
						Weight = 0
					};

					db.InfoCosts.Add(infoCost);
					AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
						infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
				}

				db.SaveChanges();
			}
		}

		public void RemoveInfoCost(InfoCost infoCost)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCostsByDate(infoCost.Date).Where(c => c.GroupId == infoCost.GroupId).ToList();

				foreach (var cost in infoCosts)
				{
					AddInfoSafe(infoCost.Date, !infoCost.IsIncoming, infoCost.Summ, infoCost.Currency, CashType.Наличка,
						infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
				}

				db.InfoCosts.RemoveRange(infoCosts);
				db.SaveChanges();
			}
		}

		public int[] GetInfoCostYears()
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Select(c => c.Date.Year)
					.Distinct()
					.OrderBy(y => y)
					.ToArray();
			}
		}

		public int[] GetInfoCostMonthes(int year)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year)
					.Select(c => c.Date.Month)
					.Distinct()
					.OrderBy(m => m)
					.ToArray();
			}
		}

		public double GetInfoCost26Summ(int year, int month)
		{
			var infoCosts = GetInfoCosts(year, month).Where(c => c.DirectoryRC.Name == "26А" && c.IsIncoming);
			return infoCosts.Any() ? infoCosts.Sum(c => c.Summ) : 0;
		}

		public string[] GetInfoCostsIncomingTotalSummsCurrency(int year, int month, string rcName, bool? isIncoming = null,
			string costItem = null)
		{
			using (var db = GetContext())
			{
				string[] totalSumms = new string[Enum.GetNames(typeof(Currency)).Count()];

				for (int i = 0; i < totalSumms.Count(); i++)
				{
					var currency = (Currency)Enum.Parse(typeof(Currency), Enum.GetName(typeof(Currency), i));
					var infoCosts =
						db.InfoCosts.Where(
							c => c.Date.Year == year && c.Date.Month == month && c.Currency == currency && c.DirectoryRC.Name == rcName);

					if (isIncoming != null)
					{
						infoCosts = infoCosts.Where(c => c.IsIncoming == isIncoming.Value);
					}

					if (costItem != null)
					{
						infoCosts = infoCosts.Where(c => c.DirectoryCostItem.Name == costItem);
					}

					if (infoCosts.Any())
					{
						double totalSummCurrency = infoCosts.Sum(c => c.Summ);
						totalSumms[i] = totalSummCurrency != 0 ? Converting.DoubleToCurrency(totalSummCurrency, currency) : null;
					}
				}

				return totalSumms;
			}
		}

		#endregion

		//done
		#region InfoLoan

		public InfoLoan AddInfoLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;

					AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + loanTaker.Name);
				}
				else
				{
					AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + directoryWorker.FullName);
				}


				var infoLoan = new InfoLoan
				{
					DateLoan = date,
					DirectoryLoanTaker = loanTaker,
					DirectoryWorker = directoryWorker,
					Summ = summ,
					Currency = currency,
					CountPayments = countPayments,
					Description = description,
				};

				db.InfoLoans.Add(infoLoan);
				db.SaveChanges();

				EditCurrencyValueSummChange("TotalLoan", currency, summ);

				var dateLoanPayment = new DateTime(date.Year, date.Month, 5);

				if (loanTakerName == null)
				{
					double onePaySumm = Math.Round(summ / countPayments, 0);

					for (int i = 0; i < countPayments; i++)
					{
						dateLoanPayment = dateLoanPayment.AddMonths(1);

						var infoPayment = new InfoPayment
						{
							Date = dateLoanPayment,
							Summ = onePaySumm,
							InfoLoanId = infoLoan.Id
						};

						db.InfoPayments.Add(infoPayment);
					}

					EditInfoMonthPayment(directoryWorker.Id, date, "PrepaymentCash", onePaySumm);

					infoLoan.DateLoanPayment = dateLoanPayment;
				}

				db.SaveChanges();

				return infoLoan;
			}
		}

		public InfoLoan EditInfoLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoLoan = db.InfoLoans.Find(id);

				EditCurrencyValueSummChange("TotalLoan", currency, summ - infoLoan.Summ);

				infoLoan.DateLoan = date;
				infoLoan.DirectoryLoanTaker = loanTaker;
				infoLoan.DirectoryWorker = directoryWorker;
				infoLoan.Summ = summ;
				infoLoan.Currency = currency;
				infoLoan.CountPayments = countPayments;
				infoLoan.Description = description;

				db.SaveChanges();

				return infoLoan;
			}
		}

		public void RemoveInfoLoan(InfoLoan selectedInfoLoan)
		{
			using (var db = GetContext())
			{
				db.InfoLoans.Remove(selectedInfoLoan);
				db.SaveChanges();
			}
		}

		public InfoLoan[] GetInfoLoans(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoLoans
					.Where(s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 &&
								DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
								(s.DateLoanPayment == null ||
								 s.DateLoanPayment != null &&
								 DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0))
					.OrderByDescending(s => s.DateLoan)
					.ToArray();
			}
		}

		public double GetLoans()
		{
			using (var db = GetContext())
			{
				return db.InfoLoans.Where(s => s.DateLoanPayment == null).ToArray().Sum(s => s.RemainingSumm);
			}
		}

		#endregion

		//done
		#region InfoPrivateLoan

		public InfoPrivateLoan AddInfoPrivateLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker,
			double summ, Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoPrivateLoan = new InfoPrivateLoan
				{
					DateLoan = date,
					DirectoryLoanTaker = loanTaker,
					DirectoryWorker = directoryWorker,
					Summ = summ,
					Currency = currency,
					CountPayments = countPayments,
					Description = description,
				};

				db.InfoPrivateLoans.Add(infoPrivateLoan);
				db.SaveChanges();

				EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ);

				var datePrivateLoanPayment = new DateTime(date.Year, date.Month, 5);

				if (loanTakerName == null)
				{
					double onePaySumm = Math.Round(summ / countPayments, 0);

					for (int i = 0; i < countPayments; i++)
					{
						datePrivateLoanPayment = datePrivateLoanPayment.AddMonths(1);

						var infoPrivatePayment = new InfoPrivatePayment
						{
							Date = datePrivateLoanPayment,
							Summ = onePaySumm,
							InfoPrivateLoanId = infoPrivateLoan.Id
						};

						db.InfoPrivatePayments.Add(infoPrivatePayment);
					}

					if (countPayments > 1)
					{
						infoPrivateLoan.DateLoanPayment = datePrivateLoanPayment;
					}
				}

				db.SaveChanges();

				return infoPrivateLoan;
			}
		}

		public InfoPrivateLoan EditInfoPrivateLoan(int id, DateTime date, string loanTakerName,
			DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoPrivateLoan = db.InfoPrivateLoans.Find(id);

				EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ - infoPrivateLoan.Summ);

				infoPrivateLoan.DateLoan = date;
				infoPrivateLoan.DirectoryLoanTaker = loanTaker;
				infoPrivateLoan.DirectoryWorker = directoryWorker;
				infoPrivateLoan.Summ = summ;
				infoPrivateLoan.Currency = currency;
				infoPrivateLoan.CountPayments = countPayments;
				infoPrivateLoan.Description = description;

				db.SaveChanges();

				return infoPrivateLoan;
			}
		}

		public void RemoveInfoPrivateLoan(InfoPrivateLoan selectedInfoPrivateLoan)
		{
			using (var db = GetContext())
			{
				db.InfoPrivateLoans.Remove(selectedInfoPrivateLoan);
				db.SaveChanges();
			}
		}

		public InfoPrivateLoan[] GetInfoPrivateLoans(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoPrivateLoans.Where(
					s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 && DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
						 (s.DateLoanPayment == null ||
						  s.DateLoanPayment != null && DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0)).
					OrderByDescending(s => s.DateLoan)
					.ToArray();
			}
		}

		public double GetPrivateLoans()
		{
			using (var db = GetContext())
			{
				return db.InfoPrivateLoans.Where(s => s.DateLoanPayment == null).ToArray().Sum(s => s.RemainingSumm);
			}
		}

		#endregion

		//done
		#region DirectoryCostItem

		public DirectoryCostItem[] GetDirectoryCostItems()
		{
			using (var db = GetContext())
			{
				return db.DirectoryCostItems.ToArray();
			}
		}

		public DirectoryCostItem GetDirectoryCostItem(string costItemName)
		{
			return GetDirectoryCostItems().First(c => c.Name == costItemName);
		}

		#endregion

		//done
		#region DirectoryNote

		public DirectoryNote[] GetDirectoryNotes()
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.ToArray();
			}
		}

		public DirectoryNote GetDirectoryNote(string description)
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.First(n => n.Description == description);
			}
		}

		public bool IsDirectoryNote(string note)
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.Select(n => n.Description).Contains(note);
			}
		}

		public DirectoryNote AddDirectoryNote(string note)
		{
			using (var db = GetContext())
			{
				var directoryNote = db.DirectoryNotes.FirstOrDefault(n => n.Description == note);

				if (directoryNote != null)
				{
					return directoryNote;
				}

				directoryNote = new DirectoryNote { Description = note };

				db.DirectoryNotes.Add(directoryNote);

				db.SaveChanges();

				return directoryNote;
			}
		}

		#endregion

		//done
		#region DefaultCosts

		public DefaultCost[] GetDefaultCosts()
		{
			using (var db = GetContext())
			{
				return db.DefaultCosts.ToArray();
			}
		}

		public DefaultCost AddDefaultCost(DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			using (var db = GetContext())
			{
				var defaultCost = new DefaultCost
				{
					DirectoryCostItem = costItem,
					DirectoryRC = rc,
					DirectoryNote = note,
					SummOfPayment = summ,
					DayOfPayment = day
				};
				db.DefaultCosts.Add(defaultCost);
				db.SaveChanges();

				var infoCost = new InfoCost
				{
					Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day),
					DirectoryCostItemId = defaultCost.DirectoryCostItemId,
					DirectoryRCId = defaultCost.DirectoryRCId,
					CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
					Summ = defaultCost.SummOfPayment,
					IsIncoming = false,
					Weight = 0,
				};

				db.InfoCosts.Add(infoCost);
				db.SaveChanges();

				return defaultCost;
			}
		}

		public void EditDefaultCost(int id, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			using (var db = GetContext())
			{
				var defaultCost = db.DefaultCosts.Find(id);

				defaultCost.DirectoryCostItem = costItem;
				defaultCost.DirectoryRC = rc;
				defaultCost.DirectoryNote = note;
				defaultCost.SummOfPayment = summ;
				defaultCost.DayOfPayment = day;

				db.SaveChanges();
			}
		}

		public void RemoveDefaultCost(DefaultCost defaultCost)
		{
			using (var db = GetContext())
			{
				db.DefaultCosts.Remove(defaultCost);
				db.SaveChanges();
			}
		}

		public void InitializeDefaultCosts()
		{
			//using (var db = GetContext())
			//{
			//	var defaultCostsDate = GetParameterValue<DateTime>(ParameterType.DefaultCostsDate);
			//	var currentDate = DateTime.Now;

			//	if (defaultCostsDate.Year < currentDate.Year ||
			//		(defaultCostsDate.Year == currentDate.Year && defaultCostsDate.Month < currentDate.Month))
			//	{
			//		var defaultCosts = GetDefaultCosts().ToList();

			//		foreach (var defaultCost in defaultCosts)
			//		{
			//			var infoCostDate = new DateTime(currentDate.Year, currentDate.Month, defaultCost.DayOfPayment);
			//			var infoCosts = GetInfoCostsByDate(infoCostDate).ToList();
			//			if (
			//				!infoCosts.Any(
			//					c =>
			//						c.DirectoryCostItem.Id == defaultCost.DirectoryCostItemId && c.DirectoryRCId == defaultCost.DirectoryRCId &&
			//						c.CurrentNotes.First().DirectoryNoteId == defaultCost.DirectoryNoteId && c.Summ == defaultCost.SummOfPayment))
			//			{
			//				var infoCost = new InfoCost
			//				{
			//					Date = infoCostDate,
			//					DirectoryCostItemId = defaultCost.DirectoryCostItemId,
			//					DirectoryRCId = defaultCost.DirectoryRCId,
			//					CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
			//					Summ = defaultCost.SummOfPayment,
			//					IsIncoming = false,
			//					Weight = 0,
			//				};

			//				db.InfoCosts.Add(infoCost);
			//			}
			//		}

			//		db.SaveChanges();
			//		EditParameter(ParameterType.DefaultCostsDate, DateTime.Now);
			//	}
			//}
		}

		#endregion

		//done
		#region DirectoryTransportCompanies

		public DirectoryTransportCompany[] GetDirectoryTransportCompanies()
		{
			using (var db = GetContext())
			{
				return db.DirectoryTransportCompanies.ToArray();
			}
		}


		#endregion

		//done
		#region CurrentRC

		public CurrentRC[] GetCurrentRCs(IEnumerable<int> ids)
		{
			return timeManagementRepository.GetCurrentRCs(ids);
		}

		#endregion
		//**
		//done
		#region DirectoryLoanTaker

		public DirectoryLoanTaker[] GetDirectoryLoanTakers()
		{
			using (var db = GetContext())
			{
				return db.DirectoryLoanTakers.ToArray();
			}
		}

		public DirectoryLoanTaker AddDirectoryLoanTaker(string name)
		{
			using (var db = GetContext())
			{
				var loanTaker = new DirectoryLoanTaker
				{
					Name = name
				};

				db.DirectoryLoanTakers.Add(loanTaker);
				db.SaveChanges();

				return loanTaker;
			}
		}

		#endregion

		//done
		#region InfoPayments

		public InfoPayment[] GetInfoPayments(int infoLoanId)
		{
			using (var db = GetContext())
			{
				return db.InfoPayments.Where(p => p.InfoLoanId == infoLoanId).OrderBy(p => p.Date).ToArray();
			}
		}

		public InfoPayment AddInfoPayment(int infoLoanId, DateTime date, double summ)
		{
			using (var db = GetContext())
			{
				var infoPayment = new InfoPayment
				{
					Date = date,
					Summ = summ,
					InfoLoanId = infoLoanId
				};

				db.InfoPayments.Add(infoPayment);
				db.SaveChanges();

				var infoLoan = db.InfoLoans.Find(infoLoanId);
				EditCurrencyValueSummChange("TotalLoan", infoLoan.Currency, -summ);
				AddInfoSafe(date, true, summ, infoLoan.Currency, CashType.Наличка, "Возврат долга: " +
																				   (infoLoan.DirectoryLoanTakerId == null
																					   ? infoLoan.DirectoryWorker.FullName
																					   : infoLoan.DirectoryLoanTaker.Name));

				return infoPayment;
			}
		}

		public void RemoveInfoPayment(InfoPayment selectedInfoPayment)
		{
			using (var db = GetContext())
			{
				var currency = db.InfoLoans.Find(selectedInfoPayment.InfoLoanId).Currency;
				EditCurrencyValueSummChange("TotalLoan", currency, selectedInfoPayment.Summ);

				db.InfoPayments.Remove(selectedInfoPayment);
				db.SaveChanges();
			}
		}

		#endregion

		//done
		#region InfoCard

		public void SetCardAvaliableSumm(string cardName, double avaliableSumm)
		{
			using (var db = GetContext())
			{
				db.InfoCards.First(c => c.CardName == cardName).AvaliableSumm = avaliableSumm;
				db.SaveChanges();
			}
		}

		public double GetCardAvaliableSumm(string cardName)
		{
			using (var db = GetContext())
			{
				return db.InfoCards.First(c => c.CardName == cardName).AvaliableSumm;
			}
		}

		#endregion

		//done
		#region InfoSafe

		public InfoSafe AddInfoSafe(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description, string bankName = null)
		{
			using (var db = GetContext())
			{
				var infoSafe = new InfoSafe
				{
					Date = date,
					IsIncoming = isIncoming,
					Summ = summCash,
					Currency = currency,
					CashType = cashType,
					Description = description,
					Bank = bankName
				};

				db.InfoSafes.Add(infoSafe);
				db.SaveChanges();

				if (cashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
				}

				return infoSafe;
			}
		}

		public InfoSafe AddInfoSafeHand(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description)
		{
			using (var db = GetContext())
			{
				var infoSafe = new InfoSafe
				{
					Date = date,
					IsIncoming = isIncoming,
					Summ = summCash,
					Currency = currency,
					CashType = cashType,
					Description = description
				};

				db.InfoSafes.Add(infoSafe);
				db.SaveChanges();

				if (cashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
					CalcTotalSumm("TotalSafe", !isIncoming, summCash, currency);
				}

				return infoSafe;
			}
		}

		private void CalcTotalSumm(string totalSummName, bool isIncoming, double summCash, Currency currency)
		{
			double summ = GetCurrencyValueSumm(totalSummName, currency);

			summ += isIncoming ? summCash : -summCash;

			EditCurrencyValueSumm(totalSummName, currency, summ);
		}

		public InfoSafe AddInfoSafeCard(DateTime date, double availableSumm, Currency currency, string description,
			string bankName)
		{
			using (var db = GetContext())
			{
				double prevAvailableSumm = GetCurrencyValue("TotalCard").RUR;

				double summ = Math.Round(availableSumm - prevAvailableSumm, 2);
				bool isIncoming = summ >= 0;

				if (!db.InfoSafes.Any(
						s => DbFunctions.DiffSeconds(s.Date, date) == 0 && s.CashType == CashType.Карточка && s.Description == description))
				{
					EditCurrencyValueSumm("TotalCard", Currency.RUR, availableSumm);

					return AddInfoSafe(date, isIncoming, Math.Abs(summ), currency, CashType.Карточка, description, bankName);
				}

				return null;
			}
		}

		public bool IsNewMessage(DateTime date, string description)
		{
			using (var db = GetContext())
			{
				return !db.InfoSafes.Any(s => DbFunctions.DiffDays(s.Date, date) == 0 &&
											  s.CashType == CashType.Карточка &&
											  s.Description == description);
			}
		}

		public InfoSafe[] GetInfoSafes()
		{
			using (var db = GetContext())
			{
				return db.InfoSafes.OrderByDescending(s => s.Date).ToArray();
			}
		}

		public InfoSafe[] GetInfoSafesByCashType(CashType cashType, DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return
					db.InfoSafes.Where(
						s => s.CashType == cashType && DbFunctions.DiffDays(from, s.Date) >= 0 && DbFunctions.DiffDays(to, s.Date) <= 0)
						.OrderByDescending(s => s.Date)
						.ToArray();
			}
		}

		public void RemoveInfoSafe(InfoSafe infoSafe)
		{
			using (var db = GetContext())
			{
				if (infoSafe.CashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", !infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
					CalcTotalSumm("TotalSafe", infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
				}

				db.InfoSafes.Remove(infoSafe);
				db.SaveChanges();
			}
		}

		#endregion

		//done
		#region InfoPrivatePayments

		public InfoPrivatePayment[] GetInfoPrivatePayments(int infoSafeId)
		{
			using (var db = GetContext())
			{
				return db.InfoPrivatePayments.Where(p => p.InfoPrivateLoanId == infoSafeId).OrderBy(p => p.Date).ToArray();
			}
		}

		public InfoPrivatePayment AddInfoPrivatePayment(int infoPrivateLoanId, DateTime date, double summ)
		{
			using (var db = GetContext())
			{
				var infoPrivatePayment = new InfoPrivatePayment
				{
					Date = date,
					Summ = summ,
					InfoPrivateLoanId = infoPrivateLoanId
				};

				db.InfoPrivatePayments.Add(infoPrivatePayment);
				db.SaveChanges();

				var currency = db.InfoPrivateLoans.Find(infoPrivateLoanId).Currency;
				EditCurrencyValueSummChange("TotalPrivateLoan", currency, -summ);

				return infoPrivatePayment;
			}
		}

		public void RemoveInfoPrivatePayment(InfoPrivatePayment selectedInfoPrivatePayment)
		{
			using (var db = GetContext())
			{
				var currency = db.InfoPrivateLoans.Find(selectedInfoPrivatePayment.InfoPrivateLoanId).Currency;
				EditCurrencyValueSummChange("TotalPrivateLoan", currency, selectedInfoPrivatePayment.Summ);

				db.InfoPrivatePayments.Remove(selectedInfoPrivatePayment);
				db.SaveChanges();
			}
		}

		#endregion

		//done
		#region CurrencyValue

		public CurrencyValue GetCurrencyValue(string name)
		{
			using (var db = GetContext())
			{
				var currencyValue = db.CurrencyValues.First(c => c.Name == name);
				db.Entry(currencyValue).Reload();

				return currencyValue;
			}
		}

		public double GetCurrencyValueSumm(string name, Currency currency)
		{
			var currencyValue = GetCurrencyValue(name);

			double summ = 0;
			switch (currency)
			{
				case Currency.RUR:
					summ = currencyValue.RUR;
					break;
				case Currency.USD:
					summ = currencyValue.USD;
					break;
				case Currency.EUR:
					summ = currencyValue.EUR;
					break;
				case Currency.BYR:
					summ = currencyValue.BYR;
					break;
				default:
					break;
			}

			return summ;
		}

		public void EditCurrencyValueSumm(string name, Currency currency, double summ)
		{
			using (var db = GetContext())
			{
				var currencyValue = GetCurrencyValue(name);

				switch (currency)
				{
					case Currency.RUR:
						currencyValue.RUR = summ;
						break;
					case Currency.USD:
						currencyValue.USD = summ;
						break;
					case Currency.EUR:
						currencyValue.EUR = summ;
						break;
					case Currency.BYR:
						currencyValue.BYR = summ;
						break;
					default:
						break;
				}

				db.SaveChanges();
			}
		}

		public void EditCurrencyValueSummChange(string name, Currency currency, double summ)
		{
			using (var db = GetContext())
			{
				var currencyValue = GetCurrencyValue(name);

				switch (currency)
				{
					case Currency.RUR:
						currencyValue.RUR += summ;
						break;
					case Currency.USD:
						currencyValue.USD += summ;
						break;
					case Currency.EUR:
						currencyValue.EUR += summ;
						break;
					case Currency.BYR:
						currencyValue.BYR += summ;
						break;
					default:
						break;
				}

				db.SaveChanges();
			}
		}

		#endregion


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

		#endregion

		//done
		#region DirectoryCarPart

		public DirectoryCarPart AddDirectoryCarPart(string article, string mark, string description, string originalNumber,
			string factoryNumber, string crossNumber, string material, string attachment, string countInBox, bool isImport)
		{
			using (var db = GetContext())
			{
				var carPart = new DirectoryCarPart
				{
					Article = article,
					Mark = mark,
					Description = description,
					OriginalNumber = originalNumber,
					Material = material,
					Attachment = attachment,
					FactoryNumber = factoryNumber,
					CrossNumber = crossNumber,
					CountInBox = countInBox,
					IsImport = isImport
				};

				db.DirectoryCarParts.Add(carPart);
				db.SaveChanges();

				return carPart;
			}
		}

		public DirectoryCarPart[] GetDirectoryCarParts()
		{
			using (var db = GetContext())
			{
				return db.DirectoryCarParts.OrderBy(c => c.Article).ToArray();
			}
		}

		public DirectoryCarPart GetDirectoryCarPart(string article, string mark)
		{
			using (var db = GetContext())
			{
				return db.DirectoryCarParts.FirstOrDefault(c => c.Article == article && c.Mark == mark);
			}
		}

		#endregion

		//done
		#region InfoContainer

		public int[] GetContainerYears(bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(c => c.IsIncoming == isIncoming)
					.Select(c => c.DatePhysical.Year)
					.Distinct()
					.OrderBy(c => c)
					.ToArray();
			}
		}

		public int[] GetContainerMonthes(int selectedYear, bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(c => c.IsIncoming == isIncoming && c.DatePhysical.Year == selectedYear)
					.Select(c => c.DatePhysical.Month)
					.Distinct()
					.OrderBy(c => c)
					.ToArray();
			}
		}

		public InfoContainer[] GetContainers(int year, int month, bool isIncoming)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Where(
					c => c.IsIncoming == isIncoming && c.DatePhysical.Year == year && c.DatePhysical.Month == month)
					.OrderBy(c => c.DatePhysical)
					.ToArray();
			}
		}

		public InfoContainer[] GetInfoContainers(List<InfoContainer> containers)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.ToList()
					.Where(c => containers.Any(c2 => c.Name == c2.Name && c.DatePhysical.Date == c2.DatePhysical.Date
													 && c.Description == c2.Description && c.IsIncoming == c2.IsIncoming))
					.Select(c => new InfoContainer
					{
						Id = c.Id,
						Name = c.Name,
						DatePhysical = c.DatePhysical,
						Description = c.Description,
						IsIncoming = c.IsIncoming,
						CarParts = containers.First(c2 => c.Name == c2.Name && c.DatePhysical.Date == c2.DatePhysical.Date
														  && c.Description == c2.Description && c.IsIncoming == c2.IsIncoming).CarParts
					})
					.ToArray();
			}
		}

		public InfoContainer GetInfoContainer(int containerId)
		{
			using (var db = GetContext())
			{
				return db.InfoContainers.Include(c => c.CarParts).First(c => c.Id == containerId);
			}
		}

		public void RemoveInfoContainer(InfoContainer container)
		{
			using (var db = GetContext())
			{
				var carParts = db.InfoContainers.Find(container.Id).CarParts.ToArray();
				db.CurrentContainerCarParts.RemoveRange(carParts);

				db.InfoContainers.Remove(container);
				db.SaveChanges();
			}
		}

		public InfoContainer AddInfoContainer(string name, string description, DateTime datePhysical, DateTime? dateOrder,
			bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			using (var db = GetContext())
			{
				var container = new InfoContainer
				{
					Name = name,
					Description = description,
					DatePhysical = datePhysical,
					DateOrder = dateOrder,
					IsIncoming = isIncoming,
					CarParts = carParts.Select(c => new CurrentContainerCarPart
					{
						DirectoryCarPartId = c.DirectoryCarPartId,
						CountCarParts = c.CountCarParts,
					}).ToList()
				};

				db.InfoContainers.Add(container);
				db.SaveChanges();

				return container;
			}
		}

		public void EditInfoContainer(int containerId, string name, string description, DateTime datePhysical,
			DateTime? dateOrder, bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			using (var db = GetContext())
			{
				var container = db.InfoContainers.Find(containerId);
				container.Name = name;
				container.Description = description;
				container.DatePhysical = datePhysical;
				container.DateOrder = dateOrder;
				container.IsIncoming = isIncoming;

				db.SaveChanges();

				container.CarParts.Clear();
				container.CarParts = carParts;

				db.SaveChanges();
			}
		}

		public InfoCarPartMovement[] GetMovementsByDates(DirectoryCarPart selectedDirectoryCarPart,
			DateTime selectedDateFrom, DateTime selectedDateTo)
		{
			using (var db = GetContext())
			{
				return (from container in db.InfoContainers
						where
							DbFunctions.DiffDays(selectedDateFrom, container.DatePhysical) >= 0 &&
							DbFunctions.DiffDays(container.DatePhysical, selectedDateTo) >= 0
						join carPart in db.CurrentContainerCarParts on container.Id equals carPart.InfoContainerId
						where carPart.DirectoryCarPartId == selectedDirectoryCarPart.Id
						orderby container.DatePhysical descending
						select new InfoCarPartMovement
						{
							Date = container.DatePhysical,
							FullDescription = container.Name + " " + container.Description,
							Incoming = container.IsIncoming ? carPart.CountCarParts : default(int?),
							Outcoming = !container.IsIncoming ? carPart.CountCarParts : default(int?)
						})
					.ToArray();
			}
		}

		public void RemoveContainers(int year, int month)
		{
			using (var db = GetContext())
			{
				var removingContainers = db.InfoContainers
					.Where(c => c.DatePhysical.Year == year && c.DatePhysical.Month == month).ToArray();

				if (removingContainers.Any())
				{
					db.InfoContainers.RemoveRange(removingContainers);
					db.SaveChanges();
				}
			}
		}

		#endregion

		//done
		#region InfoLastMonthDayRemain

		public InfoLastMonthDayRemain GetInfoLastMonthDayRemain(DateTime date, int carPartId)
		{
			using (var db = GetContext())
			{
				return db.InfoLastMonthDayRemains.FirstOrDefault(p => p.DirectoryCarPartId == carPartId &&
																	  (p.Date.Year == date.Year && p.Date.Month == date.Month));
			}
		}

		public int GetInfoCarPartIncomingCountTillDate(DateTime date, int carPartId, bool isIncoming)
		{
			using (var db = GetContext())
			{
				DateTime firstDateInMonth = new DateTime(date.Year, date.Month, 1);
				var containers = db.InfoContainers.Include(c => c.CarParts).Where(c => c.IsIncoming == isIncoming &&
																					   (DbFunctions.DiffDays(firstDateInMonth,
																						   c.DatePhysical) >= 0 &&
																						DbFunctions.DiffDays(c.DatePhysical, date) >=
																						0)).ToList();

				return containers.Sum(c => c.CarParts.Where(p => p.DirectoryCarPartId == carPartId).Sum(c2 => c2.CountCarParts));
			}
		}

		public InfoLastMonthDayRemain AddInfoLastMonthDayRemain(DirectoryCarPart carPart, DateTime date, int count)
		{
			using (var db = GetContext())
			{
				var lastMonthDayRemain = new InfoLastMonthDayRemain
				{
					DirectoryCarPart = carPart,
					Count = count,
					Date = date,
				};
				db.InfoLastMonthDayRemains.Add(lastMonthDayRemain);
				db.SaveChanges();

				return lastMonthDayRemain;
			}
		}

		public void RemoveInfoLastMonthDayRemains(int year, int month)
		{
			using (var db = GetContext())
			{
				db.InfoLastMonthDayRemains
					.RemoveRange(db.InfoLastMonthDayRemains
						.Where(r => r.Date.Year == year && r.Date.Month == month));

				db.SaveChanges();
			}
		}

		#endregion

		//done
		#region CurrentCarParts

		public CurrentCarPart AddCurrentCarPart(DirectoryCarPart directoryCarPart, DateTime priceDate, double priceBase,
			double? priceBigWholesale, double? priceSmallWholesale)
		{
			using (var db = GetContext())
			{
				var currentCarPart = new CurrentCarPart
				{
					DirectoryCarPart = directoryCarPart,
					Date = priceDate,
					PriceBase = priceBase,
					PriceBigWholesale = priceBigWholesale,
					PriceSmallWholesale = priceSmallWholesale,
				};

				db.CurrentCarParts.Add(currentCarPart);
				db.SaveChanges();

				return currentCarPart;
			}
		}

		public CurrentCarPart AddCurrentCarPartNoSave(DateTime priceDate, double priceBase, double? priceBigWholesale,
			double? priceSmallWholesale, Currency currency, string fullName)
		{
			using (var db = GetContext())
			{
				var currentCarPart = new CurrentCarPart
				{
					Date = priceDate,
					Currency = currency,
					PriceBase = priceBase,
					PriceBigWholesale = priceBigWholesale,
					PriceSmallWholesale = priceSmallWholesale,
					FullName = fullName
				};

				return currentCarPart;
			}
		}

		public CurrentCarPart[] GetCurrentCarParts()
		{
			using (var db = GetContext())
			{
				return db.CurrentCarParts.ToArray();
			}
		}

		public CurrentCarPart GetCurrentCarPart(int directoryCarPartId, DateTime date)
		{
			using (var db = GetContext())
			{
				return db.CurrentCarParts.Where(c => c.DirectoryCarPartId == directoryCarPartId)
					.OrderByDescending(c => c.Date)
					.FirstOrDefault(c => DbFunctions.DiffDays(date, c.Date) < 0);
			}
		}

		public ArticlePrice[] GetArticlePrices(DateTime date, Currency currency)
		{
			using (var db = GetContext())
			{
				var directoryCarParts = db.DirectoryCarParts.ToList();
				var currentCarParts = db.CurrentCarParts.ToList();

				var articlePrices = new List<ArticlePrice>();
				foreach (var directoryCarPart in directoryCarParts)
				{
					var currentCarPart = currentCarParts
						.Where(c => c.DirectoryCarPartId == directoryCarPart.Id && c.Currency == currency)
						.OrderByDescending(c => c.Date)
						.FirstOrDefault(c => date.Date >= c.Date.Date);

					if (currentCarPart != null)
					{
						articlePrices.Add(new ArticlePrice
						{
							Article = directoryCarPart.Article,
							Mark = directoryCarPart.Mark,
							Description = directoryCarPart.Description,
							PriceRUR = currentCarPart.PriceBase
						});
					}
				}

				return articlePrices.ToArray();
			}
		}

		#endregion

		//done
		#region CurrentCarPartsRemainsToDate

		public void SetRemainsToFirstDateInMonth()
		{
			//using (var db = GetContext())
			//{
			//	var isProcessing = GetParameterValue<bool>(ParameterType.IsProcessingLastDateInMonthRemains);

			//	var firstDateIneMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			//	if (!isProcessing)
			//	{
			//		EditParameter(ParameterType.IsProcessingLastDateInMonthRemains, true);
			//		if (!db.InfoLastMonthDayRemains.Any(d => DbFunctions.DiffDays(d.Date, firstDateIneMonth) == 0))
			//		{
			//			var lastDateInMonth = firstDateIneMonth.AddDays(-1);
			//			var carPartRemains = GetRemainsToDate(lastDateInMonth);

			//			db.InfoLastMonthDayRemains.AddRange(carPartRemains.Select(c => new InfoLastMonthDayRemain
			//			{
			//				Count = c.Remain,
			//				Date = firstDateIneMonth,
			//				DirectoryCarPartId = c.Id
			//			}));
			//			db.SaveChanges();
			//		}
			//		EditParameter(ParameterType.IsProcessingLastDateInMonthRemains, false);
			//	}
			//}
		}

		public CarPartRemain[] GetRemainsToDate(DateTime date)
		{
			using (var db = GetContext())
			{
				var lastMonthDayRemains =
					db.InfoLastMonthDayRemains.Where(p => (p.Date.Year == date.Year && p.Date.Month == date.Month)).ToList();

				var firstDateInMonth = new DateTime(date.Year, date.Month, 1);

				var containers = db.InfoContainers.Include(c => c.CarParts).Where(c =>
					(DbFunctions.DiffDays(firstDateInMonth, c.DatePhysical) >= 0 && DbFunctions.DiffDays(c.DatePhysical, date) >= 0))
					.ToList();

				var currentCarParts = db.CurrentCarParts.ToArray();
				var directoryCarParts = db.DirectoryCarParts.ToArray();
				var carPartsRUR = new List<ArticlePrice>();
				foreach (var directoryCarPart in directoryCarParts)
				{
					var currentCarPart = currentCarParts.Where(c => c.DirectoryCarPartId == directoryCarPart.Id)
						.OrderByDescending(c => c.Date)
						.FirstOrDefault(c => c.Date.Date <= date.Date && c.Currency == Currency.RUR);

					if (currentCarPart != null)
					{
						var articlePrice = new ArticlePrice
						{
							CarPartId = directoryCarPart.Id,
							Article = directoryCarPart.Article,
							Mark = directoryCarPart.Mark,
							Description = directoryCarPart.Description,
							PriceRUR = currentCarPart.PriceBase
						};
						carPartsRUR.Add(articlePrice);
					}
				}

				var carPartsId = lastMonthDayRemains.Select(r => r.DirectoryCarPartId).
					Union(containers.SelectMany(c => c.CarParts.Select(p => p.DirectoryCarPartId))).Distinct().ToList();

				var carParts = db.DirectoryCarParts.ToList();
				var marks = carParts.Select(c => c.Mark).Distinct().ToList();

				var carPartRemains = new List<CarPartRemain>();
				foreach (var carPartId in carPartsId)
				{
					var carPart = carPartsRUR.FirstOrDefault(c => c.CarPartId == carPartId);
					var baseArticle = carParts.First(c => c.Id == carPartId).Article.ToLower();

					if (carPart == null)
					{
						bool isFound = false;
						foreach (var mark in marks)
						{
							var tmpMark = mark != null ? mark.ToLower() : null;
							carPart = carPartsRUR.FirstOrDefault(c => c.Article.ToLower() == baseArticle &&
																	  (c.Mark == null && tmpMark == null ||
																	   c.Mark != null && c.Mark.ToLower() == tmpMark));
							if (carPart != null)
							{
								isFound = true;
								break;
							}
						}

						if (!isFound)
						{
							using (var sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "NonInDB.txt"), true))
							{
								sw.WriteLine(baseArticle);
							}
							continue;
						}
					}

					var lastMonthDayRemain = lastMonthDayRemains.FirstOrDefault(r => r.DirectoryCarPartId == carPartId);
					if (lastMonthDayRemain == null)
					{
						bool isFound = false;
						foreach (var mark in marks)
						{
							var tmpMark = mark != null ? mark.ToLower() : null;
							var directoryCarPart = carParts.FirstOrDefault(c => c.Article.ToLower() == baseArticle &&
																				(c.Mark == null && tmpMark == null ||
																				 c.Mark != null && c.Mark.ToLower() == tmpMark));
							if (directoryCarPart != null)
							{
								lastMonthDayRemain = lastMonthDayRemains.FirstOrDefault(r => r.DirectoryCarPartId == directoryCarPart.Id);
								if (lastMonthDayRemain != null)
								{
									isFound = true;
									break;
								}
							}
						}

						if (!isFound)
						{
							continue;
						}
					}

					var remains = lastMonthDayRemain.Count;
					remains +=
						containers.Sum(
							c =>
								c.CarParts.Where(p => p.DirectoryCarPartId == carPartId)
									.Sum(c2 => (c.IsIncoming ? c2.CountCarParts : -c2.CountCarParts)));

					if (remains <= 0)
					{
						continue;
					}

					var carPartRemain = new CarPartRemain
					{
						Id = carPartId,
						Article = carPart.Article + carPart.Mark,
						Description = carPart.Description,
						PriceRUR = carPart.PriceRUR,
						PriceUSD = carPart.PriceUSD,
						Remain = remains
					};

					carPartRemains.Add(carPartRemain);
				}

				return carPartRemains.ToArray();
			}
		}

		#endregion

		//done
		#region Warehouse

		public PalletContent[] GetPalletContents(string warehouseName, AddressCell address)
		{
			using (var db = GetContext())
			{
				int warehouseId = db.Warehouses.First(w => w.Name == warehouseName).Id;
				var location = db.PalletLocations.FirstOrDefault(l => l.WarehouseId == warehouseId && l.Row == address.Row &&
																	  l.Place == address.Place && l.Floor == address.Floor &&
																	  l.Pallet == address.Cell);

				if (location != null)
				{
					return db.PalletContents.Where(p => p.PalletLocationId == location.Id).ToArray();
				}
				else
				{
					return new PalletContent[0];
				}
			}
		}

		public PalletContent[] GetAllPallets(string warehouseName)
		{
			using (var db = GetContext())
			{
				var warehouse = db.Warehouses.FirstOrDefault(w => w.Name == warehouseName);
				return warehouse != null
					? db.PalletContents.Include(c => c.Location).Where(c => c.Location.WarehouseId == warehouse.Id).ToArray()
					: new PalletContent[0];
			}
		}

		public PalletContent[] SavePalletContents(string warehouseName, AddressCell address, CarPartPallet[] carPartPallets)
		{
			using (var db = GetContext())
			{
				int warehouseId = db.Warehouses.First(w => w.Name == warehouseName).Id;
				var removableContents =
					db.PalletContents.Include(c => c.Location).Where(c => c.Location.WarehouseId == warehouseId &&
																		  c.Location.Row == address.Row &&
																		  c.Location.Place == address.Place &&
																		  c.Location.Floor == address.Floor &&
																		  c.Location.Pallet == address.Cell);

				db.PalletContents.RemoveRange(removableContents);


				var articles = carPartPallets.Select(p => p.Article);
				var directoryCarParts = db.DirectoryCarParts.Where(c => articles.Contains(c.Article + c.Mark)).ToArray();

				var location = db.PalletLocations.FirstOrDefault(l => l.WarehouseId == warehouseId && l.Row == address.Row &&
																	  l.Place == address.Place && l.Floor == address.Floor &&
																	  l.Pallet == address.Cell);
				if (location == null)
				{
					location = new PalletLocation
					{
						WarehouseId = warehouseId,
						Row = address.Row,
						Place = address.Place,
						Floor = address.Floor,
						Pallet = address.Cell
					};
				}

				var palletContents = carPartPallets.Select(p => new PalletContent
				{
					Location = location,
					CountCarPart = p.CountCarParts,
					DirectoryCarPartId = directoryCarParts.First(c => c.FullCarPartName == p.Article).Id
				}).ToArray();

				db.PalletContents.AddRange(palletContents);
				db.SaveChanges();

				return palletContents;
			}
		}

		#endregion

		//done
		#region InfoTotalEqualCashSafeToMinsk

		public InfoTotalEqualCashSafeToMinsk[] GetTotalEqualCashSafeToMinsks(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoTotalEqualCashSafeToMinsks
					.Where(c => DbFunctions.DiffDays(c.Date, from) <= 0 && DbFunctions.DiffDays(c.Date, to) >= 0)
					.ToArray()
					.GroupBy(c => c.Date)
					.Select(g => g.OrderByDescending(c => c.LastUpdated).First())
					.ToArray();
			}
		}

		public void SaveTotalSafeAndMinskCashes(DateTime date, double minskSumm)
		{
			using (var db = GetContext())
			{
				var totalCash = new InfoTotalEqualCashSafeToMinsk();

				totalCash.Date = new DateTime(date.Year, date.Month, 1);
				totalCash.MinskCash = minskSumm;
				totalCash.LastUpdated = DateTime.Now;

				var prevDate = date.AddMonths(-1);
				var prevTotalCash =
					db.InfoTotalEqualCashSafeToMinsks.First(c => c.Date.Year == prevDate.Year && c.Date.Month == prevDate.Month);

				var costs = GetInfoCosts(prevDate.Year, prevDate.Month).ToList();
				double totalSumm = costs.Sum(c => c.IsIncoming ? c.Summ : -c.Summ);

				totalCash.SafeCash = prevTotalCash.SafeCash + totalSumm;

				db.InfoTotalEqualCashSafeToMinsks.Add(totalCash);
				db.SaveChanges();

				var monthes = db.InfoTotalEqualCashSafeToMinsks.Where(c => DbFunctions.DiffDays(c.Date, date) < 0).ToArray();
				if (monthes.Any())
				{
					foreach (var month in monthes)
					{
						var totalCashMonth = new InfoTotalEqualCashSafeToMinsk
						{
							Date = month.Date,
							MinskCash = month.MinskCash,
							LastUpdated = DateTime.Now
						};

						var prevDateMonth = month.Date.AddMonths(-1);
						var prevTotalCashMonth =
							db.InfoTotalEqualCashSafeToMinsks.First(c => c.Date.Year == prevDateMonth.Year &&
																		 c.Date.Month == prevDateMonth.Month);

						var costsMonth = GetInfoCosts(prevDateMonth.Year, prevDateMonth.Month).ToList();
						double totalSummMonth = costsMonth.Sum(c => c.IsIncoming ? c.Summ : -c.Summ);

						totalCashMonth.SafeCash = prevTotalCashMonth.SafeCash + totalSummMonth;
						db.InfoTotalEqualCashSafeToMinsks.Add(totalCashMonth);
					}

					db.SaveChanges();
				}
			}
		}

		#endregion


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

		#endregion

		//done
		#region Auth

		public bool LoginUser(int userId, string password)
		{
			using (var db = GetContext())
			{
				var auth = db.Auths.FirstOrDefault(s => s.DirectoryUserId == userId);
				if (auth == null)
				{
					return false;
				}

				var hash = CryptoHelper.GetHash(password + auth.Salt);

				return auth.Hash == hash;
			}
		}

		#endregion

		public DataContext GetContext()
		{
			return new DataContext();
		}
	}
}

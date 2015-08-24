using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using AutoMapper;
using AVService.Configuration;
using AVService.Models.DTO.Administration;
using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Helpers;
using AVService.Models.Entities.Infos;
using AVService.Models.Entities.Temps;
using AVService.Models.Entities.WareHouse;
using AVService.Models.Enums;
using AVService.Repositories;
using NLog;
using Shared.Enums;

namespace AVService
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

		private readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public AVBusinessLayer()
		{
			utilRepository = new UtilRepository();
			remainRepository = new RemainRepository(utilRepository);
			administrationRepository = new AdministrationRepository();
			warehouseRepository = new WarehouseRepository();
			costRepository = new CostRepository(utilRepository);
			timeManagementRepository = new TimeManagementRepository(utilRepository);
			initializationRepository = new InitializationRepository(utilRepository, timeManagementRepository, costRepository);

			AutoMapperConfiguration.Configurate();
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
		#region TypeOfPost

		public TypeOfPost[] GetDirectoryTypeOfPosts()
		{
			return Enum.GetNames(typeof (TypeOfPost)).Select(x => (TypeOfPost) Enum.Parse(typeof (TypeOfPost), x)).ToArray();
		}

		//public TypeOfPost AddDirectoryTypeOfPost(string directoryTypeOfPostName)
		//{
		//	return timeManagementRepository.AddDirectoryTypeOfPost(directoryTypeOfPostName);
		//}

		//public void RemoveDirectoryTypeOfPost(int directoryTypeOfPostId)
		//{
		//	timeManagementRepository.RemoveDirectoryTypeOfPost(directoryTypeOfPostId);
		//}

		public TypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date)
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

		public DirectoryPost AddDirectoryPost(string name, TypeOfPost typeOfPost, DirectoryCompany company,
			List<DirectoryPostSalary> postSalaries)
		{
			return timeManagementRepository.AddDirectoryPost(name, typeOfPost, company, postSalaries);
		}

		public DirectoryPost EditDirectoryPost(int postId, string name, TypeOfPost typeOfPost,
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

		public CurrentPost[] GetCurrentPostsByWorker(int workerId, int year, int month, int lastDayInMonth)
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

		public string GetParameterValueByString(ParameterType parameterType)
		{
			return utilRepository.GetParameterValue<string>(parameterType);
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
			Logger.Debug("InitializeAbsentDates");

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
		#region User

		public DTOUser[] GetUsers()
		{
			Logger.Debug("Get users");

			var users = administrationRepository.GetDirectoryUsers();
			return users.Select(Mapper.Map<DirectoryUser, DTOUser>).ToArray();
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
			timeManagementRepository.AddInfoOverTime(startDate, endDate, directoryRCs, description);
		}

		public void EditInfoOverTime(DateTime startDate, DateTime endDate, List<DirectoryRC> directoryRCs, string description)
		{
			timeManagementRepository.EditInfoOverTime(startDate, endDate, directoryRCs, description);
		}

		public void EditInfoOverTimeByDate(DateTime date, double hoursOverTime)
		{
			timeManagementRepository.EditInfoOverTimeByDate(date, hoursOverTime);
		}

		public InfoOverTime GetInfoOverTime(DateTime date)
		{
			return timeManagementRepository.GetInfoOverTime(date);
		}

		public InfoOverTime[] GetInfoOverTimes(int year, int month)
		{
			return timeManagementRepository.GetInfoOverTimes(year, month);
		}

		public DateTime[] GetInfoOverTimeDates(int year, int month)
		{
			return timeManagementRepository.GetInfoOverTimeDates(year, month);
		}

		public bool IsInfoOverTimeDate(DateTime date)
		{
			return timeManagementRepository.IsInfoOverTimeDate(date);
		}

		public void RemoveInfoOverTime(DateTime date)
		{
			timeManagementRepository.RemoveInfoOverTime(date);
		}

		#endregion

		//done
		#region InfoCost

		public InfoCost EditInfoCost(DateTime date, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note,
			bool isIncomming, double summ, Currency currency, double weight)
		{
			return costRepository.EditInfoCost(date,costItem,rc,note,isIncomming,summ,currency,weight);
		}

		public InfoCost[] GetInfoCostsByDate(DateTime date)
		{
			return costRepository.GetInfoCostsByDate(date);
		}

		public InfoCost GetInfoCost(int infoCostId)
		{
			return costRepository.GetInfoCost(infoCostId);
		}

		public InfoCost[] GetInfoCosts(int year, int month)
		{
			return costRepository.GetInfoCosts(year, month);
		}

		public InfoCost[] GetInfoCostsRCIncoming(int year, int month, string rcName)
		{
			return costRepository.GetInfoCostsRCIncoming(year, month, rcName);
		}

		public InfoCost[] GetInfoCosts26Expense(int year, int month)
		{
			return costRepository.GetInfoCosts26Expense(year, month);
		}


		public InfoCost[] GetInfoCostsRCAndAll(int year, int month, string rcName)
		{
			return costRepository.GetInfoCostsRCAndAll(year, month, rcName);
		}

		public InfoCost[] GetInfoCostsPAM16(int year, int month)
		{
			return costRepository.GetInfoCostsPAM16(year, month);
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnly(int year, int month)
		{
			return costRepository.GetInfoCostsTransportAndNoAllAndExpenseOnly(year, month);
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnlyByDate(DateTime date)
		{
			return costRepository.GetInfoCostsTransportAndNoAllAndExpenseOnlyByDate(date);
		}

		public void AddInfoCosts(DateTime date, DirectoryCostItem directoryCostItem, bool isIncoming,
			DirectoryTransportCompany transportCompany, double summ, Currency currency, List<Transport> transports)
		{
			costRepository.AddInfoCosts(date, directoryCostItem, isIncoming, transportCompany, summ, currency, transports);
		}

		public void RemoveInfoCost(InfoCost infoCost)
		{
			costRepository.RemoveInfoCost(infoCost);
		}

		public int[] GetInfoCostYears()
		{
			return costRepository.GetInfoCostYears();
		}

		public int[] GetInfoCostMonthes(int year)
		{
			return costRepository.GetInfoCostMonthes(year);
		}

		public double GetInfoCost26Summ(int year, int month)
		{
			return GetInfoCost26Summ(year, month);
		}

		public string[] GetInfoCostsIncomingTotalSummsCurrency(int year, int month, string rcName, bool? isIncoming = null, string costItem = null)
		{
			return costRepository.GetInfoCostsIncomingTotalSummsCurrency(year, month, rcName, isIncoming);
		}

		#endregion

		//done
		#region InfoLoan

		public InfoLoan AddInfoLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			return costRepository.AddInfoLoan(date, loanTakerName, directoryWorker, summ, currency, countPayments, description);
		}

		public InfoLoan EditInfoLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			return costRepository.EditInfoLoan(id, date, loanTakerName, directoryWorker, summ, currency, countPayments,
				description);
		}

		public void RemoveInfoLoan(InfoLoan selectedInfoLoan)
		{
			costRepository.RemoveInfoLoan(selectedInfoLoan);
		}

		public InfoLoan[] GetInfoLoans(DateTime from, DateTime to)
		{
			return costRepository.GetInfoLoans(from, to);
		}

		public double GetLoans()
		{
			return costRepository.GetLoans();
		}

		#endregion

		//done
		#region InfoPrivateLoan

		public InfoPrivateLoan AddInfoPrivateLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker,
			double summ, Currency currency, int countPayments, string description)
		{
			return costRepository.AddInfoPrivateLoan(date, loanTakerName, directoryWorker, summ, currency, countPayments,
				description);
		}

		public InfoPrivateLoan EditInfoPrivateLoan(int id, DateTime date, string loanTakerName,
			DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
		{
			return costRepository.EditInfoPrivateLoan(id, date, loanTakerName, directoryWorker, summ, currency, countPayments,
				description);
		}

		public void RemoveInfoPrivateLoan(InfoPrivateLoan selectedInfoPrivateLoan)
		{
			costRepository.RemoveInfoPrivateLoan(selectedInfoPrivateLoan);
		}

		public InfoPrivateLoan[] GetInfoPrivateLoans(DateTime from, DateTime to)
		{
			return costRepository.GetInfoPrivateLoans(from, to);
		}

		public double GetPrivateLoans()
		{
			return costRepository.GetPrivateLoans();
		}

		#endregion

		//done
		#region DirectoryCostItem

		public DirectoryCostItem[] GetDirectoryCostItems()
		{
			return costRepository.GetDirectoryCostItems();
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
			return costRepository.GetDirectoryNotes();
		}

		public DirectoryNote GetDirectoryNote(string description)
		{
			return costRepository.GetDirectoryNote(description);
		}

		public bool IsDirectoryNote(string note)
		{
			return costRepository.IsDirectoryNote(note);
		}

		public DirectoryNote AddDirectoryNote(string note)
		{
			return costRepository.AddDirectoryNote(note);
		}

		#endregion

		//done
		#region DefaultCosts

		public DefaultCost[] GetDefaultCosts()
		{
			return costRepository.GetDefaultCosts();
		}

		public DefaultCost AddDefaultCost(DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			return costRepository.AddDefaultCost(costItem, rc, note, summ, day);
		}

		public void EditDefaultCost(int id, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			costRepository.EditDefaultCost(id, costItem, rc, note, summ, day);
		}

		public void RemoveDefaultCost(DefaultCost defaultCost)
		{
			costRepository.RemoveDefaultCost(defaultCost);
		}

		public void InitializeDefaultCosts()
		{
			costRepository.InitializeDefaultCosts();
		}

		#endregion

		//done
		#region DirectoryTransportCompanies

		public DirectoryTransportCompany[] GetDirectoryTransportCompanies()
		{
			return costRepository.GetDirectoryTransportCompanies();
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
			return costRepository.GetDirectoryLoanTakers();
		}

		public DirectoryLoanTaker AddDirectoryLoanTaker(string name)
		{
			return costRepository.AddDirectoryLoanTaker(name);
		}

		#endregion

		//done
		#region InfoPayments

		public InfoPayment[] GetInfoPayments(int infoLoanId)
		{
			return costRepository.GetInfoPayments(infoLoanId);
		}

		public InfoPayment AddInfoPayment(int infoLoanId, DateTime date, double summ)
		{
			return costRepository.AddInfoPayment(infoLoanId, date, summ);
		}

		public void RemoveInfoPayment(InfoPayment selectedInfoPayment)
		{
			costRepository.RemoveInfoPayment(selectedInfoPayment);
		}

		#endregion

		//done
		#region InfoCard

		public void SetCardAvaliableSumm(string cardName, double avaliableSumm)
		{
			costRepository.SetCardAvaliableSumm(cardName, avaliableSumm);
		}

		public double GetCardAvaliableSumm(string cardName)
		{
			return costRepository.GetCardAvaliableSumm(cardName);
		}

		#endregion

		//done
		#region InfoSafe

		public InfoSafe AddInfoSafe(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description, string bankName = null)
		{
			return costRepository.AddInfoSafe(date, isIncoming, summCash, currency, cashType, description, bankName);
		}

		public InfoSafe AddInfoSafeHand(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description)
		{
			return costRepository.AddInfoSafeHand(date, isIncoming, summCash, currency, cashType, description);
		}

		public InfoSafe AddInfoSafeCard(DateTime date, double availableSumm, Currency currency, string description,
			string bankName)
		{
			return costRepository.AddInfoSafeCard(date, availableSumm, currency, description, bankName);
		}

		public bool IsNewMessage(DateTime date, string description)
		{
			return costRepository.IsNewMessage(date, description);
		}

		public InfoSafe[] GetInfoSafes()
		{
			return costRepository.GetInfoSafes();
		}

		public InfoSafe[] GetInfoSafesByCashType(CashType cashType, DateTime from, DateTime to)
		{
			return costRepository.GetInfoSafesByCashType(cashType, from, to);
		}

		public void RemoveInfoSafe(InfoSafe infoSafe)
		{
			costRepository.RemoveInfoSafe(infoSafe);
		}

		#endregion

		//done
		#region InfoPrivatePayments

		public InfoPrivatePayment[] GetInfoPrivatePayments(int infoSafeId)
		{
			return costRepository.GetInfoPrivatePayments(infoSafeId);
		}

		public InfoPrivatePayment AddInfoPrivatePayment(int infoPrivateLoanId, DateTime date, double summ)
		{
			return costRepository.AddInfoPrivatePayment(infoPrivateLoanId, date, summ);
		}

		public void RemoveInfoPrivatePayment(InfoPrivatePayment selectedInfoPrivatePayment)
		{
			costRepository.RemoveInfoPrivatePayment(selectedInfoPrivatePayment);
		}

		#endregion

		//done
		#region CurrencyValue

		public CurrencyValue GetCurrencyValue(string name)
		{
			return costRepository.GetCurrencyValue(name);
		}

		public double GetCurrencyValueSumm(string name, Currency currency)
		{
			return costRepository.GetCurrencyValueSumm(name, currency);
		}

		public void EditCurrencyValueSumm(string name, Currency currency, double summ)
		{
			costRepository.EditCurrencyValueSumm(name, currency, summ);
		}

		public void EditCurrencyValueSummChange(string name, Currency currency, double summ)
		{
			costRepository.EditCurrencyValueSummChange(name, currency, summ);
		}

		#endregion


		#region DirectoryPostSalary

		public DirectoryPostSalary[] GetDirectoryPostSalaries(int postId)
		{
			return timeManagementRepository.GetDirectoryPostSalaries(postId);
		}

		public DirectoryPostSalary GetDirectoryPostSalaryByDate(int postId, DateTime date)
		{
			return timeManagementRepository.GetDirectoryPostSalaryByDate(postId, date);
		}

		public DirectoryPostSalary[] GetDirectoryPostSalariesByMonth(int year, int month)
		{
			return timeManagementRepository.GetDirectoryPostSalariesByMonth(year, month);
		}

		#endregion

		//done
		#region DirectoryCarPart

		public DirectoryCarPart AddDirectoryCarPart(string article, string mark, string description, string originalNumber,
			string factoryNumber, string crossNumber, string material, string attachment, string countInBox, bool isImport)
		{
			return remainRepository.AddDirectoryCarPart(article, material, description, originalNumber, factoryNumber,
				crossNumber, material, attachment, countInBox, isImport);
		}

		public DirectoryCarPart[] GetDirectoryCarParts()
		{
			return remainRepository.GetDirectoryCarParts();
		}

		public DirectoryCarPart GetDirectoryCarPart(string article, string mark)
		{
			return remainRepository.GetDirectoryCarPart(article, mark);

		}

		#endregion

		//done
		#region InfoContainer

		public int[] GetContainerYears(bool isIncoming)
		{
			return remainRepository.GetContainerYears(isIncoming);
		}

		public int[] GetContainerMonthes(int selectedYear, bool isIncoming)
		{
			return remainRepository.GetContainerMonthes(selectedYear, isIncoming);
		}

		public InfoContainer[] GetContainers(int year, int month, bool isIncoming)
		{
			return remainRepository.GetContainers(year, month, isIncoming);
		}

		public InfoContainer[] GetInfoContainers(List<InfoContainer> containers)
		{
			return remainRepository.GetInfoContainers(containers);
		}

		public InfoContainer GetInfoContainer(int containerId)
		{
			return remainRepository.GetInfoContainer(containerId);
		}

		public void RemoveInfoContainer(InfoContainer container)
		{
			remainRepository.RemoveInfoContainer(container);
		}

		public InfoContainer AddInfoContainer(string name, string description, DateTime datePhysical, DateTime? dateOrder,
			bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			return remainRepository.AddInfoContainer(name, description, datePhysical, dateOrder, isIncoming, carParts);
		}

		public void EditInfoContainer(int containerId, string name, string description, DateTime datePhysical,
			DateTime? dateOrder, bool isIncoming, List<CurrentContainerCarPart> carParts)
		{
			remainRepository.EditInfoContainer(containerId,name,description,datePhysical,dateOrder,isIncoming,carParts);
		}

		public InfoCarPartMovement[] GetMovementsByDates(DirectoryCarPart selectedDirectoryCarPart,
			DateTime selectedDateFrom, DateTime selectedDateTo)
		{
			return remainRepository.GetMovementsByDates(selectedDirectoryCarPart, selectedDateFrom, selectedDateTo);
		}

		public void RemoveContainers(int year, int month)
		{
			remainRepository.RemoveContainers(year, month);
		}

		#endregion

		//done
		#region InfoLastMonthDayRemain

		public InfoLastMonthDayRemain GetInfoLastMonthDayRemain(DateTime date, int carPartId)
		{
			return remainRepository.GetInfoLastMonthDayRemain(date, carPartId);
		}

		public int GetInfoCarPartIncomingCountTillDate(DateTime date, int carPartId, bool isIncoming)
		{
			return remainRepository.GetInfoCarPartIncomingCountTillDate(date, carPartId, isIncoming);
		}

		public InfoLastMonthDayRemain AddInfoLastMonthDayRemain(DirectoryCarPart carPart, DateTime date, int count)
		{
			return remainRepository.AddInfoLastMonthDayRemain(carPart, date, count);
		}

		public void RemoveInfoLastMonthDayRemains(int year, int month)
		{
			remainRepository.RemoveInfoLastMonthDayRemains(year, month);

		}

		#endregion

		//done
		#region CurrentCarParts

		public CurrentCarPart AddCurrentCarPart(DirectoryCarPart directoryCarPart, DateTime priceDate, double priceBase,
			double? priceBigWholesale, double? priceSmallWholesale)
		{
			return remainRepository.AddCurrentCarPart(directoryCarPart, priceDate, priceBase, priceSmallWholesale,
				priceSmallWholesale);
		}

		public CurrentCarPart AddCurrentCarPartNoSave(DateTime priceDate, double priceBase, double? priceBigWholesale,
			double? priceSmallWholesale, Currency currency, string fullName)
		{
			return remainRepository.AddCurrentCarPartNoSave(priceDate, priceBase, priceSmallWholesale, priceSmallWholesale,
				currency, fullName);
		}

		public CurrentCarPart[] GetCurrentCarParts()
		{
			return remainRepository.GetCurrentCarParts();
		}

		public CurrentCarPart GetCurrentCarPart(int directoryCarPartId, DateTime date)
		{
			return remainRepository.GetCurrentCarPart(directoryCarPartId, date);
		}

		public ArticlePrice[] GetArticlePrices(DateTime date, Currency currency)
		{
			return remainRepository.GetArticlePrices(date, currency);
		}

		#endregion

		//done
		#region CurrentCarPartsRemainsToDate

		public void SetRemainsToFirstDateInMonth()
		{
			remainRepository.SetRemainsToFirstDateInMonth();
		}

		public CarPartRemain[] GetRemainsToDate(DateTime date)
		{
			return remainRepository.GetRemainsToDate(date);
		}

		#endregion

		//done
		#region Warehouse

		public PalletContent[] GetPalletContents(string warehouseName, AddressCell address)
		{
			return warehouseRepository.GetPalletContents(warehouseName, address);
		}

		public PalletContent[] GetAllPallets(string warehouseName)
		{
			return warehouseRepository.GetAllPallets(warehouseName);
		}

		public PalletContent[] SavePalletContents(string warehouseName, AddressCell address, CarPartPallet[] carPartPallets)
		{
			return warehouseRepository.SavePalletContents(warehouseName, address, carPartPallets);
		}

		public InfoTotalEqualCashSafeToMinsk[] GetTotalEqualCashSafeToMinsks(DateTime from, DateTime to)
		{
			return costRepository.GetTotalEqualCashSafeToMinsks(from, to);
		}

		public void SaveTotalSafeAndMinskCashes(DateTime date, double minskSumm)
		{
			costRepository.SaveTotalSafeAndMinskCashes(date, minskSumm);
		}

		public double GetPam16Percentage(DateTime date)
		{
			return timeManagementRepository.GetPam16Percentage(date);
		}

		public void SavePam16Percentage(DateTime date, double pam16Percentage)
		{
			timeManagementRepository.SavePam16Percentage(date, pam16Percentage);
		}

		#endregion

		//done
		#region Auth

		public bool LoginUser(int userId, string password)
		{
			return administrationRepository.LoginUser(userId, password);
		}

		#endregion

		public void AddCurrentCarParts(CurrentCarPart[] carParts)
		{
			remainRepository.AddCurrentCarParts(carParts);
		}

		public void AddDirectoryCarParts(DirectoryCarPart[] carParts)
		{
			remainRepository.AddtDirectoryCarParts(carParts);
		}

		public void AddInfoLastMonthDayRemains(InfoLastMonthDayRemain[] remains)
		{
			remainRepository.AddInfoLastMonthDayRemains(remains);
		}

		public void AddInfoContainers(InfoContainer[] containers)
		{
			remainRepository.AddInfoContainers(containers);
		}

		public void AddCurrentContainerCarParts(CurrentContainerCarPart[] carParts)
		{
			remainRepository.AddCurrentContainerCarPart(carParts);
		}
	}
}

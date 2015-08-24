using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Media.Imaging;
using AVService.Models.DTO.Administration;
using AVService.Models.Entities.Currents;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Helpers;
using AVService.Models.Entities.Infos;
using AVService.Models.Entities.Temps;
using AVService.Models.Entities.WareHouse;
using AVService.Models.Enums;
using Shared.Enums;

namespace AVService
{
	[ServiceContract]
	[ServiceKnownType(typeof(int))]
	public interface IAVBusinessLayer
	{
		[OperationContract]
		string[] GetDirectoryCompaniesByWorker(int workerId, int year, int month, int lastDayInMonth);

		[OperationContract]
		DirectoryCompany[] GetDirectoryCompanies();

		[OperationContract]
		DirectoryCompany AddDirectoryCompany(string directoryCompanyName);

		[OperationContract]
		void RemoveDirectoryCompany(int directoryCompanyId);

		[OperationContract]
		TypeOfPost[] GetDirectoryTypeOfPosts();

		//[OperationContract]
		//TypeOfPost AddDirectoryTypeOfPost(string directoryTypeOfPostName);

		//[OperationContract]
		//void RemoveDirectoryTypeOfPost(int directoryTypeOfPostId);

		[OperationContract]
		TypeOfPost GetDirectoryTypeOfPost(int workerId, DateTime date);

		[OperationContract]
		DirectoryPost[] GetDirectoryPosts();

		[OperationContract]
		DirectoryPost[] GetDirectoryPostsByCompany(DirectoryCompany company);

		[OperationContract]
		DirectoryPost GetDirectoryPost(string postName);

		[OperationContract]
		DirectoryNote AddDirectoryNote(string note);

		[OperationContract]
		DirectoryPost AddDirectoryPost(string name, TypeOfPost typeOfPost, DirectoryCompany company,
			List<DirectoryPostSalary> postSalaries);

		[OperationContract]
		DirectoryPost EditDirectoryPost(int postId, string name, TypeOfPost typeOfPost,
			DirectoryCompany company, List<DirectoryPostSalary> postSalaries);

		[OperationContract]
		void RemoveDirectoryPost(DirectoryPost post);

		[OperationContract]
		bool ExistsDirectoryPost(string name);

		[OperationContract]
		DirectoryWorker[] GetDeadSpiritDirectoryWorkers(DateTime date);

		[OperationContract]
		DirectoryWorker AddDirectoryWorkerWithMultiplyPosts(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate, BitmapImage photo,
			DateTime? fireDate, ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit);

		[OperationContract]
		DirectoryWorker AddDirectoryWorker(string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone, string cellPhone, DateTime startDate,
			DateTime? fireDate, CurrentPost currentPost);

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkers();

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkersByMonth(int year, int month);

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkersMonthTimeSheet(int year, int month);

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkersByTypeOfPost(int year, int month, bool isOffice);

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkersBetweenDates(DateTime fromDate, DateTime toDate);

		[OperationContract]
		DirectoryWorker[] GetDirectoryWorkersWithInfoDatesAndPanalties(int year, int month, bool isOffice);

		[OperationContract]
		DirectoryWorker GetDirectoryWorkerById(int workerId);

		[OperationContract]
		DirectoryWorker GetDirectoryWorkerWithPosts(int workerId);

		[OperationContract]
		DirectoryWorker GetDirectoryWorker(string lastName, string firstName);

		[OperationContract]
		DirectoryWorker EditDirectoryWorker(int id, string lastName, string firstName, string midName, Gender gender,
			DateTime birthDay, string address, string homePhone,
			string cellPhone, DateTime startDate, BitmapImage photo, DateTime? fireDate,
			ICollection<CurrentCompanyAndPost> currentCompaniesAndPosts, bool isDeadSpirit);

		[OperationContract]
		InfoDate[] GetInfoDatePanalties(int workerId, int year, int month);

		[OperationContract]
		InfoDate[] GetInfoDatePanaltiesWithoutCash(int workerId, int year, int month);

		[OperationContract]
		void EditInfoDateHour(int workerId, DateTime date, string hour);

		[OperationContract]
		InfoDate[] GetInfoDates(DateTime date);

		[OperationContract]
		InfoDate[] GetInfoDatesByWorker(int workerId, int year, int month);

		[OperationContract]
		InfoDate[] GetInfoDatesByMonth(int year, int month);

		[OperationContract]
		double? IsOverTime(InfoDate infoDate, List<DateTime> weekEnds);

		[OperationContract]
		void EditDeadSpiritHours(int deadSpiritWorkerId, DateTime date, double hoursSpiritWorker);

		[OperationContract]
		DateTime GetLastWorkDay(int workerId);

		[OperationContract]
		void EditInfoMonthPayment(int workerId, DateTime date, string propertyName, double propertyValue);

		[OperationContract]
		int[] GetYears();

		[OperationContract]
		int[] GetMonthes(int year);

		[OperationContract]
		InfoMonth GetInfoMonth(int workerId, int year, int month);

		[OperationContract]
		InfoMonth[] GetInfoMonthes(int year, int month);

		[OperationContract]
		InfoPanalty GetInfoPanalty(int workerId, DateTime date);

		[OperationContract]
		bool IsInfoPanalty(int workerId, DateTime date);

		[OperationContract]
		InfoPanalty AddInfoPanalty(int workerId, DateTime date, double summ, string description);

		[OperationContract]
		InfoPanalty EditInfoPanalty(int workerId, DateTime date, double summ, string description);

		[OperationContract]
		void RemoveInfoPanalty(int workerId, DateTime date);

		[OperationContract]
		void AddCurrentPost(int workerId, CurrentCompanyAndPost currentCompanyAndPost);

		[OperationContract]
		CurrentPost[] GetCurrentPosts(DateTime lastDateInMonth);

		[OperationContract]
		CurrentPost GetCurrentPost(int workerId, DateTime date);

		[OperationContract]
		CurrentPost GetMainPost(int workerId, DateTime date);

		[OperationContract]
		CurrentPost[] GetCurrentMainPosts(DateTime lastDateInMonth);

		[OperationContract]
		int GetCountWorkDaysInMonth(int year, int month);

		[OperationContract]
		DateTime[] GetHolidaysByMonth(int year, int month);

		[OperationContract]
		DateTime[] GetHolidaysByYear(int year);

		[OperationContract]
		DateTime[] GetHolidays(DateTime fromDate, DateTime toDate);

		[OperationContract]
		bool IsWeekend(DateTime date);

		[OperationContract]
		void SetHolidays(int year, List<DateTime> holidays);

		[OperationContract]
		void EditParameter(ParameterType parameterType, object value);

		[OperationContract]
		int GetParameterValueByInt(ParameterType parameterType);

		[OperationContract]
		double GetParameterValueByDouble(ParameterType parameterType);

		[OperationContract]
		bool GetParameterValueByBool(ParameterType parameterType);

		[OperationContract]
		string GetParameterValueByString(ParameterType parameterType);

		[OperationContract]
		DateTime GetParameterValueByDateTime(ParameterType parameterType);

		[OperationContract]
		void InitializeAbsentDates();

		[OperationContract]
		DirectoryUserStatus[] GetDirectoryUserStatuses();

		[OperationContract]
		DirectoryUserStatus AddDirectoryUserStatus(string name, List<CurrentUserStatusPrivilege> privileges);

		[OperationContract]
		void EditDirectoryUserStatus(int userStatusId, string userStatusName,
			List<CurrentUserStatusPrivilege> privileges);

		[OperationContract]
		void RemoveDirectoryUserStatus(int id);

		[OperationContract]

		DTOUser[] GetUsers();

		[OperationContract]

		DirectoryUser GetDirectoryUser(int userId);

		[OperationContract]
		DirectoryUser AddDirectoryUser(string userName, string password, DirectoryUserStatus userStatus);

		[OperationContract]
		DirectoryUser AddDirectoryUserAdmin(string userName, string password);

		[OperationContract]
		void EditDirectoryUser(int userId, string userName, string password, DirectoryUserStatus userStatus);

		[OperationContract]

		void RemoveDirectoryUser(DirectoryUser user);

		[OperationContract]
		DirectoryUserStatusPrivilege GetDirectoryUserStatusPrivilege(string privilegeName);

		[OperationContract]
		string[] GetPrivileges(int userId);

		[OperationContract]
		DirectoryRC[] GetDirectoryRCs();

		[OperationContract]
		DirectoryRC[] GetDirectoryRCsByPercentage();

		[OperationContract]
		DirectoryRC GetDirectoryRC(string name);

		[OperationContract]
		DirectoryRC AddDirectoryRC(string directoryRCName, string descriptionName, int percentes);

		[OperationContract]
		void RemoveDirectoryRC(int directoryRCId);

		[OperationContract]
		DirectoryRC[] GetDirectoryRCsMonthIncoming(int year, int month);

		[OperationContract]
		DirectoryRC[] GetDirectoryRCsMonthExpense(int year, int month);

		[OperationContract]
		void AddInfoOverTime(DateTime startDate, DateTime endDate, ICollection<DirectoryRC> directoryRCs,
			string description);

		[OperationContract]
		void EditInfoOverTime(DateTime startDate, DateTime endDate, List<DirectoryRC> directoryRCs, string description);

		[OperationContract]
		void EditInfoOverTimeByDate(DateTime date, double hoursOverTime);

		[OperationContract]
		InfoOverTime GetInfoOverTime(DateTime date);

		[OperationContract]
		InfoOverTime[] GetInfoOverTimes(int year, int month);

		[OperationContract]
		DateTime[] GetInfoOverTimeDates(int year, int month);

		[OperationContract]
		bool IsInfoOverTimeDate(DateTime date);

		[OperationContract]
		void RemoveInfoOverTime(DateTime date);

		[OperationContract]
		InfoCost EditInfoCost(DateTime date, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note,
			bool isIncomming, double summ, Currency currency, double weight);

		[OperationContract]
		InfoCost[] GetInfoCostsByDate(DateTime date);

		[OperationContract]
		InfoCost GetInfoCost(int infoCostId);

		[OperationContract]
		InfoCost[] GetInfoCosts(int year, int month);

		[OperationContract]
		InfoCost[] GetInfoCostsRCIncoming(int year, int month, string rcName);

		[OperationContract]
		InfoCost[] GetInfoCosts26Expense(int year, int month);


		[OperationContract]
		InfoCost[] GetInfoCostsRCAndAll(int year, int month, string rcName);

		[OperationContract]
		InfoCost[] GetInfoCostsPAM16(int year, int month);


		[OperationContract]
		InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnly(int year, int month);

		[OperationContract]
		InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnlyByDate(DateTime date);

		[OperationContract]
		void AddInfoCosts(DateTime date, DirectoryCostItem directoryCostItem, bool isIncoming,
			DirectoryTransportCompany transportCompany, double summ, Currency currency, List<Transport> transports);

		[OperationContract]
		void RemoveInfoCost(InfoCost infoCost);


		[OperationContract]
		int[] GetInfoCostYears();


		[OperationContract]
		int[] GetInfoCostMonthes(int year);


		[OperationContract]
		double GetInfoCost26Summ(int year, int month);

		[OperationContract]
		string[] GetInfoCostsIncomingTotalSummsCurrency(int year, int month, string rcName, bool? isIncoming = null,
			string costItem = null);

		[OperationContract]
		InfoLoan AddInfoLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description);

		[OperationContract]
		InfoLoan EditInfoLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description);

		[OperationContract]
		void RemoveInfoLoan(InfoLoan selectedInfoLoan);

		[OperationContract]
		InfoLoan[] GetInfoLoans(DateTime from, DateTime to);

		[OperationContract]
		double GetLoans();

		[OperationContract]
		InfoPrivateLoan AddInfoPrivateLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker,
			double summ, Currency currency, int countPayments, string description);

		[OperationContract]
		InfoPrivateLoan EditInfoPrivateLoan(int id, DateTime date, string loanTakerName,
			DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description);

		[OperationContract]
		void RemoveInfoPrivateLoan(InfoPrivateLoan selectedInfoPrivateLoan);

		[OperationContract]
		InfoPrivateLoan[] GetInfoPrivateLoans(DateTime from, DateTime to);

		[OperationContract]
		double GetPrivateLoans();

		[OperationContract]
		DirectoryCostItem[] GetDirectoryCostItems();


		[OperationContract]
		DirectoryCostItem GetDirectoryCostItem(string costItemName);

		[OperationContract]
		DirectoryNote[] GetDirectoryNotes();

		[OperationContract]
		DirectoryNote GetDirectoryNote(string description);

		[OperationContract]
		bool IsDirectoryNote(string note);

		[OperationContract]
		DefaultCost[] GetDefaultCosts();

		[OperationContract]
		DefaultCost AddDefaultCost(DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day);

		[OperationContract]
		void EditDefaultCost(int id, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day);

		[OperationContract]
		void RemoveDefaultCost(DefaultCost defaultCost);

		[OperationContract]
		void InitializeDefaultCosts();

		[OperationContract]
		DirectoryTransportCompany[] GetDirectoryTransportCompanies();

		[OperationContract]
		CurrentRC[] GetCurrentRCs(IEnumerable<int> ids);

		[OperationContract]
		DirectoryLoanTaker[] GetDirectoryLoanTakers();

		[OperationContract]
		DirectoryLoanTaker AddDirectoryLoanTaker(string name);

		[OperationContract]
		InfoPayment[] GetInfoPayments(int infoLoanId);

		[OperationContract]
		InfoPayment AddInfoPayment(int infoLoanId, DateTime date, double summ);

		[OperationContract]
		void RemoveInfoPayment(InfoPayment selectedInfoPayment);

		[OperationContract]
		void SetCardAvaliableSumm(string cardName, double avaliableSumm);

		[OperationContract]
		double GetCardAvaliableSumm(string cardName);

		[OperationContract]
		InfoSafe AddInfoSafe(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description, string bankName = null);

		[OperationContract]
		InfoSafe AddInfoSafeHand(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description);

		[OperationContract]
		InfoSafe AddInfoSafeCard(DateTime date, double availableSumm, Currency currency, string description,
			string bankName);

		[OperationContract]
		bool IsNewMessage(DateTime date, string description);

		[OperationContract]
		InfoSafe[] GetInfoSafes();

		[OperationContract]
		InfoSafe[] GetInfoSafesByCashType(CashType cashType, DateTime from, DateTime to);

		[OperationContract]
		void RemoveInfoSafe(InfoSafe infoSafe);

		[OperationContract]
		InfoPrivatePayment[] GetInfoPrivatePayments(int infoSafeId);

		[OperationContract]
		InfoPrivatePayment AddInfoPrivatePayment(int infoPrivateLoanId, DateTime date, double summ);

		[OperationContract]
		void RemoveInfoPrivatePayment(InfoPrivatePayment selectedInfoPrivatePayment);

		[OperationContract]
		CurrencyValue GetCurrencyValue(string name);

		[OperationContract]
		double GetCurrencyValueSumm(string name, Currency currency);

		[OperationContract]
		void EditCurrencyValueSumm(string name, Currency currency, double summ);

		[OperationContract]
		void EditCurrencyValueSummChange(string name, Currency currency, double summ);


		[OperationContract]
		DirectoryPostSalary[] GetDirectoryPostSalaries(int postId);

		[OperationContract]
		DirectoryPostSalary GetDirectoryPostSalaryByDate(int postId, DateTime date);

		[OperationContract]
		DirectoryPostSalary[] GetDirectoryPostSalariesByMonth(int year, int month);

		[OperationContract]
		DirectoryCarPart AddDirectoryCarPart(string article, string mark, string description, string originalNumber,
			string factoryNumber, string crossNumber, string material, string attachment, string countInBox, bool isImport);

		[OperationContract]
		DirectoryCarPart[] GetDirectoryCarParts();

		[OperationContract]
		DirectoryCarPart GetDirectoryCarPart(string article, string mark);

		[OperationContract]
		int[] GetContainerYears(bool isIncoming);

		[OperationContract]
		int[] GetContainerMonthes(int selectedYear, bool isIncoming);

		[OperationContract]
		InfoContainer[] GetContainers(int year, int month, bool isIncoming);

		[OperationContract]
		InfoContainer[] GetInfoContainers(List<InfoContainer> containers);


		[OperationContract]
		InfoContainer GetInfoContainer(int containerId);

		[OperationContract]
		void RemoveInfoContainer(InfoContainer container);

		[OperationContract]
		InfoContainer AddInfoContainer(string name, string description, DateTime datePhysical, DateTime? dateOrder,
			bool isIncoming, List<CurrentContainerCarPart> carParts);

		[OperationContract]
		void EditInfoContainer(int containerId, string name, string description, DateTime datePhysical,
			DateTime? dateOrder, bool isIncoming, List<CurrentContainerCarPart> carParts);

		[OperationContract]
		InfoCarPartMovement[] GetMovementsByDates(DirectoryCarPart selectedDirectoryCarPart,
			DateTime selectedDateFrom, DateTime selectedDateTo);

		[OperationContract]
		void RemoveContainers(int year, int month);

		[OperationContract]
		InfoLastMonthDayRemain GetInfoLastMonthDayRemain(DateTime date, int carPartId);

		[OperationContract]
		int GetInfoCarPartIncomingCountTillDate(DateTime date, int carPartId, bool isIncoming);

		[OperationContract]
		InfoLastMonthDayRemain AddInfoLastMonthDayRemain(DirectoryCarPart carPart, DateTime date, int count);

		[OperationContract]
		void RemoveInfoLastMonthDayRemains(int year, int month);

		[OperationContract]
		CurrentCarPart AddCurrentCarPart(DirectoryCarPart directoryCarPart, DateTime priceDate, double priceBase,
			double? priceBigWholesale, double? priceSmallWholesale);


		[OperationContract]
		CurrentCarPart AddCurrentCarPartNoSave(DateTime priceDate, double priceBase, double? priceBigWholesale,
			double? priceSmallWholesale, Currency currency, string fullName);

		[OperationContract]
		CurrentCarPart[] GetCurrentCarParts();

		[OperationContract]
		CurrentCarPart GetCurrentCarPart(int directoryCarPartId, DateTime date);

		[OperationContract]
		ArticlePrice[] GetArticlePrices(DateTime date, Currency currency);

		[OperationContract]
		void SetRemainsToFirstDateInMonth();

		[OperationContract]
		CarPartRemain[] GetRemainsToDate(DateTime date);

		[OperationContract]
		PalletContent[] GetPalletContents(string warehouseName, AddressCell address);

		[OperationContract]
		PalletContent[] GetAllPallets(string warehouseName);

		[OperationContract]
		PalletContent[] SavePalletContents(string warehouseName, AddressCell address, CarPartPallet[] carPartPallets);

		[OperationContract]
		InfoTotalEqualCashSafeToMinsk[] GetTotalEqualCashSafeToMinsks(DateTime from, DateTime to);


		[OperationContract]
		void SaveTotalSafeAndMinskCashes(DateTime date, double minskSumm);

		[OperationContract]
		double GetPam16Percentage(DateTime date);


		[OperationContract]
		void SavePam16Percentage(DateTime date, double pam16Percentage);

		[OperationContract]
		bool LoginUser(int userId, string password);

		[OperationContract]
		CurrentPost[] GetCurrentPostsByWorker(int workerId, int year, int month, int lastDayInMonth);

		[OperationContract]
		void AddCurrentCarParts(CurrentCarPart[] carParts);

		[OperationContract]
		void AddDirectoryCarParts(DirectoryCarPart[] carParts);

		[OperationContract]
		void AddInfoLastMonthDayRemains(InfoLastMonthDayRemain[] remains);

		[OperationContract]
		void AddInfoContainers(InfoContainer[] containers);

		[OperationContract]
		void AddCurrentContainerCarParts(CurrentContainerCarPart[] carParts);
	}
}

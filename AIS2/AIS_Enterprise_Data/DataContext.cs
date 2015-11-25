using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Properties;
using AIS_Enterprise_Data.WareHouse;

namespace AIS_Enterprise_Data
{
	public class DataContext : DbContext
	{
		public DataContext(string connectionString)
			: base(connectionString)
		{
		}


		#region Properties

		public DbSet<DirectoryCompany> DirectoryCompanies { get; set; }
		public DbSet<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }
		public DbSet<DirectoryPost> DirectoryPosts { get; set; }
		public DbSet<DirectoryPostSalary> DirectoryPostSalaries { get; set; }
		public DbSet<DirectoryWorker> DirectoryWorkers { get; set; }
		public DbSet<DirectoryHoliday> DirectoryHolidays { get; set; }
		public DbSet<DirectoryUser> DirectoryUsers { get; set; }
		public DbSet<DirectoryUserStatus> DirectoryUserStatuses { get; set; }
		public DbSet<DirectoryUserStatusPrivilege> DirectoryUserStatusPrivileges { get; set; }
		public DbSet<DirectoryRC> DirectoryRCs { get; set; }
		public DbSet<DirectoryCostItem> DirectoryCostItems { get; set; }
		public DbSet<DirectoryNote> DirectoryNotes { get; set; }
		public DbSet<DirectoryTransportCompany> DirectoryTransportCompanies { get; set; }
		public DbSet<DirectoryKeepingName> DirectoryKeepingNames { get; set; }
		public DbSet<DirectoryKeepingDescription> DirectoryKeepingDescriptions { get; set; }
		public DbSet<DirectoryPhoto> DirectoryPhotoes { get; set; }
		public DbSet<DirectoryLoanTaker> DirectoryLoanTakers { get; set; }
		public DbSet<DirectoryCarPart> DirectoryCarParts { get; set; }
		public DbSet<DirectoryPAM16Percentage> DirectoryPam16Percentages { get; set; }


		public DbSet<CurrentPost> CurrentPosts { get; set; }
		public DbSet<CurrentUserStatusPrivilege> CurrentUserStatusPrivileges { get; set; }
		public DbSet<CurrentUserStatus> CurrentUserStatuses { get; set; }
		public DbSet<CurrentRC> CurrentRCs { get; set; }
		public DbSet<CurrentNote> CurrentNotes { get; set; }
		public DbSet<CurrentCarPart> CurrentCarParts { get; set; }
		public DbSet<CurrentContainerCarPart> CurrentContainerCarParts { get; set; }


		public DbSet<InfoContainer> InfoContainers { get; set; }
		public DbSet<InfoDate> InfoDates { get; set; }
		public DbSet<InfoMonth> InfoMonthes { get; set; }
		public DbSet<InfoPanalty> InfoPanalties { get; set; }
		public DbSet<InfoOverTime> InfoOverTimes { get; set; }
		public DbSet<InfoCost> InfoCosts { get; set; }
		public DbSet<InfoLoan> InfoLoans { get; set; }
		public DbSet<InfoPrivateLoan> InfoPrivateLoans { get; set; }
		public DbSet<InfoPayment> InfoPayments { get; set; }
		public DbSet<InfoPrivatePayment> InfoPrivatePayments { get; set; }
		public DbSet<InfoSafe> InfoSafes { get; set; }
		public DbSet<InfoLastMonthDayRemain> InfoLastMonthDayRemains { get; set; }
		public DbSet<InfoCard> InfoCards { get; set; }
		public DbSet<InfoTotalEqualCashSafeToMinsk> InfoTotalEqualCashSafeToMinsks { get; set; }


		public DbSet<DefaultCost> DefaultCosts { get; set; }
		public DbSet<Parameter> Parameters { get; set; }
		public DbSet<Log> Logs { get; set; }
		public DbSet<CurrencyValue> CurrencyValues { get; set; }

		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<PalletLocation> PalletLocations { get; set; }
		public DbSet<PalletContent> PalletContents { get; set; }

		public DbSet<Auth> Auths { get; set; }


		#endregion
	}

}

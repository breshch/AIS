using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Helpers;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models
{
    public class DataContext : DbContext
    {
        private static string _connectionString;
        private static string _ip;
        private static string _databaseName;

        static DataContext()
        {
            _ip = Properties.Settings.Default.IP;
            _databaseName = Properties.Settings.Default.DatabaseName;

            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; Persist Security Info=False; User ID=huy; Password=huy;", _ip, _databaseName);
        }

        public DataContext()
            : base(_connectionString)
        {
            Debug.WriteLine(this.Database.Connection.ConnectionString);
        }

        public static void ChangeConnectionStringWithDefaultCredentials()
        {
            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; Persist Security Info=False; User ID=huy; Password=huy;", _ip, _databaseName);
        }

        public static void ChangeConnectionStringWithDefaultCredentials(string ip, string companyName)
        {
            _ip = ip;
            _databaseName = companyName.Replace("-", "_");

            Properties.Settings.Default.IP = _ip;
            Properties.Settings.Default.DatabaseName = _databaseName;
            Properties.Settings.Default.Save();

            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; Persist Security Info=False; User ID=huy; Password=huy;", _ip, _databaseName);
        }

        public static void ChangeUser(string userName, string password)
        {
            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};", _ip, _databaseName, userName, password);
        }

        public static void ChangeUserButler()
        {
            string nameButler = Properties.Settings.Default.NameButler;
            string passwordButler = Properties.Settings.Default.PasswordButler;

            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};", _ip, _databaseName, "huy", "huy");
        }

        public static void ChangeUserButler(string serverName)
        {
            string nameButler = Properties.Settings.Default.NameButler;
            string passwordButler = Properties.Settings.Default.PasswordButler;

            Properties.Settings.Default.IP = serverName;
            Properties.Settings.Default.Save();

            _ip = serverName;

            _connectionString = string.Format("Data Source={0}; User ID={1}; Password={2};", _ip, "huy", "huy");
        }

        public static void ChangeServerAndDataBase(string serverName, string dataBaseName)
        {
            Properties.Settings.Default.IP = serverName;
            Properties.Settings.Default.DatabaseName = dataBaseName;
            Properties.Settings.Default.Save();

            _ip = serverName;
            _databaseName = dataBaseName;

            string nameButler = Properties.Settings.Default.NameButler;
            string passwordButler = Properties.Settings.Default.PasswordButler;

            _connectionString = string.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};", _ip, _databaseName, "huy", "huy");
        }

        public static void ChangeServer(string serverName)
        {
            Properties.Settings.Default.IP = serverName;
            Properties.Settings.Default.Save();

            _ip = serverName;

            _connectionString = string.Format("Data Source={0}; User ID={1}; Password={2};", _ip, "huy", "huy");
        }

        public static bool TryConnection()
        {
            SqlConnection conn = null;

            if (string.IsNullOrWhiteSpace(_ip) || string.IsNullOrWhiteSpace(_databaseName))
            {
                return false;
            }
            
            try
            {
                conn = new SqlConnection(_connectionString);
                conn.Open();
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                if (conn != null) conn.Dispose();
            }

            return true;
        }

        public DbSet<DirectoryCompany> DirectoryCompanies { get; set; }
        public DbSet<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }
        public DbSet<DirectoryPost> DirectoryPosts { get; set; }
        public DbSet<DirectoryWorker> DirectoryWorkers { get; set; }
        public DbSet<DirectoryHoliday> DirectoryHolidays { get; set; }
        public DbSet<DirectoryUser> DirectoryUsers { get; set; }
        public DbSet<DirectoryUserStatus> DirectoryUserStatuses { get; set; }
        public DbSet<DirectoryUserStatusPrivilege> DirectoryUserStatusPrivileges { get; set; }
        public DbSet<DirectoryRC> DirectoryRCs { get; set; }
        public DbSet<DirectoryCostItem> DirectoryCostItems { get; set; }
        public DbSet<DirectoryNote> DirectoryNotes { get; set; }
        

        public DbSet<CurrentPost> CurrentPosts { get; set; }
        public DbSet<CurrentUserStatusPrivilege> CurrentUserStatusPrivileges { get; set; }
        public DbSet<CurrentUserStatus> CurrentUserStatuses { get; set; }
        public DbSet<CurrentRC> CurrentRCs { get; set; }
        public DbSet<CurrentNote> CurrentNotes { get; set; }


        public DbSet<InfoDate> InfoDates { get; set; }
        public DbSet<InfoMonth> InfoMonthes { get; set; }
        public DbSet<InfoPanalty> InfoPanalties { get; set; }
        public DbSet<InfoOverTime> InfoOverTimes { get; set; }
        public DbSet<InfoCost> InfoCosts { get; set; }



        public DbSet<Parameter> Parameters { get; set; }

    }
}

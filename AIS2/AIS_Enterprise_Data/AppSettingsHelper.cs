using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data
{
	public static class AppSettingsHelper
	{
		private const string ConnectionNameKey = "ConnectionName";

		private static string _connectionString;
		private static string _connectionPostfix = "_Remote";

		public static string GetConnectionName()
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			return configuration.AppSettings.Settings[ConnectionNameKey].Value + _connectionPostfix;
		}

		public static void ChangeConnectionPostfix()
		{
			_connectionPostfix = "_Local";
		}

		public static void SetConnectionName(string name)
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			configuration.AppSettings.Settings[ConnectionNameKey].Value = name;
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

		public static string GetConnectionStringIP(string connectionName)
		{
			string encryptedConnectionName = CryptoHelper.Encrypt(connectionName);

			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			string encryptedConnectionString = configuration.ConnectionStrings.ConnectionStrings[encryptedConnectionName].ConnectionString;
			string decryptedConnectionString = CryptoHelper.Decrypt(encryptedConnectionString);
			return decryptedConnectionString.Split(';')[0].Substring(7);
		}

		public static string GetConnectionString(string connectionName)
		{
			if (!string.IsNullOrEmpty(_connectionString))
				return _connectionString;

			string encryptedConnectionName = CryptoHelper.Encrypt(connectionName);

			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			string encryptedConnectionString = configuration.ConnectionStrings.ConnectionStrings[encryptedConnectionName].ConnectionString;
			_connectionString = CryptoHelper.Decrypt(encryptedConnectionString);
			return _connectionString;
		}

		public static string GetApplicationName()
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			return configuration.AppSettings.Settings[ConnectionNameKey].Value;
		}
	}
}

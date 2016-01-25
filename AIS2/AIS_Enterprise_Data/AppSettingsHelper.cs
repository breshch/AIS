using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AIS_Enterprise_Data
{
	public static class AppSettingsHelper
	{
		private const string ConnectionNameKey = "ConnectionName";
		private static Configuration _configuration;

		private static string _connectionString;
		private static string _connectionPostfix = "_Remote";

		public static void SetWebParameter(bool isWeb)
		{
			_configuration = !isWeb
				? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
				: WebConfigurationManager.OpenWebConfiguration("~");
		}

		public static string GetConnectionName()
		{
			return _configuration.AppSettings.Settings[ConnectionNameKey].Value + _connectionPostfix;
		}

		public static void ChangeConnectionPostfix()
		{
			_connectionPostfix = "_Local";
		}

		public static void SetConnectionName(string name)
		{
			_configuration.AppSettings.Settings[ConnectionNameKey].Value = name;
			_configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

		public static string GetConnectionStringIP(string connectionName)
		{
			string encryptedConnectionName = CryptoHelper.Encrypt(connectionName);

			string encryptedConnectionString = _configuration.ConnectionStrings.ConnectionStrings[encryptedConnectionName].ConnectionString;
			string decryptedConnectionString = CryptoHelper.Decrypt(encryptedConnectionString);
			return decryptedConnectionString.Split(';')[0].Substring(7);
		}

		public static string GetConnectionString(string connectionName)
		{
			if (!string.IsNullOrEmpty(_connectionString))
				return _connectionString;

			string encryptedConnectionName = CryptoHelper.Encrypt(connectionName);

			string encryptedConnectionString = _configuration.ConnectionStrings.ConnectionStrings[encryptedConnectionName].ConnectionString;
			_connectionString = CryptoHelper.Decrypt(encryptedConnectionString);
			return _connectionString;
		}

		public static string GetApplicationName()
		{
			return _configuration.AppSettings.Settings[ConnectionNameKey].Value;
		}
	}
}

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
		private const string ApplicationNameKey = "ApplicationName";

		public static string GetConnectionName()
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			return configuration.AppSettings.Settings[ConnectionNameKey].Value;
		}

		public static void SetConnectionName(string name)
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			configuration.AppSettings.Settings[ConnectionNameKey].Value = name;
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}

		public static string GetConnectionStringIP()
		{
			string connectionName = GetConnectionName();
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			return configuration.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.Split(';')[0].Substring(7);
		}

		public static string GetApplicationName()
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			return configuration.AppSettings.Settings[ApplicationNameKey].Value;
		}
	}
}

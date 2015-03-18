using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace AIS_Enterprise_Installer
{
	public class Installer
	{
		public bool SQLServerCheck()
		{
			DataTable dataTable = SmoApplication.EnumAvailableSqlServers(true);
			return dataTable.Rows.Count > 0;
		}

		public void InstallSQLServerSilent()
		{
			var path = Environment.CurrentDirectory;
			var process = new Process();

			process.StartInfo.FileName = Path.Combine(path, "SQLEXPR_x86_ENU\\SETUP.exe");
			process.StartInfo.Arguments = string.Format(@"/ENU=True /IACCEPTSQLSERVERLICENSETERMS=True /ACTION=Install /QUIETSIMPLE=True /SECURITYMODE=SQL /UPDATEENABLED=False /FEATURES=SQLENGINE /HELP=False /INDICATEPROGRESS=False /X86=False /INSTALLSHAREDDIR=""{0}\Microsoft SQL Server""  /INSTANCENAME=AIS /INSTANCEID=AIS /SQMREPORTING=False /ERRORREPORTING=False /INSTANCEDIR=""{0}\Microsoft SQL Server"" /AGTSVCACCOUNT=""NT AUTHORITY\SYSTEM"" /SQLCOLLATION=SQL_Latin1_General_CP1_CI_AS /SQLSVCACCOUNT=""NT AUTHORITY\SYSTEM"" /SQLSYSADMINACCOUNTS={1} /SAPWD={2} /ADDCURRENTUSERASSQLADMIN=True /TCPENABLED=1", 
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), Environment.MachineName + "\\" + Environment.UserName, "Hrt12FG144");
			process.Start();
		}

		public void InstallApplication()
		{
			var path = Environment.CurrentDirectory;
			CopyDirectory(Path.Combine(path,"Application"), 
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV", "Application"));

			CopyDirectory(Path.Combine(path, "Updater"),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV", "Updater"));
		}

		private void CopyDirectory(string strSource, string strDestination)
		{
			if (!Directory.Exists(strDestination))
			{
				Directory.CreateDirectory(strDestination);
			}

			DirectoryInfo dirInfo = new DirectoryInfo(strSource);
			FileInfo[] files = dirInfo.GetFiles();
			foreach (FileInfo tempfile in files)
			{
				tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name));
			}

			DirectoryInfo[] directories = dirInfo.GetDirectories();
			foreach (DirectoryInfo tempdir in directories)
			{
				CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name));
			}

		}
	}
}

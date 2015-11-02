using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using File = System.IO.File;

//using Microsoft.SqlServer.Management.Smo;

namespace AIS_Enterprise_Installer
{
	public class Installer
	{
		private readonly string _pathApplication;
		private readonly string _pathUpdater;
		private readonly string _shortcutName;

		public Installer()
		{
			_pathApplication = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
				"AIS_Enterprise_AV", "Application");

			_pathUpdater = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
				"AIS_Enterprise_AV", "Updater");

			_shortcutName = "AIS_Enterprise";
		}

		public void InstallApplication()
		{
			var path = Environment.CurrentDirectory;
			if (!File.Exists(Path.Combine(path, "AIS_Enterprise_Installer.exe")))
			{
				var directories = Directory.GetDirectories(path);
				foreach (var directory in directories)
				{
					if (File.Exists(Path.Combine(directory, "AIS_Enterprise_Installer.exe")))
					{
						path = directory;
						break;
					}
				}
			}

			CopyDirectory(Path.Combine(path, "Application"), _pathApplication);
			CopyDirectory(Path.Combine(path, "Updater"), _pathUpdater);

			CreateShortcut(_shortcutName);

			if (MessageBox.Show(@"Install complete") == DialogResult.OK)
			{
				Environment.Exit(0);
			}
		}

		private void AppShortcutToDesktop(string linkName)
		{
			string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			string desktopShortcut = Path.Combine(desktopDirectory, linkName + ".url");
			if (File.Exists(desktopShortcut))
			{
				File.Delete(desktopShortcut);
			}

			using (StreamWriter writer = new StreamWriter(desktopDirectory + "\\" + linkName + ".url"))
			{
				string app = Path.Combine(_pathApplication, "AIS_Enterprise.exe");
				writer.WriteLine("[InternetShortcut]");
				writer.WriteLine("URL=file:///" + app);
				writer.WriteLine("IconIndex=0");
				string icon = app.Replace('\\', '/');
				writer.WriteLine("IconFile=" + icon);
				writer.Flush();
			}
		}

		private void CreateShortcut(string linkName)
		{
			string app = Path.Combine(_pathApplication, "AIS_Enterprise.exe");

			object shDesktop = (object)"Desktop";
			WshShell shell = new WshShell();
			string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + linkName + ".lnk";
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
			shortcut.TargetPath = app;
			shortcut.WorkingDirectory = _pathApplication;
			shortcut.Save();
		}

		private void CopyDirectory(string strSource, string strDestination)
		{
			if (Directory.Exists(strDestination))
			{
				Directory.Delete(strDestination, true);
			}
			else
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

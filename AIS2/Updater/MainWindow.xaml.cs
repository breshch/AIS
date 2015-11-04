using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using FTP;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Updater
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private FTPConnector _ftpConnector;
		private DateTime _dateBackup;
		private const string DefaultFTPFolder = "ftp://172.16.0.1/";
		private readonly string PathApplication;
		private bool isUpdating = false;

		public MainWindow()
		{
			InitializeComponent();

			this.Visibility = Visibility.Hidden;
			this.ShowInTaskbar = false;

			PathApplication = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV");

			_ftpConnector = new FTPConnector("FTPUSER", "Mp~7200~aA", DefaultFTPFolder);
			_ftpConnector.OnGetFileInfo += _ftpConnector_OnGetFileInfo;
			_ftpConnector.OnFileSizeLoaded += _ftpConnector_OnFileSizeLoaded;

			Observable.Start(Updating);
			Observable.Interval(new TimeSpan(0, 30, 0))
				.Subscribe((x) => Updating());

			Observable.Interval(new TimeSpan(0, 0, 10))
				.Subscribe((x) => ProcessChecking());
		}

		private void ProcessChecking()
		{
			if (isUpdating)
			{
				return;
			}

			var processes = Process.GetProcessesByName("AIS_Enterprise");
			if (!processes.Any())
			{
				Environment.Exit(0);
			}
		}

		private void _ftpConnector_OnFileSizeLoaded(long loadedFileSize, long fileSize)
		{
			SyncContext(() => TextBlockLoaded.Text = loadedFileSize.ToString("N0"));

			double percentage = (double)loadedFileSize / fileSize * 100;
			SyncContext(() => ProgressBarPercentage.Value = percentage);
		}

		private void _ftpConnector_OnGetFileInfo(string fileName, long fileSize)
		{
			SyncContext(() => TextBlockFileName.Text = fileName);
			SyncContext(() => TextBlockFileSize.Text = fileSize.ToString("N0"));
		}

		private void Updating()
		{
			if (isUpdating)
			{
				return;
			}

			if (IsNewVersion())
			{
				isUpdating = true;

				foreach (var process in Process.GetProcessesByName("AIS_Enterprise"))
				{
					process.Kill();
				}

				SyncContext(() => TextBlockFileName.Text = null);
				SyncContext(() => TextBlockFileSize.Text = null);
				SyncContext(() => TextBlockLoaded.Text = null);
				SyncContext(() => ProgressBarPercentage.Value = 0);
				SyncContext(() => this.Visibility = Visibility.Visible);
				SyncContext(() => this.ShowInTaskbar = true);

				Backup();
				try
				{
					_ftpConnector.DownloadDirectory(@"AIS_Enterprise_AV/Application", Path.Combine(PathApplication, "Application"));
				}
				catch
				{
					Directory.Delete(Path.Combine(PathApplication, "Application"), true);
					Restore();
				}

				File.Delete(Path.Combine(PathApplication, "Backup_" + _dateBackup.Ticks + ".zip"));

				Process.Start(Path.Combine(PathApplication, "Application/AIS_Enterprise.exe"));

				SyncContext(() => this.ShowInTaskbar = false);
				SyncContext(() => this.Visibility = Visibility.Hidden);

				isUpdating = false;
			}
		}

		private bool IsNewVersion()
		{
			var pathCurrentVersion = Path.Combine(PathApplication, "Application/Version.version");
			if (File.Exists(pathCurrentVersion))
			{
				var currentVersion = Version.Parse(File.ReadAllText(Path.Combine(PathApplication, "Application/Version.version")));
				var ftpVersion = Version.Parse(_ftpConnector.GetFile("AIS_Enterprise_AV/Application/Version.version"));

				return ftpVersion > currentVersion;
			}
			return true;
		}

		private void Backup()
		{
			_dateBackup = DateTime.Now;

			if (!Directory.Exists(Path.Combine(PathApplication, "Application")))
			{
				return;
			}

			ZipFile.CreateFromDirectory(Path.Combine(PathApplication, "Application"),
				Path.Combine(PathApplication, "Backup_" + _dateBackup.Ticks + ".zip"));
		}

		private void Restore()
		{
			string pathFile = Path.Combine(PathApplication, "Backup_" + _dateBackup.Ticks + ".zip");

			if (!File.Exists(pathFile))
			{
				return;
			}

			ZipFile.ExtractToDirectory(pathFile, Path.Combine(PathApplication, "Application"));
		}

		private void SyncContext(Action action)
		{
			Application.Current.Dispatcher.BeginInvoke(new Action(action));
		}
	}
}

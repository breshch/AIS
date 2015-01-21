using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using FTP;
using Application = System.Windows.Application;

namespace Updater
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FTPConnector _ftpConnector;
        private DateTime _dateBackup;
        private const string DefaultFTPFolder = @"ftp://95.31.130.52/";
        private readonly string PathApplication;

        public MainWindow()
        {
            InitializeComponent();

            this.Visibility = Visibility.Hidden;
            this.ShowInTaskbar = false;

            PathApplication = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AIS_Enterprise_AV");

            _ftpConnector = new FTPConnector("breshch", "Mp7200aA", DefaultFTPFolder);
            _ftpConnector.OnGetFileInfo += _ftpConnector_OnGetFileInfo;
            _ftpConnector.OnFileSizeLoaded += _ftpConnector_OnFileSizeLoaded;

            Observable.Start(Updating);
            Observable.Interval(new TimeSpan(0, 30, 0))
                .Subscribe((x) => Updating());
        }

        private void _ftpConnector_OnFileSizeLoaded(long loadedFileSize, long fileSize)
        {
            SyncContext(() => TextBlockLoaded.Text = loadedFileSize.ToString("N0"));

            double percentage = (double) loadedFileSize / fileSize * 100;
            SyncContext(() => ProgressBarPercentage.Value = percentage);

        }

        private void _ftpConnector_OnGetFileInfo(string fileName, long fileSize)
        {
            SyncContext(() => TextBlockFileName.Text = fileName);
            SyncContext(() => TextBlockFileSize.Text = fileSize.ToString("N0"));
        }

        private void Updating()
        {
            if (IsNewVersion())
            {
                foreach (var process in Process.GetProcessesByName("AIS_Enterprise_AV"))
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

                Process.Start(Path.Combine(PathApplication, "Application/AIS_Enterprise_AV.exe"));

                SyncContext(() => this.ShowInTaskbar = false);
                SyncContext(() => this.Visibility = Visibility.Hidden);
            }
        }

        private bool IsNewVersion()
        {
            var currentVersion = Version.Parse(File.ReadAllText(Path.Combine(PathApplication, "Application/Version.txt")));
            var ftpVersion = Version.Parse(_ftpConnector.GetFile("AIS_Enterprise_AV/Application/Version.txt"));

            return ftpVersion > currentVersion;
        }

        private void Backup()
        {
            _dateBackup = DateTime.Now;

            ZipFile.CreateFromDirectory(Path.Combine(PathApplication, "Application"),
                Path.Combine(PathApplication, "Backup_" + _dateBackup.Ticks + ".zip"));
        }

        private void Restore()
        {
            ZipFile.ExtractToDirectory(Path.Combine(PathApplication, "Backup_" + _dateBackup.Ticks + ".zip"),
                Path.Combine(PathApplication, "Application"));
        }

        private void SyncContext(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(action));
        }
    }
}

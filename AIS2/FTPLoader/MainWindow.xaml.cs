using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FTP;

namespace FTPLoader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FTPConnector _ftpConnector;
		private const string DefaultFTPFolder = "ftp://89.20.42.182/";
		private const string PathApplication = @"f:\Dev\AIS\AIS2\AIS_Enterprise_AV\bin\Release";

        public MainWindow()
        {
            InitializeComponent();

			IncrementVersion();

	        Task.Factory.StartNew(FTPLoading);
        }

	    private void IncrementVersion()
	    {
		    string pathVersion = Path.Combine(PathApplication, "Version.version");
		    if(File.Exists(pathVersion))
		    {
			    Version version;
			    using (var sr = new StreamReader(pathVersion))
			    {
				    version = Version.Parse(sr.ReadLine());
			    }
			    var newVersion = new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
			    using (var sw = new StreamWriter(pathVersion, false))
			    {
					sw.WriteLine(newVersion); 
			    }
		    }
	    }

	    private void FTPLoading()
	    {
			_ftpConnector = new FTPConnector("FTPUSER", "Mp~7200~aA", DefaultFTPFolder);
			_ftpConnector.OnGetUploadFileInfo += _ftpConnector_OnGetUploadFileInfo;
			_ftpConnector.OnFileSizeUploaded += _ftpConnector_OnFileSizeUploaded;

		    string[] filterExtensions = { ".pdb", ".xml", ".manifest", ".application", ".txt" };
			_ftpConnector.AddFiltersExtensions(filterExtensions);

			string[] filterFolders = { "app.publish", "de", "es", "fr", "hu", "it", "pt-BR", "ro", "ru", "sv", "zh-Hans"};
		    filterFolders = filterFolders.Select(folder => Path.Combine(@"AIS_Enterprise_AV\Application", folder)).ToArray();
			_ftpConnector.AddFiltersFolders(filterFolders);

			SyncContext(() => TextBlockFileName.Text = null);
			SyncContext(() => TextBlockFileSize.Text = null);
			SyncContext(() => TextBlockLoaded.Text = null);
			SyncContext(() => ProgressBarPercentage.Value = 0);

			_ftpConnector.LoadDirectory(PathApplication, @"AIS_Enterprise_AV\Application");

		    SyncContext(() =>
		    {
				GridUpload.Visibility = Visibility.Collapsed;
				StackPannelUploaded.Visibility = Visibility.Visible;
		    });
	    }

		private void _ftpConnector_OnFileSizeUploaded(long loadedFileSize, long fileSize)
		{
			SyncContext(() => TextBlockLoaded.Text = loadedFileSize.ToString("N0"));

			double percentage = (double)loadedFileSize / fileSize * 100;
			SyncContext(() => ProgressBarPercentage.Value = percentage);
		}

		private void _ftpConnector_OnGetUploadFileInfo(string fileName, long fileSize)
		{
			SyncContext(() => TextBlockFileName.Text = fileName);
			SyncContext(() => TextBlockFileSize.Text = fileSize.ToString("N0"));
		}

		private void SyncContext(Action action)
		{
			Application.Current.Dispatcher.BeginInvoke(new Action(action));
		}

	    private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
	    {
		    this.Close();
	    }
    }
}

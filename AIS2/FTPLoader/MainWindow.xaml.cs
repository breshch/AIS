using System;
using System.IO;
using System.Linq;
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
		private const string DefaultFTPFolder = "ftp://172.16.0.1/";
		private const string PathApplication = @"D:\Dev\AIS\AIS2\AIS_Enterprise_AV\bin\Release";
		private const string PathUpdater = @"D:\Dev\AIS\AIS2\Updater\bin\Release";

        public MainWindow()
        {
            InitializeComponent();

			IncrementVersion();
			FTPLoading();
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

		    string[] filterExtensions = { ".pdb", ".xml", ".manifest", ".application", ".txt" };
			_ftpConnector.AddFiltersExtensions(filterExtensions);

			string[] filterFolders = { "app.publish", "de", "es", "fr", "hu", "it", "pt-BR", "ro", "ru", "sv", "zh-Hans"};
		    filterFolders = filterFolders.Select(folder => Path.Combine(@"AIS_Enterprise_AV\Application", folder)).ToArray();
			_ftpConnector.AddFiltersFolders(filterFolders);
			
			_ftpConnector.LoadDirectory(PathApplication, @"AIS_Enterprise_AV\Application");
	    }
    }
}

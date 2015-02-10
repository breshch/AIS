using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FTP;

namespace FTPLoader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FTPConnector _ftpConnector;
	    private const string DefaultFTPFolder = "ftp://95.31.130.52/";
	    private const string PathApplication = @"E:\Dev\AIS\AIS2\AIS_Enterprise_AV\bin\Release";
		private const string PathUpdater = @"E:\Dev\AIS\AIS2\Updater\bin\Release";

        public MainWindow()
        {
            InitializeComponent();

			IncrementVersion();
			FTPLoading();
        }

	    private void IncrementVersion()
	    {
		    string pathVersion = Path.Combine(PathApplication, "Version.txt");
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
			_ftpConnector = new FTPConnector("breshch", "Mp7200aA", DefaultFTPFolder);
			_ftpConnector.LoadDirectory(PathApplication, @"AIS_Enterprise_AV\Application");
			_ftpConnector.LoadDirectory(PathUpdater, @"AIS_Enterprise_AV\Updater");
	    }
    }
}

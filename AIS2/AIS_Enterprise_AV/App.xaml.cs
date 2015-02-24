using System.Deployment.Application;
using System.IO;
using System.Net;
using System.Net.FtpClient;
using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AIS_Enterprise_AV.WareHouse;

namespace AIS_Enterprise_AV
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

	       // new Scheme().ShowDialog();

			//string pathUpdater = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName,
			//	"Updater/AIS_Enterprise_Updater.exe");

			//if (File.Exists(pathUpdater))
			//{
			//	Process.Start(pathUpdater);
			//}

			if (DataContext.TryConnection())
			{
				HelperMethods.ShowView(new MainViewModel(), new MainView());
			}
			else
			{
				HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());
			}
        }
    }
}

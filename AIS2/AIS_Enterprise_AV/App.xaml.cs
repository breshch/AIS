using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using AIS_Enterprise_AV.Helpers.LoggerLayoutRenderers;
using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.WareHouse;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Migrations;
using NLog.Config;

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

			LoggerConfiguration.Configurate();

			string pathUpdater = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName,
				"Updater/AIS_Enterprise_Updater.exe");

			if (File.Exists(pathUpdater))
			{
				if (!Process.GetProcessesByName("AIS_Enterprise_Updater").Any())
				{
					Process.Start(pathUpdater);	
				}
			}

			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());

			HelperMethods.ShowView(new MainViewModel(), new MainView());
		}
	}
}

using System;
using System.Linq;
using System.Windows;
using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.ViewModels.Helpers;
using AIS_Enterprise_Global.Views.Directories;
using AIS_Enterprise_Global.Views.Helpers;

namespace AIS_Enterprise_AV.Views
{
	/// <summary>
	/// Логика взаимодействия для MainProjectChoiseView.xaml
	/// </summary>
	public partial class ProjectAdminView : Window
	{
		public ProjectAdminView()
		{
			InitializeComponent();
		}

		private void UserStatuses_OnClick(object sender, RoutedEventArgs e)
		{
			HelperMethods.ShowView(new DirectoryUserStatusesViewModel(), new DirectoryUserStatusesView());
		}

		private void Users_OnClick(object sender, RoutedEventArgs e)
		{
			HelperMethods.ShowView(new DirectoryUsersViewModel(), new DirectoryUsersView());
		}

		private void Logs_OnClick(object sender, RoutedEventArgs e)
		{
			HelperMethods.ShowView(new LogViewModel(), new LogView());
		}

		//private void Calendar_OnClick(object sender, RoutedEventArgs e)
		//{
		//	GetCalendar();
		//}

		//private async void GetCalendar()
		//{
		//	await ParsingCalendar.GetCalendar(BC, DateTime.Now.AddYears(1).Year);
		//}

		private void MinskCash_OnClick(object sender, RoutedEventArgs e)
		{
			HelperMethods.ShowView(new InfoAddMinskCashViewModel(), new InfoAddMinskCashView());
		}
	}
}

using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using AIS_Enterprise_AV.Auth;
using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.Properties;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_AV.WareHouse;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Migrations;
using Application = System.Windows.Forms.Application;
using Auth = AIS_Enterprise_Data.Directories.Auth;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AIS_Enterprise_AV.ViewModels
{
	public class MainViewModel : ViewModelGlobal
	{
		#region Base

		public MainViewModel()
			: base()
		{
			IsAdminButtonsVisibility = false;
			LogingVisibility = Visibility.Visible;
			ChoiseProjectsVisibility = Visibility.Collapsed;
			CreateDBCommand = new RelayCommand(CreateDB);
			KillTheDBCommand = new RelayCommand(KillTheDB);
			ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
			ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
			ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);
			CostsExcelToDBCommand = new RelayCommand(CostsExcelToDB);
			RussiansCommand = new RelayCommand(Russians);
			ImportsCommand = new RelayCommand(Imports);
			CarPartRemainsToDbCommand = new RelayCommand(CarPartRemainsToDb);

			EnteringCommand = new RelayCommand(Entering);

			IsNotInitializedDB = true;

			RefreshUsers();
		}

		private void InitializeChoiseProjects()
		{
			MonthTimeSheetCommand = new RelayCommand(MonthTimeSheet);
			RemainsCommand = new RelayCommand(Remains);
			ProcessingBookKeepingCommand = new RelayCommand(ProcessingBookKeeping);
			RemainsLoanCommand = new RelayCommand(RemainsLoan);
			WarehouseCommand = new RelayCommand(Warehouse);
			CostsCommand = new RelayCommand(Costs);
			ReportsCommand = new RelayCommand(Reports);
			AdminCommand = new RelayCommand(Admin);
			ChangeUserCommand = new RelayCommand(ChangeUser);

			MonthTimeSheetVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_MonthTimeSheetVisibility) ? Visibility.Visible : Visibility.Collapsed;
			DbFenoxVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_DbFenoxVisibility) ? Visibility.Visible : Visibility.Collapsed;
			CostsVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_CostsVisibility) ? Visibility.Visible : Visibility.Collapsed;
			ProcessingBookKeepingVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_ProcessingBookKeepingVisibility) ? Visibility.Visible : Visibility.Collapsed;
			RemainsLoanVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_RemainsLoanVisibility) ? Visibility.Visible : Visibility.Collapsed;
			ReportsVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_ReportsVisibility) ? Visibility.Visible : Visibility.Collapsed;
			AdminVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_AdminVisibility) ? Visibility.Visible : Visibility.Collapsed;

			BC.SetRemainsToFirstDateInMonth();
		}

		


		private void RefreshUsers()
		{
			Users = new ObservableCollection<DirectoryUser>(BC.GetDirectoryUsers());
		}

		#endregion


		#region Properties

		private bool _isNotInitializeDB;
		public bool IsNotInitializedDB
		{
			get
			{
				return _isNotInitializeDB;
			}
			set
			{
				_isNotInitializeDB = value;
				RaisePropertyChanged();
			}
		}

		private bool _isAdminButtonsVisibility;
		public bool IsAdminButtonsVisibility
		{
			get
			{
				return _isAdminButtonsVisibility;
			}
			set
			{
				_isAdminButtonsVisibility = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<DirectoryUser> _users;
		public ObservableCollection<DirectoryUser> Users
		{
			get
			{
				return _users;
			}
			set
			{
				_users = value;
				RaisePropertyChanged();
			}
		}
		public DirectoryUser SelectedUser { get; set; }
		public Visibility LogingVisibility { get; set; }
		public Visibility ChoiseProjectsVisibility { get; set; }

		public Visibility MonthTimeSheetVisibility { get; set; }
		public Visibility DbFenoxVisibility { get; set; }
		public Visibility CostsVisibility { get; set; }
		public Visibility ProcessingBookKeepingVisibility { get; set; }
		public Visibility RemainsLoanVisibility { get; set; }
		public Visibility ReportsVisibility { get; set; }
		public Visibility AdminVisibility { get; set; }

		#endregion


		#region Commands

		public RelayCommand CreateDBCommand { get; set; }
		public RelayCommand KillTheDBCommand { get; set; }
		public RelayCommand ShowExcelToDBCommand { get; set; }
		public RelayCommand ShowDefaultDBCommand { get; set; }
		public RelayCommand ShowDefaultOfficeDBCommand { get; set; }
		public RelayCommand EnteringCommand { get; set; }
		public RelayCommand CostsExcelToDBCommand { get; set; }
		public RelayCommand RussiansCommand { get; set; }
		public RelayCommand ImportsCommand { get; set; }
		public RelayCommand CarPartRemainsToDbCommand { get; set; }

		public RelayCommand MonthTimeSheetCommand { get; set; }
		public RelayCommand RemainsCommand { get; set; }
		public RelayCommand CostsCommand { get; set; }
		public RelayCommand ProcessingBookKeepingCommand { get; set; }
		public RelayCommand RemainsLoanCommand { get; set; }
		public RelayCommand WarehouseCommand { get; set; }
		public RelayCommand ReportsCommand { get; set; }
		public RelayCommand AdminCommand { get; set; }
		public RelayCommand ChangeUserCommand { get; set; }

		private void MonthTimeSheet(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Hidden;

			var monthTimeSheetView = new MonthTimeSheetView();
			monthTimeSheetView.ShowDialog();

			window.Visibility = Visibility.Visible;
		}

		private void Remains(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			HelperMethods.ShowView(new InfoRemainsViewModel(), new InfoRemainsView(), window);

			window.Visibility = Visibility.Visible;
		}

		private void ProcessingBookKeeping(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			HelperMethods.ShowView(new PercentageProcessingBookKeepingViewModel(), new PercentageProcessingBookKeepingView());

			window.Visibility = Visibility.Visible;
		}

		private void RemainsLoan(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			HelperMethods.ShowView(new PickDateReportViewModel(), new PickDateReportView());

			window.Visibility = Visibility.Visible;
		}

		private void Warehouse(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var scheme = new Scheme();
			scheme.ShowDialog();

			window.Visibility = Visibility.Visible;
		}
		private void Costs(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var costsView = new ProjectCostsView();
			costsView.ShowDialog();

			window.Visibility = Visibility.Visible;
		}

		private void Reports(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Hidden;

			var reports = new ProjectReportsView();
			reports.Owner = window;
			reports.ShowDialog();

			window.Visibility = Visibility.Visible;
		}

		private void Admin(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var admin = new ProjectAdminView();
			admin.ShowDialog();

			window.Visibility = Visibility.Visible;
		}
		private void ChangeUser(object parameter)
		{
			LogingVisibility = Visibility.Visible;
			ChoiseProjectsVisibility = Visibility.Collapsed;
		}



		private void CreateDB(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());

			window.Close();
		}

		private void KillTheDB(object parameter)
		{
			IsNotInitializedDB = false;
			BC.RemoveDB();
			IsNotInitializedDB = true;

			HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());

			BC.RefreshContext();
			RefreshUsers();
		}

		private void ShowExcelToDB(object parameter)
		{
			IsNotInitializedDB = false;
			Task.Factory.StartNew(() => ConvertingWorkersExcelToDB.ConvertExcelToDB(BC)).ContinueWith(
				(t) =>
				{
					IsNotInitializedDB = true;
				});
		}

		private void ShowDefaultDB(object parameter)
		{
			IsNotInitializedDB = false;
			Task.Factory.StartNew(BC.InitializeDefaultDataBaseWithWorkers).ContinueWith((t) => IsNotInitializedDB = true);
		}

		private void ShowDefaultOfficeDB(object parameter)
		{
			IsNotInitializedDB = false;
			Task.Factory.StartNew(BC.InitializeDefaultDataBaseWithOfficeWorkers).ContinueWith((t) => IsNotInitializedDB = true);
		}

		private void Entering(object parameter)
		{
			var window = parameter as Window;
			var passwordBox = window.FindName("PasswordBoxPass") as PasswordBox;

			string password = passwordBox.Password;

			if (!BC.LoginUser(SelectedUser.Id, password))
			{
				MessageBox.Show(@"Error");
				return;
			}

			Privileges.LoadUserPrivileges(SelectedUser.Id);
			InitializeChoiseProjects();
			
			LogingVisibility = Visibility.Collapsed;
			ChoiseProjectsVisibility = Visibility.Visible;

			//HelperMethods.ShowView(new MainProjectChoiseViewModel(), new MainProjectChoiseView(), window);
			passwordBox.Password = null;

			//window.Visibility = Visibility.Visible;
			//IsAdminButtonsVisibility = Privileges.HasAccess(UserPrivileges.ButtonsVisibility_AdminButtons);
		}

		private Timer _timer;

		private void CheckNewVersion()
		{
			FirstChecking();

			if (_timer != null)
			{
				_timer.Dispose();
			}

			_timer = new Timer
			{
				Interval = 1000 * 60 * 10
			};

			_timer.Tick += _timer_Tick;
			_timer.Start();
		}

		void _timer_Tick(object sender, EventArgs e)
		{
			FirstChecking();
		}

		private void FirstChecking()
		{
			var version = Version.Parse(Settings.Default.ApplicationVersion);
			if (HelperMethods.IsNewVersion(BC, ref version))
			{
				Settings.Default.ApplicationVersion = version.ToString();
				Settings.Default.Save();

				System.Windows.MessageBox.Show("Вышла новая версия. Программа будет обновлена и перезагружена.", "Новая версия", MessageBoxButton.OK);
				Process.Start(Application.ExecutablePath);
				System.Windows.Application.Current.Shutdown();
			}
		}

		private void CostsExcelToDB(object parameter)
		{
			IsNotInitializedDB = false;
			Task.Factory.StartNew(() => ConvertingCostsExcelToDB.ConvertExcelToDB(BC)).ContinueWith((t) => IsNotInitializedDB = true);
		}

		private void Russians(object parameter)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string path = dialog.FileName;
				IsNotInitializedDB = false;
				Task.Factory.StartNew(() => ConvertingCarPartsExcelToDB.ConvertRussian(BC, path)).ContinueWith((t) => IsNotInitializedDB = true);

			}
		}

		private void Imports(object parameter)
		{

			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string path = dialog.FileName;
				IsNotInitializedDB = false;
				Task.Factory.StartNew(() => ConvertingCarPartsExcelToDB.ConvertImport(BC, path)).ContinueWith((t) => IsNotInitializedDB = true);
			}
		}

		private void CarPartRemainsToDb(object parameter)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string path = dialog.FileName;
				IsNotInitializedDB = false;
				Task.Factory.StartNew(() => ConvertingCarPartsExcelToDB.ConvertingCarPartRemainsToDb(BC, path)).ContinueWith((t) => IsNotInitializedDB = true);
			}
		}
		#endregion
	}
}

﻿using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.Properties;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
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

			//var passwords = new[]
			//{
			//	new {Id = 5, Password = "196309"},
			//	new {Id = 6, Password = "196309"},
			//	new {Id = 8, Password = "196309"},
			//	new {Id = 10, Password = "196309"},
			//	new {Id = 11, Password = "196309"},
			//	new {Id = 12, Password = "2802"},
			//	new {Id = 13, Password = "196309aA"},
			//};

			//foreach (var password in passwords)
			//{
			//	var salt = Path.GetRandomFileName() + Path.GetRandomFileName();

			//	var hash = CryptoHelper.GetHash(password.Password + salt);
			//	BC.DataContext.Auths.Add(new Auth
			//	{
			//		DirectoryUserId = password.Id,
			//		Hash = hash,
			//		Salt = salt
			//	});
			//}

			//BC.DataContext.SaveChanges();


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

			DirectoryUser.ChangeUserId(BC, SelectedUser.Id, SelectedUser.UserName);

			window.Visibility = Visibility.Hidden;

			HelperMethods.ShowView(new MainProjectChoiseViewModel(), new MainProjectChoiseView());
			passwordBox.Password = null;

			window.Visibility = Visibility.Visible;
			IsAdminButtonsVisibility = HelperMethods.IsPrivilege(BC, UserPrivileges.ButtonsVisibility_AdminButtons);
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

﻿using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
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

            Servers = new ObservableCollection<string>(HelperMethods.GetServers());
            ////string defaultServer = Properties.Settings.Default.DefaultServer;
            string defaultServer = "95.31.130.52";

            //if (Servers.Contains(defaultServer))
            {
                SelectedServer = defaultServer;

                if (DataBases != null)
                {
                    DataBases.Clear();
                }
                else
                {
                    DataBases = new ObservableCollection<string>();
                }

                foreach (var dataBase in DBCustomQueries.GetDataBases(SelectedServer))
                {
                    DataBases.Add(dataBase);
                }

                string defaultDataBase = Settings.Default.DefaultDataBase;

                if (DataBases.Contains(defaultDataBase))
                {
                    SelectedDataBase = defaultDataBase;
                }
            }

            CreateDBCommand = new RelayCommand(CreateDB);
            KillTheDBCommand = new RelayCommand(KillTheDB);
            ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
            ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
            ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);
            RefreshDataBasesCommand = new RelayCommand(RefreshDataBases);
            CostsExcelToDBCommand = new RelayCommand(CostsExcelToDB);
            RussiansCommand = new RelayCommand(Russians);
            ImportsCommand = new RelayCommand(Imports);
            CarPartRemainsToDbCommand = new RelayCommand(CarPartRemainsToDb);

            EnteringCommand = new RelayCommand(Entering);

            IsNotInitializedDB = true;

            Languages = new ObservableCollection<string>(new[] { "ru-RU", "en-US" });
        }

        private void RefreshUsers()
        {
            if (Users != null)
            {
                Users.Clear();
            }
            else
            {
                Users = new ObservableCollection<DirectoryUser>();
            }

            //foreach (var item in BC.DataContext.InfoLoans)
            //{
            //    if (item.CountPayments == null)
            //    {
            //        item.CountPayments = 1;
            //    }
            //}

            //foreach (var item in BC.DataContext.InfoPrivateLoans)
            //{
            //    if (item.CountPayments == null)
            //    {
            //        item.CountPayments = 1;
            //    }
            //}

            //BC.DataContext.SaveChanges();

            foreach (var user in BC.GetDirectoryUsers())
            {
                Users.Add(user);
            }

            string defaultUser = Settings.Default.DefaultUser;

            if (Users.Select(u => u.TranscriptionName).Contains(defaultUser))
            {
                SelectedUser = Users.First(u => u.TranscriptionName == defaultUser);
            }
        }

        #endregion


        #region Properties

        public ObservableCollection<string> Languages { get; set; }

        private ObservableCollection<string> _servers;
        public ObservableCollection<string> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
                RaisePropertyChanged();
            }
        }

        //private string _selectedServer;

        public string SelectedServer { get; set; }

        //public string SelectedServer
        //{
        //    get
        //    {
        //        return _selectedServer;
        //    }
        //    set
        //    {
        //        _selectedServer = value;
        //        RaisePropertyChanged();


        //    }
        //}

        private ObservableCollection<string> _dataBases;
        public ObservableCollection<string> DataBases
        {
            get
            {
                return _dataBases;
            }
            set
            {
                _dataBases = value;
                RaisePropertyChanged();
            }
        }

        public string _selectedDataBase;

        public string SelectedDataBase
        {
            get
            {
                return _selectedDataBase;
            }
            set
            {
                _selectedDataBase = value;
                RaisePropertyChanged();

                if (_selectedDataBase != null)
                {
                    DataContext.ChangeServerAndDataBase(SelectedServer, _selectedDataBase);

                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
                    BC.RefreshContext();

                    RefreshUsers();
                }
            }
        }

        private string _selectedLanguage;
        [NoMagic]
        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged();

                Settings.Default.Language = _selectedLanguage;
                Settings.Default.Save();
            }
        }

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
        public RelayCommand RefreshDataBasesCommand { get; set; }
        public RelayCommand CostsExcelToDBCommand { get; set; }
        public RelayCommand RussiansCommand { get; set; }
        public RelayCommand ImportsCommand { get; set; }
        public RelayCommand CarPartRemainsToDbCommand { get; set; }

        private void RefreshDataBases(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            //passwordBox.Password = null;

            //DataContext.ChangeServer(SelectedServer);
            //BC.RefreshContext();

            HelperMethods.AddServer(SelectedServer);

            if (!Servers.Any(s => s == SelectedServer))
            {
                Servers.Add(SelectedServer);
            }

            //string selectedServer = SelectedServer;
            //Servers.Clear();

            //foreach (var server in HelperMethods.GetServers())
            //{
            //    Servers.Add(server);
            //}

            // SelectedServer = selectedServer;

            if (DataBases != null)
            {
                DataBases.Clear();
            }
            else
            {
                DataBases = new ObservableCollection<string>();
            }

            foreach (var dataBase in DBCustomQueries.GetDataBases(SelectedServer))
            {
                DataBases.Add(dataBase);

            }

            string defaultDataBase = Settings.Default.DefaultDataBase;

            if (DataBases.Contains(defaultDataBase))
            {
                SelectedDataBase = defaultDataBase;
            }
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

            if (Settings.Default.DefaultServer != SelectedServer ||
                Settings.Default.DefaultDataBase != SelectedDataBase ||
                DirectoryUser.CurrentUserId != SelectedUser.Id)
            {
                DataContext.ChangeUser(SelectedUser.TranscriptionName, password);

                if (!DataContext.TryConnection())
                {
                    MessageBox.Show("Errororoororr");
                    return;
                }

                BC.RefreshContext();

                //CheckNewVersion();

                Settings.Default.DefaultServer = SelectedServer;
                Settings.Default.DefaultDataBase = SelectedDataBase;
                Settings.Default.DefaultUser = SelectedUser.TranscriptionName;
                Settings.Default.Save();

                DirectoryUser.ChangeUserId(BC, SelectedUser.Id, SelectedUser.UserName);
            }

            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new MainProjectChoiseViewModel(), new MainProjectChoiseView());
            passwordBox.Password = null;

            window.Visibility = Visibility.Visible;

            //DataContext.ChangeConnectionStringWithDefaultCredentials();
            //BC.RefreshContext();

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

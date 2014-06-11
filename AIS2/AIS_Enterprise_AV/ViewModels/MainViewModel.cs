using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Directories;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainViewModel : ViewModelAV
    {
        #region Base

        public MainViewModel()
            : base()
        {
            WindowVisibility = Visibility.Visible;
            IsAdminButtonsVisibility = false;

            Servers = new ObservableCollection<string>(HelperMethods.GetServers());
            string defaultServer = Properties.Settings.Default.DefaultServer;

            if (Servers.Contains(defaultServer))
            {
                SelectedServer = defaultServer;
            }

            KillTheDBCommand = new RelayCommand(KillTheDB);
            ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
            ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
            ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);

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

            foreach (var user in BC.GetDirectoryUsers())
            {
                Users.Add(user);
            }

            if (Users.Any())
            {
                SelectedUser = Users.First();
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<string> Languages { get; set; }

        public ObservableCollection<string> Servers { get; set; }

        private string _selectedServer;

        public string SelectedServer
        {
            get
            {
                return _selectedServer;
            }
            set
            {
                _selectedServer = value;
                OnPropertyChanged();

                DataContext.ChangeUserButler(_selectedServer);
                BC.RefreshContext();

                if (DataBases != null)
                {
                    DataBases.Clear();
                }
                else
                {
                    DataBases = new ObservableCollection<string>();
                }

                foreach (var dataBase in DBCustomQueries.GetDataBases(BC, _selectedServer))
                {
                    DataBases.Add(dataBase);
                    Debug.WriteLine(dataBase);
                }

                string defaultDataBase = Properties.Settings.Default.DefaultDataBase;
                
                if (DataBases.Contains(defaultDataBase))
                {
                    SelectedDataBase = defaultDataBase;
                }
            }
        }

        public ObservableCollection<string> DataBases { get; set; }

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
                OnPropertyChanged();

                if (_selectedDataBase != null)
                {
                    DataContext.ChangeServerAndDataBase(SelectedServer, _selectedDataBase);
                    BC.RefreshContext();

                    RefreshUsers();
                }
            } 
        }

        private string _selectedLanguage;
        [StopNotify]
        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();

                Properties.Settings.Default.Language = _selectedLanguage;
                Properties.Settings.Default.Save();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DirectoryUser> Users { get; set; }
        public DirectoryUser SelectedUser { get; set; }


        private Visibility _windowVisibility;
        public Visibility WindowVisibility
        {
            get
            {
                return _windowVisibility;
            }
            set
            {
                _windowVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand KillTheDBCommand { get; set; }
        public RelayCommand ShowExcelToDBCommand { get; set; }
        public RelayCommand ShowDefaultDBCommand { get; set; }
        public RelayCommand ShowDefaultOfficeDBCommand { get; set; }
        public RelayCommand EnteringCommand { get; set; }


        private void KillTheDB(object parameter)
        {
            IsNotInitializedDB = false;
            //Task.Factory.StartNew(BC.RemoveDB).ContinueWith((t) =>
               // {
                    BC.RemoveDB();
                    IsNotInitializedDB = true;

                    HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());

                    BC.RefreshContext();
                    RefreshUsers();
                //});

            
        }

        private void ShowExcelToDB(object parameter)
        {
            IsNotInitializedDB = false;
            Task.Factory.StartNew(() => ConvertingExcelToDB.ConvertExcelToDB(BC)).ContinueWith((t) => IsNotInitializedDB = true);
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
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox.Password;

            DataContext.ChangeUser(SelectedUser.TranscriptionName, password);

            if (DataContext.TryConnection())
            {
                BC.RefreshContext();

                Properties.Settings.Default.DefaultServer = SelectedServer;
                Properties.Settings.Default.DefaultDataBase = SelectedDataBase;
                Properties.Settings.Default.Save();
                

                DirectoryUser.ChangeUserId(SelectedUser.Id);

                WindowVisibility = Visibility.Collapsed;

                var monthTimeSheetView = new MonthTimeSheetView();
                monthTimeSheetView.ShowDialog();

                WindowVisibility = Visibility.Visible;

                BC.RefreshContext();
                IsAdminButtonsVisibility = HelperMethods.IsPrivilege(BC, UserPrivileges.ButtonsVisibility_AdminButtons);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Errororoororr");
            }
        }

        #endregion
    }
}

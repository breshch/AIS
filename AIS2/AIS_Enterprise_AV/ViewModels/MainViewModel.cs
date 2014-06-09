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
using System.Windows.Controls;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainViewModel : ViewModelAV
    {
        #region Base

        public MainViewModel()
            : base()
        {
            KillTheDBCommand = new RelayCommand(KillTheDB);
            ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
            ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
            ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);

            EnteringCommand = new RelayCommand(Entering);

            RefreshUsers();

            SelectedUser = Users.First();
           
            IsNotInitializedDB = true;

            Languages = new ObservableCollection<string>(new[] { "ru-RU", "en-US" });
        }

        private void RefreshUsers()
        {
            Users = new ObservableCollection<DirectoryUser>(BC.GetDirectoryUsers());
        }

        #endregion

        #region Properties

        public ObservableCollection<string> Languages { get; set; }

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

        public ObservableCollection<DirectoryUser> Users { get; set; }
        public DirectoryUser SelectedUser { get; set; }

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

                DirectoryUser.ChangeUserId(SelectedUser.Id);

                var monthTimeSheetView = new MonthTimeSheetView();
                monthTimeSheetView.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Errororoororr");
            }
        }

        #endregion
    }
}

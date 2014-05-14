using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Directories;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
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

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainViewModel : ViewModelAV
    {
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

        public bool IsNotInitializedDB { get; set; }

        public RelayCommand ShowMonthTimeSheetViewCommand { get; set; }
        public RelayCommand ShowExcelToDBCommand { get; set; }
        public RelayCommand ShowDefaultDBCommand { get; set; }
        public RelayCommand ShowDefaultOfficeDBCommand { get; set; }

        public MainViewModel() : base()
        {
            ShowMonthTimeSheetViewCommand = new RelayCommand(ShowMonthTimeSheetView);
            ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
            ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
            ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);

            IsNotInitializedDB = true;

            Languages = new ObservableCollection<string>(new[] { "ru-RU", "en-US" });
        }

        private void ShowMonthTimeSheetView(object parameter)
        {
            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();
        }

        private void ShowExcelToDB(object parameter)
        {
            IsNotInitializedDB = false;
            Task.Factory.StartNew(() => ConvertingExcelToDB.ConvertExcelToDB(BC)).ContinueWith((t) => IsNotInitializedDB = true);
        }

        private void ShowDefaultDB(object parameter)
        {
            IsNotInitializedDB = false;
            Task.Factory.StartNew(BC.InitializeDefaultDataBaseWithWorkers).ContinueWith((t) => IsNotInitializedDB = true );
        }

        private void ShowDefaultOfficeDB(object parameter)
        {
            IsNotInitializedDB = false;
            Task.Factory.StartNew(BC.InitializeDefaultDataBaseWithOfficeWorkers).ContinueWith((t) => IsNotInitializedDB = true);
        }
    }
}

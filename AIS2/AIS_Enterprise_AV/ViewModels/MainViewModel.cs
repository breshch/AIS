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

        public RelayCommand ShowDirectoryCompanyViewCommand { get; set; }
        public RelayCommand ShowDirectoryTypeOfPostViewCommand { get; set; }
        public RelayCommand ShowDirectoryPostViewCommand { get; set; }
        public RelayCommand ShowDirectoryAddWorkerViewCommand { get; set; }
        public RelayCommand ShowDirectoryWorkerListViewCommand { get; set; }
        public RelayCommand ShowMonthTimeSheetViewCommand { get; set; }
        public RelayCommand ShowDirectoryRCViewCommand { get; set; }
        public RelayCommand ShowSalaryViewCommand { get; set; }
        public RelayCommand ShowExcelToDBCommand { get; set; }
        public RelayCommand ShowDefaultDBCommand { get; set; }
        public RelayCommand ShowDefaultOfficeDBCommand { get; set; }


        public MainViewModel() : base()
        {
            ShowDirectoryCompanyViewCommand = new RelayCommand(ShowDirectoryCompanyView);
            ShowDirectoryTypeOfPostViewCommand = new RelayCommand(ShowDirectoryTypeOfPostView);
            ShowDirectoryPostViewCommand = new RelayCommand(ShowDirectoryPostView);
            ShowDirectoryAddWorkerViewCommand = new RelayCommand(ShowDirectoryAddWorkerView);
            ShowDirectoryWorkerListViewCommand = new RelayCommand(ShowDirectoryWorkerListView);
            ShowMonthTimeSheetViewCommand = new RelayCommand(ShowMonthTimeSheetView);
            ShowDirectoryRCViewCommand = new RelayCommand(ShowDirectoryRCView);
            ShowSalaryViewCommand = new RelayCommand(ShowSalaryView);
            ShowExcelToDBCommand = new RelayCommand(ShowExcelToDB);
            ShowDefaultDBCommand = new RelayCommand(ShowDefaultDB);
            ShowDefaultOfficeDBCommand = new RelayCommand(ShowDefaultOfficeDB);

            IsNotInitializedDB = true;

            Languages = new ObservableCollection<string>(new[] { "ru-RU", "en-US" });

            
        }

        private void ShowDirectoryTypeOfPostView(object parameter)
        {
            var directoryTypeOfPostViewModel = new DirectoryTypeOfPostViewModel();
            var directoryTypeOfPostView = new DirectoryTypeOfPostView();

            directoryTypeOfPostView.DataContext = directoryTypeOfPostViewModel;
            directoryTypeOfPostView.ShowDialog();
        }

        private void ShowDirectoryPostView(object parameter)
        {
            var directoryPostViewModel = new DirectoryPostViewModel();
            var directoryPostView = new DirectoryPostView();

            directoryPostView.DataContext = directoryPostViewModel;
            directoryPostView.ShowDialog();
        }

        private void ShowDirectoryCompanyView(object parameter)
        {
            var directoryCompanyViewModel = new DirectoryCompanyViewModel();
            var directoryCompanyView = new DirectoryCompanyView();

            directoryCompanyView.DataContext = directoryCompanyViewModel;
            directoryCompanyView.ShowDialog();
        }

        private void ShowDirectoryAddWorkerView(object parameter)
        {
            var directoryWorkerViewModel = new DirectoryAddWorkerViewModel();
            var directoryWorkerView = new DirectoryAddWorkerView();

            directoryWorkerView.DataContext = directoryWorkerViewModel;
            directoryWorkerView.ShowDialog();
        }

        private void ShowDirectoryWorkerListView(object parameter)
        {
            var directoryWorkerListViewModel = new DirectoryWorkerListViewModel();
            var directoryWorkerListView = new DirectoryWorkerListView();

            directoryWorkerListView.DataContext = directoryWorkerListViewModel;
            directoryWorkerListView.ShowDialog();
        }

        private void ShowMonthTimeSheetView(object parameter)
        {
            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();
        }

        private void ShowDirectoryRCView(object parameter)
        {
            var directoryRCViewModel = new DirectoryRCViewModel();
            var directoryRCView = new DirectoryRCView();

            directoryRCView.DataContext = directoryRCViewModel;
            directoryRCView.ShowDialog();
        }

        private void ShowSalaryView(object parameter)
        {
            var salaryViewModel = new SalaryViewModel();
            var salaryView = new SalaryView();

            salaryView.DataContext = salaryViewModel;
            salaryView.ShowDialog();
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

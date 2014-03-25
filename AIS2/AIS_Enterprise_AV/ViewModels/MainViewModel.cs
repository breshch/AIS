using AIS_Enterprise_AV.Views;
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
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainViewModel : ViewModel
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

        public RelayCommand ShowDirectoryCompanyViewCommand { get; set; }
        public RelayCommand ShowDirectoryTypeOfPostViewCommand { get; set; }
        public RelayCommand ShowDirectoryPostViewCommand { get; set; }
        public RelayCommand ShowDirectoryAddWorkerViewCommand { get; set; }
        public RelayCommand ShowDirectoryWorkerListViewCommand { get; set; }
        public RelayCommand ShowMonthTimeSheetViewCommand { get; set; }

        public MainViewModel() : base()
        {
            ShowDirectoryCompanyViewCommand = new RelayCommand(ShowDirectoryCompanyView);
            ShowDirectoryTypeOfPostViewCommand = new RelayCommand(ShowDirectoryTypeOfPostView);
            ShowDirectoryPostViewCommand = new RelayCommand(ShowDirectoryPostView);
            ShowDirectoryAddWorkerViewCommand = new RelayCommand(ShowDirectoryAddWorkerView);
            ShowDirectoryWorkerListViewCommand = new RelayCommand(ShowDirectoryWorkerListView);
            ShowMonthTimeSheetViewCommand = new RelayCommand(ShowMonthTimeSheetView);

            Languages = new ObservableCollection<string>(new[] { "ru-RU", "en-US" });



            //HelperDefaultDataBase.SetDataBase();
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
            var monthTimeSheetViewModel = new MonthTimeSheetViewModel();
            var monthTimeSheetView = new MonthTimeSheetView();

            monthTimeSheetView.DataContext = monthTimeSheetViewModel;
            monthTimeSheetView.ShowDialog();
        }
    }
}

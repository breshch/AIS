using AIS_Enterprise.Helpers;
using AIS_Enterprise.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand ShowDirectoryTypeOfCompanyViewCommand { get; set; }
        public RelayCommand ShowDirectoryCompanyViewCommand { get; set; }
        public RelayCommand ShowDirectoryPostViewCommand { get; set; }

        public MainViewModel()
        {
            ShowDirectoryTypeOfCompanyViewCommand = new RelayCommand(ShowDirectoryTypeOfCompanyView);
            ShowDirectoryCompanyViewCommand = new RelayCommand(ShowDirectoryCompanyView);
            ShowDirectoryPostViewCommand = new RelayCommand(ShowDirectoryTypeOfPostView);
        }

        private void ShowDirectoryTypeOfPostView(object parameter)
        {
            var directoryTypeOfPostViewModel = new DirectoryTypeOfPostViewModel();
            var directoryTypeOfPostView = new DirectoryTypeOfPostView();

            directoryTypeOfPostView.Owner = App.Current.MainWindow;
            directoryTypeOfPostView.DataContext = directoryTypeOfPostViewModel;
            directoryTypeOfPostView.ShowDialog();
        }

        private void ShowDirectoryTypeOfCompanyView(object parameter)
        {
            var directoryTypeOfCompanyViewModel = new DirectoryTypeOfCompanyViewModel();
            var directoryTypeOfCompanyView = new DirectoryTypeOfCompanyView();

            directoryTypeOfCompanyView.Owner = App.Current.MainWindow;
            directoryTypeOfCompanyView.DataContext = directoryTypeOfCompanyViewModel;
            directoryTypeOfCompanyView.ShowDialog();
        }

        private void ShowDirectoryCompanyView(object parameter)
        {
            var directoryCompanyViewModel = new DirectoryCompanyViewModel();
            var directoryCompanyView = new DirectoryCompanyView();

            directoryCompanyView.Owner = App.Current.MainWindow;
            directoryCompanyView.DataContext = directoryCompanyViewModel;
            directoryCompanyView.ShowDialog();
        }
    }
}

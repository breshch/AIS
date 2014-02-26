using AIS_Enterprise.Helpers;
using AIS_Enterprise.Views.DirectoryViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand ShowDirectoryCompanyViewCommand { get; set; }
        public RelayCommand ShowDirectoryTypeOfPostViewCommand { get; set; }
        public RelayCommand ShowDirectoryPostViewCommand { get; set; }

        public MainViewModel()
        {
            ShowDirectoryCompanyViewCommand = new RelayCommand(ShowDirectoryCompanyView);
            ShowDirectoryTypeOfPostViewCommand = new RelayCommand(ShowDirectoryTypeOfPostView);
            ShowDirectoryPostViewCommand = new RelayCommand(ShowDirectoryPostView);
        }

        private void ShowDirectoryTypeOfPostView(object parameter)
        {
            var directoryTypeOfPostViewModel = new DirectoryTypeOfPostViewModel();
            var directoryTypeOfPostView = new DirectoryTypeOfPostView();

            directoryTypeOfPostView.Owner = App.Current.MainWindow;
            directoryTypeOfPostView.DataContext = directoryTypeOfPostViewModel;
            directoryTypeOfPostView.ShowDialog();
        }

        private void ShowDirectoryPostView(object parameter)
        {
            var directoryPostViewModel = new DirectoryPostViewModel();
            var directoryPostView = new DirectoryPostView();

            directoryPostView.Owner = App.Current.MainWindow;
            directoryPostView.DataContext = directoryPostViewModel;
            directoryPostView.ShowDialog();
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

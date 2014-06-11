using AIS_Enterprise_AV.Models;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class InitializingDBViewModel : ViewModelBase
    {
        #region Base

        public InitializingDBViewModel()
        {
            ApplyParametersCommand = new RelayCommand(ApplyParameters);
            SkipCommand = new RelayCommand(Skip);
        }

        #endregion

        #region Properties

        public string CompanyName { get; set; }
        public string IP { get; set; }
        public string AdminName { get; set; }

        #endregion

        #region Commands

        public RelayCommand ApplyParametersCommand { get; set; }
        public RelayCommand SkipCommand { get; set; }

        private void ApplyParameters(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox.Password;

            DataContext.ChangeConnectionStringWithDefaultCredentials(IP, CompanyName);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContextAV>());

            using (var bc = new BusinessContextAV())
            {
                bc.CreateDatabase();
                bc.InitializeDefaultDataBaseWithoutWorkers();
                bc.AddDirectoryUserAdmin(AdminName, password);
                bc.AddUserButler();
            }

            DataContext.ChangeUserButler();

            HelperMethods.AddServer(IP);
        }

        private void Skip(object parameter)
        {
            var window = (Window)parameter;

            if (window != null)
            {
                window.Visibility = Visibility.Collapsed;

                HelperMethods.ShowView(new MainViewModel(), new MainView());

                window.Close();
            }
        }

        #endregion
    }
}

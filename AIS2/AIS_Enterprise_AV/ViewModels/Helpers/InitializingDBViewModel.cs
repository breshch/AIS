﻿using AIS_Enterprise_AV.Models;
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
        }

        #endregion


        #region Properties

        public string CompanyName { get; set; }
        public string IP { get; set; }
        public string AdminName { get; set; }

        #endregion


        #region Commands

        public RelayCommand ApplyParametersCommand { get; set; }

        private void ApplyParameters(object parameter)
        {
            var window = parameter as Window;
            var passwordBox = window.FindName("PasswordBoxAdminPass") as PasswordBox;

            string password = passwordBox.Password;

            DataContext.ChangeConnectionStringWithDefaultCredentials(IP, CompanyName);

            using (var bc = new BusinessContextAV())
            {
                bc.CreateDatabase();
                bc.InitializeDefaultDataBaseWithoutWorkers();
                bc.AddDirectoryUserAdmin(AdminName, password);
                //bc.AddUserButler();
            }

            //DataContext.ChangeUserButler();

            HelperMethods.AddServer(IP);

            window.Visibility = Visibility.Collapsed;

            HelperMethods.ShowView(new MainViewModel(), new MainView());

            window.Close();
        }

        #endregion
    }
}

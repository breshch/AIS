using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!DataContext.TryConnection())
            {
                HelperMethods.ShowView(new MainViewModel(), new MainView());
            }
            else
            {
                HelperMethods.ShowView(new InitializingDBViewModel(), new InitializingDBView());
            }
        }
    }
}

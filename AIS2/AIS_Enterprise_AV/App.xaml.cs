using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
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

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AIS_Enterprise_Global.Models.DataContext>());

            using (var bc = new BusinessContext())
            {
                bc.InitializeDatabase();
            }

            var mainViewModel = new MainViewModel();
            var mainView = new MainView();

            mainView.DataContext = mainViewModel;
            mainView.ShowDialog();
        }
    }
}

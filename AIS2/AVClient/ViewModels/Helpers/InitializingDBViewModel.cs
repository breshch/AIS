using System.Windows;
using AVClient.Helpers;
using AVClient.Views;

namespace AVClient.ViewModels.Helpers
{
    public class InitializingDBViewModel : ViewModelGlobal
    {
        #region Base

        public InitializingDBViewModel()
        {
            CreateEmptyDBCommand = new RelayCommand(CreateEmptyDB);
            ApplyParametersCommand = new RelayCommand(ApplyParameters);
            SkipCommand = new RelayCommand(Skip);

            IP = "127.1";
        }

        #endregion


        #region Properties

        public string CompanyName { get; set; }
        public string IP { get; set; }
        public string AdminName { get; set; }

        #endregion


        #region Commands

        public RelayCommand CreateEmptyDBCommand { get; set; }
        public RelayCommand ApplyParametersCommand { get; set; }
        public RelayCommand SkipCommand { get; set; }

		//TODO REFACTORING
        private void CreateEmptyDB(object parameter)
        {
			//var window = parameter as Window;
			//var passwordBox = window.FindName("PasswordBoxAdminPass") as PasswordBox;

			//string password = passwordBox.Password;


			//using (var bc = new BusinessContext())
			//{
			//	bc.CreateDatabase();
			//	bc.InitializeEmptyDB();
			//	bc.AddDirectoryUserAdmin(AdminName, password);
			//}

			//window.Visibility = Visibility.Collapsed;

			//Settings.Default.DefaultServer = null;
			//Settings.Default.DefaultDataBase = null;
			//Settings.Default.DefaultUser = null;
			//Settings.Default.Save();

			//HelperMethods.ShowView(new MainViewModel(), new MainView());

			//window.Close();
        }

        private void ApplyParameters(object parameter)
        {
			//var window = parameter as Window;
			//var passwordBox = window.FindName("PasswordBoxAdminPass") as PasswordBox;

			//string password = passwordBox.Password;


			//using (var bc = new BusinessContext())
			//{
			//	bc.CreateDatabase();
			//	bc.InitializeDefaultDataBaseWithoutWorkers();
			//	bc.AddDirectoryUserAdmin(AdminName, password);
			//}

			//window.Visibility = Visibility.Collapsed;

			//Settings.Default.DefaultServer = null;
			//Settings.Default.DefaultDataBase = null;
			//Settings.Default.DefaultUser = null;
			//Settings.Default.Save();

			//HelperMethods.ShowView(new MainViewModel(), new MainView());

			//window.Close();
        }

        private void Skip(object parameter)
        {
            var window = parameter as Window;

            window.Visibility = Visibility.Collapsed;

            HelperMethods.ShowView(new MainViewModel(), new MainView());

            window.Close();
        }

        #endregion
    }
}

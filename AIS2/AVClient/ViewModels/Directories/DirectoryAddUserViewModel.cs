using System.Windows;
using System.Windows.Controls;
using AVClient.Helpers;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryAddUserViewModel : DirectoryUserBaseViewModel
    {

        #region Base

        public DirectoryAddUserViewModel()
        {
            AddCommand = new RelayCommand(Add);
        } 

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            var window = parameter as Window;
            var passwordBox = window.FindName("PasswordBoxPass") as PasswordBox;

            string password = passwordBox.Password;

            BC.AddDirectoryUser(DirectoryUserName, password, SelectedDirectoryUserStatus);

            window.Close();
        }
        
        #endregion
    }
}

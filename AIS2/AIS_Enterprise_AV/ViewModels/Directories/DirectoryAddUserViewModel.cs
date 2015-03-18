using System.Windows;
using System.Windows.Controls;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories.Base;

namespace AIS_Enterprise_Global.ViewModels.Directories
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

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories.Base;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryEditUserViewModel : DirectoryUserBaseViewModel
    {

        #region Base

        private int _userId;
        public DirectoryEditUserViewModel(DirectoryUser user)
        {
            _userId = user.Id;
            DirectoryUserName = user.UserName;
            SelectedDirectoryUserStatus = DirectoryUserStatuses.First(s => s.Name == user.CurrentUserStatus.DirectoryUserStatus.Name);
            EditCommand = new RelayCommand(Edit);
        } 
        #endregion


        #region Commands
        public RelayCommand EditCommand { get; set; }

        private void Edit(object parameter)
        {
	        var window = parameter as Window;

			var passwordBox = window.FindName("PasswordBoxPass") as PasswordBox;
            string password = passwordBox.Password;

            BC.EditDirectoryUser(_userId, DirectoryUserName, password, SelectedDirectoryUserStatus);

			window.Close();
        }
        
        #endregion

    }
}

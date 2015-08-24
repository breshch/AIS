using System.Collections.Generic;
using System.Windows;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryAddUserStatusViewModel : DirectoryUserStatusBaseViewModel
    {
        #region Base

        public DirectoryAddUserStatusViewModel()
        {
            AddCommand = new RelayCommand(Add);
        }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            var privileges = new List<CurrentUserStatusPrivilege>();

            foreach (var mainParent in GroupPrivileges)
            {
                string privilageName = "";
                AddPrivilege(mainParent, privilageName, privileges);
            }

            BC.AddDirectoryUserStatus(UserStatusName, privileges.ToArray());

            var window = (Window)parameter;
            window.Close();
        }

        #endregion
    }
}

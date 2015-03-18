using System.Collections.Generic;
using System.Windows;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories.Base;

namespace AIS_Enterprise_Global.ViewModels.Directories
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

            BC.AddDirectoryUserStatus(UserStatusName, privileges);

            var window = (Window)parameter;
            window.Close();
        }

        #endregion
    }
}

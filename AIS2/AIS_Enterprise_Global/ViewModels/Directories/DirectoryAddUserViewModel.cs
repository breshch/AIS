using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox.Password;

            BC.AddDirectoryUser(DirectoryUserName, password, SelectedDirectoryUserStatus);
        }
        
        #endregion



    }
}

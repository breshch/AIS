using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryUsersViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryUsersViewModel()
        {
            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser);
            RemoveUserCommand = new RelayCommand(RemoveUser);

            RefreshUsers();
        }

        private void RefreshUsers()
        {
            Users = new ObservableCollection<DirectoryUser>(BC.GetDirectoryUsers());
        }

        #endregion

        #region Properties

        public ObservableCollection<DirectoryUser> Users { get; set; }
        public DirectoryUser SelectedUser { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand EditUserCommand { get; set; }
        public RelayCommand RemoveUserCommand { get; set; }

        private void AddUser(object parameter)
        { 
            HelperMethods.ShowView(new DirectoryAddUserViewModel(), new DirectoryAddUserView());

            RefreshUsers();
        }

        private void EditUser(object parameter)
        {
            HelperMethods.ShowView(new DirectoryEditUserViewModel(SelectedUser), new DirectoryEditUserView());

            BC.RefreshContext();
            RefreshUsers();
        }

        private void RemoveUser(object parameter)
        {
            BC.RemoveDirectoryUser(SelectedUser);

            RefreshUsers();
        }
        #endregion
    }
}

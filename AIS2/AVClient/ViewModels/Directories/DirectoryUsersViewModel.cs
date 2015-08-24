using System.Collections.ObjectModel;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Directories
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
            Users = new ObservableCollection<DTOUser>(BC.GetUsers());
        }

        #endregion

        #region Properties

        public ObservableCollection<DTOUser> Users { get; set; }
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

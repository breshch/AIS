using System.Collections.ObjectModel;
using System.Linq;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryUserStatusesViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryUserStatusesViewModel()
        {
            RefreshDirectoryUserStatuses();

            AddCommand = new RelayCommand(Add, CanAdding);
            EditCommand = new RelayCommand(Edit, IsSelected);
            RemoveCommand = new RelayCommand(Remove, IsSelected);

        }

        private void RefreshDirectoryUserStatuses()
        {
            DirectoryUserStatuses = new ObservableCollection<DirectoryUserStatus>(BC.GetDirectoryUserStatuses());
        }


        #endregion


        #region Properties

        public ObservableCollection<DirectoryUserStatus> DirectoryUserStatuses { get; set; }

        public DirectoryUserStatus SelectedDirectoryUserStatus { get; set; }

       
        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public void Add(object parameter)
        {
            HelperMethods.ShowView(new DirectoryAddUserStatusViewModel(), new DirectoryAddUserStatusView());

            RefreshDirectoryUserStatuses();
        }

        public void Edit(object parameter)
        {
            HelperMethods.ShowView(new DirectoryEditUserStatusViewModel(SelectedDirectoryUserStatus), new DirectoryEditUserStatusView());

            RefreshDirectoryUserStatuses();
        }

        public bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        public void Remove(object parameter)
        {
            BC.RemoveDirectoryUserStatus(SelectedDirectoryUserStatus.Id);

            RefreshDirectoryUserStatuses();
            
            if (DirectoryUserStatuses.Any())
            {
                SelectedDirectoryUserStatus = DirectoryUserStatuses.Last();
            }
        }

        public bool IsSelected(object parameter)
        {
            return SelectedDirectoryUserStatus != null;
        }

        #endregion
    }
}

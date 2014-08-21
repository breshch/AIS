using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AIS_Enterprise_Global.Views.Directories;
using AIS_Enterprise_Global.ViewModels.Directories;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryUserStatusesViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryUserStatusesViewModel() : base()
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

            BC.RefreshContext();
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

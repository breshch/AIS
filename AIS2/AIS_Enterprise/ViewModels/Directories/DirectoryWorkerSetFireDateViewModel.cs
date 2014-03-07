using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryWorkerSetFireDateViewModel : ViewModel
    {

        #region Base

        public DirectoryWorkerSetFireDateViewModel()
        {
            SelectedDirectoryWorkerFireDate = DateTime.Now;

            FireDirectoryWorkerCommand = new RelayCommand(FireDirectoryWorker);
        }

        private void FireDirectoryWorker(object parameter)
        {
            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion

        #region DirectoryWorkerFireDate

        private DateTime? _selectedDirectoryWorkerFireDate;
        public DateTime? SelectedDirectoryWorkerFireDate
        {
            get 
            {
                return _selectedDirectoryWorkerFireDate;
            }
            set
            {
                _selectedDirectoryWorkerFireDate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand FireDirectoryWorkerCommand { get; set; }

        public override void ViewClose(object parameter)
        {
            //SelectedDirectoryWorkerFireDate = null;

            base.ViewClose(parameter);
        }
        #endregion


    }
}

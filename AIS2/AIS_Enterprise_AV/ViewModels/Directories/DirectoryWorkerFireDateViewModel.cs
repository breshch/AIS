using System;
using System.Windows;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryWorkerFireDateViewModel : ViewModelGlobal
    {

        #region Base

        private bool _isFireDate;

        public DirectoryWorkerFireDateViewModel(int workerId)
        {
            SelectedDirectoryWorkerFireDate = BC.GetLastWorkDay(workerId);

            FireDirectoryWorkerCommand = new RelayCommand(FireDirectoryWorker);
        }

        private void FireDirectoryWorker(object parameter)
        {
            _isFireDate = true;
            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion


        #region Properties

        public DateTime? SelectedDirectoryWorkerFireDate { get; set; }

        #endregion


        #region Commands

        public RelayCommand FireDirectoryWorkerCommand { get; set; }

        public override void ViewClose(object parameter)
        {
            if (!_isFireDate)
            {
                SelectedDirectoryWorkerFireDate = null;
            }
        }
        #endregion
    }
}

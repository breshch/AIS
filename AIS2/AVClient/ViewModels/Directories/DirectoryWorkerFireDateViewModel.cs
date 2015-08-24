using System;
using System.Windows;
using AVClient.Helpers;

namespace AVClient.ViewModels.Directories
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

		//TODO Refactor
		//public override void ViewClose(object parameter)
		//{
		//	if (!_isFireDate)
		//	{
		//		SelectedDirectoryWorkerFireDate = null;
		//	}
		//}
        #endregion
    }
}

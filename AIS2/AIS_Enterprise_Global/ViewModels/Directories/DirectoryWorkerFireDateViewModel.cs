using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryWorkerFireDateViewModel : ViewModel
    {

        #region Base

        public DirectoryWorkerFireDateViewModel()
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

        public DateTime? SelectedDirectoryWorkerFireDate { get; set; }

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

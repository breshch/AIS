using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryWorkerListViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryWorkerListViewModel() : base()
        {
            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(BC.GetDirectoryWorkers().ToList().OrderBy(w => w.Status));

            ShowDirectoryEditWorkerCommand = new RelayCommand(ShowDirectoryEditWorker);
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryWorker> DirectoryWorkers { get; set; }

        public DirectoryWorker SelectedDirectoryWorker { get; set; }

        #endregion


        #region Commands

        public RelayCommand ShowDirectoryEditWorkerCommand { get; set; }

        private void ShowDirectoryEditWorker(object parameter)
        {
            if (SelectedDirectoryWorker != null)
            {
                var directoryEditWorkerViewModel = new DirectoryEditWorkerViewModel(SelectedDirectoryWorker.Id);
                var directoryEditWorkerView = new DirectoryEditWorkerView();

                directoryEditWorkerView.DataContext = directoryEditWorkerViewModel;
                directoryEditWorkerView.ShowDialog();
            }
        }

        #endregion
    }
}

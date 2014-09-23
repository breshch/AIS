using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryWorkerListViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryWorkerListViewModel() : base()
        {
            var firstWorkingArea = System.Windows.Forms.Screen.AllScreens[0].WorkingArea;
            MaxHeightForm = firstWorkingArea.Height - 100;
            var directoryWorkers = new List<DirectoryWorker>();
            
            var workers = BC.GetDirectoryWorkers().ToList();
            var workerWarehouses = workers.Where(w => !w.IsDeadSpirit && BC.GetDirectoryTypeOfPost(w.Id, DateTime.Now).Name == "Склад").ToList();
            directoryWorkers.AddRange(workerWarehouses);

            var privileges = DirectoryUser.Privileges;

            if (HelperMethods.IsPrivilege(privileges, UserPrivileges.WorkersVisibility_DeadSpirit))
            {
                var workerDeadSpirits = workers.Where(w => w.IsDeadSpirit).ToList();

                directoryWorkers.AddRange(workerDeadSpirits);
            }

            if (HelperMethods.IsPrivilege(privileges, UserPrivileges.WorkersVisibility_Office))
            {
                var workerOffices = workers.Where(w => !w.IsDeadSpirit && BC.GetDirectoryTypeOfPost(w.Id, DateTime.Now).Name == "Офис").ToList();

                directoryWorkers.AddRange(workerOffices);
            }

            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(directoryWorkers.OrderBy(w => w.Status));

            ShowDirectoryEditWorkerCommand = new RelayCommand(ShowDirectoryEditWorker);
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryWorker> DirectoryWorkers { get; set; }

        public DirectoryWorker SelectedDirectoryWorker { get; set; }

        public int MaxHeightForm { get; set; }

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

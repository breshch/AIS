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
    public class DirectoryWorkerListViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryWorkerListViewModel() : base()
        {
            var directoryWorkers = new List<DirectoryWorker>();

            var workers = BC.GetDirectoryWorkers().ToList();

            var workerWarehouses = workers.Where(w => !w.IsDeadSpirit && w.CurrentDirectoryPost.DirectoryTypeOfPost.Name == "Склад").ToList();
            directoryWorkers.AddRange(workerWarehouses);

            var user = BC.GetDirectoryUser(DirectoryUser.CurrentUserId);
            var privileges = user.CurrentUserStatus.DirectoryUserStatus.Privileges.Select(p => p.DirectoryUserStatusPrivilege.Name).ToList();

            if (HelperMethods.IsPrivilege(privileges, UserPrivileges.WorkersVisibility_DeadSpirit))
            {
                var workerDeadSpirits = workers.Where(w => w.IsDeadSpirit).ToList();

                directoryWorkers.AddRange(workerDeadSpirits);
            }

            if (HelperMethods.IsPrivilege(privileges, UserPrivileges.WorkersVisibility_Office))
            {
                var workerOffices = workers.Where(w => !w.IsDeadSpirit && w.CurrentDirectoryPost.DirectoryTypeOfPost.Name == "Офис").ToList();

                directoryWorkers.AddRange(workerOffices);
            }

            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(directoryWorkers.OrderBy(w => w.Status));

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

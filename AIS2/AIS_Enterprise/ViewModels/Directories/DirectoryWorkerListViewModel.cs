using AIS_Enterprise.Helpers;
using AIS_Enterprise.Models.Directories;
using AIS_Enterprise.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.ViewModels.Directories
{
    public class DirectoryWorkerListViewModel : ViewModel
    {

        #region Base

        public DirectoryWorkerListViewModel() : base()
        {
            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(BC.GetDirectoryWorkers().ToList().OrderBy(w => w.Status));

            ShowDirectoryEditWorkerCommand = new RelayCommand(ShowDirectoryEditWorker);

        }

        #endregion

        #region DirectoryWorkers

        public ObservableCollection<DirectoryWorker> DirectoryWorkers { get; set; }

        private DirectoryWorker _selectedDirectoryWorker;
        public DirectoryWorker SelectedDirectoryWorker
        {
            get
            {
                return _selectedDirectoryWorker;
            }
            set 
            {
                _selectedDirectoryWorker = value;
                OnPropertyChanged();
            }
        }

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

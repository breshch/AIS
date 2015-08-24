using System.Collections.ObjectModel;
using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos
{
    public class InfoPanaltiesViewModel : ViewModelGlobal
    {
        #region Base
        public InfoPanaltiesViewModel(int workerId, int year, int month)
        {
            WorkerFullName = BC.GetDirectoryWorkerById(workerId).FullName;
            Panalties = new ObservableCollection<InfoDate>(BC.GetInfoDatePanalties(workerId, year, month));

        }
        #endregion
        #region Properties
        public string WorkerFullName { get; set; }
        public ObservableCollection<InfoDate> Panalties { get; set; }
        #endregion
    }
}

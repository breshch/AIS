using System.Collections.ObjectModel;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_Global.ViewModels.Infos
{
    public class InfoPanaltiesViewModel : ViewModelGlobal
    {
        #region Base
        public InfoPanaltiesViewModel(int workerId, int year, int month)
        {
            WorkerFullName = BC.GetDirectoryWorker(workerId).FullName;
            Panalties = new ObservableCollection<InfoDate>(BC.GetInfoDatePanalties(workerId, year, month));

        }
        #endregion
        #region Properties
        public string WorkerFullName { get; set; }
        public ObservableCollection<InfoDate> Panalties { get; set; }
        #endregion
    }
}

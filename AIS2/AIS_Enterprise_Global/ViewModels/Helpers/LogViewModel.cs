using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels.Helpers
{
    public class LogViewModel : ViewModelGlobal
    {

        #region Base

        public LogViewModel()
        {
            SelectedDate = DateTime.Now;

            Logs = new ObservableCollection<Log>(BC.GetLogs(SelectedDate));
        }


        #endregion

        #region Properties 

        public DateTime SelectedDate { get; set; }

        public ObservableCollection<Log> Logs { get; set; }

        #endregion
    }
}

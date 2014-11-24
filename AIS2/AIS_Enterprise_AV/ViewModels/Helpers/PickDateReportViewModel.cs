using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class PickDateReportViewModel : ViewModelGlobal
    {
        #region Base

        public PickDateReportViewModel()
        {
            SelectedDate = DateTime.Now;
            FormingReportCommand = new RelayCommand(FormingReport);
            TitleName = "Остатки залог";
        }

        #endregion


        #region Properties

        public DateTime SelectedDate { get; set; }

        public string TitleName { get; set; }

        #endregion


        #region Commands

        public RelayCommand FormingReportCommand { get; set; }

        private void FormingReport(object parameter)
        {
            CarPartReports.ComplitedLoanRemainsToDate(BC, SelectedDate);
        }
        #endregion
    }
}
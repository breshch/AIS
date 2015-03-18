using System;
using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class FromToDatesReportViewModel : ViewModelGlobal
    {
        #region Base

        public FromToDatesReportViewModel()
        {
            SelectedFromDate = DateTime.Now;
            SelectedToDate = DateTime.Now;
            FormingReportCommand = new RelayCommand(FormingReport);
        }

        #endregion


        #region Properties

        public DateTime SelectedFromDate { get; set; }
        public DateTime SelectedToDate { get; set; }

        public string TitleName { get; set; }

        #endregion


        #region Commands

        public RelayCommand FormingReportCommand { get; set; }

        private void FormingReport(object parameter)
        {
            CarsReports.Cars(BC, SelectedFromDate, SelectedToDate);
        }
        #endregion
    }
}
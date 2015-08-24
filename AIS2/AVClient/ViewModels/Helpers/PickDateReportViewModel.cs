using System;
using AVClient.Helpers;
using AVClient.Reports;

namespace AVClient.ViewModels.Helpers
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
            CarPartReports.ComplitedLoanRemainsToDate(SelectedDate);
        }
        #endregion
    }
}
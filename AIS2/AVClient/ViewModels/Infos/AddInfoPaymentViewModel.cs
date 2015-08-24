using System;
using System.Windows;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos
{
    public class AddInfoPaymentViewModel : ViewModelGlobal
    {
        #region Base
        private int _infoLoanId;
        public AddInfoPaymentViewModel(int infoLoanId)
        {
            SelectedDateLoanPayment = DateTime.Now;

            PayCommand = new RelayCommand(Pay);
            _infoLoanId = infoLoanId;
        }
        #endregion

        #region Properties

        public DateTime SelectedDateLoanPayment { get; set; }
        public double SummLoanPayment { get; set; }

        #endregion

        #region Commands
        public RelayCommand PayCommand { get; set; }

        private void Pay(object parameter)
        {
            BC.AddInfoPayment(_infoLoanId, SelectedDateLoanPayment, SummLoanPayment);
          
            var window = parameter as Window;
            window.Close();
        }
        #endregion
    }
}

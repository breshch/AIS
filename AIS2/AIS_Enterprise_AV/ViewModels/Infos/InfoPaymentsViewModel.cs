using System.Collections.ObjectModel;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoPaymentsViewModel :ViewModelGlobal
    {

        #region Base
        private int _infoLoanId;
        public InfoPaymentsViewModel (int infoLoanId)
	    {

            AddCommand = new RelayCommand(Add);
            RemoveCommand = new RelayCommand(Remove, IsSelectedPayment);
            _infoLoanId = infoLoanId;

            RefreshPayments();
	    }

        private void RefreshPayments()
        {
            InfoPayments = new ObservableCollection<InfoPayment>(BC.GetInfoPayments(_infoLoanId));
        }
        #endregion

        #region Properties

        public ObservableCollection<InfoPayment> InfoPayments { get; set; }
        public InfoPayment SelectedInfoPayment { get; set; }

       
        #endregion

        #region Commands
        
        public RelayCommand AddCommand{ get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private void Add (object parameter)
        {
            HelperMethods.ShowView(new AddInfoPaymentViewModel(_infoLoanId), new AddInfoPaymentView());
            RefreshPayments();
        }

        private void Remove (object parameter)
        {
            BC.RemoveInfoPayment(SelectedInfoPayment);
            RefreshPayments();
        }

        private bool IsSelectedPayment(object parameter)
        {
            return SelectedInfoPayment != null;
        }

        #endregion
    }
}
 
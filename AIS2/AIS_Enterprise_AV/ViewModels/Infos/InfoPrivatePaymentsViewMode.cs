using System.Collections.ObjectModel;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoPrivatePaymentsViewModel :ViewModelGlobal
    {

        #region Base
        private int _infoPrivateLoanId;
        public InfoPrivatePaymentsViewModel (int infoPrivateLoanid)
	    {

            AddCommand = new RelayCommand(Add);
            RemoveCommand = new RelayCommand(Remove, IsSelectedPrivatePayment);
            _infoPrivateLoanId = infoPrivateLoanid;

            RefreshPayments();
	    }

        private void RefreshPayments()
        {
            InfoPayments = new ObservableCollection<InfoPrivatePayment>(BC.GetInfoPrivatePayments(_infoPrivateLoanId));
        }
        #endregion

        #region Properties

        public ObservableCollection<InfoPrivatePayment> InfoPayments { get; set; }
        public InfoPrivatePayment SelectedInfoPayment { get; set; }

       
        #endregion

        #region Commands
        
        public RelayCommand AddCommand{ get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private void Add (object parameter)
        {
            HelperMethods.ShowView(new AddInfoPrivatePaymentViewModel(_infoPrivateLoanId), new AddInfoPaymentView());
            RefreshPayments();
        }

        private void Remove (object parameter)
        {
            BC.RemoveInfoPrivatePayment(SelectedInfoPayment);
            RefreshPayments();
        }

        private bool IsSelectedPrivatePayment(object parameter)
        {
            return SelectedInfoPayment != null;
        }

        #endregion
    }
}
 
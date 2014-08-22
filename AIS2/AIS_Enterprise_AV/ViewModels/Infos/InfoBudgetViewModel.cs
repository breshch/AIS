using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Infos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoBudgetViewModel : ViewModelGlobal
    {
        #region Base

        public InfoBudgetViewModel()
        {
            SelectedInfoLoanDate = DateTime.Now;

            RefreshInfoLoans();
            RefreshInfoSafesCash();

            AddLoanCommand = new RelayCommand(AddLoan);
            EditLoanCommand = new RelayCommand(EditLoan, IsSelectedInfoLoan);
            RemoveLoanCommand = new RelayCommand(RemoveLoan, IsSelectedInfoLoan);
            ShowPaymentsCommand = new RelayCommand(ShowPayments, IsSelectedInfoLoan);

            AddSafeCashCommand = new RelayCommand(AddSafeCash);
            RemoveSafeCashCommand = new RelayCommand(RemoveSafeCash, IsSelectedInfoSafeCash);

        }

        private void RefreshInfoLoans()
        {
            InfoLoans = new ObservableCollection<InfoLoan>(BC.GetInfoLoans(SelectedInfoLoanDate));
            TotalSummLoans = BC.GetLoans();
        }

        private void RefreshInfoSafesCash()
        {
            InfoSafesCash = new ObservableCollection<InfoSafe>(BC.GetInfoSafes());
        }

        #endregion

        #region Properties

        public ObservableCollection<InfoLoan> InfoLoans { get; set; }
        private DateTime _selectedInfoLoanDate;
        public DateTime SelectedInfoLoanDate 
        {
            get
            {
                return _selectedInfoLoanDate;
            }
            set 
            {
                _selectedInfoLoanDate = value;
                RaisePropertyChanged();

                TotalSummSelectedDate = BC.GetInfoCash(_selectedInfoLoanDate);
                TotalSummEndingMonth = BC.GetInfoCash(_selectedInfoLoanDate.Year, _selectedInfoLoanDate.Month);


            }
        
        }
        public InfoLoan SelectedInfoLoan { get; set; }
        public double TotalSummSelectedDate { get; set; }
        public double TotalSummEndingMonth { get; set; }
        public double TotalSummLoans { get; set; }


        public ObservableCollection<InfoSafe> InfoSafesCash { get; set; }
        public InfoSafe SelectedInfoSafeCash { get; set; }

        public double TotalSummCash { get; set; }





        public double TotalSummSafe { get; set; }
        public double TotalSummCard { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddLoanCommand { get; set; }
        public RelayCommand EditLoanCommand { get; set; }
        public RelayCommand RemoveLoanCommand { get; set; }
        public RelayCommand ShowPaymentsCommand { get; set; }

        public RelayCommand AddSafeCashCommand { get; set; }
        public RelayCommand RemoveSafeCashCommand { get; set; }

        private void AddLoan(object parameter)
        {
            HelperMethods.ShowView(new AddInfoLoanViewModel(), new AddEditInfoLoanView());

            RefreshInfoLoans();

        }

        private void EditLoan(object parameter)
        {
            HelperMethods.ShowView(new EditInfoLoanViewModel(SelectedInfoLoan), new AddEditInfoLoanView());

            BC.RefreshContext();

            RefreshInfoLoans();
        }

        private void RemoveLoan(object parameter)
        {
            BC.RemoveInfoLoan(SelectedInfoLoan);

            RefreshInfoLoans();
        }

        private bool IsSelectedInfoLoan(object parameter)
        {
            return SelectedInfoLoan != null;
        }


        private void ShowPayments(object parameter)
        {
            HelperMethods.ShowView(new InfoPaymentsViewModel(SelectedInfoLoan.Id), new InfoPaymentsView());
        }


        private void AddSafeCash(object parameter)
        {
            HelperMethods.ShowView(new AddInfoSafeViewModel(), new AddInfoSafeView());
            RefreshInfoSafesCash();
        }

        private void RemoveSafeCash(object parameter)
        {
            BC.RemoveInfoSafe(SelectedInfoSafeCash);
            RefreshInfoSafesCash();
        }

        private bool IsSelectedInfoSafeCash(object parameter)
        {
            return SelectedInfoSafeCash != null && !SelectedInfoSafeCash.IsIncoming && SelectedInfoSafeCash.CashType == CashType.Наличка;
        }

        #endregion
    }
}

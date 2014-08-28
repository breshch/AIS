using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Infos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_AV.Helpers.Temps;
using System.Windows.Media;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoBudgetViewModel : ViewModelGlobal
    {
        #region Base

        public InfoBudgetViewModel()
        {
            SelectedInfoLoanDate = DateTime.Now;
            SelectedInfoPrivateLoanDate = DateTime.Now;

            RefreshInfoLoans();
            RefreshInfoPrivateLoans();
            RefreshInfoSafesCash();
            RefreshInfoSafesCard();
            RefreshAllSafeData();

            AddLoanCommand = new RelayCommand(AddLoan);
            EditLoanCommand = new RelayCommand(EditLoan, IsSelectedInfoLoan);
            RemoveLoanCommand = new RelayCommand(RemoveLoan, IsSelectedInfoLoan);
            ShowPaymentsCommand = new RelayCommand(ShowPayments, IsSelectedInfoLoan);

            AddPrivateLoanCommand = new RelayCommand(AddPrivateLoan);
            EditPrivateLoanCommand = new RelayCommand(EditPrivateLoan, IsSelectedInfoPrivateLoan);
            RemovePrivateLoanCommand = new RelayCommand(RemovePrivateLoan, IsSelectedInfoPrivateLoan);
            ShowPrivatePaymentsCommand = new RelayCommand(ShowPrivatePayments, IsSelectedInfoPrivateLoan);

            AddSafeCashCommand = new RelayCommand(AddSafeCash);
            RemoveSafeCashCommand = new RelayCommand(RemoveSafeCash, IsSelectedInfoSafeCash);
            InsertSafeCashCommand = new RelayCommand(InsertSafeCash);
        }

        private void RefreshAllSafeData()
        {
            AllSafeData = new ObservableCollection<SafeData>();

            double totalSummLoans = BC.GetLoans();
            double totalSummCash = BC.GetParameterValue<double>("TotalCash");
            double totalSummSafe = BC.GetParameterValue<double>("TotalSafe");
            double totalSummCard = BC.GetParameterValue<double>("TotalCard");
            double totalSummAll = totalSummSafe + totalSummCash + totalSummCard + totalSummLoans;

            double totalSummPrivateLoans = BC.GetPrivateLoans();

            AllSafeData.Add(new SafeData { Name = "Сейф", Summ = totalSummSafe });
            AllSafeData.Add(new SafeData { Name = "Наличка", Summ = totalSummCash });
            AllSafeData.Add(new SafeData { Name = "Карточка", Summ = totalSummCard });
            AllSafeData.Add(new SafeData { Name = "Долги", Summ = totalSummLoans });
            AllSafeData.Add(new SafeData { Name = "Итого", Summ = totalSummAll, Color = Brushes.MediumSlateBlue });
            AllSafeData.Add(new SafeData { Name = null, Summ = 0 });
            AllSafeData.Add(new SafeData { Name = "Частные долги", Summ = totalSummPrivateLoans });
        }

        private void RefreshInfoLoans()
        {
            InfoLoans = new ObservableCollection<InfoLoan>(BC.GetInfoLoans(SelectedInfoLoanDate));
            RefreshAllSafeData();
        }

        private void RefreshInfoPrivateLoans()
        {
            InfoPrivateLoans = new ObservableCollection<InfoPrivateLoan>(BC.GetInfoPrivateLoans(SelectedInfoPrivateLoanDate));
            RefreshAllSafeData();
        }

        private void RefreshInfoSafesCash()
        {
            InfoSafesCash = new ObservableCollection<InfoSafe>(BC.GetInfoSafes(CashType.Наличка));
            RefreshAllSafeData();
        }

        private void RefreshInfoSafesCard()
        {
            InfoSafesCard = new ObservableCollection<InfoSafe>(BC.GetInfoSafes(CashType.Карточка));
            RefreshAllSafeData();
        }

        #endregion

        #region Properties

        public ObservableCollection<SafeData> AllSafeData { get; set; }
        public ObservableCollection<InfoLoan> InfoLoans { get; set; }
        public DateTime SelectedInfoLoanDate { get; set; }
        public InfoLoan SelectedInfoLoan { get; set; }
        public ObservableCollection<InfoSafe> InfoSafesCash { get; set; }
        public InfoSafe SelectedInfoSafeCash { get; set; }
        public ObservableCollection<InfoSafe> InfoSafesCard { get; set; }

        public DateTime SelectedInfoPrivateLoanDate { get; set; }
        public ObservableCollection<InfoPrivateLoan> InfoPrivateLoans { get; set; }
        public InfoPrivateLoan SelectedInfoPrivateLoan { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddLoanCommand { get; set; }
        public RelayCommand EditLoanCommand { get; set; }
        public RelayCommand RemoveLoanCommand { get; set; }
        public RelayCommand ShowPaymentsCommand { get; set; }

        public RelayCommand AddPrivateLoanCommand { get; set; }
        public RelayCommand EditPrivateLoanCommand { get; set; }
        public RelayCommand RemovePrivateLoanCommand { get; set; }
        public RelayCommand ShowPrivatePaymentsCommand { get; set; }

        public RelayCommand AddSafeCashCommand { get; set; }
        public RelayCommand RemoveSafeCashCommand { get; set; }
        public RelayCommand InsertSafeCashCommand { get; set; }

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

        private void AddPrivateLoan(object parameter)
        {
            HelperMethods.ShowView(new AddInfoPrivateLoanViewModel(), new AddEditInfoLoanView());

            RefreshInfoPrivateLoans();
        }

        private void EditPrivateLoan(object parameter)
        {
            HelperMethods.ShowView(new EditInfoPrivateLoanViewModel(SelectedInfoPrivateLoan), new AddEditInfoLoanView());

            BC.RefreshContext();

            RefreshInfoPrivateLoans();
        }

        private void RemovePrivateLoan(object parameter)
        {
            BC.RemoveInfoPrivateLoan(SelectedInfoPrivateLoan);

            RefreshInfoPrivateLoans();
        }

        private bool IsSelectedInfoPrivateLoan(object parameter)
        {
            return SelectedInfoPrivateLoan != null;
        }

        private void ShowPrivatePayments(object parameter)
        {
            HelperMethods.ShowView(new InfoPrivatePaymentsViewModel(SelectedInfoPrivateLoan.Id), new InfoPaymentsView());
        }

        private void AddSafeCash(object parameter)
        {
            HelperMethods.ShowView(new AddInfoSafeViewModel("Изъятие средств", "Изъять", true), new AddInfoSafeView());
            RefreshInfoSafesCash();
        }

        private void RemoveSafeCash(object parameter)
        {
            BC.RemoveInfoSafe(SelectedInfoSafeCash);
            RefreshInfoSafesCash();
        }

       
        private void InsertSafeCash(object parameter)
        {
            HelperMethods.ShowView(new AddInfoSafeViewModel("Внесение средств", "Внести", false), new AddInfoSafeView());
            RefreshInfoSafesCash();
        }

        private bool IsSelectedInfoSafeCash(object parameter)
        {
            return SelectedInfoSafeCash != null && SelectedInfoSafeCash.IsIncoming && SelectedInfoSafeCash.CashType == CashType.Наличка;
        }

        #endregion
    }
}

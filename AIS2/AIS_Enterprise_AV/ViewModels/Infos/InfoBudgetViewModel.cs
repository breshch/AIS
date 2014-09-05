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
using AIS_Enterprise_Data;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoBudgetViewModel : ViewModelGlobal
    {
        #region Base

        public InfoBudgetViewModel()
        {
            SelectedInfoLoanDate = DateTime.Now;
            SelectedInfoPrivateLoanDate = DateTime.Now;

            SelectedInfoCashFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectedInfoCashToDate = DateTime.Now;

            SelectedInfoCardFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectedInfoCardToDate = DateTime.Now;

            SelectedInfoLoanFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectedInfoLoanToDate = DateTime.Now;

            SelectedInfoPrivateLoanFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectedInfoPrivateLoanToDate = DateTime.Now;

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

            var totalSummCash = BC.GetCurrencyValue("TotalCash");
            var totalSummSafe = BC.GetCurrencyValue("TotalSafe");
            var totalSummCard = BC.GetCurrencyValue("TotalCard");
            var totalSummLoan = BC.GetCurrencyValue("TotalLoan");
            var totalSummPrivateLoan = BC.GetCurrencyValue("TotalPrivateLoan");

            string totalSummAllRUR = Converting.DoubleToCurrency(totalSummCash.RUR + totalSummCard.RUR + totalSummSafe.RUR + totalSummLoan.RUR, "RUR");
            string totalSummAllUSD = Converting.DoubleToCurrency(totalSummCash.USD + totalSummCard.USD + totalSummSafe.USD + totalSummLoan.USD, "USD");
            string totalSummAllEUR = Converting.DoubleToCurrency(totalSummCash.EUR + totalSummCard.EUR + totalSummSafe.EUR + totalSummLoan.EUR, "EUR");
            string totalSummAllBYR = Converting.DoubleToCurrency(totalSummCash.BYR + totalSummCard.BYR + totalSummSafe.BYR + totalSummLoan.BYR, "BYR");

            AllSafeData.Add(new SafeData { Name = "Сейф", SummRUR = totalSummSafe.GetRUR, SummUSD = totalSummSafe.GetUSD, SummEUR = totalSummSafe.GetEUR, SummBYR = totalSummSafe.GetBYR });
            AllSafeData.Add(new SafeData { Name = "Наличка", SummRUR = totalSummCash.GetRUR, SummUSD = totalSummCash.GetUSD, SummEUR = totalSummCash.GetEUR, SummBYR = totalSummCash.GetBYR });
            AllSafeData.Add(new SafeData { Name = "Карточка", SummRUR = totalSummCard.GetRUR, SummUSD = totalSummCard.GetUSD, SummEUR = totalSummCard.GetEUR, SummBYR = totalSummCard.GetBYR });
            AllSafeData.Add(new SafeData { Name = "Долги", SummRUR = totalSummLoan.GetRUR, SummUSD = totalSummLoan.GetUSD, SummEUR = totalSummLoan.GetEUR, SummBYR = totalSummLoan.GetBYR });
            AllSafeData.Add(new SafeData { Name = "Итого", SummRUR = totalSummAllRUR, SummUSD = totalSummAllUSD, SummEUR = totalSummAllEUR, SummBYR = totalSummAllBYR, Color = Brushes.MediumSlateBlue });
            AllSafeData.Add(new SafeData { Name = null });
            AllSafeData.Add(new SafeData { Name = "Частные долги", SummRUR = totalSummPrivateLoan.GetRUR, SummUSD = totalSummPrivateLoan.GetUSD, 
                SummEUR = totalSummPrivateLoan.GetEUR, SummBYR = totalSummPrivateLoan.GetBYR });
        }

        private void RefreshInfoLoans()
        {
            InfoLoans = new ObservableCollection<InfoLoan>(BC.GetInfoLoans(SelectedInfoLoanFromDate, SelectedInfoLoanToDate));
            RefreshAllSafeData();
        }

        private void RefreshInfoPrivateLoans()
        {
            InfoPrivateLoans = new ObservableCollection<InfoPrivateLoan>(BC.GetInfoPrivateLoans(SelectedInfoPrivateLoanFromDate, SelectedInfoPrivateLoanToDate));
            RefreshAllSafeData();
        }

        private void RefreshInfoSafesCash()
        {
            InfoSafesCash = new ObservableCollection<InfoSafe>(BC.GetInfoSafes(CashType.Наличка, SelectedInfoCashFromDate, SelectedInfoCashToDate));
            RefreshAllSafeData();
        }

        private void RefreshInfoSafesCard()
        {
            InfoSafesCard = new ObservableCollection<InfoSafe>(BC.GetInfoSafes(CashType.Карточка, SelectedInfoCardFromDate, SelectedInfoCardToDate));
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

        private DateTime _selectedInfoCashFromDate;
        public DateTime SelectedInfoCashFromDate
        {
            get
            {
                return _selectedInfoCashFromDate;
            }
            set
            {
                _selectedInfoCashFromDate = value;
                RaisePropertyChanged();

                RefreshInfoSafesCash();
            }
        }

        private DateTime _selectedInfoCashToDate;
        public DateTime SelectedInfoCashToDate
        {
            get
            {
                return _selectedInfoCashToDate;
            }
            set
            {
                _selectedInfoCashToDate = value;
                RaisePropertyChanged();

                RefreshInfoSafesCash();
            }
        }

        private DateTime _selectedInfoCardFromDate;
        public DateTime SelectedInfoCardFromDate
        {
            get
            {
                return _selectedInfoCardFromDate;
            }
            set
            {
                _selectedInfoCardFromDate = value;
                RaisePropertyChanged();

                RefreshInfoSafesCard();
            }
        }

        private DateTime _selectedInfoCardToDate;
        public DateTime SelectedInfoCardToDate
        {
            get
            {
                return _selectedInfoCardToDate;
            }
            set
            {
                _selectedInfoCardToDate = value;
                RaisePropertyChanged();

                RefreshInfoSafesCard();
            }
        }




        private DateTime _selectedInfoLoanFromDate;
        public DateTime SelectedInfoLoanFromDate
        {
            get
            {
                return _selectedInfoLoanFromDate;
            }
            set
            {
                _selectedInfoLoanFromDate = value;
                RaisePropertyChanged();

                RefreshInfoLoans();
            }
        }

        private DateTime _selectedInfoLoanToDate;
        public DateTime SelectedInfoLoanToDate
        {
            get
            {
                return _selectedInfoLoanToDate;
            }
            set
            {
                _selectedInfoLoanToDate = value;
                RaisePropertyChanged();

                RefreshInfoLoans();
            }
        }


        private DateTime _selectedInfoPrivateLoanFromDate;
        public DateTime SelectedInfoPrivateLoanFromDate
        {
            get
            {
                return _selectedInfoPrivateLoanFromDate;
            }
            set
            {
                _selectedInfoPrivateLoanFromDate = value;
                RaisePropertyChanged();

                RefreshInfoPrivateLoans();
            }
        }

        private DateTime _selectedInfoPrivateLoanToDate;
        public DateTime SelectedInfoPrivateLoanToDate
        {
            get
            {
                return _selectedInfoPrivateLoanToDate;
            }
            set
            {
                _selectedInfoPrivateLoanToDate = value;
                RaisePropertyChanged();

                RefreshInfoPrivateLoans();
            }
        }

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
            RefreshInfoSafesCash();
        }

        private void EditLoan(object parameter)
        {
            HelperMethods.ShowView(new EditInfoLoanViewModel(SelectedInfoLoan), new AddEditInfoLoanView());

            BC.RefreshContext();

            RefreshInfoLoans();
            RefreshInfoSafesCash();
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

            RefreshInfoSafesCash();
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

            RefreshAllSafeData();
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

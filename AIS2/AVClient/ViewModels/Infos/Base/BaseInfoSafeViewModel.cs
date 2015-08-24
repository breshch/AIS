using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos.Base
{
    public abstract class BaseInfoSafeViewModel : ViewModelGlobal
    {
        
        #region Base

        public BaseInfoSafeViewModel()
        {
            LoanTakers = new ObservableCollection<DirectoryLoanTaker>(BC.GetDirectoryLoanTakers());
            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(BC.GetDirectoryWorkersByMonth(DateTime.Now.Year, DateTime.Now.Month));
            Currencies = new ObservableCollection<Currency>(Enum.GetNames(typeof (Currency)).Select(c => (Currency)Enum.Parse(typeof(Currency), c)));
            SelectedCurrency = Currencies.First();

            CountPayments = 1;
            
            VisibilityMultiplyPayments = Visibility.Collapsed;
        }

        #endregion


        #region Properties

        public DateTime SelectedDate { get; set; }

        private bool? _isWorker;
        public bool? IsWorker 
        { 
            get
            {
                return _isWorker;
            }
            set
            {
                _isWorker = value;
                RaisePropertyChanged();

                if (_isWorker.Value)
                {
                    VisibilityLoanTaker = Visibility.Collapsed;
                    VisibilityWorker = Visibility.Visible;
                    VisibilityIsMultiplyPayments = Visibility.Visible;
                    SelectedLoanTaker = null;
                }
                else
                {
                    IsMultiplyPayments = false;
                    VisibilityLoanTaker = Visibility.Visible;
                    VisibilityWorker = Visibility.Collapsed;
                    VisibilityIsMultiplyPayments = Visibility.Collapsed;
                    SelectedWorker = null;
                }
            }
        }
        public ObservableCollection<DirectoryLoanTaker> LoanTakers { get; set; }
        public string SelectedLoanTaker { get; set; }
        public Visibility VisibilityLoanTaker { get; set; }

        public ObservableCollection<DirectoryWorker> DirectoryWorkers { get; set; }
        public Visibility VisibilityWorker { get; set; }
        public DirectoryWorker SelectedWorker { get; set; }

        public double SummLoan { get; set; }

        public ObservableCollection<Currency> Currencies { get; set; }
        public Currency SelectedCurrency { get; set; }

        public Visibility VisibilityIsMultiplyPayments { get; set; }

        private bool _isMultiplyPayments;
        public bool IsMultiplyPayments
        {
            get
            {
                return _isMultiplyPayments;
            }
            set
            {
                _isMultiplyPayments = value;
                RaisePropertyChanged();

                VisibilityMultiplyPayments = _isMultiplyPayments ? Visibility.Visible : Visibility.Collapsed;
                
                if (!_isMultiplyPayments)
                {
                    CountPayments = 1;
                }
                else
                {
                    CountPayments = 2;
                }
            }
        }

        public Visibility VisibilityMultiplyPayments { get; set; }
        public int CountPayments { get; set; }
        public string Description { get; set; }

        public string Title { get; set; }
        public string AddEditName { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddEditCommand { get; set; }

        #endregion

    }
}

using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public abstract class BaseInfoSafeViewModel : ViewModelGlobal
    {
        
        #region Base

        public BaseInfoSafeViewModel()
        {
            LoanTakers = new ObservableCollection<DirectoryLoanTaker>(BC.GetDirectoryLoanTakers());
            DirectoryWorkers = new ObservableCollection<DirectoryWorker>(BC.GetDirectoryWorkers(DateTime.Now.Year, DateTime.Now.Month));

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
                    CountPayments = 0;
                }
                else
                {
                    CountPayments = 1;
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

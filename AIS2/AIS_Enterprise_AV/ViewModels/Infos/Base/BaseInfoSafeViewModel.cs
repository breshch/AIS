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

            VisibilityMultiplyPayments = Visibility.Collapsed;
        }

        #endregion

        #region Properties

        public DateTime SelectedDate { get; set; }
        public ObservableCollection<DirectoryLoanTaker> LoanTakers { get; set; }
        public string SelectedLoanTaker { get; set; }
        public double SummLoan { get; set; }

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

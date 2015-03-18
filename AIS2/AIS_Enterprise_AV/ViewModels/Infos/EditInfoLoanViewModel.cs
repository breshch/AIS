using System.Linq;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class EditInfoLoanViewModel : BaseInfoSafeViewModel
    {
        #region Base

        private int _infoLoanId;

        public EditInfoLoanViewModel(InfoLoan infoLoan) : base()
        {
            Title = "Редактирование";
            AddEditName = "Изменить";

            _infoLoanId = infoLoan.Id;

            SelectedDate = infoLoan.DateLoan;
            IsWorker = infoLoan.DirectoryWorker == null ? false : true;

            if (IsWorker.Value)
            {
                SelectedWorker = DirectoryWorkers.First(w => w.Id == infoLoan.DirectoryWorkerId);
            }
            else
            {
                SelectedLoanTaker = infoLoan.DirectoryLoanTaker.Name;
            }
            
            SummLoan = infoLoan.Summ;

            if (infoLoan.CountPayments != 1)
            {
                IsMultiplyPayments = true;
                VisibilityMultiplyPayments = Visibility.Visible;
            }

            CountPayments = infoLoan.CountPayments;

            Description = infoLoan.Description;

            AddEditCommand = new RelayCommand(Edit);
        }

        #endregion


        #region Commands

        private void Edit(object parameter)
        {
            BC.EditInfoLoan(_infoLoanId, SelectedDate, SelectedLoanTaker, SelectedWorker, SummLoan, SelectedCurrency, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}

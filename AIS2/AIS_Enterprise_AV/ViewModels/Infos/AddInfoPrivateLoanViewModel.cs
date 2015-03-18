using System;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddInfoPrivateLoanViewModel : BaseInfoSafeViewModel
    {
        #region Base

        public AddInfoPrivateLoanViewModel() : base()
        {
            Title = "Добавление";
            AddEditName = "Добавить";

            SelectedDate = DateTime.Now;

            IsWorker = true;

            AddEditCommand = new RelayCommand(Add);

        }

        #endregion


        #region Commands

        private void Add(object parameter)
        {
            BC.AddInfoPrivateLoan(SelectedDate, SelectedLoanTaker, SelectedWorker, SummLoan, SelectedCurrency, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}


 
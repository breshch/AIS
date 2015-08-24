using System;
using System.Windows;
using AVClient.Helpers;
using AVClient.ViewModels.Infos.Base;

namespace AVClient.ViewModels.Infos
{
    public class AddInfoPrivateLoanViewModel : BaseInfoSafeViewModel
    {
        #region Base

        public AddInfoPrivateLoanViewModel()
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


 
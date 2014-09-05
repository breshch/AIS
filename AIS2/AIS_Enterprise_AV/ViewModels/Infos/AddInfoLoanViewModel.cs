using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddInfoLoanViewModel : BaseInfoSafeViewModel
    {
        #region Base

        public AddInfoLoanViewModel() : base()
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
            BC.AddInfoLoan(SelectedDate, SelectedLoanTaker, SelectedWorker, SummLoan, SelectedCurrency, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}


 
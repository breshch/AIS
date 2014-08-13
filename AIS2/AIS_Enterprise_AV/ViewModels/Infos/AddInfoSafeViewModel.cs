using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddInfoSafeViewModel : BaseInfoSafeViewModel
    {
        #region Base

        public AddInfoSafeViewModel() : base()
        {
            Title = "Добавление";
            AddEditName = "Добавить";

            SelectedDate = DateTime.Now;

            AddEditCommand = new RelayCommand(Add);
        }

        #endregion


        #region Commands

        private void Add(object parameter)
        {
            BC.AddInfoSafe(SelectedDate, SelectedLoanTaker, SummLoan, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}

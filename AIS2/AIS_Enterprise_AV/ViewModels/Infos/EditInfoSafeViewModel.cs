using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class EditInfoSafeViewModel : BaseInfoSafeViewModel
    {
        #region Base

        private int _infoSafeId;

        public EditInfoSafeViewModel(InfoSafe infoSafe) : base()
        {
            Title = "Редактирование";
            AddEditName = "Изменить";

            _infoSafeId = infoSafe.Id;

            SelectedDate = infoSafe.DateLoan;
            IsWorker = infoSafe.DirectoryWorker == null ? false : true;

            if (IsWorker.Value)
            {
                SelectedWorker = DirectoryWorkers.First(w => w.Id == infoSafe.DirectoryWorkerId);
            }
            else
            {
                SelectedLoanTaker = infoSafe.DirectoryLoanTaker.Name;
            }
            
            SummLoan = infoSafe.Summ;

            if (infoSafe.CountPayments != null)
            {
                IsMultiplyPayments = true;
                VisibilityMultiplyPayments = System.Windows.Visibility.Visible;
                CountPayments = infoSafe.CountPayments.Value;
            }

            Description = infoSafe.Description;

            AddEditCommand = new RelayCommand(Edit);
        }

        #endregion


        #region Commands

        private void Edit(object parameter)
        {
            BC.EditInfoSafe(_infoSafeId, SelectedDate, SelectedLoanTaker, SelectedWorker, SummLoan, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}

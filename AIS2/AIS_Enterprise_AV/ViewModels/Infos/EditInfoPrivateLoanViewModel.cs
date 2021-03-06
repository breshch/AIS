﻿using System.Linq;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class EditInfoPrivateLoanViewModel : BaseInfoSafeViewModel
    {
        #region Base

        private int _infoLoanId;

        public EditInfoPrivateLoanViewModel(InfoPrivateLoan infoPrivateLoan) : base()
        {
            Title = "Редактирование";
            AddEditName = "Изменить";

            _infoLoanId = infoPrivateLoan.Id;

            SelectedDate = infoPrivateLoan.DateLoan;
            IsWorker = infoPrivateLoan.DirectoryWorker == null ? false : true;

            if (IsWorker.Value)
            {
                SelectedWorker = DirectoryWorkers.First(w => w.Id == infoPrivateLoan.DirectoryWorkerId);
            }
            else
            {
                SelectedLoanTaker = infoPrivateLoan.DirectoryLoanTaker.Name;
            }
            
            SummLoan = infoPrivateLoan.Summ;

            if (infoPrivateLoan.CountPayments != 1)
            {
                IsMultiplyPayments = true;
                VisibilityMultiplyPayments = Visibility.Visible;
                
            }

            CountPayments = infoPrivateLoan.CountPayments;

            Description = infoPrivateLoan.Description;

            AddEditCommand = new RelayCommand(Edit);
        }

        #endregion


        #region Commands

        private void Edit(object parameter)
        {
            BC.EditInfoPrivateLoan(_infoLoanId, SelectedDate, SelectedLoanTaker, SelectedWorker, SummLoan, SelectedCurrency, CountPayments, Description);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}

﻿using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddInfoPrivatePaymentViewModel : ViewModelGlobal
    {
        #region Base
        private int _infoPrivateLoanId;
        public AddInfoPrivatePaymentViewModel(int infoPrivateLoanId)
        {
            SelectedDateLoanPayment = DateTime.Now;

            PayCommand = new RelayCommand(Pay);
            _infoPrivateLoanId = infoPrivateLoanId;
        }
        #endregion

        #region Properties

        public DateTime SelectedDateLoanPayment { get; set; }
        public double SummLoanPayment { get; set; }

        #endregion

        #region Commands
        public RelayCommand PayCommand { get; set; }

        private void Pay(object parameter)
        {
            BC.AddInfoPrivatePayment(_infoPrivateLoanId, SelectedDateLoanPayment, SummLoanPayment);
          
            var window = parameter as Window;
            window.Close();
        }
        #endregion
    }
}
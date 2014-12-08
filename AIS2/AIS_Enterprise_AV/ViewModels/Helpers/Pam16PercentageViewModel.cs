using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class Pam16PercentageViewModel : ViewModelGlobal
    {
        #region Base

        public Pam16PercentageViewModel()
        {
            Pam16Percentage = BC.GetParameterValue<double>(ParameterType.Pam16Percentage);

            EditCommand = new RelayCommand(Edit);
        }

        #endregion

        #region Properties

        public double Pam16Percentage { get; set; }

        #endregion

        #region Commands

        public RelayCommand EditCommand { get; set; }

        private void Edit(object parameter)
        {
            BC.EditParameter(ParameterType.Pam16Percentage, Pam16Percentage.ToString());
            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

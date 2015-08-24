using System;
using AVClient.Helpers;

namespace AVClient.ViewModels.Helpers
{
    public class Pam16PercentageViewModel : ViewModelGlobal
    {
        #region Base

        public Pam16PercentageViewModel()
        {
	        Date = DateTime.Now;
	        Pam16Percentage = BC.GetPam16Percentage(Date);

            EditCommand = new RelayCommand(Edit);
        }

        #endregion

        #region Properties

        public double Pam16Percentage { get; set; }
		public DateTime Date { get; set; }

        #endregion

        #region Commands

        public RelayCommand EditCommand { get; set; }

        private void Edit(object parameter)
        {
	        BC.SavePam16Percentage(new DateTime(Date.Year,Date.Month,1),Pam16Percentage);
            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

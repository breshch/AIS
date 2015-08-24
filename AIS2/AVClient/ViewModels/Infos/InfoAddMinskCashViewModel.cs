using System;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos
{
	public class InfoAddMinskCashViewModel : ViewModelGlobal
	{
		#region Base

		public InfoAddMinskCashViewModel()
		{
			Date = DateTime.Now;	
			SaveCommand = new RelayCommand(Save);
		}

		#endregion

		#region Properties

		public DateTime Date { get; set; }
		public string MinskSumm { get; set; }

		#endregion

		#region Commands

		public RelayCommand SaveCommand { get; set; }

		private void Save(object parameter)
		{
			MinskSumm = MinskSumm.Replace(".", ",").Replace(" ","");
			double summ = double.Parse(MinskSumm);

			BC.SaveTotalSafeAndMinskCashes(Date, summ);

			HelperMethods.CloseWindow(parameter);
		}

		#endregion


	}
}

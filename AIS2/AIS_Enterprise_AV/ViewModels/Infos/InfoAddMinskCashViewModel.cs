using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
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
		public double MinskSumm { get; set; }

		#endregion

		#region Commands

		public RelayCommand SaveCommand { get; set; }

		private void Save(object parameter)
		{
			BC.SaveTotalSafeAndMinskCashes(Date, MinskSumm);

			HelperMethods.CloseWindow(parameter);
		}

		#endregion


	}
}

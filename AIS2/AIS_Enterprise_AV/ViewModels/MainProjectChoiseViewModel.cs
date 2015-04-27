using System;
using System.IO;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_AV.WareHouse;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainProjectChoiseViewModel : ViewModelGlobal
    {
        #region Base

        public MainProjectChoiseViewModel()
        {
            MonthTimeSheetCommand = new RelayCommand(MonthTimeSheet);
            RemainsCommand = new RelayCommand(Remains);
            ProcessingBookKeepingCommand = new RelayCommand(ProcessingBookKeeping);
            RemainsLoanCommand = new RelayCommand(RemainsLoan);
			WarehouseCommand = new RelayCommand(Warehouse);
            
            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_MonthTimeSheetEnable.ToString()))
            {
                IsEnabledMonthTimeSheet = true;
            }

            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_DbFenoxEnable.ToString()))
            {
                IsEnabledDbFenox = true;
            }

            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_DbFenoxEnable.ToString()))
            {
                IsEnabledDbFenox = true;
            }

			BC.SetRemainsToFirstDateInMonth();
			
			//initialize dbminskcash
			
			//using (var sr = new StreamReader(@"C:\Users\Alexey\Desktop\1.csv"))
			//{
			//	while (!sr.EndOfStream)
			//	{
			//		var line = sr.ReadLine();
			//		var date = DateTime.Parse("01." + line.Substring(0, 7).Replace(",","."));
			//		var summ = double.Parse(line.Substring(8).Replace(" ","").Replace(".",","));
			//		BC.SaveTotalSafeAndMinskCashes(date,summ);
			//	}
			//}		
        }

        #endregion


        #region Properties

        public bool IsEnabledMonthTimeSheet { get; set; }

        public bool IsEnabledDbFenox { get; set; }
        public bool IsEnabledProcessingBookKeeping { get; set; }
        #endregion


        #region Commands

        public RelayCommand MonthTimeSheetCommand { get; set; }
        public RelayCommand RemainsCommand { get; set; }
        public RelayCommand ProcessingBookKeepingCommand { get; set; }
        public RelayCommand RemainsLoanCommand { get; set; }
	    public RelayCommand WarehouseCommand { get; set; }


        private void MonthTimeSheet(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();

            HelperMethods.CloseWindow(parameter);
        }

        private void Remains(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new InfoRemainsViewModel(), new InfoRemainsView());

            HelperMethods.CloseWindow(parameter);
        }

        private void ProcessingBookKeeping(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new PercentageProcessingBookKeepingViewModel(), new PercentageProcessingBookKeepingView());

            HelperMethods.CloseWindow(parameter);
        }

        private void RemainsLoan(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new PickDateReportViewModel(), new PickDateReportView());

            HelperMethods.CloseWindow(parameter);
        }

		private void Warehouse(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Hidden;

			var scheme = new Scheme();
			scheme.ShowDialog();
			
			HelperMethods.CloseWindow(parameter);
		}

        #endregion
    }
}

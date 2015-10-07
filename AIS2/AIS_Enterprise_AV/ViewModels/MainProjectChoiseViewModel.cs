using System;
using System.IO;
using System.Windows;
using AIS_Enterprise_AV.Auth;
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
			CostsCommand = new RelayCommand(Costs);
			ReportsCommand = new RelayCommand(Reports);
			AdminCommand = new RelayCommand(Admin);

			MonthTimeSheetVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_MonthTimeSheetVisibility)? Visibility.Visible : Visibility.Collapsed;
			DbFenoxVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_DbFenoxVisibility) ? Visibility.Visible : Visibility.Collapsed;
			CostsVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_CostsVisibility) ? Visibility.Visible : Visibility.Collapsed;
			ProcessingBookKeepingVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_ProcessingBookKeepingVisibility) ? Visibility.Visible : Visibility.Collapsed;
			RemainsLoanVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_RemainsLoanVisibility) ? Visibility.Visible : Visibility.Collapsed;
			ReportsVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_ReportsVisibility) ? Visibility.Visible : Visibility.Collapsed;
			AdminVisibility = Privileges.HasAccess(UserPrivileges.MultyProject_AdminVisibility) ? Visibility.Visible : Visibility.Collapsed;

			BC.SetRemainsToFirstDateInMonth();
        }

        #endregion


        #region Properties

		public Visibility MonthTimeSheetVisibility { get; set; }
		public Visibility DbFenoxVisibility { get; set; }
		public Visibility CostsVisibility { get; set; }
		public Visibility ProcessingBookKeepingVisibility { get; set; }
		public Visibility RemainsLoanVisibility { get; set; }
		public Visibility ReportsVisibility { get; set; }
		public Visibility AdminVisibility { get; set; }

        #endregion


        #region Commands

        public RelayCommand MonthTimeSheetCommand { get; set; }
        public RelayCommand RemainsCommand { get; set; }
		public RelayCommand CostsCommand { get; set; }
        public RelayCommand ProcessingBookKeepingCommand { get; set; }
        public RelayCommand RemainsLoanCommand { get; set; }
	    public RelayCommand WarehouseCommand { get; set; }
		public RelayCommand ReportsCommand { get; set; }
		public RelayCommand AdminCommand { get; set; }


        private void MonthTimeSheet(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            var monthTimeSheetView = new MonthTimeSheetView();
	        monthTimeSheetView.Owner = window;
            monthTimeSheetView.ShowDialog();

			window.Visibility = Visibility.Visible;
        }

        private void Remains(object parameter)
        {
            var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

            HelperMethods.ShowView(new InfoRemainsViewModel(), new InfoRemainsView());

			window.Visibility = Visibility.Visible;
        }

        private void ProcessingBookKeeping(object parameter)
        {
            var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

            HelperMethods.ShowView(new PercentageProcessingBookKeepingViewModel(), new PercentageProcessingBookKeepingView());

			window.Visibility = Visibility.Visible;
        }

        private void RemainsLoan(object parameter)
        {
            var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

            HelperMethods.ShowView(new PickDateReportViewModel(), new PickDateReportView());

			window.Visibility = Visibility.Visible;
        }

		private void Warehouse(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var scheme = new Scheme();
			scheme.ShowDialog();

			window.Visibility = Visibility.Visible;
		}
		private void Costs(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var costsView = new ProjectCostsView();
			costsView.ShowDialog();

			window.Visibility = Visibility.Visible;
		}

		private void Reports(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Hidden;

			var reports = new ProjectReportsView();
			reports.Owner = window;
			reports.ShowDialog();

			window.Visibility = Visibility.Visible;
		}

		private void Admin(object parameter)
		{
			var window = parameter as Window;
			window.Visibility = Visibility.Collapsed;

			var admin = new ProjectAdminView();
			admin.ShowDialog();

			window.Visibility = Visibility.Visible;
		}
        #endregion
    }
}

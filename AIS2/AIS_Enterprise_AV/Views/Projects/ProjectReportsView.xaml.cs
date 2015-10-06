﻿using System;
using System.Linq;
using System.Windows;
using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Views
{
    /// <summary>
    /// Логика взаимодействия для MainProjectChoiseView.xaml
    /// </summary>
	public partial class ProjectReportsView : Window
    {
        public ProjectReportsView()
        {
            InitializeComponent();
        }

	    private void ReportSalaryPrint_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new MonthReportViewModel(
			  "Зарплата",
			  (BC, SelectedYear, SelectedMonth) =>
			  {
				  WorkerSalaryReports.ComplitedReportSalaryWorkers(SelectedYear, SelectedMonth);
			  },
			  (BC) => BC.GetYears().OrderBy(y => y).ToList(),
			  (BC, year) => BC.GetMonthes(year).OrderBy(m => m).ToList()
			  ), new MonthReportView());
	    }

	    private void ReportSalaryMinsk_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new MonthReportViewModel(
			  "Зарплата",
			  (BC, SelectedYear, SelectedMonth) =>
			  {
				  WorkerSalaryReports.ComplitedReportSalaryOvertimeTransportMinsk(SelectedYear, SelectedMonth);
			  },
			  (BC) => BC.GetYears().OrderBy(y => y).ToList(),
			  (BC, year) => BC.GetMonthes(year).OrderBy(m => m).ToList()
			  ), new MonthReportView());
	    }

	    private void ReportPam16Percentage_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new Pam16PercentageViewModel(), new Pam16PercentageView());
	    }

	    private void ReportCosts_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new MonthReportViewModel(
			   "Затраты",
			   (BC, SelectedYear, SelectedMonth) =>
			   {
				   var directoryRCs = BC.GetDirectoryRCsMonthIncoming(SelectedYear, SelectedMonth).ToList();

				   foreach (var rc in directoryRCs)
				   {
					   CashReports.IncomingRC(rc, BC, SelectedYear, SelectedMonth);
				   }

				   CashReports.Incoming26(BC, SelectedYear, SelectedMonth);

				   CashReports.Expense26(BC, SelectedYear, SelectedMonth);

				   CashReports.ExpenseRCs(BC, SelectedYear, SelectedMonth);

				   CashReports.ExpensePAM16(BC, SelectedYear, SelectedMonth);
			   },
			   (BC) => BC.GetInfoCostYears().OrderBy(y => y).ToList(),
			   (BC, year) => BC.GetInfoCostMonthes(year).OrderBy(m => m).ToList()
			   ), new MonthReportView());
	    }

	    private void ReportCash_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new FromToDatesReportViewModel(WorkerSalaryReports.ComplitedMonthCashReportMinsk), new FromToDatesReportView());
	    }

	    private void ReportCars_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new FromToDatesReportViewModel(CarsReports.Cars), new FromToDatesReportView());
	    }

	    private void ReportProfit_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new MonthReportViewModel(
				 "Профит",
				 (BC, SelectedYear, SelectedMonth) =>
				 {
					 HelperMethods.ShowView(new ProfitViewModel(SelectedYear, SelectedMonth), new ProfitView());
				 },
				 (BC) => BC.GetYears().OrderBy(y => y).ToList(),
				 (BC, year) => BC.GetMonthes(year).OrderBy(m => m).ToList()
				 ), new MonthReportView());
	    }

	    private void ReportDiffSumToMinsk_OnClick(object sender, RoutedEventArgs e)
	    {
			HelperMethods.ShowView(new FromToDatesReportViewModel(SafeReports.SafeToMinsk), new FromToDatesReportView());
	    }
    }
}

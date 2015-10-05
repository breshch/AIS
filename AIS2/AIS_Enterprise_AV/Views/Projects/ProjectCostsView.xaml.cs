using System;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Views
{
    /// <summary>
    /// Логика взаимодействия для MainProjectChoiseView.xaml
    /// </summary>
	public partial class ProjectCostsView : Window
    {
        public ProjectCostsView()
        {
            InitializeComponent();

	        InitializePrivileges();
        }

	    private void InitializePrivileges()
	    {
		    
	    }

	    private void DayCosts_OnClick(object sender, RoutedEventArgs e)
	    {
		     var costView = new DayCostsView(DateTime.Now);
            costView.ShowDialog();
	    }

	    private void MonthCosts_OnClick(object sender, RoutedEventArgs e)
	    {
		    HelperMethods.ShowView(new MonthCostsViewModel(), new MonthCostsView());
	    }

	    private void DefaultCosts_OnClick(object sender, RoutedEventArgs e)
	    {
		    HelperMethods.ShowView(new DefaultCostsViewModel(), new DefaultCostsView());
	    }

	    private void Safe_OnClick(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new InfoBudgetViewModel(), new InfoBudgetView());
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIS_Enterprise_AV.Auth;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Views.Infos
{
    public partial class DayCostsView : Window
    {
        private BusinessContext _bc = new BusinessContext();
		private bool _isNotTransportOnly;
        

        public DayCostsView(DateTime date)
        {
            InitializeComponent();

			_isNotTransportOnly = Privileges.HasAccess(UserPrivileges.CostsVisibility_IsNotTransportOnly);

			FillDataGrid(date);
        }
       

        private void FillDataGrid(DateTime date)
        {
            DataGridCurrentDateCosts.ItemsSource = null;
            if (_isNotTransportOnly)
            {
                DataGridCurrentDateCosts.ItemsSource = _bc.GetInfoCosts(date).ToList();
            }
            else
            {
                DataGridCurrentDateCosts.ItemsSource = _bc.GetInfoCostsTransportAndNoAllAndExpenseOnly(date).ToList();
            }

	        DatePickerDate.SelectedDate = date;
	        TextBlockDate.Text = date.ToShortDateString();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _bc.Dispose();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
	        var form = new AddEditDayCost(DatePickerDate.SelectedDate.Value);
	        form.ShowDialog();

	        if (form.Date != null)
	        {
		        FillDataGrid(form.Date.Value);
	        }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            var infoCost = DataGridCurrentDateCosts.SelectedItem as InfoCost;
	        var date = infoCost.Date;
            _bc.RemoveInfoCost(infoCost);

			FillDataGrid(date);
        }

        private void DataGridCurrentDateCosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonRemove.IsEnabled = DataGridCurrentDateCosts.SelectedIndex != -1 ? true : false;

	        var infoCost = DataGridCurrentDateCosts.SelectedItem as InfoCost;
	        if (infoCost != null && infoCost.DirectoryCostItem.Name != "Транспорт (5031)")
	        {
		        ButtonEdit.IsEnabled = true;
	        }
	        else
	        {
				ButtonEdit.IsEnabled = false;
	        }
        }

	    private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
	    {
			var infoCost = DataGridCurrentDateCosts.SelectedItem as InfoCost;

			var form = new AddEditDayCost(infoCost.Date, infoCost.Id);
			form.ShowDialog();

		    if (form.Date != null)
		    {
			    FillDataGrid(form.Date.Value);
		    }
	    }

	    private void DatePickerDate_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
	    {
		    FillDataGrid(DatePickerDate.SelectedDate.Value);
	    }
    }
}

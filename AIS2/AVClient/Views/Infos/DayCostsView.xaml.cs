using System;
using System.Windows;
using System.Windows.Controls;
using AVClient.AVServiceReference;
using Shared.Enums;

namespace AVClient.Views.Infos
{
    public partial class DayCostsView : Window
    {
        private AVBusinessLayerClient _bc = ServerConnector.GetInstanse;
		private bool _isNotTransportOnly;
        

        public DayCostsView(DateTime date)
        {
            InitializeComponent();

			_isNotTransportOnly = HelperMethods.IsPrivilege(UserPrivileges.CostsVisibility_IsNotTransportOnly);

			FillDataGrid(date);
        }
       

        private void FillDataGrid(DateTime date)
        {
            DataGridCurrentDateCosts.ItemsSource = null;
            if (_isNotTransportOnly)
            {
	            DataGridCurrentDateCosts.ItemsSource = _bc.GetInfoCostsByDate(date);
            }
            else
            {
                DataGridCurrentDateCosts.ItemsSource = _bc.GetInfoCostsTransportAndNoAllAndExpenseOnlyByDate(date);
            }

	        DatePickerDate.SelectedDate = date;
	        TextBlockDate.Text = date.ToShortDateString();
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

using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Diagnostics;
using AIS_Enterprise_Global.Helpers.Temps;

namespace AIS_Enterprise_AV.Costs.Views
{
    public partial class CostsView : Window
    {
        private BusinessContext _bc = new BusinessContext();
        public CostsView()
        {
            InitializeComponent();
            InitializeData();

        }

        private void DatePickerDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDataGrid(DatePickerDate.SelectedDate.Value);
        }

        private void FillDataGrid(DateTime date)
        {
            DataGridCurrentDateCosts.ItemsSource = null;
            DataGridCurrentDateCosts.ItemsSource = _bc.GetInfoCosts(date).ToList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _bc.Dispose();
        }

        private void InitializeData()
        {
            DatePickerDate.SelectedDate = DateTime.Now;
            ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().ToList();
            ComboBoxRCs_1.ItemsSource = _bc.GetDirectoryRCs().ToList();
            ComboBoxNotes_1.ItemsSource = _bc.GetDirectoryNotes().ToList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var transports = new List<Transport>();
            for (int i = 1; i <= GridCosts.RowDefinitions.Count; i++)
            {
                string note = (GridCosts.FindName("ComboBoxNotes_" + i) as ComboBox).Text;

                DirectoryNote directoryNote = null;

                if (!_bc.IsDirectoryNote(note))
                {
                    directoryNote = _bc.AddDirectoryNote(note);
                }
                else
                {
                    directoryNote = _bc.GetDirectoryNote(note);
                }

                var transport = new Transport();
                //{
                transport.DirectoryRC = (GridCosts.FindName("ComboBoxRCs_" + i) as ComboBox).SelectedItem as DirectoryRC;
                   transport.DirectoryNote = directoryNote;
                   transport.Weight = double.Parse((GridCosts.FindName("TextBoxWeight_" + i) as TextBox).Text);
                //};

                transports.Add(transport);
            }

            _bc.AddInfoCosts(DatePickerDate.SelectedDate.Value, ComboBoxCostItems.SelectedItem as DirectoryCostItem, RadioButtonIncoming.IsChecked.Value, 
                double.Parse(TextBoxSumm.Text), transports);

            FillDataGrid(DatePickerDate.SelectedDate.Value);

            ComboBoxNotes_1.ItemsSource = null;
            ComboBoxNotes_1.ItemsSource = _bc.GetDirectoryNotes().ToList();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            var infoCost = DataGridCurrentDateCosts.SelectedItem as InfoCost;

            _bc.RemoveInfoCost(infoCost);

            FillDataGrid(DatePickerDate.SelectedDate.Value);
        }

        private void ComboBoxCostItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var costItem = ComboBoxCostItems.SelectedItem as DirectoryCostItem;

            if (costItem != null)
	        {
                if (costItem.Name == "Транспорт (5031)")
                {
                    StackPanelWeight.Visibility = System.Windows.Visibility.Visible;
                    ButtonAddNewCargo.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    StackPanelWeight.Visibility = System.Windows.Visibility.Collapsed;
                    ButtonAddNewCargo.Visibility = System.Windows.Visibility.Collapsed;
                    TextBoxWeight_1.Text = null;
                }   
	        }
        }

        private void AddNewCargo_Click(object sender, RoutedEventArgs e)
        {
            var rowDefinition = new RowDefinition();
            GridCosts.RowDefinitions.Add(rowDefinition);

            NameScope nameScope = new NameScope();
            NameScope.SetNameScope(this, nameScope);
            

            var comboBox = new ComboBox();
            nameScope.RegisterName("ComboBoxRCs_" + GridCosts.RowDefinitions.Count, comboBox);
            comboBox.DisplayMemberPath = "FullName";
            comboBox.IsEditable = true;
            comboBox.Margin = new Thickness(10, 10, 0, 0);
            comboBox.SetValue(Grid.ColumnProperty, 1);
            comboBox.SetValue(Grid.RowProperty, GridCosts.RowDefinitions.Count - 1);
            comboBox.ItemsSource = _bc.GetDirectoryRCs().ToList();

            GridCosts.Children.Add(comboBox);

            comboBox = new ComboBox();
            nameScope.RegisterName("ComboBoxNotes_" + GridCosts.RowDefinitions.Count, comboBox);
            comboBox.DisplayMemberPath = "Description";
            comboBox.IsEditable = true;
            comboBox.Margin = new Thickness(20, 10, 10, 0);
            comboBox.SetValue(Grid.ColumnProperty, 2);
            comboBox.SetValue(Grid.RowProperty, GridCosts.RowDefinitions.Count - 1);
            comboBox.ItemsSource = _bc.GetDirectoryNotes().ToList();

            GridCosts.Children.Add(comboBox);

            var textBox = new TextBox();
            nameScope.RegisterName("TextBoxWeight_" + GridCosts.RowDefinitions.Count, textBox);
            textBox.Height = 22;
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            textBox.Margin = new Thickness(0, 10, 10, 0);
            textBox.SetValue(Grid.ColumnProperty, 3);
            textBox.SetValue(Grid.RowProperty, GridCosts.RowDefinitions.Count - 1);

            GridCosts.Children.Add(textBox);

            var button = new Button();
            nameScope.RegisterName("ButtonRemoveCargo_" + GridCosts.RowDefinitions.Count, button);
            button.Content = "Удалить";
            button.Margin = new Thickness(10, 10, 0, 0);
            button.SetValue(Grid.ColumnProperty, 4);
            button.SetValue(Grid.RowProperty, GridCosts.RowDefinitions.Count - 1);

            GridCosts.Children.Add(button);
        } 
    }
}

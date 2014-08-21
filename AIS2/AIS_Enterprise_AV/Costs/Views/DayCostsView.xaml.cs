using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
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
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_AV.Costs.Views
{
    public partial class DayCostsView : Window
    {
        private BusinessContext _bc = new BusinessContext();
        private bool _isNotTransportOnly;
        private List<DirectoryNote> _notes;

        public DayCostsView(DateTime date)
        {
            InitializeComponent();

            _isNotTransportOnly = HelperMethods.IsPrivilege(_bc, UserPrivileges.CostsVisibility_IsNotTransportOnly);

            InitializeData();

            DatePickerDate.SelectedDate = date;

            ComboBoxRCs_1.Resources.Add(SystemColors.HighlightBrushKey, Brushes.Red);

            InitializeValidation(GridCosts.Children);
            ChangeButtonAddVisibility(null);

            if (!_isNotTransportOnly)
            {
                RadioButtonExpense.IsChecked = true;
                RadioButtonExpense.IsEnabled = false;
                RadioButtonIncoming.IsEnabled = false;

                ComboBoxCostItems.SelectedItem = _bc.GetDirectoryCostItem("Транспорт (5031)");

                ComboBoxValidation(ComboBoxCostItems);
                ComboBoxValidation(ComboBoxTransportCompanies);
            }
        }

        private void OrderNotes()
        {
            var noteLetters = _notes.Where(n => char.IsLetter(n.Description[0])).OrderBy(n => n.Description).ToList();
            var noteDigits = _notes.Where(n => char.IsDigit(n.Description[0])).OrderBy(n => n.Description).ToList();

            _notes.Clear();

            foreach (var note in noteLetters)
            {
                _notes.Add(note);
            }

            foreach (var note in noteDigits)
            {
                _notes.Add(note);
            }
        }


        private void InitializeValidation(UIElementCollection children)
        {
            foreach (var item in children)
            {
                var textBox = item as TextBox;
                if (textBox != null)
                {
                    TextBoxValidation(textBox);
                }

                var comboBox = item as ComboBox;
                if (comboBox != null)
                {
                    ComboBoxValidation(comboBox);
                }

                var grid = item as Grid;
                if (grid != null)
                {
                    InitializeValidation(grid.Children);
                }

                var stackPanel = item as StackPanel;
                if (stackPanel != null)
                {
                    InitializeValidation(stackPanel.Children);
                }
            }
        }

        private void DatePickerDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDataGrid(DatePickerDate.SelectedDate.Value);
            TextBlockDate.Text = DatePickerDate.SelectedDate.Value.ToShortDateString();
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

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _bc.Dispose();
        }

        private void InitializeData()
        {
            DatePickerDate.SelectedDate = DateTime.Now;

            if (!_isNotTransportOnly)
            {
                ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().Where(c => c.Name == "Транспорт (5031)" || c.Name == "Приход").ToList();
            }
            else
            {
                ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().ToList();
            }

            ComboBoxRCs_1.ItemsSource = _bc.GetDirectoryRCs().ToList();

            _notes = _bc.GetDirectoryNotes().ToList();
            OrderNotes();
            ComboBoxNotes_1.ItemsSource = _notes;
            ComboBoxTransportCompanies.ItemsSource = _bc.GetDirectoryTransportCompanies().ToList();
            RadioButtonExpense.IsChecked = true;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var transports = new List<Transport>();
            for (int i = 1; i <= GridTransports.RowDefinitions.Count; i++)
            {
                string note = (WindowCosts.FindName("ComboBoxNotes_" + i) as ComboBox).Text;

                DirectoryNote directoryNote = null;

                if (!_bc.IsDirectoryNote(note))
                {
                    directoryNote = _bc.AddDirectoryNote(note);
                }
                else
                {
                    directoryNote = _bc.GetDirectoryNote(note);
                }

                string weightText = (WindowCosts.FindName("TextBoxWeight_" + i) as TextBox).Text;

                var transport = new Transport();

                transport.DirectoryRC = (WindowCosts.FindName("ComboBoxRCs_" + i) as ComboBox).SelectedItem as DirectoryRC;
                transport.DirectoryNote = directoryNote;
                transport.Weight = string.IsNullOrWhiteSpace(weightText) ? 0 : double.Parse(weightText);

                transports.Add(transport);
            }

            _bc.AddInfoCosts(DatePickerDate.SelectedDate.Value, ComboBoxCostItems.SelectedItem as DirectoryCostItem, RadioButtonIncoming.IsChecked.Value,
                ComboBoxTransportCompanies.SelectedItem as DirectoryTransportCompany, double.Parse(TextBoxSumm.Text), transports);

            FillDataGrid(DatePickerDate.SelectedDate.Value);

            ComboBoxNotes_1.ItemsSource = null;
            ComboBoxNotes_1.ItemsSource = _bc.GetDirectoryNotes().ToList();

            ClearForm();
            InitializeValidation(GridCosts.Children);
        }

        private void ClearForm()
        {
            RadioButtonExpense.IsChecked = true;
            TextBoxSumm.Text = null;

            ComboBoxCostItems.SelectedItem = null;
            ComboBoxRCs_1.SelectedItem = null;
            ComboBoxNotes_1.SelectedItem = null;
            ComboBoxNotes_1.Text = null;
            ComboBoxTransportCompanies.SelectedItem = null;
            TextBoxWeight_1.Text = null;

            ComboBoxCostItems.IsEnabled = true;

            StackPanelTransportCompanies.Visibility = System.Windows.Visibility.Collapsed;
            StackPanelWeight.Visibility = System.Windows.Visibility.Collapsed;
            ButtonAddNewCargo.Visibility = System.Windows.Visibility.Collapsed;

            BorderCostItems.BorderThickness = new Thickness(2);
            BorderRCs_1.BorderThickness = new Thickness(2);
            BorderNotes_1.BorderThickness = new Thickness(2);
            BorderTransportCompanies.BorderThickness = new Thickness(2);

            for (int i = GridTransports.RowDefinitions.Count; i > 1; i--)
            {
                RemoveGridRow(i);
            }
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
                    StackPanelTransportCompanies.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    StackPanelWeight.Visibility = System.Windows.Visibility.Collapsed;
                    ButtonAddNewCargo.Visibility = System.Windows.Visibility.Collapsed;
                    StackPanelTransportCompanies.Visibility = System.Windows.Visibility.Collapsed;
                    TextBoxWeight_1.Text = null;
                    ComboBoxTransportCompanies.SelectedItem = null;
                }

                if (costItem.Name == "Приход")
                {
                    ComboBoxCostItems.Text = "Приход";
                    RadioButtonIncoming.IsChecked = true;
                    RadioButtonExpense.IsEnabled = false;
                }
                else
                {
                    RadioButtonExpense.IsChecked = true;
                    RadioButtonExpense.IsEnabled = true;
                }
            }
        }

        private void AddNewCargo_Click(object sender, RoutedEventArgs e)
        {
            var rowDefinition = new RowDefinition();
            GridTransports.RowDefinitions.Add(rowDefinition);

            var border = new Border();
            border.Name = "BorderRCs_" + GridTransports.RowDefinitions.Count;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.Red;
            border.SetValue(Grid.ColumnProperty, 1);
            border.SetValue(Grid.RowProperty, GridTransports.RowDefinitions.Count - 1);
            border.Margin = new Thickness(10, 10, 0, 0);
            NameScope.GetNameScope(WindowCosts).RegisterName(border.Name, border);

            var comboBox = new ComboBox();
            comboBox.Name = "ComboBoxRCs_" + GridTransports.RowDefinitions.Count;
            comboBox.DisplayMemberPath = "FullName";
            comboBox.IsEditable = true;
            comboBox.ItemsSource = _bc.GetDirectoryRCs().Where(r => r.Name != "26А").ToList();
            comboBox.LostFocus += ComboBoxRCs_SelectionChanged;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            border.Child = comboBox;

            GridTransports.Children.Add(border);
            NameScope.GetNameScope(WindowCosts).RegisterName(comboBox.Name, comboBox);


            border = new Border();
            border.Name = "BorderBoxNotes_" + GridTransports.RowDefinitions.Count;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.Red;
            border.SetValue(Grid.ColumnProperty, 2);
            border.SetValue(Grid.RowProperty, GridTransports.RowDefinitions.Count - 1);
            border.Margin = new Thickness(20, 10, 10, 0);
            NameScope.GetNameScope(WindowCosts).RegisterName(border.Name, border);

            comboBox = new ComboBox();
            comboBox.Name = "ComboBoxNotes_" + GridTransports.RowDefinitions.Count;
            comboBox.DisplayMemberPath = "Description";
            comboBox.IsEditable = true;
            comboBox.ItemsSource = _notes;
            comboBox.LostFocus += ComboBox_TextChanged;

            border.Child = comboBox;

            GridTransports.Children.Add(border);
            NameScope.GetNameScope(WindowCosts).RegisterName(comboBox.Name, comboBox);


            var textBox = new TextBox();
            textBox.Name = "TextBoxWeight_" + GridTransports.RowDefinitions.Count;
            textBox.Height = 22;
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            textBox.Margin = new Thickness(0, 10, 10, 0);
            textBox.SetValue(Grid.ColumnProperty, 3);
            textBox.SetValue(Grid.RowProperty, GridTransports.RowDefinitions.Count - 1);
            textBox.TextChanged += TextBox_TextChanged;

            GridTransports.Children.Add(textBox);
            NameScope.GetNameScope(WindowCosts).RegisterName(textBox.Name, textBox);

            var button = new Button();
            button.Name = "ButtonRemoveCargo_" + GridTransports.RowDefinitions.Count;
            button.Content = "Удалить";
            button.Margin = new Thickness(10, 10, 0, 0);
            button.SetValue(Grid.ColumnProperty, 4);
            button.SetValue(Grid.RowProperty, GridTransports.RowDefinitions.Count - 1);
            button.Click += buttonRemoveCargo_Click;

            GridTransports.Children.Add(button);
            NameScope.GetNameScope(WindowCosts).RegisterName(button.Name, button);

            InitializeValidation(GridCosts.Children);
            ChangeButtonAddVisibility(null);

            ComboBoxCostItems.IsEnabled = false;
        }

        private void buttonRemoveCargo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int number = int.Parse(button.Name.Substring(button.Name.LastIndexOf("_") + 1));
            RemoveGridRow(number);

            for (int i = number + 1; i <= GridTransports.RowDefinitions.Count + 1; i++)
            {
                RenameGridRow(i);
            }

            ChangeButtonAddVisibility("1");

            if (_isNotTransportOnly)
            {
                if (GridTransports.RowDefinitions.Count == 1)
                {
                    ComboBoxCostItems.IsEnabled = true;
                }
            }
        }

        private void RemoveGridRow(int number)
        {
            var comboBox = WindowCosts.FindName("ComboBoxRCs_" + number) as ComboBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
            GridTransports.Children.Remove(comboBox);

            comboBox = WindowCosts.FindName("ComboBoxNotes_" + number) as ComboBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
            GridTransports.Children.Remove(comboBox);

            var textBox = WindowCosts.FindName("TextBoxWeight_" + number) as TextBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(textBox.Name);
            GridTransports.Children.Remove(textBox);

            var button = WindowCosts.FindName("ButtonRemoveCargo_" + number) as Button;
            NameScope.GetNameScope(WindowCosts).UnregisterName(button.Name);
            GridTransports.Children.Remove(button);

            GridTransports.RowDefinitions.RemoveAt(number - 1);
        }

        private void RenameGridRow(int number)
        {
            var comboBox = WindowCosts.FindName("ComboBoxRCs_" + number) as ComboBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
            comboBox.Name = ("ComboBoxRCs_" + (number - 1));
            NameScope.GetNameScope(WindowCosts).RegisterName(comboBox.Name, comboBox);
            comboBox.SetValue(Grid.RowProperty, (number - 2));

            comboBox = WindowCosts.FindName("ComboBoxNotes_" + number) as ComboBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
            comboBox.Name = ("ComboBoxNotes_" + (number - 1));
            NameScope.GetNameScope(WindowCosts).RegisterName(comboBox.Name, comboBox);
            comboBox.SetValue(Grid.RowProperty, (number - 2));

            var textBox = WindowCosts.FindName("TextBoxWeight_" + number) as TextBox;
            NameScope.GetNameScope(WindowCosts).UnregisterName(textBox.Name);
            textBox.Name = ("TextBoxWeight_" + (number - 1));
            NameScope.GetNameScope(WindowCosts).RegisterName(textBox.Name, textBox);
            textBox.SetValue(Grid.RowProperty, (number - 2));

            var button = WindowCosts.FindName("ButtonRemoveCargo_" + number) as Button;
            NameScope.GetNameScope(WindowCosts).UnregisterName(button.Name);
            button.Name = ("ButtonRemoveCargo_" + (number - 1));
            NameScope.GetNameScope(WindowCosts).RegisterName(button.Name, button);
            button.SetValue(Grid.RowProperty, (number - 2));
        }

        private void ChangeButtonAddVisibility(object tag)
        {
            if (tag == null)
            {
                ButtonAdd.IsEnabled = false;
                return;
            }

            if (TextBoxSumm.Tag == null)
            {
                ButtonAdd.IsEnabled = false;
                return;
            }

            bool isTransport = ComboBoxCostItems.Text == "Транспорт (5031)" && (ComboBoxRCs_1.Text != "26А" || RadioButtonIncoming.IsChecked == false) ? true : false;

            if (isTransport)
            {
                if (ComboBoxTransportCompanies.SelectedIndex == -1)
                {
                    ButtonAdd.IsEnabled = false;
                    return;
                }
            }


            foreach (var item in GridTransports.Children)
            {
                var stackPanel = item as StackPanel;

                if (stackPanel != null)
                {
                    foreach (var subItem in stackPanel.Children)
                    {
                        var combobox = subItem as ComboBox;

                        if (combobox != null && combobox.Tag == null)
                        {
                            ButtonAdd.IsEnabled = false;
                            return;
                        }

                        if (isTransport)
                        {
                            var textBox = subItem as TextBox;

                            if (textBox != null && textBox.Tag == null)
                            {
                                ButtonAdd.IsEnabled = false;
                                return;
                            }
                        }
                    }
                }

                var combobox2 = item as ComboBox;

                if (combobox2 != null && combobox2.Tag == null)
                {
                    ButtonAdd.IsEnabled = false;
                    return;
                }

                if (isTransport)
                {
                    var textBox2 = item as TextBox;

                    if (textBox2 != null && textBox2.Tag == null)
                    {
                        ButtonAdd.IsEnabled = false;
                        return;
                    }
                }
            }

            ButtonAdd.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            TextBoxValidation(textBox);
        }

        private void TextBoxValidation(TextBox textBox)
        {
            double summ;
            if (!string.IsNullOrWhiteSpace(textBox.Text) && double.TryParse(textBox.Text, out summ))
            {
                textBox.BorderBrush = Brushes.DarkGray;
                textBox.BorderThickness = new Thickness(1);

                textBox.Tag = "1";
            }
            else
            {
                textBox.BorderBrush = Brushes.Red;
                textBox.BorderThickness = new Thickness(2);

                textBox.Tag = null;
            }

            ChangeButtonAddVisibility(textBox.Tag);
        }

        private void ComboBoxCostItems_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string borderName = "Border" + comboBox.Name.Substring(8);
            var border = WindowCosts.FindName(borderName) as Border;

            if (!string.IsNullOrWhiteSpace(comboBox.Text) && comboBox.ItemsSource.Cast<DirectoryCostItem>().Select(c => c.Name).Contains(comboBox.Text))
            {
                border.BorderThickness = new Thickness(0);

                comboBox.Tag = "1";
            }
            else
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(2);

                comboBox.Tag = null;
            }

            VisibilityWeight();
            ChangeButtonAddVisibility(comboBox.Tag);
        }

        private void ComboBoxRCs_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string borderName = "Border" + comboBox.Name.Substring(8);
            var border = WindowCosts.FindName(borderName) as Border;

            if (!string.IsNullOrWhiteSpace(comboBox.Text) && comboBox.ItemsSource.Cast<DirectoryRC>().Select(r => r.FullName).Contains(comboBox.Text))
            {
                border.BorderThickness = new Thickness(0);

                comboBox.Tag = "1";
            }
            else
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(2);

                comboBox.Tag = null;
            }

            VisibilityWeight();
            ChangeButtonAddVisibility(comboBox.Tag);
        }

        private void ComboBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            ComboBoxValidation(comboBox);
        }

        private void ComboBoxValidation(ComboBox comboBox)
        {
            string borderName = "Border" + comboBox.Name.Substring(8);
            var border = WindowCosts.FindName(borderName) as Border;

            if (!string.IsNullOrWhiteSpace(comboBox.Text))
            {
                border.BorderThickness = new Thickness(0);

                comboBox.Tag = "1";
            }
            else
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(2);

                comboBox.Tag = null;
            }

            ChangeButtonAddVisibility(comboBox.Tag);
        }

        private void RadioButtonIncomingExpense_Checked(object sender, RoutedEventArgs e)
        {
            VisibilityWeight();
            ChangeButtonAddVisibility(true);
        }

        private void VisibilityWeight()
        {
            if (ComboBoxCostItems.Text == "Транспорт (5031)")
            {
                if (ComboBoxRCs_1.Text == "26А" && RadioButtonIncoming.IsChecked.Value)
                {
                    StackPanelWeight.Visibility = System.Windows.Visibility.Collapsed;
                    ButtonAddNewCargo.Visibility = System.Windows.Visibility.Collapsed;
                    TextBoxWeight_1.Text = null;
                }
                else
                {
                    StackPanelWeight.Visibility = System.Windows.Visibility.Visible;
                    ButtonAddNewCargo.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisibilityWeight();
            ChangeButtonAddVisibility(true);
        }

        private void DataGridCurrentDateCosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonRemove.IsEnabled = DataGridCurrentDateCosts.SelectedIndex != -1 ? true : false;
        }

        private void ComboBoxTransportCompanies_SelectedChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string borderName = "Border" + comboBox.Name.Substring(8);
            var border = WindowCosts.FindName(borderName) as Border;

            if (comboBox.SelectedIndex != -1)
            {
                border.BorderThickness = new Thickness(0);

                comboBox.Tag = "1";
            }
            else
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(2);

                comboBox.Tag = null;
            }

            ChangeButtonAddVisibility(comboBox.Tag);
        }
    }
}

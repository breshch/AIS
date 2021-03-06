﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using AIS_Enterprise_AV.Auth;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;
using Microsoft.Vbe.Interop;
using Window = System.Windows.Window;

namespace AIS_Enterprise_AV.Views.Infos
{
	/// <summary>
	/// Логика взаимодействия для AddEditDayCost.xaml
	/// </summary>
	public partial class AddEditDayCost : Window
	{
		private BusinessContext _bc = new BusinessContext();
		private List<DirectoryNote> _notes;
		private bool _isNotTransportOnly;
		private bool _isEdit;
		private InfoCost _infoCost;

		public DateTime? Date;

		public AddEditDayCost(DateTime date, int? infoCostId = null)
		{
			InitializeComponent();

			_isNotTransportOnly = Privileges.HasAccess(UserPrivileges.CostsVisibility_IsNotTransportOnly);

			DatePickerDate.SelectedDate = date;

			_isEdit = infoCostId != null;
			if (_isEdit)
			{
				_infoCost = _bc.GetInfoCost(infoCostId.Value);

				ButtonAdd.Content = "Редактировать";
				WindowCosts.Title = "Редактирование";
				InitializeEditableData();
			}
			else
			{
				ButtonAdd.Content = "Добавить";
				WindowCosts.Title = "Добавление";

				InitializeData();

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


			ComboBoxRCs_1.Resources.Add(SystemColors.HighlightBrushKey, Brushes.Red);

			InitializeValidation(GridCosts.Children);
			ChangeButtonAddVisibility(null);
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

		private void InitializeData()
		{
			ComboBoxCurrencies.ItemsSource = Enum.GetNames(typeof(Currency));
			ComboBoxCurrencies.SelectedIndex = 0;

			if (!_isNotTransportOnly)
			{
				ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().Where(c => c.Name == "Транспорт (5031)" || c.Name == "Приход").ToList();
			}
			else
			{
				ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().ToList();
			}

			var rcs = _bc.GetDirectoryRCs().ToList();
			ComboBoxRCs_1.ItemsSource = rcs;
			if (rcs.Count == 1)
			{
				ComboBoxRCs_1.SelectedItem = rcs[0];
				ComboBoxRCs_1.IsReadOnly = true;
			}

			_notes = _bc.GetDirectoryNotes().ToList();
			OrderNotes();
			ComboBoxNotes_1.ItemsSource = _notes;
			ComboBoxTransportCompanies.ItemsSource = _bc.GetDirectoryTransportCompanies().ToList();
			RadioButtonExpense.IsChecked = true;
		}

		private void InitializeEditableData()
		{
			ComboBoxCurrencies.ItemsSource = Enum.GetNames(typeof(Currency));
			ComboBoxCurrencies.SelectedItem = _infoCost.Currency.ToString();

			if (!_isNotTransportOnly)
			{
				ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().Where(c => c.Name == "Транспорт (5031)"
					|| c.Name == "Приход").ToList();
			}
			else
			{
				ComboBoxCostItems.ItemsSource = _bc.GetDirectoryCostItems().ToList();
			}

			ComboBoxCostItems.SelectedItem = _infoCost.DirectoryCostItem;
			var rcs = _bc.GetDirectoryRCs().ToList();
			ComboBoxRCs_1.ItemsSource = rcs;
			if (rcs.Count == 1)
			{
				ComboBoxRCs_1.SelectedItem = rcs[0];
				ComboBoxRCs_1.IsReadOnly = true;
			}
			else
			{
				ComboBoxRCs_1.SelectedItem = _infoCost.DirectoryRC;
			}
			

			_notes = _bc.GetDirectoryNotes().ToList();
			OrderNotes();
			ComboBoxNotes_1.ItemsSource = _notes;
			ComboBoxNotes_1.SelectedItem = _infoCost.CurrentNotes[0].DirectoryNote;

			ComboBoxTransportCompanies.ItemsSource = _bc.GetDirectoryTransportCompanies().ToList();
			ComboBoxTransportCompanies.SelectedItem = _infoCost.DirectoryTransportCompany;
			RadioButtonExpense.IsChecked = !_infoCost.IsIncoming;

			TextBoxSumm.Text = _infoCost.Summ.ToString();
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

		private void VisibilityWeight()
		{
			if (ComboBoxCostItems.Text == "Транспорт (5031)")
			{
				if (ComboBoxRCs_1.Text == "26А" && RadioButtonIncoming.IsChecked.Value)
				{
					StackPanelWeight.Visibility = Visibility.Collapsed;
					ButtonAddNewCargo.Visibility = Visibility.Collapsed;
					TextBoxWeight_1.Text = null;
				}
				else
				{
					StackPanelWeight.Visibility = Visibility.Visible;
					ButtonAddNewCargo.Visibility = Visibility.Visible;
				}
			}
		}

		private void RadioButtonIncomingExpense_Checked(object sender, RoutedEventArgs e)
		{
			VisibilityWeight();
			ChangeButtonAddVisibility(true);
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

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;

			TextBoxValidation(textBox);
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
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			VisibilityWeight();
			ChangeButtonAddVisibility(true);
		}
		private void ComboBoxRCs_SelectionChanged(object sender, RoutedEventArgs e)
		{
			var comboBox = sender as ComboBox;

			var costItem = ComboBoxCostItems.SelectedItem as DirectoryCostItem;
			if (costItem != null)
			{
				if (costItem.Name == "Транспорт (5031)")
				{
					var rc = comboBox.SelectedItem as DirectoryRC;
					if (rc != null)
					{
						if (rc.Name == "ПАМ-16")
						{
							RadioButtonIncoming.IsEnabled = true;
						}
						else
						{
							RadioButtonIncoming.IsEnabled = false;
						}
					}
				}
				else
				{
					RadioButtonIncoming.IsEnabled = false;
				}
			}

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
		private void ComboBox_TextChanged(object sender, RoutedEventArgs e)
		{
			var comboBox = sender as ComboBox;

			ComboBoxValidation(comboBox);
		}
		private void AddNewCargo_Click(object sender, RoutedEventArgs e)
		{
			AddNewCargo();
		}

		private void AddNewCargo()
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
			border.Name = "BorderNotes_" + GridTransports.RowDefinitions.Count;
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
			textBox.VerticalContentAlignment = VerticalAlignment.Center;
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
				if (comboBox != null && comboBox.Name != "ComboBoxCurrencies")
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
		private void RemoveGridRow(int number)
		{
			var comboBox = WindowCosts.FindName("ComboBoxRCs_" + number) as ComboBox;
			NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
			GridTransports.Children.Remove(comboBox);

			var border = WindowCosts.FindName("BorderRCs_" + number) as Border;
			NameScope.GetNameScope(WindowCosts).UnregisterName(border.Name);
			GridTransports.Children.Remove(border);

			comboBox = WindowCosts.FindName("ComboBoxNotes_" + number) as ComboBox;
			NameScope.GetNameScope(WindowCosts).UnregisterName(comboBox.Name);
			GridTransports.Children.Remove(comboBox);

			border = WindowCosts.FindName("BorderNotes_" + number) as Border;
			NameScope.GetNameScope(WindowCosts).UnregisterName(border.Name);
			GridTransports.Children.Remove(border);

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

		private void AddEditDayCost_OnClosing(object sender, CancelEventArgs e)
		{
			_bc.Dispose();
		}

		private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
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
			var currency = (Currency)Enum.Parse(typeof(Currency), ComboBoxCurrencies.SelectedItem.ToString());

			Date = DatePickerDate.SelectedDate.Value;

			if (!_isEdit)
			{
				_bc.AddInfoCosts(DatePickerDate.SelectedDate.Value, ComboBoxCostItems.SelectedItem as DirectoryCostItem,
					RadioButtonIncoming.IsChecked.Value,
					ComboBoxTransportCompanies.SelectedItem as DirectoryTransportCompany, double.Parse(TextBoxSumm.Text), currency,
					transports);

				ComboBoxNotes_1.ItemsSource = null;
				ComboBoxNotes_1.ItemsSource = _bc.GetDirectoryNotes().ToList();

				ClearForm();
				InitializeValidation(GridCosts.Children);
			}
			else
			{
				_bc.RemoveInfoCost(_infoCost);
				_bc.AddInfoCosts(DatePickerDate.SelectedDate.Value, ComboBoxCostItems.SelectedItem as DirectoryCostItem,
					RadioButtonIncoming.IsChecked.Value,
					ComboBoxTransportCompanies.SelectedItem as DirectoryTransportCompany, double.Parse(TextBoxSumm.Text), currency,
					transports);
			}

			this.Close();
		}

		private void ClearForm()
		{
			RadioButtonExpense.IsChecked = true;
			TextBoxSumm.Text = null;
			ComboBoxCurrencies.SelectedIndex = 0;

			ComboBoxCostItems.SelectedItem = null;
			ComboBoxRCs_1.SelectedItem = null;
			ComboBoxNotes_1.SelectedItem = null;
			ComboBoxNotes_1.Text = null;
			ComboBoxTransportCompanies.SelectedItem = null;
			TextBoxWeight_1.Text = null;

			ComboBoxCostItems.IsEnabled = true;

			StackPanelTransportCompanies.Visibility = Visibility.Collapsed;
			StackPanelWeight.Visibility = Visibility.Collapsed;
			ButtonAddNewCargo.Visibility = Visibility.Collapsed;

			BorderCostItems.BorderThickness = new Thickness(2);
			BorderRCs_1.BorderThickness = new Thickness(2);
			BorderNotes_1.BorderThickness = new Thickness(2);
			BorderTransportCompanies.BorderThickness = new Thickness(2);

			for (int i = GridTransports.RowDefinitions.Count; i > 1; i--)
			{
				RemoveGridRow(i);
			}
		}

		private void ComboBoxCostItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var costItem = ComboBoxCostItems.SelectedItem as DirectoryCostItem;

			if (costItem != null)
			{
				if (costItem.Name == "Транспорт (5031)")
				{
					StackPanelWeight.Visibility = Visibility.Visible;
					ButtonAddNewCargo.Visibility = Visibility.Visible;
					StackPanelTransportCompanies.Visibility = Visibility.Visible;

					var rc = ComboBoxRCs_1.SelectedItem as DirectoryRC;

					if (rc != null)
					{
						if (rc.Name == "ПАМ-16")
						{
							RadioButtonIncoming.IsEnabled = true;
						}
						else
						{
							RadioButtonIncoming.IsEnabled = false;
						}
					}
				}
				else
				{
					StackPanelWeight.Visibility = Visibility.Collapsed;
					ButtonAddNewCargo.Visibility = Visibility.Collapsed;
					StackPanelTransportCompanies.Visibility = Visibility.Collapsed;
					TextBoxWeight_1.Text = null;
					ComboBoxTransportCompanies.SelectedItem = null;
					RadioButtonIncoming.IsEnabled = false;
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
	}
}

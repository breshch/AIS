using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Data.WareHouse;
using AIS_Enterprise_Global.Helpers;
using Button = System.Windows.Controls.Button;
using DataGrid = System.Windows.Controls.DataGrid;
using Orientation = System.Windows.Controls.Orientation;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace AIS_Enterprise_AV.WareHouse
{
	/// <summary>
	/// Логика взаимодействия для Scheme.xaml
	/// </summary>
	public partial class Scheme : Window
	{
		private readonly SchemeDrawingWarehouse _schemeDrawingWarehouse;
		private readonly SchemeDrawingBlock _schemeDrawingBlock;
		private SchemeData _schemeData;
		private readonly BusinessContext _bc;
		private const string _fontFamilyDefault = "Segoe UI Light";
		private readonly Size _warehouseSizeCell = new Size(40, 20);
		private readonly Size _warehouseSizeRoad = new Size(10, 10);
		private readonly Size _blockSizeCell = new Size(240, 60);
		private const double _warehouseFontSize = 12;
		private const double _blockFontSize = 14;

		private bool _isPalletSelected = false;

		private readonly DirectoryCarPart[] _carParts;

		public Scheme()
		{
			InitializeComponent();

			_bc = new BusinessContext();
			_carParts = _bc.GetDirectoryCarParts().ToArray();

			InitializeWarehouse();
			_schemeDrawingWarehouse = new SchemeDrawingWarehouse(SurfaceWarehouse, _schemeData);
			_schemeDrawingWarehouse.SetFontFamilyDefault(_fontFamilyDefault);
			_schemeDrawingBlock = new SchemeDrawingBlock(SurfaceBlock);
			_schemeDrawingBlock.SetFontFamilyDefault(_fontFamilyDefault);
			_schemeDrawingWarehouse.DrawWarehouse(_warehouseSizeCell, _warehouseSizeRoad, _warehouseFontSize);
		}

		private void InitializeWarehouse()
		{
			_schemeData = new SchemeData("Main1", 23, 7);
			var palletContents = _bc.GetAllPallets(_schemeData.WarehouseName);

			for (int row = 1; row <= _schemeData.CountRows; row++)
			{
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					const int countCells = 3;
					var countFloors = row == 23 ? 5 : 4;
					for (int floor = 1; floor <= countFloors; floor++)
					{
						for (int pallet = 1; pallet <= countCells; pallet++)
						{
							if (!((place == 1 && (row == 1 || row == 2)) ||
								((place == 1 || place == 7) && row == 23) ||
								(place == 4 && (row >= 3 && row <= 22) && floor <= 2)))
							{
								var carPartsData = palletContents.Where(c => c.Location.Row == row && c.Location.Place == place &&
									c.Location.Floor == floor && c.Location.Pallet == pallet)
									.Select(p => new CarPartData
									{
										CarPart = p.CarPart,
										CountCarParts = p.CountCarPart
									})
									.ToArray();

								var address = new AddressCell
								{
									Row = row,
									Place = place,
									Floor = floor,
									Cell = pallet
								};
								_schemeData.AddCell(address, carPartsData);
							}
						}
					}
				}
			}

			for (int row = 0; row <= _schemeData.CountRows - 1; row += 2)
			{
				_schemeData.SetRoad(new SchemeRoad
				{
					Type = RoadType.Row,
					StartRow = row,
					FinishRow = row + 1,
					StartPlace = 1,
					FinishPlace = _schemeData.CountPlaces
				});
			}
		}

		private void Surface_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (_isPalletSelected) return;

			var mousePoint = e.GetPosition(SurfaceWarehouse);
			var block = _schemeDrawingWarehouse.GetBlock(mousePoint);
			if (block != null)
			{
				Mouse.OverrideCursor = Cursors.Hand;

				if (_schemeDrawingWarehouse.SelectedBlock == null)
				{
					SurfaceBlock.Children.Clear();

					var cells = _schemeData.GetCells(block.Row, block.Place);
					_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize);
				}
			}
			else
			{
				Mouse.OverrideCursor = Cursors.Arrow;
				if (_schemeDrawingWarehouse.SelectedBlock == null)
				{
					SurfaceBlock.Children.Clear();
				}

			}
		}

		private void Surface_OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (_isPalletSelected) return;

			if (Mouse.OverrideCursor != Cursors.Arrow)
			{
				Mouse.OverrideCursor = Cursors.Arrow;
				if (_schemeDrawingWarehouse.SelectedBlock == null)
				{
					SurfaceBlock.Children.Clear();
				}
			}
		}

		private void SurfaceWarehouse_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_isPalletSelected) return;

			var mousePoint = e.GetPosition(SurfaceWarehouse);
			var block = _schemeDrawingWarehouse.GetBlock(mousePoint);
			if (block != null)
			{
				SurfaceWarehouse.Children.Clear();
				_schemeDrawingWarehouse.DrawWarehouse(_warehouseSizeCell, _warehouseSizeRoad, _warehouseFontSize, block);

				SurfaceBlock.Children.Clear();
				var cells = _schemeData.GetCells(block.Row, block.Place);
				_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize);

				_schemeDrawingWarehouse.SelectedBlock = block;
			}
		}

		private void Scheme_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_isPalletSelected) return;

			var mousePoint = e.GetPosition(WindowWarehouse);

			SurfaceWarehouse.UpdateLayout();
			SurfaceBlock.UpdateLayout();

			var surfaseWarehousePoint = SurfaceWarehouse.TranslatePoint(new Point(0, 0), WindowWarehouse);
			var surfaseBlockPoint = SurfaceBlock.TranslatePoint(new Point(0, 0), WindowWarehouse);

			if (!(surfaseWarehousePoint.X <= mousePoint.X && (surfaseWarehousePoint.X + _schemeDrawingWarehouse.Size.Width) >= mousePoint.X &&
				surfaseWarehousePoint.Y <= mousePoint.Y && (surfaseWarehousePoint.Y + _schemeDrawingWarehouse.Size.Height) >= mousePoint.Y) &&
				!(surfaseBlockPoint.X <= mousePoint.X && (surfaseBlockPoint.X + _schemeDrawingBlock.Size.Width) >= mousePoint.X &&
				surfaseBlockPoint.Y <= mousePoint.Y && (surfaseBlockPoint.Y + _schemeDrawingBlock.Size.Height) >= mousePoint.Y))
			{
				SurfaceWarehouse.Children.Clear();
				_schemeDrawingWarehouse.DrawWarehouse(_warehouseSizeCell, _warehouseSizeRoad, _warehouseFontSize);

				_schemeDrawingWarehouse.SelectedBlock = null;

				SurfaceBlock.Children.Clear();
			}
		}

		private void Scheme_OnClosing(object sender, CancelEventArgs e)
		{
			if (_bc != null)
			{
				_bc.Dispose();
			}
		}

		private void SurfaceBlock_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (_isPalletSelected) return;

			var mousePoint = e.GetPosition(SurfaceBlock);
			var pallet = _schemeDrawingBlock.GetPallet(mousePoint);
			Mouse.OverrideCursor = pallet != null ? Cursors.Hand : Cursors.Arrow;
		}

		private void SurfaceBlock_OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (_isPalletSelected) return;

			Mouse.OverrideCursor = Cursors.Arrow;
		}

		private void SurfaceBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_isPalletSelected) return;

			var mousePoint = e.GetPosition(SurfaceBlock);
			var pallet = _schemeDrawingBlock.GetPallet(mousePoint);
			if (pallet != null)
			{
				_isPalletSelected = true;
				Mouse.OverrideCursor = Cursors.Arrow;

				SurfaceBlock.Children.Clear();
				var block = _schemeDrawingWarehouse.SelectedBlock;
				var cells = _schemeData.GetCells(block.Row, block.Place);
				_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize, pallet);

				var address = new AddressCell
				{
					Row = block.Row,
					Place = block.Place,
					Floor = pallet.Floor,
					Cell = pallet.Cell
				};
				var cell = _schemeData.GetCell(address);

				_schemeDrawingBlock.SelectedPallet = pallet;

				DrawTableArticlesInPallet(cell);
			}
		}

		private void DrawTableArticlesInPallet(SchemeCell cell)
		{
			var grid = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
				}
			};

			Canvas.SetLeft(grid, 20);
			Canvas.SetTop(grid, _schemeDrawingBlock.Size.Height + 20);

			var tableArtictesInPallet = new DataGrid
			{
				CanUserAddRows = true,
				CanUserDeleteRows = true,
				HeadersVisibility = DataGridHeadersVisibility.Column,
				AutoGenerateColumns = false,
				Width = _schemeDrawingBlock.Size.Width - 20,
				MaxHeight = 120
			};

			Grid.SetRow(tableArtictesInPallet, 0);

			var columnArticle = new DataGridTextColumn
			{
				Width = new DataGridLength(1, DataGridLengthUnitType.Star),
				Header = "Артикул",
				Binding = new Binding("Article"),
			};

			tableArtictesInPallet.Columns.Add(columnArticle);

			var columnCountArticle = new DataGridTextColumn
			{
				Width = new DataGridLength(1, DataGridLengthUnitType.Star),
				Header = "Количество",
				Binding = new Binding("CountCarParts")
			};
			tableArtictesInPallet.Columns.Add(columnCountArticle);

			var carPartsInPallet = new ObservableCollection<CarPartPallet>(cell.CarParts.Select(carPart => new CarPartPallet
			{
				Article = carPart.CarPart.FullCarPartName,
				CountCarParts = carPart.CountCarParts
			}));

			tableArtictesInPallet.ItemsSource = carPartsInPallet;

			grid.Children.Add(tableArtictesInPallet);

			var textBlock = new TextBlock
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				Visibility = Visibility.Collapsed,
				Foreground = System.Windows.Media.Brushes.Brown,
				FontWeight = FontWeight.FromOpenTypeWeight(999),
				Margin = new Thickness(0, 4, 0, 0)
			};

			Grid.SetRow(textBlock, 1);
			grid.Children.Add(textBlock);

			var stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0, 15, 0, 0)
			};
			Grid.SetRow(stackPanel, 2);

			double buttonWidth = (tableArtictesInPallet.Width / 2 - 30) / 2;

			var buttonSave = new Button
			{
				Content = "Сохранить",
				Width = buttonWidth,
				Height = 24,
				Margin = new Thickness(0, 0, 15, 0),
				IsEnabled = false
			};

			buttonSave.Click += (sender, e) =>
			{
				var errorMessage = IsValidateTableArticles(tableArtictesInPallet);
				if (errorMessage != null)
				{
					textBlock.Text = errorMessage;
					textBlock.Visibility = Visibility.Visible;
					buttonSave.IsEnabled = false;
				}
				else
				{
					textBlock.Text = null;
					textBlock.Visibility = Visibility.Collapsed;
					buttonSave.IsEnabled = true;
				}

				if (!buttonSave.IsEnabled) return;

				var address = new AddressCell
				{
					Row = _schemeDrawingWarehouse.SelectedBlock.Row,
					Place = _schemeDrawingWarehouse.SelectedBlock.Place,
					Floor = _schemeDrawingBlock.SelectedPallet.Floor,
					Cell = _schemeDrawingBlock.SelectedPallet.Cell
				};

				var carPartPallets = new List<CarPartPallet>();
				foreach (var item in tableArtictesInPallet.Items)
				{
					var carPartPallet = item as CarPartPallet;
					if (carPartPallet != null)
					{
						carPartPallets.Add(carPartPallet);
					}
				}

				var palletContents =_bc.SavePalletContents(_schemeData.WarehouseName, address, carPartPallets.ToArray());
				var carPartsData = palletContents.Select(c => new CarPartData
				{
					CarPart = c.CarPart,
					CountCarParts = c.CountCarPart
				}).ToArray();

				_schemeData.AddCell(address, carPartsData);

				_isPalletSelected = false;

				SurfaceWarehouse.Children.Clear();
				_schemeDrawingWarehouse.DrawWarehouse(_warehouseSizeCell, _warehouseSizeRoad, _warehouseFontSize, 
					_schemeDrawingWarehouse.SelectedBlock);

				SurfaceBlock.Children.Clear();

				var block = _schemeDrawingWarehouse.SelectedBlock;
				var cells = _schemeData.GetCells(block.Row, block.Place);
				_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize);
			};

			var buttonCancel = new Button
			{
				Content = "Отменить",
				Width = buttonWidth,
				Height = 24,
				Margin = new Thickness(15, 0, 0, 0)
			};
			buttonCancel.Click += (sender, e) =>
			{
				_isPalletSelected = false;

				SurfaceBlock.Children.Clear();

				var block = _schemeDrawingWarehouse.SelectedBlock;
				var cells = _schemeData.GetCells(block.Row, block.Place);
				_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize);
			};

			stackPanel.Children.Add(buttonSave);
			stackPanel.Children.Add(buttonCancel);

			grid.Children.Add(stackPanel);
			SurfaceBlock.Children.Add(grid);

			tableArtictesInPallet.CurrentCellChanged += (sender, e) =>
			{
				var errorMessage = IsValidateTableArticles(tableArtictesInPallet);
				if (errorMessage != null)
				{
					textBlock.Text = errorMessage;
					textBlock.Visibility = Visibility.Visible;
					buttonSave.IsEnabled = false;
				}
				else
				{
					textBlock.Text = null;
					textBlock.Visibility = Visibility.Collapsed;
					buttonSave.IsEnabled = true;
				}
			};
		}

		private string IsValidateTableArticles(DataGrid tableArtictesInPallet)
		{
			for (int i = 0; i < tableArtictesInPallet.Items.Count; i++)
			{
				var row = (DataGridRow)tableArtictesInPallet.ItemContainerGenerator.ContainerFromIndex(i);
				if (row == null)
				{
					tableArtictesInPallet.UpdateLayout();
					tableArtictesInPallet.ScrollIntoView(tableArtictesInPallet.Items[i]);
					row = (DataGridRow)tableArtictesInPallet.ItemContainerGenerator.ContainerFromIndex(i);
				}

				if (row != null && Validation.GetHasError(row))
				{
					return "Введите только цифры.";
				}
			}

			foreach (var item in tableArtictesInPallet.Items)
			{
				var carPartPallet = item as CarPartPallet;
				if (carPartPallet != null)
				{
					if (_carParts.All(p => p.FullCarPartName != carPartPallet.Article))
					{
						return "Артикул " + carPartPallet.Article + " не найден в базе.";
					}
				}
			}

			return null;
		}
	}
}

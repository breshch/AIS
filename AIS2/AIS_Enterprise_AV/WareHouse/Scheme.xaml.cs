using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;

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

		public Scheme()
		{
			InitializeComponent();

			_bc = new BusinessContext();

			InitializeWarehouse();
			_schemeDrawingWarehouse = new SchemeDrawingWarehouse(SurfaceWarehouse, _schemeData);
			_schemeDrawingWarehouse.SetFontFamilyDefault(_fontFamilyDefault);
			_schemeDrawingBlock = new SchemeDrawingBlock(SurfaceBlock);
			_schemeDrawingBlock.SetFontFamilyDefault(_fontFamilyDefault);
			_schemeDrawingWarehouse.DrawWarehouse(_warehouseSizeCell, _warehouseSizeRoad, _warehouseFontSize);
		}

		private void InitializeWarehouse()
		{
			_schemeData = new SchemeData(23, 7);

			var carParts = _bc.GetDirectoryCarParts().ToArray();

			for (int row = 1; row <= _schemeData.CountRows; row++)
			{
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					const int countCells = 3;
					var countFloors = row == 23 ? 5 : 4;
					for (int floor = 1; floor <= countFloors; floor++)
					{
						for (int cell = 1; cell <= countCells; cell++)
						{
							if (!((place == 1 && (row == 1 || row == 2)) ||
								((place == 1 || place == 7) && row == 23) ||
								(place == 4 && (row >= 3 && row <= 22) && floor <= 2)))
							{
								var carPartData = HelperMethods.GetRandomNumber(0, 2) == 0
									? Enumerable.Range(1, HelperMethods.GetRandomNumber(1, 4))
										.Select(x => new CarPartData
										{
											CarPart = carParts[HelperMethods.GetRandomNumber(0, carParts.Length)],
											CountCarPart = HelperMethods.GetRandomNumber(1, 100000)
										})
										.ToArray()
									: new CarPartData[0];

								_schemeData.AddCell(row, place, floor, cell, carPartData);
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
			var mousePoint = e.GetPosition(SurfaceBlock);
			var pallet = _schemeDrawingBlock.GetPallet(mousePoint);
			Mouse.OverrideCursor = pallet != null ? Cursors.Hand : Cursors.Arrow;
		}

		private void SurfaceBlock_OnMouseLeave(object sender, MouseEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;
		}

		private void SurfaceBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var mousePoint = e.GetPosition(SurfaceBlock);
			var pallet = _schemeDrawingBlock.GetPallet(mousePoint);
			if (pallet != null)
			{
				SurfaceBlock.Children.Clear();
				var block = _schemeDrawingWarehouse.SelectedBlock;
				var cells = _schemeData.GetCells(block.Row, block.Place);
				_schemeDrawingBlock.DrawBlock(_blockSizeCell, cells, _blockFontSize, pallet);

				_schemeDrawingBlock.SelectedPallet = pallet;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace AIS_Enterprise_AV.WareHouse
{
	/// <summary>
	/// Логика взаимодействия для Scheme.xaml
	/// </summary>
	public partial class Scheme : Window
	{
		private readonly SchemeDrawing _schemeDrawing;
		private SchemeData _schemeData;

		public Scheme()
		{
			InitializeComponent();

			InitializeWarehouse();
			_schemeDrawing = new SchemeDrawing(Surface, _schemeData);
			_schemeDrawing.DrawWarehouse(new Point(0, 0), new Size(40, 20), new Size(10, 10), 12);
		}

		private void InitializeWarehouse()
		{
			_schemeData = new SchemeData(23, 7);

			for (int row = 1; row <= _schemeData.CountRows; row++)
			{
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					int countCells = 3;
					var countFloors = row == 23 ? 5 : 4;
					for (int floor = 1; floor <= countFloors; floor++)
					{
						for (int cell = 1; cell <= countCells; cell++)
						{
							if (!((place == 1 && (row == 1 || row == 2)) ||
								((place == 1 || place == 7) && row == 23) ||
								(place == 4 && (row >= 3 && row <= 22) && floor >= 3)))
							{
								_schemeData.AddCell(row, place, floor, cell);
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

			//_schemeData.SetRoad(new SchemeRoad
			//{
			//	Type = RoadType.Place,
			//	StartPlace = 3,
			//	FinishPlace = 4,
			//	StartRow = 3,
			//	FinishRow = _schemeData.CountRows
			//});

			//_schemeData.SetRoad(new SchemeRoad
			//{
			//	Type = RoadType.Place,
			//	StartPlace = 4,
			//	FinishPlace = 5,
			//	StartRow = 3,
			//	FinishRow = _schemeData.CountRows
			//});
		}

		private void Surface_OnMouseMove(object sender, MouseEventArgs e)
		{
			var mousePoint = e.GetPosition(Surface);
			var block = _schemeData.GetBlock(mousePoint);
			if (block != null)
			{
				Mouse.OverrideCursor = Cursors.Hand;

				TableCells.Columns.Clear();
				TableCells.Items.Clear();


				var cells = _schemeData.GetCells(block.Row, block.Place);
				int countFloors = cells.Max(c => c.Address.Floor) - cells.Min(c => c.Address.Floor) + 1;
				int countCellsInFloor = cells.Length / countFloors;

				for (int i = 0; i < countCellsInFloor; i++)
				{
					var textColumn = new DataGridTextColumn
					{
						Width = 60
					};
					TableCells.Columns.Add(textColumn);
				}

				for (int floor = 1; floor <= countFloors; floor++)
				{
					dynamic item = new ExpandoObject();
					for (int cell = 1; cell <= 1; cell++)
					{
						var cellInFloor = cells.First(c => c.Address.Floor == floor && c.Address.Cell == cell);

						var dataGridCell = new DataGridCell();
						((StackPanel) dataGridCell.Content).Children.Add(new TextBlock
						{
							Text = cellInFloor.CarParts.Length + " carparts"
						});
						item.Column1 = dataGridCell;
					}

					TableCells.Items.Add(item);
				}

				TableCells.Visibility = Visibility.Visible;
			}
			else
			{
				Mouse.OverrideCursor = Cursors.Arrow;
				TableCells.Visibility = Visibility.Collapsed;
			}
		}

		private void Surface_OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (Mouse.OverrideCursor != Cursors.Arrow)
			{
				Mouse.OverrideCursor = Cursors.Arrow;
				TableCells.Visibility = Visibility.Collapsed;
			}
		}
	}
}

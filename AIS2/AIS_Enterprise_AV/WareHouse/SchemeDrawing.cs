using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DocumentFormat.OpenXml.Drawing.Charts;
using Size = System.Windows.Size;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeDrawing
	{
		private readonly Canvas _surface;
		private readonly SchemeData _schemeData;

		public SchemeDrawing(Canvas surfase, SchemeData schemeData)
		{
			_surface = surfase;
			_schemeData = schemeData;
		}

		public void DrawRectangle(double startPointX, double startPointY, double width, double height, Brush strokeBrush, Brush fillBrush)
		{
			var rectangle = new Rectangle()
			{
				Width = width,
				Height = height,
				Fill = fillBrush,
				Stroke = strokeBrush,
				StrokeThickness = 1,
			};
			Canvas.SetLeft(rectangle, startPointX);
			Canvas.SetTop(rectangle, startPointY);

			_surface.Children.Add(rectangle);
		}

		public void DrawString(Point point, string value,
			Brush foregroundBrush, string fontFamily, double fontSize)
		{
			var textBlock = new TextBlock
			{
				Foreground = foregroundBrush,
				Text = value,
				FontFamily = new FontFamily(fontFamily),
				FontSize = fontSize
			};

			Canvas.SetLeft(textBlock, point.X);
			Canvas.SetTop(textBlock, point.Y);

			_surface.Children.Add(textBlock);
		}

		private Size GetSizeString(string value, string fontFamily, double fontSize)
		{
			var ft = new FormattedText(value, new CultureInfo("ru-RU"), FlowDirection.LeftToRight,
				new Typeface(fontFamily), fontSize, Brushes.Azure);
			return new Size(ft.Width, ft.Height);
		}

		public void DrawWarehouse(Point startPoint, Size sizeCell, Size sizeRoad, double fontSizeString)
		{
			const string fontFamilyCell = "Verdana";
			Brush brushRoad = Brushes.DarkSlateBlue;

			double totalHeightRoads = 0;
			for (int row = _schemeData.CountRows; row >= 1; row--)
			{
				bool isRoadRow = false;

				double newY = startPoint.Y + sizeCell.Height * (_schemeData.CountRows - row) + totalHeightRoads;
				double totalWidthRoads = 0;
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					double maxCells = _schemeData.GetMaxCells(row, place);

					double newX = startPoint.X + sizeCell.Width * (place - 1) + totalWidthRoads;

					if (!_schemeData.IsDisableCells(row, place))
					{
						int countFullCells = _schemeData.GetCountFullCells(row, place);

						double percentage = countFullCells / maxCells;
						Brush brushCell;
						if (percentage < 0.334)
						{
							brushCell = Brushes.Yellow;
						}
						else if (percentage < 0.667)
						{
							brushCell = Brushes.Orange;
						}
						else
						{
							brushCell = Brushes.OrangeRed;
						}

						DrawRectangle(newX, newY, sizeCell.Width, sizeCell.Height, Brushes.AliceBlue, brushCell);
						_schemeData.FillCoordinates(row, place, new Point(newX, newY), sizeCell);

						var sizeString = GetSizeString(countFullCells.ToString(), fontFamilyCell, fontSizeString);

						DrawString(new Point(newX + sizeCell.Width / 2 - sizeString.Width / 2, 
							newY + sizeCell.Height / 2 - sizeString.Height / 2),
							countFullCells.ToString(), Brushes.Black, fontFamilyCell, fontSizeString);
					}
					else
					{
						DrawRectangle(newX, newY - 1, sizeCell.Width, sizeCell.Height + 2, brushRoad, brushRoad);
					}

					if (_schemeData.IsRoad(RoadType.Row, row, row - 1, place))
					{
						DrawRectangle(newX, newY + sizeCell.Height, sizeCell.Width, sizeRoad.Height, brushRoad, brushRoad);

						if (!isRoadRow)
						{
							totalHeightRoads += sizeRoad.Height;
							isRoadRow = true;
						}
					}

					if (_schemeData.IsRoad(RoadType.Place, place, place + 1, row))
					{
						DrawRectangle(newX + sizeCell.Width, newY - 1, sizeRoad.Width, sizeCell.Height + 2, brushRoad, brushRoad);
						totalWidthRoads += sizeRoad.Width;
					}

					if (_schemeData.IsRoad(RoadType.Row, row, row - 1, place) && _schemeData.IsRoad(RoadType.Place, place, place + 1, row))
					{
						DrawRectangle(newX + sizeCell.Width, newY + sizeCell.Height, sizeRoad.Width, sizeRoad.Height, brushRoad, brushRoad);
					}
				}
			}
		}
	}
}

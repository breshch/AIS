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
				Stroke = strokeBrush
			};
			Canvas.SetLeft(rectangle, startPointX);
			Canvas.SetTop(rectangle, startPointY);

			_surface.Children.Add(rectangle);
		}

		public void DrawString(double startPointX, double startPointY, string value,
			Brush foregroundBrush, string fontFamily, double fontSize)
		{
			var textBlock = new TextBlock
			{
				Foreground = foregroundBrush,
				Text = value,
				FontFamily = new FontFamily(fontFamily),
				FontSize = fontSize
			};

			Canvas.SetLeft(textBlock, startPointX);
			Canvas.SetTop(textBlock, startPointY);

			_surface.Children.Add(textBlock);
		}

		private Size GetSizeString(string value, string fontFamily, double fontSize)
		{
			var ft = new FormattedText(value, new CultureInfo("ru-RU"), FlowDirection.LeftToRight,
				new Typeface(fontFamily), fontSize, Brushes.Azure);
			return new Size(ft.Width, ft.Height);
		}


		public void DrawWarehouse()
		{
			const double x = 100;
			const double y = 30;
			const double widthCell = 40;
			const double heightCell = 20;
			const double heightRoad = 10;
			const string fontFamilyCell = "Verdana";
			const double fontSizeString = 12;

			for (int row = _schemeData.CountRows; row >= 1; row--)
			{
				double newY = y + heightCell * (_schemeData.CountRows - row);
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					if (!_schemeData.IsDisableCells(row, place))
					{
						double newX = x + widthCell * (place - 1);
						DrawRectangle(newX, newY, widthCell, heightCell, Brushes.AliceBlue, Brushes.Black);

						var countFullCells = _schemeData.GetCountFullCells(row, place);

						var sizeString = GetSizeString(countFullCells.ToString(), fontFamilyCell, fontSizeString);

						DrawString(newX + widthCell / 2 - sizeString.Width / 2, newY + heightCell / 2 - sizeString.Height / 2,
							countFullCells.ToString(), Brushes.AliceBlue, fontFamilyCell, fontSizeString);
					}



				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeDrawingBlock
	{
		public Size Size { get; private set; }
		public AddressPallet SelectedPallet { get; set; }
		
		private readonly SchemeDrawing _schemeDrawing;
		private readonly List<AddressPallet> _schemeAddressPallets;

		public SchemeDrawingBlock(Canvas surface)
		{
			_schemeDrawing = new SchemeDrawing(surface);
			
			_schemeAddressPallets = new List<AddressPallet>();
		}

		public void SetFontFamilyDefault(string fontFamily)
		{
			_schemeDrawing.SetFontFamilyDefault(fontFamily);
		}

		public void DrawBlock(Size sizeCell, SchemeCell[] cells, double fontSizeString, AddressPallet addressPallet = null)
		{
			_schemeAddressPallets.Clear();

			Brush brushArticle = Brushes.Azure;
			Brush brushArticleCount = Brushes.Azure;
			Brush brushCell = Brushes.Brown;
			Brush brushCellBorder = Brushes.Azure;
			Brush brushHeader = Brushes.Black;

			double opacity = 1;
			if (addressPallet != null)
			{
				opacity = 0.3;
			}

			int countFloors = cells.Max(c => c.Address.Floor) - cells.Min(c => c.Address.Floor) + 1;
			int countCellsInFloor = cells.Length / countFloors;

			double newHeaderY = 0;
			double newHeaderX = 20;

			for (int i = 0; i < countCellsInFloor; i++)
			{
				var headerValue = (i + 1) + " паллет";
				var offsetHeaderX = (sizeCell.Width / 2) - (_schemeDrawing.GetSizeString(headerValue,  13).Width / 2);
				newHeaderX += offsetHeaderX;
				_schemeDrawing.DrawString(new Point(newHeaderX, newHeaderY), headerValue, brushHeader,  13);
				newHeaderX += sizeCell.Width - offsetHeaderX;
			}

			newHeaderY = 7;

			var maxFloor = cells.Max(cell => cell.Address.Floor);

			for (int floor = maxFloor; floor >= maxFloor - countFloors + 1; floor--)
			{
				var headerValue = floor + " этаж";
				var offsetHeaderY = (sizeCell.Height / 2) - (_schemeDrawing.GetSizeString(headerValue,  13).Height / 2);
				newHeaderY += offsetHeaderY;
				_schemeDrawing.DrawString(new Point(0, newHeaderY), headerValue, brushHeader,  13, 1, -90);
				newHeaderY += sizeCell.Height - offsetHeaderY;
			}

			newHeaderY = 20;
			newHeaderX = 20;

			for (int floor = maxFloor; floor >= maxFloor - countFloors + 1; floor--)
			{
				double newY = sizeCell.Height * (maxFloor - floor) + newHeaderY;
				for (int cell = 1; cell <= countCellsInFloor; cell++)
				{
					var cellInFloor = cells.First(c => c.Address.Floor == floor && c.Address.Cell == cell);

					double opacityPallet = opacity;
					if (addressPallet != null && addressPallet.Floor == floor && addressPallet.Cell == cell)
					{
						opacityPallet = 1;
					}

					double newX = sizeCell.Width * (cell - 1) + newHeaderX;
					_schemeDrawing.DrawRectangle(new Point(newX, newY), sizeCell, brushCellBorder, brushCell, opacityPallet);
					FillCoordinates(floor, cell, new Point(newX, newY), sizeCell);
					if (cellInFloor.CarParts.Any())
					{
						var firstCarPart = cellInFloor.CarParts[0];

						string firstValueName = firstCarPart.CarPart.FullCarPartName;
						var sizeString = _schemeDrawing.GetSizeString(firstValueName,  12);

						double offsetY = 0;
						double distance = (sizeCell.Height - (sizeString.Height * cellInFloor.CarParts.Length)) /
							(cellInFloor.CarParts.Length + 1);
						foreach (var carPart in cellInFloor.CarParts)
						{
							string article = carPart.CarPart.FullCarPartName;
							string countArticle = carPart.CountCarPart + " шт.";
							offsetY += distance;
							_schemeDrawing.DrawString(new Point(newX + 10, newY + offsetY), article, brushArticle,  12, opacityPallet);
							_schemeDrawing.DrawString(new Point(newX + sizeCell.Width / 3 * 2, newY + offsetY), countArticle, brushArticleCount,
								 12, opacityPallet);
							offsetY += sizeString.Height;
						}
					}
				}
			}

			double x = sizeCell.Width * countCellsInFloor + newHeaderX;
			double y = sizeCell.Height * countFloors + newHeaderY;
			Size = new Size(x, y);
		}

		public AddressPallet GetPallet(Point mousePoint)
		{
			foreach (var pallet in _schemeAddressPallets)
			{
				if (pallet.Point.X <= mousePoint.X && (pallet.Point.X + pallet.Size.Width) >= mousePoint.X &&
					pallet.Point.Y <= mousePoint.Y && (pallet.Point.Y + pallet.Size.Height) >= mousePoint.Y)
				{
					return pallet;
				}
			}

			return null;
		}

		private void FillCoordinates( int floor, int cell, Point point, Size size)
		{
			_schemeAddressPallets.Add(new AddressPallet
			{
				Cell = cell,
				Floor = floor,
				Point = point,
				Size = size
			});
		}
	}
}

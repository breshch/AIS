using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeDrawingWarehouse
	{
		public Size Size { get; private set; }
		public AddressBlock SelectedBlock { get; set; }

		private readonly SchemeData _schemeData;
		private readonly SchemeDrawing _schemeDrawing;
		private readonly List<AddressBlock> _schemeAddressBlocks;

		public SchemeDrawingWarehouse(Canvas surface, SchemeData schemeData)
		{
			_schemeData = schemeData;
			_schemeDrawing = new SchemeDrawing(surface);
			
			_schemeAddressBlocks = new List<AddressBlock>();
		}

		public void SetFontFamilyDefault(string fontFamily)
		{
			_schemeDrawing.SetFontFamilyDefault(fontFamily);
		}

		public void DrawWarehouse(Size sizeCell, Size sizeRoad, double fontSizeString, AddressBlock addressBlock = null)
		{
			_schemeAddressBlocks.Clear();

			Brush brushRoad = Brushes.DarkSlateBlue;
			Brush brushHeader = Brushes.Brown;
			Brush brushHeaderBorder = Brushes.Brown;
			Brush brushHeaderText = Brushes.Azure;
			Brush brushHeaderTitle = Brushes.Black;

			double opacity = 1;
			if (addressBlock != null)
			{
				opacity = 0.3;
			}

			double totalHeightRoads = 0;
			double totalWidthRoads = 0;

			var widthWarehouse = sizeCell.Width * _schemeData.CountPlaces;
			var titleHeaderValue = "Места";
			var titleHeaderX = (widthWarehouse + sizeCell.Height + 20) / 2 - _schemeDrawing.GetSizeString(titleHeaderValue, 13).Width / 2;
			_schemeDrawing.DrawString(new Point(titleHeaderX, 0), titleHeaderValue, brushHeaderTitle, 13);

			double titleHeaderY = 20;
			titleHeaderX = 20;
			for (int place = 1; place <= _schemeData.CountPlaces; place++)
			{
				double headerX = sizeCell.Width * (place - 1) + totalWidthRoads + sizeCell.Height + titleHeaderX;

				_schemeDrawing.DrawRectangle(new Point(headerX, titleHeaderY), new Size(sizeCell.Width, sizeCell.Width), brushHeaderBorder, brushHeader);

				if (_schemeData.IsRoad(RoadType.Place, place, place + 1, _schemeData.CountRows))
				{
					_schemeDrawing.DrawRectangle(new Point(headerX + sizeCell.Width, titleHeaderY), new Size(sizeRoad.Width, sizeCell.Width), brushRoad, brushRoad);
					totalWidthRoads += sizeRoad.Width;
				}

				double halfCellWidth = sizeCell.Width / 2;
				double halfCellHeight = sizeCell.Width / 2;
				var sizeString = _schemeDrawing.GetSizeString(place.ToString(), 12);
				_schemeDrawing.DrawString(new Point(halfCellWidth - sizeString.Width / 2 + headerX, halfCellHeight - sizeString.Height / 2 + titleHeaderY), place.ToString(),
					brushHeaderText, 12);
			}

			double heightWarehouse = 0;
			titleHeaderX = 20;
			for (int row = _schemeData.CountRows; row >= 1; row--)
			{
				double headerY = sizeCell.Height * (_schemeData.CountRows - row) + totalHeightRoads + sizeCell.Width + titleHeaderY;
				_schemeDrawing.DrawRectangle(new Point(titleHeaderX, headerY), new Size(sizeCell.Height, sizeCell.Height), brushHeaderBorder, brushHeader);
				if (_schemeData.IsRoad(RoadType.Row, row, row - 1, 1))
				{
					_schemeDrawing.DrawRectangle(new Point(titleHeaderX, headerY + sizeCell.Height), new Size(sizeCell.Height, sizeRoad.Height), brushRoad, brushRoad);
					totalHeightRoads += sizeRoad.Height;
				}

				double halfCellWidth = sizeCell.Height / 2;
				double halfCellHeight = sizeCell.Height / 2;
				var sizeString = _schemeDrawing.GetSizeString(row.ToString(), 12);
				_schemeDrawing.DrawString(new Point((halfCellWidth - sizeString.Width / 2 + titleHeaderX), halfCellHeight - sizeString.Height / 2 + headerY), row.ToString(),
					brushHeaderText, 12);

				heightWarehouse = headerY + sizeCell.Height;
			}

			titleHeaderValue = "Ряды";
			titleHeaderY = (heightWarehouse / 2) - (_schemeDrawing.GetSizeString(titleHeaderValue, 13).Height / 2);
			_schemeDrawing.DrawString(new Point(0, titleHeaderY), titleHeaderValue, brushHeaderTitle, 13, 1, -90);

			titleHeaderY = 20;

			_schemeDrawing.DrawRectangle(new Point(titleHeaderX, titleHeaderY), new Size(sizeCell.Height, sizeCell.Width), brushHeaderBorder, brushHeader);

			totalHeightRoads = 0;
			for (int row = _schemeData.CountRows; row >= 1; row--)
			{
				bool isRoadRow = false;

				double newY = sizeCell.Height * (_schemeData.CountRows - row) + totalHeightRoads + sizeCell.Width + titleHeaderY;

				totalWidthRoads = 0;
				for (int place = 1; place <= _schemeData.CountPlaces; place++)
				{
					double maxCells = _schemeData.GetMaxCells(row, place);

					double newX = sizeCell.Width * (place - 1) + totalWidthRoads + sizeCell.Height + titleHeaderX;

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

						double opacityBlock = opacity;
						if (addressBlock != null && addressBlock.Row == row && addressBlock.Place == place)
						{
							opacityBlock = 1;
						}
						_schemeDrawing.DrawRectangle(new Point(newX, newY), new Size(sizeCell.Width, sizeCell.Height), Brushes.AliceBlue, brushCell, opacityBlock);
						FillCoordinates(row, place, new Point(newX, newY), sizeCell);

						var sizeString = _schemeDrawing.GetSizeString(countFullCells.ToString(), fontSizeString);

						_schemeDrawing.DrawString(new Point(newX + sizeCell.Width / 2 - sizeString.Width / 2,
							newY + sizeCell.Height / 2 - sizeString.Height / 2),
							countFullCells.ToString(), Brushes.Black, fontSizeString, opacityBlock);
					}
					else
					{
						_schemeDrawing.DrawRectangle(new Point(newX, newY - 1), new Size(sizeCell.Width, sizeCell.Height + 2), brushRoad, brushRoad, opacity);
					}

					if (_schemeData.IsRoad(RoadType.Row, row, row - 1, place))
					{
						_schemeDrawing.DrawRectangle(new Point(newX, newY + sizeCell.Height), new Size(sizeCell.Width, sizeRoad.Height), brushRoad, brushRoad, opacity);

						if (!isRoadRow)
						{
							totalHeightRoads += sizeRoad.Height;
							isRoadRow = true;
						}
					}

					if (_schemeData.IsRoad(RoadType.Place, place, place + 1, row))
					{
						_schemeDrawing.DrawRectangle(new Point(newX + sizeCell.Width, newY - 1), new Size(sizeRoad.Width, sizeCell.Height + 2), brushRoad, brushRoad, opacity);
						totalWidthRoads += sizeRoad.Width;
					}

					if (_schemeData.IsRoad(RoadType.Row, row, row - 1, place) && _schemeData.IsRoad(RoadType.Place, place, place + 1, row))
					{
						_schemeDrawing.DrawRectangle(new Point(newX + sizeCell.Width, newY + sizeCell.Height), new Size(sizeRoad.Width, sizeRoad.Height),
							brushRoad, brushRoad, opacity);
					}
				}
			}

			double x = sizeCell.Width * (_schemeData.CountPlaces - 1) + totalWidthRoads + sizeCell.Height + sizeCell.Width + titleHeaderX;
			double y = sizeCell.Height * (_schemeData.CountRows - 1) + totalHeightRoads + sizeCell.Width + sizeCell.Height + titleHeaderY;
			Size = new Size(x, y);
		}

		public AddressBlock GetBlock(Point mousePoint)
		{
			foreach (var block in _schemeAddressBlocks)
			{
				if (block.Point.X <= mousePoint.X && (block.Point.X + block.Size.Width) >= mousePoint.X &&
					block.Point.Y <= mousePoint.Y && (block.Point.Y + block.Size.Height) >= mousePoint.Y)
				{
					return block;
				}
			}

			return null;
		}

		private void FillCoordinates(int row, int place, Point point, Size size)
		{
			_schemeAddressBlocks.Add(new AddressBlock
			{
				Row = row,
				Place = place,
				Point = point,
				Size = size
			});
		}
	}
}

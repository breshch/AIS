using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeData
	{
		private List<SchemeCell> _schemeCells = new List<SchemeCell>();
		private List<SchemeRoad> _schemeRoads = new List<SchemeRoad>();

		public int CountRows { get; private set; }
		public int CountPlaces { get; private set; }

		public SchemeData(int countRows, int countPlaces)
		{
			CountRows = countRows;
			CountPlaces = countPlaces;
		}

		public void AddCell(int row, int place, int floor, int cell)
		{
			_schemeCells.Add(new SchemeCell
			{
				Address = new AddressCell
				{
					Row = row,
					Place = place,
					Floor = floor,
					Cell = cell
				},
			});
		}

		public void SetRoad(SchemeRoad schemeRoad)
		{
			_schemeRoads.Add(schemeRoad);
		}

		public int GetCountFullCells(int row, int place)
		{
			return _schemeCells.Count(c => c.Address.Row == row && c.Address.Place == place && c.IsFull);
		}

		public int GetMaxCells(int row, int place)
		{
			return _schemeCells.Count(c => c.Address.Row == row && c.Address.Place == place);
		}

		public bool IsDisableCells(int row, int place)
		{
			return !_schemeCells.Any(c => c.Address.Row == row && c.Address.Place == place);
		}

		public AddressBlock GetBlock(Point mousePoint)
		{
			foreach (var cell in _schemeCells)
			{
				if (cell.IsFind(mousePoint))
				{
					return new AddressBlock
					{
						Row = cell.Address.Row,
						Place = cell.Address.Place
					};
				}
			}

			return null;
		}

		public bool IsRoad(RoadType roadType, int startRoadType, int finishRoadType, int positionInverseRoadType)
		{
			var roads = _schemeRoads.Where(r => r.Type == roadType);
			switch (roadType)
			{
				case RoadType.Row:
					return roads.Any(r => ((r.StartRow == startRoadType && r.FinishRow == finishRoadType) ||
					                       (r.StartRow == finishRoadType && r.FinishRow == startRoadType)) &&
					                      ((r.StartPlace <= positionInverseRoadType && r.FinishPlace >= positionInverseRoadType) ||
					                       (r.FinishPlace <= positionInverseRoadType && r.StartPlace >= positionInverseRoadType)));
				case RoadType.Place:
					return roads.Any(r => ((r.StartPlace == startRoadType && r.FinishPlace == finishRoadType) ||
					                       (r.StartPlace == finishRoadType && r.FinishPlace == startRoadType)) &&
					                      ((r.StartRow <= positionInverseRoadType && r.FinishRow >= positionInverseRoadType) ||
					                       (r.FinishRow <= positionInverseRoadType && r.StartRow >= positionInverseRoadType)));
			}

			return false;
		}

		public void FillCoordinates(int row, int place, Point coordinates, Size size)
		{
			var cells =_schemeCells.Where(c => c.Address.Row == row && c.Address.Place == place);
			foreach (var cell in cells)
			{
				cell.Coordinates = coordinates;
				cell.Size = size;
			}
		}

		public SchemeCell[] GetCells(int row, int place)
		{
			return _schemeCells.Where(c => c.Address.Row == row && c.Address.Place == place).ToArray();
		}
	}
}

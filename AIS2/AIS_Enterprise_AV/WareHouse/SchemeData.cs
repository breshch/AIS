using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeData
	{
		private readonly List<SchemeCell> _schemeCells = new List<SchemeCell>();
		private readonly List<SchemeRoad> _schemeRoads = new List<SchemeRoad>();

		public int CountRows { get; private set; }
		public int CountPlaces { get; private set; }

		public SchemeData(int countRows, int countPlaces)
		{
			CountRows = countRows;
			CountPlaces = countPlaces;
		}

		public void AddCell(int row, int place, int floor, int cell, CarPartData[] carPartData)
		{
			_schemeCells.Add(new SchemeCell(carPartData)
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

		public SchemeCell[] GetCells(int row, int place)
		{
			return _schemeCells.Where(c => c.Address.Row == row && c.Address.Place == place).ToArray();
		}

		public SchemeCell GetCell(int row, int place, int floor, int cell)
		{
			return _schemeCells.FirstOrDefault(c => c.Address.Row == row && c.Address.Place == place &&
													c.Address.Floor == floor && c.Address.Cell == cell);
		}
	}
}

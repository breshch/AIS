using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AIS_Enterprise_Data.WareHouse;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeData
	{
		public string WarehouseName { get; private set; }

		private readonly List<SchemeCell> _schemeCells = new List<SchemeCell>();
		private readonly List<SchemeRoad> _schemeRoads = new List<SchemeRoad>();

		public int CountRows { get; private set; }
		public int CountPlaces { get; private set; }

		public SchemeData(string warehouseName, int countRows, int countPlaces)
		{
			WarehouseName = warehouseName;
			CountRows = countRows;
			CountPlaces = countPlaces;
		}

		public void AddCell(AddressCell address, CarPartData[] carPartData)
		{
			var pallet = GetCell(address);

			var schemeCell = new SchemeCell(carPartData)
			{
				Address = address
			};

			if (pallet == null)
			{
				_schemeCells.Add(schemeCell);
			}
			else
			{
				_schemeCells[_schemeCells.IndexOf(pallet)] = schemeCell;
			}
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

		public SchemeCell GetCell(AddressCell address)
		{
			return _schemeCells.FirstOrDefault(c => c.Address.Row == address.Row && c.Address.Place == address.Place &&
													c.Address.Floor == address.Floor && c.Address.Cell == address.Cell);
		}
	}
}

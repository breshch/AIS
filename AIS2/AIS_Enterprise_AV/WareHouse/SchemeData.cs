using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeData
	{
		private SchemeCell[] _schemeCells;
		private List<SchemeRoad> _schemeRoads = new List<SchemeRoad>();

		public int CountRows { get; private set; }
		public int CountPlaces { get; private set; }
		public int CountFloors { get; private set; }
		public int CountCells { get; private set; }

		public SchemeData(int countRows, int countPlaces, int countFloors, int countCells)
		{
			CountRows = countRows;
			CountPlaces = countPlaces;
			CountFloors = countFloors;
			CountCells = countCells;

			_schemeCells = new SchemeCell[countRows * countPlaces * countFloors * countCells];
			for (int row = 0; row < countRows; row++)
			{
				for (int place = 0; place < countPlaces; place++)
				{
					for (int floor = 0; floor < countFloors; floor++)
					{
						for (int cell = 0; cell < countCells; cell++)
						{	
							int index = row * countPlaces * countFloors * countCells + 
								place * countFloors * countCells + floor * countCells + cell;
							_schemeCells[index] = new SchemeCell
							{
								Address = new AddressCell
								{
									Row = row + 1,
									Place = place + 1,
									Cell = cell + 1,
									Floor = floor + 1
								}
							};
						}
					}
				}
			}
		}

		public void SetDisableCells(int row, int place)
		{
			foreach (var schemeCell in _schemeCells)
			{
				if (schemeCell.Address.Row == row && schemeCell.Address.Place == place)
				{
					schemeCell.IsEnabled = false;
				}
			}
		}

		public void SetDisableCells(int row, int place, int floor)
		{
			foreach (var schemeCell in _schemeCells)
			{
				if (schemeCell.Address.Row == row && schemeCell.Address.Place == place && schemeCell.Address.Floor == floor)
				{
					schemeCell.IsEnabled = false;
				}
			}
		}

		public void SetRoad(SchemeRoad schemeRoad)
		{
			_schemeRoads.Add(schemeRoad);
		}

		public int GetCountFullCells(int row, int place)
		{
			return _schemeCells.Count(c => c.IsEnabled && c.Address.Row == row && c.Address.Place == place && c.IsFull);
		}

		public bool IsDisableCells(int row, int place)
		{
			return !_schemeCells.Any(c => c.IsEnabled && c.Address.Row == row && c.Address.Place == place);
		}
	}
}

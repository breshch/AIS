using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.WareHouse
{
	public class PalletLocation
	{
		public int Id { get; set; }
		public int Row { get; set; }
		public int Place { get; set; }
		public int Floor { get; set; }
		public int Pallet { get; set; }

		public int WarehouseId { get; set; }
		public Warehouse Warehouse { get; set; }
	}
}

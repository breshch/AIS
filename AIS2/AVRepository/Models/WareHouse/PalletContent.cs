using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.WareHouse
{
	public class PalletContent
	{
		public int Id { get; set; }
		public int CountCarPart { get; set; }

		public int PalletLocationId { get; set; }
		public PalletLocation Location { get; set; }
		public int DirectoryCarPartId { get; set; }
		public DirectoryCarPart CarPart { get; set; }
	}
}

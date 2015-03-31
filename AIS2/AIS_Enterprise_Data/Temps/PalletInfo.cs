using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.WareHouse;

namespace AIS_Enterprise_Data.Temps
{
	public class PalletInfo
	{
		public PalletLocation Location { get; set; }
		public PalletContent[] Contents { get; set; }
	}
}

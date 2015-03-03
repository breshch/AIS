using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeRoad
	{
		public int StartRow { get; set; }
		public int FinishRow { get; set; }
		public int StartPlace { get; set; }
		public int FinishPlace { get; set; }
		public RoadType Type { get; set; }
	}
}

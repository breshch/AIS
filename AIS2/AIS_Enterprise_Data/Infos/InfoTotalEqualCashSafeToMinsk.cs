using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
	public class InfoTotalEqualCashSafeToMinsk
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public double SafeCash { get; set; }
		public double? MinskCash { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}

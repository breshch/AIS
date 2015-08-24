using System;

namespace AVService.Models.Entities.Infos
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

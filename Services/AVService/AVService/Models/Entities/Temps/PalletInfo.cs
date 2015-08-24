using AVService.Models.Entities.WareHouse;

namespace AVService.Models.Entities.Temps
{
	public class PalletInfo
	{
		public PalletLocation Location { get; set; }
		public PalletContent[] Contents { get; set; }
	}
}

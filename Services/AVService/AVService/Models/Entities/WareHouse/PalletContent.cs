using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.WareHouse
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

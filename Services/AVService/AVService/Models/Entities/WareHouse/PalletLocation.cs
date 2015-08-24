namespace AVService.Models.Entities.WareHouse
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

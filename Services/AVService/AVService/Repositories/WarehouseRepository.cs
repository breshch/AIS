using System.Data.Entity;
using System.Linq;
using AVService.Models.Entities.WareHouse;

namespace AVService.Repositories
{
	public class WarehouseRepository : BaseRepository
	{
		#region Warehouse

		public PalletContent[] GetPalletContents(string warehouseName, AddressCell address)
		{
			using (var db = GetContext())
			{
				int warehouseId = db.Warehouses.First(w => w.Name == warehouseName).Id;
				var location = db.PalletLocations.FirstOrDefault(l => l.WarehouseId == warehouseId && l.Row == address.Row &&
																	  l.Place == address.Place && l.Floor == address.Floor &&
																	  l.Pallet == address.Cell);

				if (location != null)
				{
					return db.PalletContents.Where(p => p.PalletLocationId == location.Id).ToArray();
				}
				else
				{
					return new PalletContent[0];
				}
			}
		}

		public PalletContent[] GetAllPallets(string warehouseName)
		{
			using (var db = GetContext())
			{
				var warehouse = db.Warehouses.FirstOrDefault(w => w.Name == warehouseName);
				return warehouse != null
					? db.PalletContents.Include(c => c.Location).Where(c => c.Location.WarehouseId == warehouse.Id).ToArray()
					: new PalletContent[0];
			}
		}

		public PalletContent[] SavePalletContents(string warehouseName, AddressCell address, CarPartPallet[] carPartPallets)
		{
			using (var db = GetContext())
			{
				int warehouseId = db.Warehouses.First(w => w.Name == warehouseName).Id;
				var removableContents =
					db.PalletContents.Include(c => c.Location).Where(c => c.Location.WarehouseId == warehouseId &&
																		  c.Location.Row == address.Row &&
																		  c.Location.Place == address.Place &&
																		  c.Location.Floor == address.Floor &&
																		  c.Location.Pallet == address.Cell);

				db.PalletContents.RemoveRange(removableContents);


				var articles = carPartPallets.Select(p => p.Article);
				var directoryCarParts = db.DirectoryCarParts.Where(c => articles.Contains(c.Article + c.Mark)).ToArray();

				var location = db.PalletLocations.FirstOrDefault(l => l.WarehouseId == warehouseId && l.Row == address.Row &&
																	  l.Place == address.Place && l.Floor == address.Floor &&
																	  l.Pallet == address.Cell);
				if (location == null)
				{
					location = new PalletLocation
					{
						WarehouseId = warehouseId,
						Row = address.Row,
						Place = address.Place,
						Floor = address.Floor,
						Pallet = address.Cell
					};
				}

				var palletContents = carPartPallets.Select(p => new PalletContent
				{
					Location = location,
					CountCarPart = p.CountCarParts,
					DirectoryCarPartId = directoryCarParts.First(c => c.FullCarPartName == p.Article).Id
				}).ToArray();

				db.PalletContents.AddRange(palletContents);
				db.SaveChanges();

				return palletContents;
			}
		}

		#endregion
	}
}

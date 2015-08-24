using System.Linq;
using AVClient.AVServiceReference;

namespace AVClient.WareHouse
{
	public class SchemeCell
	{
		public AddressCell Address { get; set; }

		public CarPartData[] CarParts { get; }

		public SchemeCell(CarPartData[] carParts)
		{
			CarParts = carParts;
		}

		public bool IsFull
		{
			get
			{
				return CarParts.Any();
			}
		}
	}
}

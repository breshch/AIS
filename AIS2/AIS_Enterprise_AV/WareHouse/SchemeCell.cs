using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeCell
	{
		public AddressCell Address { get; set; }
		public bool IsEnabled { get; set; }

		private CarPartData[] _carParts;

		private static readonly Random _random = new Random();
		private static readonly object _lockRandom = new object();

		private static int GetRandom(int min, int max)
		{
			lock (_lockRandom)
			{
				return _random.Next(min, max);
			}
		}

		public SchemeCell()
		{
			IsEnabled = true;

			_carParts = GetRandom(0, 2) == 0
				? Enumerable
					.Range(1, GetRandom(1, 4))
					.Select(number => new CarPartData
					{
						CarPart = new DirectoryCarPart
						{
							Article = Path.GetRandomFileName()
						},
						CountCarPart = GetRandom(1, 500)
					}).ToArray()
				: new CarPartData[0];
		}

		public bool IsFull
		{
			get
			{
				return _carParts.Any();
			}
		}
	}
}

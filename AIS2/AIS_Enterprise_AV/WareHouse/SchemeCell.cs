﻿using System.Linq;
using System.Windows;

namespace AIS_Enterprise_AV.WareHouse
{
	public class SchemeCell
	{
		public AddressCell Address { get; set; }

		public CarPartData[] CarParts { get; private set; }

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

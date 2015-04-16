using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data;

namespace AIS_Enterprise_Cards.Cards
{
	public class CardPSB : CardBase
	{
		public CardPSB(string body)
			: base(body)
		{
			_cardName = "PSB";
		}
	}
}

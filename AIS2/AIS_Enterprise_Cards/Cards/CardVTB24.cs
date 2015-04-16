using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data;

namespace AIS_Enterprise_Cards.Cards
{
	public class CardVTB24 : CardBase
	{
		public CardVTB24(string body)
			: base(body)
		{
			_cardName = "VTB24";
		}
	}
}

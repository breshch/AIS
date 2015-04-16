using AIS_Enterprise_Data;

namespace AIS_Enterprise_CardService.Cards
{
	public abstract class CardBase
	{
		protected string _cardRemain = "dostupno";
		protected string _cardName = null;
		protected string _body;

		protected CardBase(string body)
		{
			_body = body;
		}

		public virtual double? GetSum()
		{
			int indexRemain = _body.ToLower().LastIndexOf(_cardRemain.ToLower());
			
			if (indexRemain != -1)
			{
				int indexCurrency = _body.IndexOf("RUR",indexRemain);
				string temp = _body.Substring(indexRemain + _cardRemain.Length + 1, indexCurrency -
					(indexRemain + _cardRemain.Length + 1)).Replace(" ","").Replace(".", ",");
				double newAvaliableSum = double.Parse(temp);

				using (var bc = new BusinessContext())
				{
					var prevAvaliableSum = bc.GetCardAvaliableSumm(_cardName);
					double differenceSum = newAvaliableSum - prevAvaliableSum;

					bc.SetCardAvaliableSumm(_cardName, newAvaliableSum);

					return differenceSum;
				}
			}
			return null;
		}
	}
}

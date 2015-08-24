using System.ComponentModel.DataAnnotations.Schema;

namespace AVService.Models.Entities.Helpers
{
    public class CurrencyValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double RUR { get; set; }
        public double USD { get; set; }
        public double EUR { get; set; }
        public double BYR { get; set; }

	    [NotMapped]
	    public string GetRUR { get; set; }

	    //public string GetRUR //TODO Refactor
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(RUR, "RUR");
		//    }
		//}

	    [NotMapped]
	    public string GetUSD { get; set; }

	    //public string GetUSD
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(USD, "USD");
		//    }
		//}

	    [NotMapped]
	    public string GetEUR { get; set; }

	    //public string GetEUR
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(EUR, "EUR");
		//    }
		//}

	    [NotMapped]
	    public string GetBYR { get; set; }

	    //public string GetBYR
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(BYR, "BYR");
		//    }
		//}
	}
}

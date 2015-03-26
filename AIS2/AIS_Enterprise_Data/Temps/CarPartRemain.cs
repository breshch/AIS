namespace AIS_Enterprise_Data.Temps
{
    public class CarPartRemain
    {
	    public int Id { get; set; }
        public string Article { get; set; }
        public string Description { get; set; }
        public int Remain { get; set; }
        public double PriceRUR { get; set; }
        public double? PriceUSD { get; set; }
    }
}

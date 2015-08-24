namespace AVService.Models.Entities.Temps
{
    public class ArticlePrice
    {
        public int CarPartId { get; set; }
        public string Article { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public double PriceRUR { get; set; }
        public double? PriceUSD{ get; set; }
    }
}

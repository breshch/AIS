namespace AIS_Enterprise_AV.Models
{
    public class Invoice
    {
        public string Article  { get; set; }
        public string Description { get; set; }
        public double PriceBase { get; set; }
        public int Count { get; set; }
        public bool IsRus { get; set; }
    }
}

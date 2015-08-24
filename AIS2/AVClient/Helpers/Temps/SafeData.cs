using System.Windows.Media;

namespace AVClient.Helpers.Temps
{
    public class SafeData
    {
        public string Name { get; set; }
        public string SummRUR { get; set; }
        public string SummUSD { get; set; }
        public string SummEUR { get; set; }
        public string SummBYR { get; set; }

        public SolidColorBrush Color { get; set; }

        public SafeData()
        {
            Color = Brushes.WhiteSmoke;
        }
    }
}

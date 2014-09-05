using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AIS_Enterprise_AV.Helpers.Temps
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

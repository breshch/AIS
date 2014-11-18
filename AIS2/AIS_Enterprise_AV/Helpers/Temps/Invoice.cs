using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_AV.Helpers.Temps
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

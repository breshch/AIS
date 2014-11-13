using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentCarPart
    {
        public int Id { get; set; }

        public int DirectoryCarPartId { get; set; }
        public virtual DirectoryCarPart DirectoryCarPart { get; set; }
        
        public DateTime Date { get; set; }
        public double PriceBase { get; set; }
        public double? PriceBigWholesale { get; set; }
        public double? PriceSmallWholesale { get; set; }
    }
}

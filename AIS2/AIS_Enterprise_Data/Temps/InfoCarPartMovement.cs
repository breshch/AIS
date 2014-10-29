using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Temps
{
    public class InfoCarPartMovement
    {
        public DateTime Date { get; set; }
        public int? Incoming { get; set; }
        public int? Outcoming { get; set; }
        public string FullDescription { get; set; }
    }
}

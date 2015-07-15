using System;

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

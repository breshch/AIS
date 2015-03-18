using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AIS_Enterprise_Data.Currents;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoOverTime
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<CurrentRC> CurrentRCs { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public InfoOverTime()
        {
            CurrentRCs = new List<CurrentRC>();
        }
    }
}

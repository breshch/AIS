using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Infos
{
    public class InfoOverTime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}

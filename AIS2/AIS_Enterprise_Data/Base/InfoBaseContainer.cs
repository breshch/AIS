using AIS_Enterprise_Data.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoBaseContainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<CurrentBaseContainerCarPart> CarParts { get; set; }

        public InfoBaseContainer()
        {

        }
    }
}

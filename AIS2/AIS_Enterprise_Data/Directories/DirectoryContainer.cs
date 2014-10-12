using AIS_Enterprise_Data.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryContainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual List<CurrentContainerCarPart> CarParts { get; set; }

        public DirectoryContainer()
        {
            CarParts = new List<CurrentContainerCarPart>();
        }
    }
}

using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentContainerCarPart
    {
        public int Id { get; set; }

        public int DirectoryCarPartId { get; set; }
        public virtual DirectoryCarPart DirectoryCarPart { get; set; }
        public int CountCarParts { get; set; }

        public int InfoContainerId { get; set; }
    }
}

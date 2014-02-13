using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoCargo
    {
        public int Id { get; set; }
        public string Basement { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }

        public int DirectoryPoDId { get; set; }
        public virtual DirectoryPoD DirectoryPoD { get; set; }

        public double Weight { get; set; }
        public double? Length { get; set; }
        public double Volume { get; set; }
        public double Sum { get; set; }
    }
}

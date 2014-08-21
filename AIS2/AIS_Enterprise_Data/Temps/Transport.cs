using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Temps
{
    public class Transport
    {
        public DirectoryRC DirectoryRC { get; set; }
        public DirectoryNote DirectoryNote { get; set; }
        public double Weight { get; set; }
    }
}

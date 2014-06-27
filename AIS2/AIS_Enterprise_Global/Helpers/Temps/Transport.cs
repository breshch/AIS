using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class Transport
    {
        public DirectoryRC DirectoryRC { get; set; }
        public DirectoryNote DirectoryNote { get; set; }
        public double Weight { get; set; }
    }
}

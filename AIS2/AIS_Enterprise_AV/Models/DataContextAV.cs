using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_AV.Models.Infos;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models
{
    public class DataContextAV : DataContext
    {
        public DataContextAV() : base("Default")
        {

        }

        public DbSet<InfoOverTime> InfoOverTimes { get; set; }

        public DbSet<DirectoryRC> DirectoryRCs { get; set; }
    }
}

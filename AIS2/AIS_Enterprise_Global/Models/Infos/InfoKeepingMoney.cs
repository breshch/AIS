using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Infos
{
    public class InfoKeepingMoney
    {
        public int Id { get; set; }
        public int DirectoryKeepingNameId { get; set; }
        public virtual DirectoryKeepingName DirectoryKeepingName { get; set; }

        public double Money { get; set; }

        public int DirectoryKeepingDescriptionId { get; set; }
        public virtual DirectoryKeepingDescription DirectoryKeepingDescription { get; set; }

        public string  AdvansedDescription { get; set; }
        public DateTime DateOfChange { get; set; }

    }
}

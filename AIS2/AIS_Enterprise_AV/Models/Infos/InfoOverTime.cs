using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models.Infos
{
    public class InfoOverTime
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<DirectoryRC> DirectoryRCs { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public InfoOverTime()
        {
            DirectoryRCs = new List<DirectoryRC>();
        }
    }
}

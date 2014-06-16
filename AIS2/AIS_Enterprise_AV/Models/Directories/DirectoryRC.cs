using AIS_Enterprise_AV.Models.Infos;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models.Directories
{
    public class DirectoryRC 
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        public int Percentes { get; set; }
        public int DirectoryCompanyId { get; set; }
        public virtual DirectoryCompany DirectoryCompany { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
    }
}

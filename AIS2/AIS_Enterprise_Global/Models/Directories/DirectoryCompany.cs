using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Directories
{
    public class DirectoryCompany
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }
    }
}

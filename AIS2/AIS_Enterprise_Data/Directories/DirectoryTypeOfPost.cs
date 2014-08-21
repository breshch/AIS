using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryTypeOfPost 
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }
    }
}

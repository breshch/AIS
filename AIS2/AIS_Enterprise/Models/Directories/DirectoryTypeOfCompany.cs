using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Models.Directories
{
    public class DirectoryTypeOfCompany
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelUniversalDB.Directories
{
    public class DirectoryCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DirectoryTypeOfCompanyId { get; set; }
        public DirectoryTypeOfCompany DirectoryTypeOfCompany { get; set; }
    }
}

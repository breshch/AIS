using ModelUniversalDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Currents
{
    public class CurrentCompany
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime? FireDate { get; set; }

        public int DirectoryCompanyId { get; set; }
        public DirectoryCompany DirectoryCompany { get; set; }
    }
}

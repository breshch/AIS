using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoTypeOfCompany
    {
        public int Id { get; set; }

        public int DirectoryTypeOfCompanyId { get; set; }
        public DirectoryTypeOfCompany DirectoryTypeOfCompany { get; set; }

        public ICollection<InfoCompany> InfoCompanies { get; set; }
    }
}

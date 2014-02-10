using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public ICollection<InfoCompany> InfoCompanies { get; set; }
    }
}

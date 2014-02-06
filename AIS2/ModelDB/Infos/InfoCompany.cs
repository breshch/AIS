using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoCompany
    {
        public int Id { get; set; }

        public int DirectoryCompanyId { get; set; }
        public DirectoryCompany DirectoryCompany { get; set; }

        public ICollection<InfoWorker> InfoWorkers { get; set; }
        public ICollection<InfoPermitForCar> InfoPermitForCars { get; set; }
    }
}

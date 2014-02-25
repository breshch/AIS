using ModelDB.Directories;
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
        public virtual DirectoryCompany DirectoryCompany { get; set; }

        public virtual ICollection<InfoWorker> InfoWorkers { get; set; }
        public virtual ICollection<InfoPermitForCar> InfoPermitForCars { get; set; }

        public InfoCompany()
        {
            InfoWorkers = new List<InfoWorker>();
            InfoPermitForCars = new List<InfoPermitForCar>();
        }
    }
}

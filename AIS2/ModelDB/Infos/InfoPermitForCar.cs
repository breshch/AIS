using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoPermitForCar
    {
        public int Id { get; set; }

        public int InfoDriverId { get; set; }
        public InfoDriver InfoDriver { get; set; }

        public int InfoCarId { get; set; }
        public InfoCar InfoCar { get; set; }

        public ICollection<InfoCar> InfoCargoes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Directories
{
    public class DirectoryCar
    {
        public int Id { get; set; }

        public int TypeOfCarId { get; set; }
        public virtual DirectoryTypeOfCar TypeOfCar { get; set; }

        public string StateNumber { get; set; }
        public string VINNumber { get; set; }

        //public int? PTSId { get; set; }
        //public virtual PTS PTS { get; set; }
    }
}

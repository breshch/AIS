using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(10)]
        public string StateNumber { get; set; }

        [StringLength(17)]
        public string VINNumber { get; set; }
    }
}

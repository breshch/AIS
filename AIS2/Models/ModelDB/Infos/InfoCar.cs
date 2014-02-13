using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoCar
    {
        public int Id { get; set; }

        public int DirectoryCarId { get; set; }
        public virtual DirectoryCar DirectoryCar { get; set; }
    }
}

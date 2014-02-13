using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoWorker
    {
        public int Id { get; set; }

        public int DirectoryWorkerId { get; set; }
        public virtual DirectoryWorker DirectoryWorker { get; set; }

        public double CountHours { get; set; }
        public TypeOfMissingHours TypeOfMissingHours { get; set; }

        public int? InfoPanaltyId { get; set; }
        public virtual InfoPanalty InfoPanalty { get; set; }
    }
}

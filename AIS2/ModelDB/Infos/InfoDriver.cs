using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoDriver
    {
        public int Id { get; set; }

        public int DirectoryWorkerId { get; set; }
        public DirectoryWorker DirectoryWorker { get; set; }
    }
}

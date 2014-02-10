using ModelUniversalDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Currents
{
    public class CurrentPost
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime? FireDate { get; set; }
        public double UserWorkerSalary { get; set; }
        public double? AdminWorkerSalary { get; set; }
        public double? UserHalfWorkerSalary { get; set; }

        public int DirectoryPostId { get; set; }
        public DirectoryPost DirectoryPost { get; set; }

    }
}

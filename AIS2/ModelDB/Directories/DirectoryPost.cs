using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelUniversalDB.Directories
{
    public class DirectoryPost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UserWorkerSalary { get; set; }
        public double? AdminWorkerSalary { get; set; }
        public double? UserHalfWorkerSalary { get; set; }


    }
}

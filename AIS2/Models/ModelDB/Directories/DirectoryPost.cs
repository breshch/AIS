using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Directories
{
    public class DirectoryPost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UserWorkerSalary { get; set; }
        public double? AdminWorkerSalary { get; set; }
        public double? UserHalfWorkerSalary { get; set; }
        
        public int DirectoryTypeOfPostId { get; set; }
        public DirectoryTypeOfPost DirectoryTypeOfPost { get; set; }

        public int DirectoryCompanyId { get; set; }
        public DirectoryCompany DirectoryCompany { get; set; }


    }
}

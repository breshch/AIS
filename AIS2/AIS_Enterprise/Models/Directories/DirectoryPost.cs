using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Models.Directories
{
    public class DirectoryPost
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public double UserWorkerSalary { get; set; }
        //public double? AdminWorkerSalary { get; set; }
        public double? UserWorkerHalfSalary { get; set; }
        
        public int DirectoryTypeOfPostId { get; set; }
        public virtual DirectoryTypeOfPost DirectoryTypeOfPost { get; set; }

        public int DirectoryCompanyId { get; set; }
        public virtual DirectoryCompany DirectoryCompany { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryPost
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public int DirectoryTypeOfPostId { get; set; }
        public virtual DirectoryTypeOfPost DirectoryTypeOfPost { get; set; }

        public int DirectoryCompanyId { get; set; }
        public virtual DirectoryCompany DirectoryCompany { get; set; }

        public virtual List<DirectoryPostSalary> DirectoryPostSalaries { get; set; }

        public DirectoryPost()
        {
            DirectoryPostSalaries = new List<DirectoryPostSalary>();
        }

        [NotMapped]
        public DateTime Date
        {
            get
            {
                if (DirectoryPostSalaries.Count() != 0)
                {
                    return DirectoryPostSalaries.OrderByDescending(s => s.Date).First().Date;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        [NotMapped]
        public double UserWorkerSalary
        {
            get
            {
                if (DirectoryPostSalaries.Count() != 0)
                {
                    return DirectoryPostSalaries.OrderByDescending(s => s.Date).First().UserWorkerSalary;
                }
                else
                {
                    return 0;
                }
            }
        }

        [NotMapped]
        public double? AdminWorkerSalary
        {
            get
            {
                if (DirectoryPostSalaries.Count() != 0)
                {
                    return DirectoryPostSalaries.OrderByDescending(s => s.Date).First().AdminWorkerSalary;
                }
                else
                {
                    return 0;
                }
            }
        }

        [NotMapped]
        public double? UserWorkerHalfSalary
        {
            get
            {
                if (DirectoryPostSalaries.Count() != 0)
                {
                    return DirectoryPostSalaries.OrderByDescending(s => s.Date).First().UserWorkerHalfSalary;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}

using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Temps
{
    public class CurrentCompanyAndPost
    {
        public DirectoryPost DirectoryPost { get; set; }
        public DateTime PostChangeDate { get; set; }
        public DateTime? PostFireDate { get; set; }
        public bool IsTwoCompanies { get; set; }
        public double Salary { get; set; }
    }
}

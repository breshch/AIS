using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
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

using System;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Temps
{
    public class CurrentCompanyAndPost
    {
        public DirectoryPost DirectoryPost { get; set; }
        public DateTime PostChangeDate { get; set; }
        public DateTime? PostFireDate { get; set; }
        public bool IsTwoCompanies { get; set; }
        public bool IsTemporaryPost { get; set; }
        public double Salary { get; set; }
    }
}

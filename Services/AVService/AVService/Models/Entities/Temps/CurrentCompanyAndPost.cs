using System;
using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Temps
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

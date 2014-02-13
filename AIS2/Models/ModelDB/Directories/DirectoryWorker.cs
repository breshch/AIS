using ModelDB.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Directories
{
    public class DirectoryWorker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FireDate { get; set; }
        public virtual ICollection<CurrentCompany> CurrentCompanies { get; set; }
        public virtual ICollection<CurrentPost> Posts { get; set; }
        //public Photo { get; set; }
        //public Docs { get; set; }

        //public DateTime? TimeStamp { get; set; }
    }
}

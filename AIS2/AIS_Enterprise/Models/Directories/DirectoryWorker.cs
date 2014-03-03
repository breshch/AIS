using AIS_Enterprise.Helpers;
using AIS_Enterprise.Models.Currents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Models.Directories
{
    public class DirectoryWorker
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string FirstName { get; set; }

        [StringLength(32)]
        public string MidName { get; set; }

        [StringLength(32)]
        public string LastName { get; set; }

        public Gender Gender { get; set; }
        public DateTime BirthDay { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(16)]
        public string HomePhone { get; set; }

        [StringLength(16)]
        public string CellPhone { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? FireDate { get; set; }

        public virtual ICollection<CurrentPost> CurrentCompaniesAndPosts { get; set; }
        //public Photo { get; set; }
        //public Docs { get; set; }

        //public DateTime? TimeStamp { get; set; }

        public DirectoryWorker()
        {
            CurrentCompaniesAndPosts = new List<CurrentPost>();
        }
    }
}

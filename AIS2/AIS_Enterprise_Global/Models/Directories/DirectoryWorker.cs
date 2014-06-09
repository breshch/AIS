using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Directories
{
    public class DirectoryWorker
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string LastName { get; set; }

        [StringLength(32)]
        public string FirstName { get; set; }

        [StringLength(32)]
        public string MidName { get; set; }

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

        public virtual ICollection<InfoDate> InfoDates { get; set; }
        
        public virtual ICollection<InfoMonth> InfoMonthes { get; set; }
        public bool IsDeadSpirit { get; set; }
 
        //public Photo { get; set; }
        //public Docs { get; set; }

        //public DateTime? TimeStamp { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName + " " + MidName;
            }
        }

        [NotMapped]
        public DirectoryPost CurrentDirectoryPost
        {
            get
            {
                return CurrentCompaniesAndPosts.OrderByDescending(p => p.ChangeDate).First().DirectoryPost;
            }
        }

        [NotMapped]
        public string Status
        {
            get 
            {
                return FireDate == null ? "Работает" : "Уволен: " + FireDate.Value.ToShortDateString();  
            }
        }

        public DirectoryWorker()
        {
            CurrentCompaniesAndPosts = new List<CurrentPost>();
            InfoDates = new List<InfoDate>();
            InfoMonthes = new List<InfoMonth>();
        }
    }
}

using System;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentPost
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime? FireDate { get; set; }

        public int DirectoryPostId { get; set; }
        public virtual DirectoryPost DirectoryPost { get; set; }
        public bool IsTwoCompanies { get; set; }
        public bool  IsTemporaryPost { get; set; }

        public int DirectoryWorkerId { get; set; }
    }
}

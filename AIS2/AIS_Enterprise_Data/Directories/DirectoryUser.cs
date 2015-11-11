using System.Collections.Generic;
using AIS_Enterprise_Data.Currents;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int CurrentUserStatusId { get; set; }
        public virtual CurrentUserStatus CurrentUserStatus { get; set; }
    }
}

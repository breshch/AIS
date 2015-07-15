using System.Collections.Generic;
using AIS_Enterprise_Data.Currents;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryUserStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<CurrentUserStatusPrivilege> Privileges { get; set; }

        public DirectoryUserStatus()
        {
            Privileges = new List<CurrentUserStatusPrivilege>();
        }
    }
}

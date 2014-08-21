using AIS_Enterprise_Data.Currents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

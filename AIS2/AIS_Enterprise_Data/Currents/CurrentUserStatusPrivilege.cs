using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentUserStatusPrivilege
    {
        public int Id { get; set; }
        public int DirecoryUserStatusPrivilegeId { get; set; }
        public virtual DirectoryUserStatusPrivilege DirectoryUserStatusPrivilege { get; set; }

        public int DirectoryUserStatusId { get; set; }
    }
}

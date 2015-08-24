using System.Collections.Generic;
using AVService.Models.Entities.Currents;

namespace AVService.Models.Entities.Directories
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

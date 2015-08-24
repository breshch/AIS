using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Currents
{
    public class CurrentUserStatusPrivilege
    {
        public int Id { get; set; }
        public int DirectoryUserStatusPrivilegeId { get; set; }
        public virtual DirectoryUserStatusPrivilege DirectoryUserStatusPrivilege { get; set; }

        public int DirectoryUserStatusId { get; set; }
    }
}

using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentUserStatusPrivilege
    {
        public int Id { get; set; }
        public int DirectoryUserStatusPrivilegeId { get; set; }
        public virtual DirectoryUserStatusPrivilege DirectoryUserStatusPrivilege { get; set; }

        public int DirectoryUserStatusId { get; set; }
    }
}

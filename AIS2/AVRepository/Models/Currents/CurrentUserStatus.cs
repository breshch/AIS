using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentUserStatus
    {
        public int Id { get; set; }
        public int DirectoryUserStatusId { get; set; }
        public virtual DirectoryUserStatus DirectoryUserStatus { get; set; }
    }
}

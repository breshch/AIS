using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Currents
{
    public class CurrentUserStatus
    {
        public int Id { get; set; }
        public int DirectoryUserStatusId { get; set; }
        public virtual DirectoryUserStatus DirectoryUserStatus { get; set; }
    }
}

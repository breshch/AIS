using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Currents
{
    public class CurrentRC
    {
        public int Id { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }

        public int InfoOverTimeId { get; set; }
    }
}

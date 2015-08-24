using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Temps
{
    public class Transport
    {
        public DirectoryRC DirectoryRC { get; set; }
        public DirectoryNote DirectoryNote { get; set; }
        public double Weight { get; set; }
    }
}

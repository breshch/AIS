using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Currents
{
    public class CurrentContainerCarPart
    {
        public int Id { get; set; }

        public int DirectoryCarPartId { get; set; }
        public virtual DirectoryCarPart DirectoryCarPart { get; set; }
        public int CountCarParts { get; set; }

        public int? InfoContainerId { get; set; }
    }
}

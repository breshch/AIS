using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Currents
{
    public class CurrentNote
    {
        public int Id { get; set; }

        public int DirectoryNoteId { get; set; }
        public virtual DirectoryNote DirectoryNote { get; set; }

        public int InfoCostId { get; set; }
    }
}

using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentNote
    {
        public int Id { get; set; }

        public int DirectoryNoteId { get; set; }
        public virtual DirectoryNote DirectoryNote { get; set; }

        public int InfoCostId { get; set; }
    }
}

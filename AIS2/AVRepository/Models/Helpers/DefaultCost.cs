using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Helpers
{
    public class DefaultCost
    {
        public int Id { get; set; }

        public int DirectoryCostItemId { get; set; }
        public virtual DirectoryCostItem DirectoryCostItem { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }
        
        public int DirectoryNoteId { get; set; }
        public virtual DirectoryNote DirectoryNote { get; set; }

        public double SummOfPayment { get; set; }
        public int DayOfPayment { get; set; }

    }
}

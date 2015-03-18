using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_Data.Currents
{
    public class CurrentRC
    {
        public int Id { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }

        public int InfoOverTimeId { get; set; }
    }
}

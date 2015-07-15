using System.ComponentModel.DataAnnotations;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryTypeOfPost 
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }
    }
}

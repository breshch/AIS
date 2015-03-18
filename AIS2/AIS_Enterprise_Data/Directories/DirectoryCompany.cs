using System.ComponentModel.DataAnnotations;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryCompany
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }
    }
}

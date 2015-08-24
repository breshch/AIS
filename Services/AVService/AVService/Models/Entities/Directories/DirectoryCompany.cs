using System.ComponentModel.DataAnnotations;

namespace AVService.Models.Entities.Directories
{
    public class DirectoryCompany
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }
    }
}

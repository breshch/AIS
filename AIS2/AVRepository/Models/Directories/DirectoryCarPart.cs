using System.ComponentModel.DataAnnotations.Schema;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryCarPart
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public string OriginalNumber { get; set; }
        public string FactoryNumber { get; set; }
        public string CrossNumber { get; set; }
        public string Material { get; set; }
        public string Attachment { get; set; }
        public bool IsImport { get; set; }

        public string CountInBox { get; set; }

        [NotMapped]
        public string FullCarPartName
        {
            get
            {
                return Article + Mark;
            }
        }
    }
}

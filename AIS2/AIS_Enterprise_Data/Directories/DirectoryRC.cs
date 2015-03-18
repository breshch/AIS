using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryRC 
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        public string DescriptionName { get; set; }
        public string ReportName { get; set; }
        public int Percentes { get; set; }
       

        [NotMapped]
        public bool IsChecked { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (Name == "ВСЕ" || Name == "26А")
                {
                    return Name;
                }



                return DescriptionName + " " + Name;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryCarPart
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public string OriginalNumber { get; set; }

        public CarPartNote Note { get; set; }

        public CarPartFactoryAndCross FactoryAndCross { get; set; }

        public string CountInBox { get; set; }

        [NotMapped]
        public string FullCarPartName
        {
            get
            {
                return Article + " " + Mark;
            }
        }
    }



    public class CarPartFactoryAndCross
    {
        public string FactoryNumber { get; set; }
        public string CrossNumber { get; set; }
    }

    public class CarPartNote
    {
        public string Material { get; set; }
        public string Attachment { get; set; }
    }


}

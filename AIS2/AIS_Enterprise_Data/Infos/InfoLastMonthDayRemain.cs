using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoLastMonthDayRemain
    {
        public int Id { get; set; }
       
        public int DirectoryCarPartId { get; set; }
        public virtual DirectoryCarPart  DirectoryCarPart { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }
    }
}

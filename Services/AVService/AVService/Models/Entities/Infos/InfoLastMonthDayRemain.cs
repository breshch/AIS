using System;
using AVService.Models.Entities.Directories;

namespace AVService.Models.Entities.Infos
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

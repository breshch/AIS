using System;
using Shared.Enums;

namespace AVService.Models.Entities.Infos
{
    public class InfoDate
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }
        public double? CountHours { get; set; }
        public DescriptionDay DescriptionDay { get; set; }

        public int? InfoPanaltyId { get; set; }
        public virtual InfoPanalty InfoPanalty { get; set; }

        public int DirectoryWorkerId { get; set; }
    }
}

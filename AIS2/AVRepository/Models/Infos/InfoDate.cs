using System;
using System.ComponentModel.DataAnnotations.Schema;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_Data.Infos
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

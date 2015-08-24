using System;
using System.Collections.Generic;
using AVService.Models.Entities.Currents;

namespace AVService.Models.Entities.Infos
{
    public class InfoContainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DatePhysical { get; set; }
        public DateTime? DateOrder { get; set; }
        public string Description { get; set; }
        public bool IsIncoming { get; set; }

        public virtual List<CurrentContainerCarPart> CarParts { get; set; }

        public InfoContainer()
        {
            CarParts = new List<CurrentContainerCarPart>();
        }
    }
}

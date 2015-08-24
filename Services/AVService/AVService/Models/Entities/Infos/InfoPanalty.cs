using System.ComponentModel.DataAnnotations;

namespace AVService.Models.Entities.Infos
{
    public class InfoPanalty
    {
        public int Id { get; set; }
        public double Summ { get; set; }
        
        [StringLength(256)]
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoPanalty
    {
        public int Id { get; set; }
        public double Summ { get; set; }
        
        [StringLength(256)]
        public string Description { get; set; }
    }
}

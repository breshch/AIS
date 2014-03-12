using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Infos
{
    public class InfoPanalty
    {
        public int Id { get; set; }
        public double SumOfMoney { get; set; }
        
        [StringLength(256)]
        public string Description { get; set; }
    }
}

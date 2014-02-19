using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoPanalty
    {
        public int Id { get; set; }
        public double Money { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
    }
}

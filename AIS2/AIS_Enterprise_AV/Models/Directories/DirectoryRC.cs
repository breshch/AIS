﻿using AIS_Enterprise_AV.Models.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models.Directories
{
    public class DirectoryRC 
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        //[ForeignKey("InfoOverTime")]
        //public int? InfoOverTimeId { get; set; }
        //public InfoOverTime InfoOverTime { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
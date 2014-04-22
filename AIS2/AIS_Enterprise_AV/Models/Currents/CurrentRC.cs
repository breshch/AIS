﻿using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_AV.Models.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Models.Currents
{
    public class CurrentRC 
    {
        public int Id { get; set; }

        public int DirectoryRCId { get; set; }
        public virtual DirectoryRC DirectoryRC { get; set; }
    }
}

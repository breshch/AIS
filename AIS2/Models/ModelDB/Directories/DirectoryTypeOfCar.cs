﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Directories
{
    public class DirectoryTypeOfCar
    {
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public double Sum { get; set; }
    }
}

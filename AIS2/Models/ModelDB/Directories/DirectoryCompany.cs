﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Directories
{
    public class DirectoryCompany
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        public int DirectoryTypeOfCompanyId { get; set; }
        public virtual DirectoryTypeOfCompany DirectoryTypeOfCompany { get; set; }
    }
}

﻿using ModelDB.Directories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Currents
{
    public class CurrentPost
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime? FireDate { get; set; }

        public int DirectoryPostId { get; set; }
        public virtual DirectoryPost DirectoryPost { get; set; }
    }
}

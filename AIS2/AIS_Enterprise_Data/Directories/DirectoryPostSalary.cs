﻿using System;

namespace AIS_Enterprise_Data.Directories
{
    public class DirectoryPostSalary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double UserWorkerSalary { get; set; }
        public double? AdminWorkerSalary { get; set; }
        public double? UserWorkerHalfSalary { get; set; }

        public int DirectoryPostId { get; set; }
    }
}

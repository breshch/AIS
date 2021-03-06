﻿namespace AIS_Enterprise_AV.Models
{
    public class WorkerPostReportSalary
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public double AdminWorkerSalary { get; set; }
        public int ChangePostDay { get; set; }
        public int CountWorkDays { get; set; }
        public double CountWorkHours { get; set; }
        public double CountWorkOverTimeHours { get; set; }
    }
}

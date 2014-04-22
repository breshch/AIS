using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.Temps
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

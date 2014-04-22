using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.Temps
{
    public class WorkerSummForReport
    {
        public int WorkerId { get; set; }
        public List<WorkerRCSummForReport> WorkerRCSummForReports { get; set; }

        public WorkerSummForReport()
        {
            WorkerRCSummForReports = new List<WorkerRCSummForReport>();
        }
    }
}

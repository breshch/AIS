using System.Collections.Generic;

namespace AIS_Enterprise_AV.Models
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

using System.Collections.Generic;

namespace AVClient.Helpers.Temps
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

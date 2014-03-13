using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class MonthTimeSheetWorker
    {
        public int WorkerId { get; set; }
        public string FullName { get; set; }
        public int DirectoryPostId { get; set; }
        public string PostName { get; set; }
        public double SalaryInHour { get; set; }
        public string[] Hours { get; set; }

        public double OverTime { get; set; }
        public int VocationDays { get; set; }
        public int SickDays { get; set; }
        public int MissDays { get; set; }

        public MonthTimeSheetWorker()
        {
            Hours = new string[31];
        }
    }
}

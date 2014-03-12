using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class MonthTimeSheetWorker
    {
        public string FullName { get; set; }
        public string PostName { get; set; }
        public double SalaryInHour { get; set; }
        public List<string> Hours { get; set; }

        public MonthTimeSheetWorker()
        {
            Hours = new List<string>();
        }
    }
}

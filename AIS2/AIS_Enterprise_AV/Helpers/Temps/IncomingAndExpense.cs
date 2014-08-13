using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Helpers.Temps
{
    public class IncomingAndExpense : PropertyChangedBase
    {
        public double Incoming { get; set; }
        public double Expense { get; set; }
    }
}

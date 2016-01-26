using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Models
{
    public class IncomingAndExpense : PropertyChangedBase
    {
        public double Incoming { get; set; }
        public double Expense { get; set; }
		public double ExpenseCompensation { get; set; }
    }
}

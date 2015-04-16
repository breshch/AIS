using System;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoSafe : IncomingAndExpenseAndSumm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CashType CashType { get; set; }
	    public string Bank { get; set; }
        public string Description { get; set; }
    }
}

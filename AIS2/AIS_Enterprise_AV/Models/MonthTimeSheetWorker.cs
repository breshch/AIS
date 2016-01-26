using System;

namespace AIS_Enterprise_AV.Models
{
    public class MonthTimeSheetWorker
    {
        public int WorkerId { get; set; }
        public int? WorkerSerialId { get; set; }
        public string FullName { get; set; }
        public int DirectoryPostId { get; set; }
        public DateTime PostChangeDate { get; set; }
        public string PostName { get; set; }
        public double SalaryInHour { get; set; }
        
        public string[] Hours { get; set; }

        public double OverTime { get; set; }
        public int VocationDays { get; set; }
        public int SickDays { get; set; }
        public int MissDays { get; set; }

        public string PrepaymentCash { get; set; }
        public string PrepaymentBankTransaction { get; set; }
        public string Compensation { get; set; }
        public string VocationPayment { get; set; }
        public string CardAV { get; set; }
        public string CardFenox { get; set; }
        public string Panalty { get; set; }
        public string Inventory { get; set; }
        public double? BirthDays { get; set; }
        public string Bonus { get; set; }
        public double? FinalSalary { get; set; }
        public bool IsOdd { get; set; }
        public bool IsDeadSpirit { get; set; }

	    public bool IsFired { get; set; }
    }
}

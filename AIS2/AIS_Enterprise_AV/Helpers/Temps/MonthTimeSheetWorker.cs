using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.Helpers.Temps
{
    public class MonthTimeSheetWorker
    {
        public int WorkerId { get; set; }
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
        public string VocationPayment { get; set; }
        public string SalaryAV { get; set; }
        public string SalaryFenox { get; set; }
        public string Panalty { get; set; }
        public string Inventory { get; set; }
        public double? BirthDays { get; set; }
        public string Bonus { get; set; }
        public double? FinalSalary { get; set; }
    }
}

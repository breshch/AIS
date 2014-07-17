using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Models.Infos
{
    public class InfoMonth
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CountVacationDays { get; set; }
        public int CountSickDays { get; set; }
        public int CountMissDays { get; set; }
        public double PrepaymentCash { get; set; }
        public double PrepaymentBankTransaction { get; set; }
        public double Compensation { get; set; }
        public double VocationPayment { get; set; }
        public double CardAV { get; set; }
        public double CardFenox { get; set; }
        public double Panalty { get; set; }
        public double Inventory { get; set; }
        public double BirthDays { get; set; }
        public double Bonus { get; set; }
        public int DirectoryWorkerId { get; set; }
    }
}

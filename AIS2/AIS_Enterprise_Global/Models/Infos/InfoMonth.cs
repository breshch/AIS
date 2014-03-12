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
        public double PrePayment { get; set; }
        public double PrePaymentBR { get; set; }
        public double VacationBR { get; set; }
        public double SalaryBR { get; set; }
        public double Inventory { get; set; }
        public double BirthdayPay { get; set; }
        public double Bonus { get; set; }
    }
}

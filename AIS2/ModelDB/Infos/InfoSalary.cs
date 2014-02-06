using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDB.Infos
{
    public class InfoSalary
    {
        public int Id { get; set; }
        
        public int DirectoryWorkerId { get; set; }
        public DirectoryWorker DirectoryWorker { get; set; }

        public int CountVacationDays { get; set; }
        public int CountSickDays { get; set; }
        public int CountMissDays { get; set; }
        public double PrePayment { get; set; }
        public double PrePaymentBR { get; set; }
        public double VacationBR { get; set; }
        public double Inventory { get; set; }
        public double BirthdayPay { get; set; }
        public double Bonus { get; set; }
    }
}

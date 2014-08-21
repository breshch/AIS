using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoSafe
    {
        public int Id { get; set; }
        
        public int? DirectoryLoanTakerId { get; set; }
        public virtual DirectoryLoanTaker DirectoryLoanTaker { get; set; }

        public int? DirectoryWorkerId { get; set; }
        public virtual DirectoryWorker DirectoryWorker { get; set; }
       
        public double Summ { get; set; }
        public int? CountPayments { get; set; }

        public DateTime DateLoan { get; set; }
        public DateTime? DateLoanPayment { get; set; }

        public virtual IEnumerable<InfoPayment> InfoPayments { get; set; }

        [MaxLength(512)]
        public string  Description { get; set; }

        public InfoSafe()
        {
            InfoPayments = new List<InfoPayment>();
        }

        [NotMapped]
        public double RemainingSumm
        {
            get 
            {
                double summPayments = 0;
                if (InfoPayments.Any())
                {
                    summPayments = InfoPayments.Sum(p => p.Summ);
                }

                return Summ - summPayments;
            }
        }

        [NotMapped]
        public string LoanTakerName
        {
            get
            {
                return DirectoryWorkerId != null ? DirectoryWorker.FullName : DirectoryLoanTaker.Name;
            }
        }
    }

}

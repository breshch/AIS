﻿using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoPrivateLoan : IncomingAndExpenseAndSumm
    {
        public int Id { get; set; }
        
        public int? DirectoryLoanTakerId { get; set; }
        public virtual DirectoryLoanTaker DirectoryLoanTaker { get; set; }

        public int? DirectoryWorkerId { get; set; }
        public virtual DirectoryWorker DirectoryWorker { get; set; }
       
        public int CountPayments { get; set; }

        public DateTime DateLoan { get; set; }
        public DateTime? DateLoanPayment { get; set; }

        public virtual IEnumerable<InfoPrivatePayment> InfoPayments { get; set; }

        [MaxLength(512)]
        public string  Description { get; set; }

        public InfoPrivateLoan()
        {
            InfoPayments = new List<InfoPrivatePayment>();
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
        public string RemainingSummCurrency
        {
            get
            {
                return Converting.DoubleToCurrency(RemainingSumm, Currency);
            }
        }

        [NotMapped]
        public string SummCurrency
        {
            get
            {
                return Converting.DoubleToCurrency(Summ, Currency);
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
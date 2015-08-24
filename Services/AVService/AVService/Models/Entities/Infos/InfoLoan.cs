using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Temps;

namespace AVService.Models.Entities.Infos
{
    public class InfoLoan : IncomingAndExpenseAndSumm
    {
        public int Id { get; set; }
        
        public int? DirectoryLoanTakerId { get; set; }
        public virtual DirectoryLoanTaker DirectoryLoanTaker { get; set; }

        public int? DirectoryWorkerId { get; set; }
        public virtual DirectoryWorker DirectoryWorker { get; set; }
       
        public int CountPayments { get; set; }

        public DateTime DateLoan { get; set; }
        public DateTime? DateLoanPayment { get; set; }

        public virtual IEnumerable<InfoPayment> InfoPayments { get; set; }

        [MaxLength(512)]
        public string  Description { get; set; }

        public InfoLoan()
        {
            InfoPayments = new List<InfoPayment>();
        }

	    [NotMapped]
	    public double RemainingSumm { get; set; }

	    //public double RemainingSumm //TOD Refactor
		//{
		//    get 
		//    {
		//        double summPayments = 0;
		//        if (InfoPayments.Any())
		//        {
		//            summPayments = InfoPayments.Sum(p => p.Summ);
		//        }

		//        return Summ - summPayments;
		//    }
		//}

	    [NotMapped]
	    public string RemainingSummCurrency { get; set; }

	    //public string RemainingSummCurrency
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(RemainingSumm, Currency);
		//    }
		//}

	    [NotMapped]
	    public string SummCurrency { get; set; }

	    //public string SummCurrency
		//{
		//    get
		//    {
		//        return Converting.DoubleToCurrency(Summ, Currency);
		//    }
		//}

	    [NotMapped]
	    public string LoanTakerName { get; set; }

	    //public string LoanTakerName
		//{
		//    get
		//    {
		//        return DirectoryWorkerId != null ? DirectoryWorker.FullName : DirectoryLoanTaker.Name;
		//    }
		//}
	}

}

using System;

namespace AVService.Models.Entities.Infos
{
    public class InfoPayment
    {
        public int Id { get; set; }
        public double Summ { get; set; }
        public DateTime Date { get; set; }

        public int InfoLoanId { get; set; }
    }
}

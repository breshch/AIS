using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoPrivatePayment
    {
        public int Id { get; set; }
        public double Summ { get; set; }
        public DateTime Date { get; set; }

        public int InfoPrivateLoanId { get; set; }
    }
}

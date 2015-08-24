using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace AVService.Models.Entities.Temps
{
    public class IncomingAndExpenseAndSumm
    {
        public bool IsIncoming { get; set; }
        public double Summ { get; set; }
        public Currency Currency { get; set; }

        [NotMapped]
        public string Incoming
        {
            get
            {
                if (IsIncoming)
                {
                    string tmp = Summ.ToString("c");
                    tmp = tmp.Substring(0, tmp.LastIndexOf(" "));

                    return tmp + " " + Currency;
                }
                else
                {
                    return null;
                }
            }
        }

        [NotMapped]
        public string Expense
        {
            get
            {
                if (!IsIncoming)
                {
                    string tmp = Summ.ToString("c");
                    tmp = tmp.Substring(0, tmp.LastIndexOf(" "));

                    return tmp + " " + Currency;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}

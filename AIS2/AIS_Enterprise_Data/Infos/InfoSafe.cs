using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data.Infos
{
    public class InfoSafe
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsIncoming { get; set; }
        public double Summ { get; set; }
        public CashType CashType { get; set; }

        [NotMapped]
        public double Incoming 
        {
            get 
            {
                return IsIncoming ? Summ : 0;
            }
        }
        
        [NotMapped]
        public double Expense
        {
            get
            {
                return !IsIncoming ? Summ : 0;
            }
        }
    }
}

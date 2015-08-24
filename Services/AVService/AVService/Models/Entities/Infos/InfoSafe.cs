using System;
using AVService.Models.Entities.Temps;
using Shared.Enums;

namespace AVService.Models.Entities.Infos
{
    public class InfoSafe : IncomingAndExpenseAndSumm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CashType CashType { get; set; }
	    public string Bank { get; set; }
        public string Description { get; set; }
    }
}

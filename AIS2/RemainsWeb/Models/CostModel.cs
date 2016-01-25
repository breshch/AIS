using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Global.Helpers;

namespace RemainsWeb.Models
{
	public class CostModel
	{
		public DateTime Date { get; set; }
		public string CostItem { get; set; }
		public double Summ { get; set; }
		public bool IsIncoming { get; set; }
		public Currency Currency { get; set; }
		public string Description { get; set; }
	}
}

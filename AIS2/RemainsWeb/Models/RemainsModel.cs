using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Global.Helpers;

namespace RemainsWeb.Models
{
	public class RemainsModel
	{
		public Dictionary<Currency, double> CurrentRemains { get; set; }
		public CostModel[] CurrentCosts { get; set; }
	}
}

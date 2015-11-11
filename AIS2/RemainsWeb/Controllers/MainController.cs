using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;

namespace RemainsWeb.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
	        var currentRemains = GetCurrentRemains();

			return View(currentRemains);
        }


	    private Dictionary<Currency, double> GetCurrentRemains()
	    {
		    using (var bc = new BusinessContext())
		    {
			    var date = DateTime.Now;
			    DateTime startDate = date;

			    int counter = 0;
			    double? minskSum = null;
			    while (!minskSum.HasValue)
			    {
				    counter++;
				    minskSum = bc.GetTotalEqualCashSafeToMinsks(startDate);
				    if (!minskSum.HasValue)
				    {
					    startDate = startDate.AddMonths(-1);
				    }

				    if (counter > 12)
				    {
					    break;
				    }
			    }

				var sums = new Dictionary<Currency, double>();
			    if (minskSum.HasValue)
			    {
				    var costsToPeriod = bc.GetInfoCosts(new DateTime(startDate.Year, startDate.Month, 1), date.AddMonths(1)).ToArray();
				    var costsSum = costsToPeriod
					    .Where(x => x.Currency == Currency.RUR)
					    .Sum(x => x.IsIncoming ? x.Summ : -x.Summ);

				    double sumRUR = minskSum.Value + costsSum;
					sums.Add(Currency.RUR, sumRUR);

				    var currencies = Enum.GetNames(typeof (Currency))
						.Select(x => (Currency) Enum.Parse(typeof (Currency), x))
						.Where(x => x != Currency.RUR)
						.ToArray();

				    foreach (var currency in currencies)
				    {
					    double sum = costsToPeriod.Where(x => x.Currency == currency)
						    .Sum(x => x.IsIncoming ? x.Summ : -x.Summ);

					    if (sum == 0)
					    {
						    continue;
					    }

						sums.Add(currency, sum);
				    }

				    return sums;
			    }
			    else
			    {
				    throw new Exception("Error");
			    }
		    }
	    }
    }
}
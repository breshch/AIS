using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using FluentScheduler;
using RemainsWeb.Models;

namespace RemainsWeb.Infrastructure
{
	public static class CustomCache
	{
		private static readonly MemoryCache Cache = MemoryCache.Default;

		static CustomCache()
		{
			TaskManager.AddTask(UpdateRemainsAndCosts, x => x.ToRunEvery(30).Minutes());
		}

		private static void UpdateRemainsAndCosts()
		{
			var model = GetRemainsAndCostsDb();
			AddToCache("RemainsAndCosts", model);
		}

		public static RemainsModel GetRemainsAndCosts()
		{
			if (!Cache.Contains("RemainsAndCosts"))
			{
				UpdateRemainsAndCosts();
			}

			return (RemainsModel)Cache.Get("RemainsAndCosts");
		}

		private static RemainsModel GetRemainsAndCostsDb()
		{
			var model = new RemainsModel
			{
				CurrentRemains = GetCurrentRemains(),
				CurrentCosts = GetCurrentCosts()
			};
			return model;
		}

		private static Dictionary<Currency, double> GetCurrentRemains()
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
					var costsToPeriod =
						bc.GetInfoCosts(new DateTime(startDate.Year, startDate.Month, 1), date.AddMonths(1)).ToArray();
					var costsSum = costsToPeriod
						.Where(x => x.Currency == Currency.RUR)
						.Sum(x => x.IsIncoming ? x.Summ : -x.Summ);

					double sumRUR = minskSum.Value + costsSum;
					sums.Add(Currency.RUR, sumRUR);

					var currencies = Enum.GetNames(typeof(Currency))
						.Select(x => (Currency)Enum.Parse(typeof(Currency), x))
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
				}

				return sums;
			}
		}

		private static CostModel[] GetCurrentCosts()
		{
			using (var bc = new BusinessContext())
			{
				var dateTo = DateTime.Now;
				var dateFrom = new DateTime(dateTo.Year, dateTo.Month, 1);

				var costs = bc.GetInfoCosts(dateFrom, dateTo)
					.OrderByDescending(x => x.Date)
					.ToArray();

				return costs
					.Select(x => new CostModel
					{
						Date = x.Date,
						CostItem = x.DirectoryCostItem.Name.IndexOf("(") != -1
							? x.DirectoryCostItem.Name.Substring(0, x.DirectoryCostItem.Name.IndexOf("(") - 1)
							: x.DirectoryCostItem.Name,
						Summ = x.Summ,
						IsIncoming = x.IsIncoming,
						Currency = x.Currency,
						Description = x.Weight != 0
							? x.Weight + " кг."
							: x.ConcatNotes[0].ToString().ToUpper() +
								new string(Enumerable.Repeat('*', x.ConcatNotes.Length - 2).ToArray()) +
								x.ConcatNotes[x.ConcatNotes.Length - 1]
					})
					.ToArray();
			}
		}

		private static void AddToCache(string key, object value)
		{
			Cache.Add(key, value, new CacheItemPolicy());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AVRepository.Repositories
{
	public class CostRepository : BaseRepository
	{
		private readonly UtilRepository utilRepository;

		public CostRepository(UtilRepository utilRepository)
		{
			this.utilRepository = utilRepository;
		}

		#region InfoTotalEqualCashSafeToMinsk

		public InfoTotalEqualCashSafeToMinsk[] GetTotalEqualCashSafeToMinsks(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoTotalEqualCashSafeToMinsks
					.Where(c => DbFunctions.DiffDays(c.Date, from) <= 0 && DbFunctions.DiffDays(c.Date, to) >= 0)
					.ToArray()
					.GroupBy(c => c.Date)
					.Select(g => g.OrderByDescending(c => c.LastUpdated).First())
					.ToArray();
			}
		}

		public void SaveTotalSafeAndMinskCashes(DateTime date, double minskSumm)
		{
			using (var db = GetContext())
			{
				var totalCash = new InfoTotalEqualCashSafeToMinsk();

				totalCash.Date = new DateTime(date.Year, date.Month, 1);
				totalCash.MinskCash = minskSumm;
				totalCash.LastUpdated = DateTime.Now;

				var prevDate = date.AddMonths(-1);
				var prevTotalCash =
					db.InfoTotalEqualCashSafeToMinsks.First(c => c.Date.Year == prevDate.Year && c.Date.Month == prevDate.Month);

				var costs = GetInfoCosts(prevDate.Year, prevDate.Month).ToList();
				double totalSumm = costs.Sum(c => c.IsIncoming ? c.Summ : -c.Summ);

				totalCash.SafeCash = prevTotalCash.SafeCash + totalSumm;

				db.InfoTotalEqualCashSafeToMinsks.Add(totalCash);
				db.SaveChanges();

				var monthes = db.InfoTotalEqualCashSafeToMinsks.Where(c => DbFunctions.DiffDays(c.Date, date) < 0).ToArray();
				if (monthes.Any())
				{
					foreach (var month in monthes)
					{
						var totalCashMonth = new InfoTotalEqualCashSafeToMinsk
						{
							Date = month.Date,
							MinskCash = month.MinskCash,
							LastUpdated = DateTime.Now
						};

						var prevDateMonth = month.Date.AddMonths(-1);
						var prevTotalCashMonth =
							db.InfoTotalEqualCashSafeToMinsks.First(c => c.Date.Year == prevDateMonth.Year &&
																		 c.Date.Month == prevDateMonth.Month);

						var costsMonth = GetInfoCosts(prevDateMonth.Year, prevDateMonth.Month).ToList();
						double totalSummMonth = costsMonth.Sum(c => c.IsIncoming ? c.Summ : -c.Summ);

						totalCashMonth.SafeCash = prevTotalCashMonth.SafeCash + totalSummMonth;
						db.InfoTotalEqualCashSafeToMinsks.Add(totalCashMonth);
					}

					db.SaveChanges();
				}
			}
		}

		#endregion

		#region CurrencyValue

		public CurrencyValue GetCurrencyValue(string name)
		{
			using (var db = GetContext())
			{
				var currencyValue = db.CurrencyValues.First(c => c.Name == name);
				db.Entry(currencyValue).Reload();

				return currencyValue;
			}
		}

		public double GetCurrencyValueSumm(string name, Currency currency)
		{
			var currencyValue = GetCurrencyValue(name);

			double summ = 0;
			switch (currency)
			{
				case Currency.RUR:
					summ = currencyValue.RUR;
					break;
				case Currency.USD:
					summ = currencyValue.USD;
					break;
				case Currency.EUR:
					summ = currencyValue.EUR;
					break;
				case Currency.BYR:
					summ = currencyValue.BYR;
					break;
				default:
					break;
			}

			return summ;
		}

		public void EditCurrencyValueSumm(string name, Currency currency, double summ)
		{
			using (var db = GetContext())
			{
				var currencyValue = GetCurrencyValue(name);

				switch (currency)
				{
					case Currency.RUR:
						currencyValue.RUR = summ;
						break;
					case Currency.USD:
						currencyValue.USD = summ;
						break;
					case Currency.EUR:
						currencyValue.EUR = summ;
						break;
					case Currency.BYR:
						currencyValue.BYR = summ;
						break;
					default:
						break;
				}

				db.SaveChanges();
			}
		}

		public void EditCurrencyValueSummChange(string name, Currency currency, double summ)
		{
			using (var db = GetContext())
			{
				var currencyValue = GetCurrencyValue(name);

				switch (currency)
				{
					case Currency.RUR:
						currencyValue.RUR += summ;
						break;
					case Currency.USD:
						currencyValue.USD += summ;
						break;
					case Currency.EUR:
						currencyValue.EUR += summ;
						break;
					case Currency.BYR:
						currencyValue.BYR += summ;
						break;
					default:
						break;
				}

				db.SaveChanges();
			}
		}

		#endregion

		#region DirectoryLoanTaker

		public DirectoryLoanTaker[] GetDirectoryLoanTakers()
		{
			using (var db = GetContext())
			{
				return db.DirectoryLoanTakers.ToArray();
			}
		}

		public DirectoryLoanTaker AddDirectoryLoanTaker(string name)
		{
			using (var db = GetContext())
			{
				var loanTaker = new DirectoryLoanTaker
				{
					Name = name
				};

				db.DirectoryLoanTakers.Add(loanTaker);
				db.SaveChanges();

				return loanTaker;
			}
		}

		#endregion


		#region InfoPayments

		public InfoPayment[] GetInfoPayments(int infoLoanId)
		{
			using (var db = GetContext())
			{
				return db.InfoPayments.Where(p => p.InfoLoanId == infoLoanId).OrderBy(p => p.Date).ToArray();
			}
		}

		public InfoPayment AddInfoPayment(int infoLoanId, DateTime date, double summ)
		{
			using (var db = GetContext())
			{
				var infoPayment = new InfoPayment
				{
					Date = date,
					Summ = summ,
					InfoLoanId = infoLoanId
				};

				db.InfoPayments.Add(infoPayment);
				db.SaveChanges();

				var infoLoan = db.InfoLoans.Find(infoLoanId);
				EditCurrencyValueSummChange("TotalLoan", infoLoan.Currency, -summ);
				AddInfoSafe(date, true, summ, infoLoan.Currency, CashType.Наличка, "Возврат долга: " +
																				   (infoLoan.DirectoryLoanTakerId == null
																					   ? infoLoan.DirectoryWorker.FullName
																					   : infoLoan.DirectoryLoanTaker.Name));

				return infoPayment;
			}
		}

		public void RemoveInfoPayment(InfoPayment selectedInfoPayment)
		{
			using (var db = GetContext())
			{
				var currency = db.InfoLoans.Find(selectedInfoPayment.InfoLoanId).Currency;
				EditCurrencyValueSummChange("TotalLoan", currency, selectedInfoPayment.Summ);

				db.InfoPayments.Remove(selectedInfoPayment);
				db.SaveChanges();
			}
		}

		#endregion


		#region InfoCard

		public void SetCardAvaliableSumm(string cardName, double avaliableSumm)
		{
			using (var db = GetContext())
			{
				db.InfoCards.First(c => c.CardName == cardName).AvaliableSumm = avaliableSumm;
				db.SaveChanges();
			}
		}

		public double GetCardAvaliableSumm(string cardName)
		{
			using (var db = GetContext())
			{
				return db.InfoCards.First(c => c.CardName == cardName).AvaliableSumm;
			}
		}

		#endregion


		#region InfoSafe

		public InfoSafe AddInfoSafe(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description, string bankName = null)
		{
			using (var db = GetContext())
			{
				var infoSafe = new InfoSafe
				{
					Date = date,
					IsIncoming = isIncoming,
					Summ = summCash,
					Currency = currency,
					CashType = cashType,
					Description = description,
					Bank = bankName
				};

				db.InfoSafes.Add(infoSafe);
				db.SaveChanges();

				if (cashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
				}

				return infoSafe;
			}
		}

		public InfoSafe AddInfoSafeHand(DateTime date, bool isIncoming, double summCash, Currency currency, CashType cashType,
			string description)
		{
			using (var db = GetContext())
			{
				var infoSafe = new InfoSafe
				{
					Date = date,
					IsIncoming = isIncoming,
					Summ = summCash,
					Currency = currency,
					CashType = cashType,
					Description = description
				};

				db.InfoSafes.Add(infoSafe);
				db.SaveChanges();

				if (cashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", isIncoming, summCash, currency);
					CalcTotalSumm("TotalSafe", !isIncoming, summCash, currency);
				}

				return infoSafe;
			}
		}

		private void CalcTotalSumm(string totalSummName, bool isIncoming, double summCash, Currency currency)
		{
			double summ = GetCurrencyValueSumm(totalSummName, currency);

			summ += isIncoming ? summCash : -summCash;

			EditCurrencyValueSumm(totalSummName, currency, summ);
		}

		public InfoSafe AddInfoSafeCard(DateTime date, double availableSumm, Currency currency, string description,
			string bankName)
		{
			using (var db = GetContext())
			{
				double prevAvailableSumm = GetCurrencyValue("TotalCard").RUR;

				double summ = Math.Round(availableSumm - prevAvailableSumm, 2);
				bool isIncoming = summ >= 0;

				if (!db.InfoSafes.Any(
						s => DbFunctions.DiffSeconds(s.Date, date) == 0 && s.CashType == CashType.Карточка && s.Description == description))
				{
					EditCurrencyValueSumm("TotalCard", Currency.RUR, availableSumm);

					return AddInfoSafe(date, isIncoming, Math.Abs(summ), currency, CashType.Карточка, description, bankName);
				}

				return null;
			}
		}

		public bool IsNewMessage(DateTime date, string description)
		{
			using (var db = GetContext())
			{
				return !db.InfoSafes.Any(s => DbFunctions.DiffDays(s.Date, date) == 0 &&
											  s.CashType == CashType.Карточка &&
											  s.Description == description);
			}
		}

		public InfoSafe[] GetInfoSafes()
		{
			using (var db = GetContext())
			{
				return db.InfoSafes.OrderByDescending(s => s.Date).ToArray();
			}
		}

		public InfoSafe[] GetInfoSafes(CashType cashType, DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return
					db.InfoSafes.Where(
						s => s.CashType == cashType && DbFunctions.DiffDays(from, s.Date) >= 0 && DbFunctions.DiffDays(to, s.Date) <= 0)
						.OrderByDescending(s => s.Date)
						.ToArray();
			}
		}

		public void RemoveInfoSafe(InfoSafe infoSafe)
		{
			using (var db = GetContext())
			{
				if (infoSafe.CashType == CashType.Наличка)
				{
					CalcTotalSumm("TotalCash", !infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
					CalcTotalSumm("TotalSafe", infoSafe.IsIncoming, infoSafe.Summ, infoSafe.Currency);
				}

				db.InfoSafes.Remove(infoSafe);
				db.SaveChanges();
			}
		}

		#endregion


		#region InfoPrivatePayments

		public InfoPrivatePayment[] GetInfoPrivatePayments(int infoSafeId)
		{
			using (var db = GetContext())
			{
				return db.InfoPrivatePayments.Where(p => p.InfoPrivateLoanId == infoSafeId).OrderBy(p => p.Date).ToArray();
			}
		}

		public InfoPrivatePayment AddInfoPrivatePayment(int infoPrivateLoanId, DateTime date, double summ)
		{
			using (var db = GetContext())
			{
				var infoPrivatePayment = new InfoPrivatePayment
				{
					Date = date,
					Summ = summ,
					InfoPrivateLoanId = infoPrivateLoanId
				};

				db.InfoPrivatePayments.Add(infoPrivatePayment);
				db.SaveChanges();

				var currency = db.InfoPrivateLoans.Find(infoPrivateLoanId).Currency;
				EditCurrencyValueSummChange("TotalPrivateLoan", currency, -summ);

				return infoPrivatePayment;
			}
		}

		public void RemoveInfoPrivatePayment(InfoPrivatePayment selectedInfoPrivatePayment)
		{
			using (var db = GetContext())
			{
				var currency = db.InfoPrivateLoans.Find(selectedInfoPrivatePayment.InfoPrivateLoanId).Currency;
				EditCurrencyValueSummChange("TotalPrivateLoan", currency, selectedInfoPrivatePayment.Summ);

				db.InfoPrivatePayments.Remove(selectedInfoPrivatePayment);
				db.SaveChanges();
			}
		}

		#endregion

		#region InfoCost

		public InfoCost EditInfoCost(DateTime date, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note,
			bool isIncomming, double summ, Currency currency, double weight)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCosts(date);
				var infoCost =
					infoCosts.FirstOrDefault(
						c =>
							c.DirectoryCostItem.Id == costItem.Id && c.DirectoryRCId == rc.Id &&
							c.CurrentNotes.First().DirectoryNoteId == note.Id);
				if (infoCost == null)
				{
					infoCost = new InfoCost
					{
						GroupId = Guid.NewGuid(),
						Date = date,
						DirectoryCostItemId = costItem.Id,
						DirectoryRC = rc,
						CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNote = note } },
						Summ = summ,
						Currency = currency,
						IsIncoming = isIncomming,
						Weight = weight,
					};

					db.InfoCosts.Add(infoCost);

					AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
						rc.Name + " " + infoCost.ConcatNotes);
				}
				else
				{
					double prevSumm = infoCost.Summ;

					infoCost.Summ = summ;
					infoCost.Currency = currency;
					infoCost.IsIncoming = isIncomming;
					infoCost.Weight = weight;

					if (infoCost.Summ - prevSumm != 0)
					{
						AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ - prevSumm, currency, CashType.Наличка,
							infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
					}
				}

				db.SaveChanges();
				return infoCost;
			}
		}

		public InfoCost[] GetInfoCosts(DateTime date)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => DbFunctions.DiffDays(date, c.Date) == 0)
					.ToArray();
			}
		}

		public InfoCost GetInfoCost(int infoCostId)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts.Find(infoCostId);
			}
		}

		public InfoCost[] GetInfoCosts(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Include(c => c.DirectoryCostItem)
					.Include(c => c.DirectoryRC)
					.Include(c => c.DirectoryTransportCompany)
					.Include(c => c.CurrentNotes.Select(n => n.DirectoryNote))
					.Where(c => c.Date.Year == year && c.Date.Month == month)
					.OrderBy(c => c.Date)
					.ToArray();
			}
		}

		public InfoCost[] GetInfoCostsRCIncoming(int year, int month, string rcName)
		{

			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year &&
								c.Date.Month == month &&
								c.DirectoryRC.Name == rcName &&
								c.DirectoryCostItem.Name == "Приход")
					.ToArray();
			}
		}

		public InfoCost[] GetInfoCosts26Expense(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year &&
								c.Date.Month == month &&
								c.DirectoryRC.Name == "26А" &&
								!c.IsIncoming)
					.ToArray();
			}
		}


		public InfoCost[] GetInfoCostsRCAndAll(int year, int month, string rcName)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCosts(year, month).ToList();
				var infoCostsRC = infoCosts.Where(c => c.DirectoryRC.Name == rcName && c.Currency == Currency.RUR).ToList();

				if (db.DirectoryRCs.First(r => r.Name == rcName).Percentes > 0)
				{
					infoCostsRC.AddRange(infoCosts.Where(c => c.DirectoryRC.Name == "ВСЕ" && c.Currency == Currency.RUR).ToList());
				}

				return infoCostsRC.ToArray();
			}
		}

		public InfoCost[] GetInfoCostsPAM16(int year, int month)
		{
			var infoCosts = GetInfoCosts(year, month).ToList();
			var infoCostsRC = infoCosts.Where(c => c.DirectoryRC.Name == "ПАМ-16" && c.Currency == Currency.RUR).ToList();

			infoCostsRC.AddRange(infoCosts.Where(c => c.DirectoryRC.Name == "ВСЕ" && c.Currency == Currency.RUR).ToList());

			return infoCostsRC.ToArray();
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnly(int year, int month)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Include(c => c.DirectoryCostItem)
					.Include(c => c.DirectoryRC)
					.Include(c => c.DirectoryTransportCompany)
					.Include(c => c.CurrentNotes.Select(n => n.DirectoryNote))
					.Where(c => !c.IsIncoming && c.DirectoryCostItem.Name == "Транспорт (5031)" && c.DirectoryRC.Name != "ВСЕ")
					.ToArray();
			}
		}

		public InfoCost[] GetInfoCostsTransportAndNoAllAndExpenseOnly(DateTime date)
		{
			return GetInfoCosts(date)
				.Where(c => !c.IsIncoming &&
					c.DirectoryCostItem.Name == "Транспорт (5031)" &&
					c.DirectoryRC.Name != "ВСЕ")
				.ToArray();
		}

		public void AddInfoCosts(DateTime date, DirectoryCostItem directoryCostItem, bool isIncoming,
			DirectoryTransportCompany transportCompany, double summ, Currency currency, List<Transport> transports)
		{
			using (var db = GetContext())
			{
				var groupId = Guid.NewGuid();
				if (directoryCostItem.Name == "Транспорт (5031)" && (transports[0].DirectoryRC.Name != "26А" || !isIncoming))
				{
					double commonWeight = transports.Sum(t => t.Weight);

					int indexCargo = 1;

					double totalSummTransport = 0;
					var cargos = transports.Select(t => t.DirectoryRC.Name).Distinct();

					foreach (var rc in cargos)
					{
						var currentNotes = new List<CurrentNote>();
						double weightRC = 0;

						foreach (var transport in transports.Where(t => t.DirectoryRC.Name == rc))
						{
							currentNotes.Add(new CurrentNote { DirectoryNote = db.DirectoryNotes.Find(transport.DirectoryNote.Id) });
							weightRC += transport.Weight;
						}

						double summTransport = 0;
						if (indexCargo < cargos.Count())
						{
							summTransport = weightRC != 0 ? Math.Round(summ / commonWeight * weightRC, 0) : summ;
							totalSummTransport += summTransport;
						}
						else
						{
							summTransport = summ - totalSummTransport;
						}

						var infoCost = new InfoCost
						{
							GroupId = groupId,
							Date = date,
							DirectoryCostItem = db.DirectoryCostItems.Find(directoryCostItem.Id),
							DirectoryRC = db.DirectoryRCs.First(r => r.Name == rc),
							IsIncoming = isIncoming,
							DirectoryTransportCompany =
								transportCompany != null ? db.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
							Summ = summTransport,
							Currency = currency,
							CurrentNotes = currentNotes,
							Weight = weightRC
						};

						db.InfoCosts.Add(infoCost);

						AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
							infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);

						indexCargo++;
					}
				}
				else
				{
					var infoCost = new InfoCost
					{
						GroupId = groupId,
						Date = date,
						DirectoryCostItem = db.DirectoryCostItems.Find(directoryCostItem.Id),
						DirectoryRC = db.DirectoryRCs.Find(transports.First().DirectoryRC.Id),
						IsIncoming = isIncoming,
						DirectoryTransportCompany =
							transportCompany != null ? db.DirectoryTransportCompanies.Find(transportCompany.Id) : null,
						Summ = summ,
						Currency = currency,
						CurrentNotes =
							new List<CurrentNote>
							{
								new CurrentNote {DirectoryNote = db.DirectoryNotes.Find(transports.First().DirectoryNote.Id)}
							},
						Weight = 0
					};

					db.InfoCosts.Add(infoCost);
					AddInfoSafe(infoCost.Date, infoCost.IsIncoming, infoCost.Summ, currency, CashType.Наличка,
						infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
				}

				db.SaveChanges();
			}
		}

		public void RemoveInfoCost(InfoCost infoCost)
		{
			using (var db = GetContext())
			{
				var infoCosts = GetInfoCosts(infoCost.Date).Where(c => c.GroupId == infoCost.GroupId).ToList();

				foreach (var cost in infoCosts)
				{
					AddInfoSafe(infoCost.Date, !infoCost.IsIncoming, infoCost.Summ, infoCost.Currency, CashType.Наличка,
						infoCost.DirectoryRC.Name + " " + infoCost.ConcatNotes);
				}

				db.InfoCosts.RemoveRange(infoCosts);
				db.SaveChanges();
			}
		}

		public int[] GetInfoCostYears()
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Select(c => c.Date.Year)
					.Distinct()
					.OrderBy(y => y)
					.ToArray();
			}
		}

		public int[] GetInfoCostMonthes(int year)
		{
			using (var db = GetContext())
			{
				return db.InfoCosts
					.Where(c => c.Date.Year == year)
					.Select(c => c.Date.Month)
					.Distinct()
					.OrderBy(m => m)
					.ToArray();
			}
		}

		public double GetInfoCost26Summ(int year, int month)
		{
			var infoCosts = GetInfoCosts(year, month).Where(c => c.DirectoryRC.Name == "26А" && c.IsIncoming);
			return infoCosts.Any() ? infoCosts.Sum(c => c.Summ) : 0;
		}

		public string[] GetInfoCostsIncomingTotalSummsCurrency(int year, int month, string rcName, bool? isIncoming = null,
			string costItem = null)
		{
			using (var db = GetContext())
			{
				string[] totalSumms = new string[Enum.GetNames(typeof(Currency)).Count()];

				for (int i = 0; i < totalSumms.Count(); i++)
				{
					var currency = (Currency)Enum.Parse(typeof(Currency), Enum.GetName(typeof(Currency), i));
					var infoCosts =
						db.InfoCosts.Where(
							c => c.Date.Year == year && c.Date.Month == month && c.Currency == currency && c.DirectoryRC.Name == rcName);

					if (isIncoming != null)
					{
						infoCosts = infoCosts.Where(c => c.IsIncoming == isIncoming.Value);
					}

					if (costItem != null)
					{
						infoCosts = infoCosts.Where(c => c.DirectoryCostItem.Name == costItem);
					}

					if (infoCosts.Any())
					{
						double totalSummCurrency = infoCosts.Sum(c => c.Summ);
						totalSumms[i] = totalSummCurrency != 0 ? Converting.DoubleToCurrency(totalSummCurrency, currency) : null;
					}
				}

				return totalSumms;
			}
		}

		#endregion


		#region InfoLoan

		public InfoLoan AddInfoLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;

					AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + loanTaker.Name);
				}
				else
				{
					AddInfoSafe(date, false, summ, currency, CashType.Наличка, "Выдача долга: " + directoryWorker.FullName);
				}


				var infoLoan = new InfoLoan
				{
					DateLoan = date,
					DirectoryLoanTaker = loanTaker,
					DirectoryWorker = directoryWorker,
					Summ = summ,
					Currency = currency,
					CountPayments = countPayments,
					Description = description,
				};

				db.InfoLoans.Add(infoLoan);
				db.SaveChanges();

				EditCurrencyValueSummChange("TotalLoan", currency, summ);

				var dateLoanPayment = new DateTime(date.Year, date.Month, 5);

				if (loanTakerName == null)
				{
					double onePaySumm = Math.Round(summ / countPayments, 0);

					for (int i = 0; i < countPayments;i++)
					{
						dateLoanPayment = dateLoanPayment.AddMonths(1);

						var infoPayment = new InfoPayment
						{
							Date = dateLoanPayment,
							Summ = onePaySumm,
							InfoLoanId = infoLoan.Id
						};

						db.InfoPayments.Add(infoPayment);
					}

					EditInfoMonthPayment(directoryWorker.Id, date, onePaySumm);

					infoLoan.DateLoanPayment = dateLoanPayment;
				}

				db.SaveChanges();

				return infoLoan;
			}
		}

		public InfoLoan EditInfoLoan(int id, DateTime date, string loanTakerName, DirectoryWorker directoryWorker, double summ,
			Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoLoan = db.InfoLoans.Find(id);

				EditCurrencyValueSummChange("TotalLoan", currency, summ - infoLoan.Summ);

				infoLoan.DateLoan = date;
				infoLoan.DirectoryLoanTaker = loanTaker;
				infoLoan.DirectoryWorker = directoryWorker;
				infoLoan.Summ = summ;
				infoLoan.Currency = currency;
				infoLoan.CountPayments = countPayments;
				infoLoan.Description = description;

				db.SaveChanges();

				return infoLoan;
			}
		}

		public void RemoveInfoLoan(InfoLoan selectedInfoLoan)
		{
			using (var db = GetContext())
			{
				db.InfoLoans.Remove(selectedInfoLoan);
				db.SaveChanges();
			}
		}

		public InfoLoan[] GetInfoLoans(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoLoans
					.Where(s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 &&
								DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
								(s.DateLoanPayment == null ||
								 s.DateLoanPayment != null &&
								 DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0))
					.OrderByDescending(s => s.DateLoan)
					.ToArray();
			}
		}

		public double GetLoans()
		{
			using (var db = GetContext())
			{
				return db.InfoLoans.Where(s => s.DateLoanPayment == null).ToArray().Sum(s => s.RemainingSumm);
			}
		}

		#endregion


		#region InfoPrivateLoan

		public InfoPrivateLoan AddInfoPrivateLoan(DateTime date, string loanTakerName, DirectoryWorker directoryWorker,
			double summ, Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoPrivateLoan = new InfoPrivateLoan
				{
					DateLoan = date,
					DirectoryLoanTaker = loanTaker,
					DirectoryWorker = directoryWorker,
					Summ = summ,
					Currency = currency,
					CountPayments = countPayments,
					Description = description,
				};

				db.InfoPrivateLoans.Add(infoPrivateLoan);
				db.SaveChanges();

				EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ);

				var datePrivateLoanPayment = new DateTime(date.Year, date.Month, 5);

				if (loanTakerName == null)
				{
					double onePaySumm = Math.Round(summ / countPayments, 0);

					for (int i = 0; i < countPayments; i++)
					{
						datePrivateLoanPayment = datePrivateLoanPayment.AddMonths(1);

						var infoPrivatePayment = new InfoPrivatePayment
						{
							Date = datePrivateLoanPayment,
							Summ = onePaySumm,
							InfoPrivateLoanId = infoPrivateLoan.Id
						};

						db.InfoPrivatePayments.Add(infoPrivatePayment);
					}

					if (countPayments > 1)
					{
						infoPrivateLoan.DateLoanPayment = datePrivateLoanPayment;
					}
				}

				db.SaveChanges();

				return infoPrivateLoan;
			}
		}

		public InfoPrivateLoan EditInfoPrivateLoan(int id, DateTime date, string loanTakerName,
			DirectoryWorker directoryWorker, double summ, Currency currency, int countPayments, string description)
		{
			using (var db = GetContext())
			{
				DirectoryLoanTaker loanTaker = null;

				if (loanTakerName != null)
				{
					loanTaker = db.DirectoryLoanTakers.FirstOrDefault(l => l.Name == loanTakerName);

					if (loanTaker == null)
					{
						loanTaker = AddDirectoryLoanTaker(loanTakerName);
					}
					;
				}

				var infoPrivateLoan = db.InfoPrivateLoans.Find(id);

				EditCurrencyValueSummChange("TotalPrivateLoan", currency, summ - infoPrivateLoan.Summ);

				infoPrivateLoan.DateLoan = date;
				infoPrivateLoan.DirectoryLoanTaker = loanTaker;
				infoPrivateLoan.DirectoryWorker = directoryWorker;
				infoPrivateLoan.Summ = summ;
				infoPrivateLoan.Currency = currency;
				infoPrivateLoan.CountPayments = countPayments;
				infoPrivateLoan.Description = description;

				db.SaveChanges();

				return infoPrivateLoan;
			}
		}

		public void RemoveInfoPrivateLoan(InfoPrivateLoan selectedInfoPrivateLoan)
		{
			using (var db = GetContext())
			{
				db.InfoPrivateLoans.Remove(selectedInfoPrivateLoan);
				db.SaveChanges();
			}
		}

		public InfoPrivateLoan[] GetInfoPrivateLoans(DateTime from, DateTime to)
		{
			using (var db = GetContext())
			{
				return db.InfoPrivateLoans.Where(
					s => DbFunctions.DiffDays(from, s.DateLoan) >= 0 && DbFunctions.DiffDays(to, s.DateLoan) <= 0 &&
						 (s.DateLoanPayment == null ||
						  s.DateLoanPayment != null && DbFunctions.DiffDays(DateTime.Now, s.DateLoanPayment) >= 0)).
					OrderByDescending(s => s.DateLoan)
					.ToArray();
			}
		}

		public double GetPrivateLoans()
		{
			using (var db = GetContext())
			{
				return db.InfoPrivateLoans.Where(s => s.DateLoanPayment == null).ToArray().Sum(s => s.RemainingSumm);
			}
		}

		#endregion


		#region DirectoryCostItem

		public DirectoryCostItem[] GetDirectoryCostItems()
		{
			using (var db = GetContext())
			{
				return db.DirectoryCostItems.ToArray();
			}
		}

		public DirectoryCostItem GetDirectoryCostItem(string costItemName)
		{
			return GetDirectoryCostItems().First(c => c.Name == costItemName);
		}

		#endregion


		#region DirectoryNote

		public DirectoryNote[] GetDirectoryNotes()
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.ToArray();
			}
		}

		public DirectoryNote GetDirectoryNote(string description)
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.First(n => n.Description == description);
			}
		}

		public bool IsDirectoryNote(string note)
		{
			using (var db = GetContext())
			{
				return db.DirectoryNotes.Select(n => n.Description).Contains(note);
			}
		}

		public DirectoryNote AddDirectoryNote(string note)
		{
			using (var db = GetContext())
			{
				var directoryNote = db.DirectoryNotes.FirstOrDefault(n => n.Description == note);

				if (directoryNote != null)
				{
					return directoryNote;
				}

				directoryNote = new DirectoryNote { Description = note };

				db.DirectoryNotes.Add(directoryNote);

				db.SaveChanges();

				return directoryNote;
			}
		}

		#endregion


		#region DefaultCosts

		public DefaultCost[] GetDefaultCosts()
		{
			using (var db = GetContext())
			{
				return db.DefaultCosts.ToArray();
			}
		}

		public DefaultCost AddDefaultCost(DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			using (var db = GetContext())
			{
				var defaultCost = new DefaultCost
				{
					DirectoryCostItem = costItem,
					DirectoryRC = rc,
					DirectoryNote = note,
					SummOfPayment = summ,
					DayOfPayment = day
				};
				db.DefaultCosts.Add(defaultCost);
				db.SaveChanges();

				var infoCost = new InfoCost
				{
					Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day),
					DirectoryCostItemId = defaultCost.DirectoryCostItemId,
					DirectoryRCId = defaultCost.DirectoryRCId,
					CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
					Summ = defaultCost.SummOfPayment,
					IsIncoming = false,
					Weight = 0,
				};

				db.InfoCosts.Add(infoCost);
				db.SaveChanges();

				return defaultCost;
			}
		}

		public void EditDefaultCost(int id, DirectoryCostItem costItem, DirectoryRC rc, DirectoryNote note, double summ, int day)
		{
			using (var db = GetContext())
			{
				var defaultCost = db.DefaultCosts.Find(id);

				defaultCost.DirectoryCostItem = costItem;
				defaultCost.DirectoryRC = rc;
				defaultCost.DirectoryNote = note;
				defaultCost.SummOfPayment = summ;
				defaultCost.DayOfPayment = day;

				db.SaveChanges();
			}
		}

		public void RemoveDefaultCost(DefaultCost defaultCost)
		{
			using (var db = GetContext())
			{
				db.DefaultCosts.Remove(defaultCost);
				db.SaveChanges();
			}
		}

		public void InitializeDefaultCosts()
		{
			using (var db = GetContext())
			{
				var defaultCostsDate = utilRepository.GetParameterValue<DateTime>(ParameterType.DefaultCostsDate);
				var currentDate = DateTime.Now;

				if (defaultCostsDate.Year < currentDate.Year ||
					(defaultCostsDate.Year == currentDate.Year && defaultCostsDate.Month < currentDate.Month))
				{
					var defaultCosts = GetDefaultCosts().ToList();

					foreach (var defaultCost in defaultCosts)
					{
						var infoCostDate = new DateTime(currentDate.Year, currentDate.Month, defaultCost.DayOfPayment);
						var infoCosts = GetInfoCosts(infoCostDate).ToList();
						if (
							!infoCosts.Any(
								c =>
									c.DirectoryCostItem.Id == defaultCost.DirectoryCostItemId && c.DirectoryRCId == defaultCost.DirectoryRCId &&
									c.CurrentNotes.First().DirectoryNoteId == defaultCost.DirectoryNoteId && c.Summ == defaultCost.SummOfPayment))
						{
							var infoCost = new InfoCost
							{
								Date = infoCostDate,
								DirectoryCostItemId = defaultCost.DirectoryCostItemId,
								DirectoryRCId = defaultCost.DirectoryRCId,
								CurrentNotes = new List<CurrentNote> { new CurrentNote { DirectoryNoteId = defaultCost.DirectoryNoteId } },
								Summ = defaultCost.SummOfPayment,
								IsIncoming = false,
								Weight = 0,
							};

							db.InfoCosts.Add(infoCost);
						}
					}

					db.SaveChanges();
					utilRepository.EditParameter(ParameterType.DefaultCostsDate, DateTime.Now);
				}
			}
		}

		#endregion


		#region DirectoryTransportCompanies

		public DirectoryTransportCompany[] GetDirectoryTransportCompanies()
		{
			using (var db = GetContext())
			{
				return db.DirectoryTransportCompanies.ToArray();
			}
		}


		#endregion

		private void EditInfoMonthPayment(int workerId, DateTime date, double sum)
		{
			using (var db = GetContext())
			{
				var infoMonth = db.InfoMonthes.First(m => m.DirectoryWorkerId == workerId && 
					m.Date.Year == date.Year && m.Date.Month == date.Month);

				infoMonth.PrepaymentCash = sum;
				db.SaveChanges();
			}
		}
	}
}

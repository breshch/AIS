using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;

namespace AVRepository.Repositories
{
	public class InitializationRepository : BaseRepository
	{
		private readonly UtilRepository utilRepository;
		private readonly TimeManagementRepository timeManagementRepository;
		private readonly CostRepository costRepository;

		public InitializationRepository(UtilRepository utilRepository, TimeManagementRepository timeManagementRepository, CostRepository costRepository)
		{
			this.utilRepository = utilRepository;
			this.timeManagementRepository = timeManagementRepository;
			this.costRepository = costRepository;
		}

		public void InitializeAbsentDates()
		{
			var lastDate = utilRepository.GetParameterValue<DateTime>(ParameterType.LastDate);

			double birthday = utilRepository.GetParameterValue<double>(ParameterType.Birthday);

			using (var db = GetContext())
			{
				if (DateTime.Now.Date > lastDate.Date)
				{
					var workers = timeManagementRepository.GetDirectoryWorkers(lastDate, DateTime.Now).ToList();
					var holidays = utilRepository.GetHolidays(lastDate.AddDays(-14), DateTime.Now).ToList();

					for (var date = lastDate.AddDays(1); date.Date <= DateTime.Now.Date; date = date.AddDays(1))
					{
						var firstDateInMonth = new DateTime(date.Year, date.Month, 1);
						foreach (var worker in workers)
						{
							if (!worker.InfoMonthes.Any(m => m.Date.Year == date.Year && m.Date.Month == date.Month))
							{
								var infoMonth = new InfoMonth();
								infoMonth.Date = firstDateInMonth;
								if (worker.CurrentCompaniesAndPosts.Last().DirectoryPost.DirectoryTypeOfPost.Name != "Офис")
								{
									infoMonth.BirthDays = !worker.IsDeadSpirit ? birthday : 0;
								}
								;
								worker.InfoMonthes.Add(infoMonth);

								db.SaveChanges();
							}

							if (worker.StartDate.Date <= date.Date &&
								(worker.FireDate == null || worker.FireDate != null && worker.FireDate.Value.Date >= date.Date))
							{
								if (!worker.InfoDates.Any(d => d.Date.Date == date.Date))
								{
									int countPrevDays = 0;
									InfoDate prevInfoDate = null;
									do
									{
										countPrevDays++;
										prevInfoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == date.AddDays(-countPrevDays).Date);
									} while (prevInfoDate == null || prevInfoDate != null && holidays.Any(h => h.Date == prevInfoDate.Date.Date));

									var prevDescriptionDay = DescriptionDay.Был;
									if (prevInfoDate != null)
									{
										prevDescriptionDay = prevInfoDate.DescriptionDay;
									}

									var infoDate = new InfoDate
									{
										Date = date,
										DescriptionDay = DescriptionDay.Был
									};

									if (!holidays.Any(h => h.Date == date.Date))
									{
										if (prevDescriptionDay == DescriptionDay.Был)
										{
											infoDate.CountHours = 8;
										}

										infoDate.DescriptionDay = prevDescriptionDay;
									}

									worker.InfoDates.Add(infoDate);

									var salaryDate = new DateTime(date.Year, date.Month, 5);
									if (lastDate.Date < salaryDate.Date && date.Date >= salaryDate.Date)
									{
										InitializeWorkerLoanPayments(worker,
											worker.InfoMonthes.First(m => m.Date.Year == date.Year && m.Date.Month == date.Month), salaryDate);
									}
								}
							}
						}

						db.SaveChanges();

						utilRepository.EditParameter(ParameterType.LastDate, date);
						lastDate = date;
					}
				}
			}
		}

		private void InitializeWorkerLoanPayments(DirectoryWorker worker, InfoMonth infoMonth, DateTime salaryDate)
		{
			using (var db = GetContext())
			{
				var infoLoans = db.InfoLoans.Where(s => s.DirectoryWorkerId == worker.Id && (s.DateLoanPayment == null ||
																							 (s.DateLoanPayment != null &&
																							  DbFunctions.DiffDays(
																								  s.DateLoanPayment, salaryDate) <= 0)))
					.ToList();

				if (infoLoans.Any())
				{
					foreach (var loan in infoLoans)
					{
						var payments = db.InfoPayments.Where(p => p.InfoLoanId == loan.Id).ToList();
						if (payments.Any())
						{
							var payment = payments.FirstOrDefault(p => p.Date.Date == salaryDate.Date);
							if (payment != null)
							{
								infoMonth.PrepaymentCash = payment.Summ;
								costRepository.EditCurrencyValueSummChange("TotalLoan", loan.Currency, payment.Summ);

								costRepository.AddInfoSafe(payment.Date, true, payment.Summ, loan.Currency, CashType.Наличка,
									"Возврат долга: " + worker.FullName);

								break;
							}
						}
					}
				}
			}
		}
	}
}

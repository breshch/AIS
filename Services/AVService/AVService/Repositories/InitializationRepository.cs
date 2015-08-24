using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using AVService.Models.Entities.Directories;
using AVService.Models.Entities.Infos;
using AVService.Models.Enums;
using Shared.Enums;

namespace AVService.Repositories
{
	public class InitializationRepository : BaseRepository
	{
		private readonly UtilRepository utilRepository;
		private readonly TimeManagementRepository timeManagementRepository;
		private readonly CostRepository costRepository;

		private bool _isInitializeAbsentDatesProcessing = false;

		public InitializationRepository(UtilRepository utilRepository, TimeManagementRepository timeManagementRepository, CostRepository costRepository)
		{
			this.utilRepository = utilRepository;
			this.timeManagementRepository = timeManagementRepository;
			this.costRepository = costRepository;
		}

		public void InitializeAbsentDates()
		{
			while (_isInitializeAbsentDatesProcessing)
			{
				Thread.Sleep(500);
			}

			_isInitializeAbsentDatesProcessing = true;

			var lastDate = utilRepository.GetParameterValue<DateTime>(ParameterType.LastEnteringDate);
			if (DateTime.Now.Date > lastDate.Date)
			{
				double monthlyBirthdayPayment = utilRepository.GetParameterValue<double>(ParameterType.MonthlyBirthdayPayment);

				using (var db = GetContext())
				{
					var holidays = utilRepository.GetHolidays(lastDate.AddDays(-14), DateTime.Now);

					var workersModel = timeManagementRepository.GetWorkersModel(lastDate, DateTime.Now, 
						WorkerModelQueryRule.DirectoryPosts | WorkerModelQueryRule.InfoDates | WorkerModelQueryRule.InfoMonths);

					var infoMonths = new List<InfoMonth>();
					var infoDates = new List<InfoDate>();
					for (var date = lastDate.AddDays(1); date.Date <= DateTime.Now.Date; date = date.AddDays(1))
					{
						var firstDateInMonth = new DateTime(date.Year, date.Month, 1);
						foreach (var workerModel in workersModel)
						{
							if (!workerModel.InfoMonthes.Any(m => m.Date.Year == date.Year && m.Date.Month == date.Month))
							{
								var infoMonth = new InfoMonth
								{
									Date = firstDateInMonth,
									DirectoryWorkerId = workerModel.Worker.Id
								};

								if (workerModel.DirectoryPosts.Last().TypeOfPost == TypeOfPost.Warehouse)
								{
									infoMonth.BirthDays = !workerModel.Worker.IsDeadSpirit ? monthlyBirthdayPayment : 0;
								}

								infoMonths.Add(infoMonth);
							}

							if (workerModel.Worker.StartDate.Date <= date.Date &&
								(workerModel.Worker.FireDate == null || workerModel.Worker.FireDate != null && workerModel.Worker.FireDate.Value.Date >= date.Date))
							{
								if (!workerModel.InfoDates.Any(d => d.Date.Date == date.Date))
								{
									int countPrevDays = 0;
									InfoDate prevInfoDate;
									do
									{
										countPrevDays++;
										prevInfoDate = workerModel.InfoDates.FirstOrDefault(d => d.Date.Date == date.AddDays(-countPrevDays).Date);
									} while (prevInfoDate == null || prevInfoDate != null && holidays.Any(h => h.Date == prevInfoDate.Date.Date));

									var prevDescriptionDay = DescriptionDay.Был;
									if (prevInfoDate != null)
									{
										prevDescriptionDay = prevInfoDate.DescriptionDay;
									}

									var infoDate = new InfoDate
									{
										Date = date,
										DescriptionDay = DescriptionDay.Был,
										DirectoryWorkerId = workerModel.Worker.Id
									};

									if (!holidays.Any(h => h.Date == date.Date))
									{
										if (prevDescriptionDay == DescriptionDay.Был)
										{
											infoDate.CountHours = 8;
										}

										infoDate.DescriptionDay = prevDescriptionDay;
									}


									infoDates.Add(infoDate);

									var salaryDate = new DateTime(date.Year, date.Month, 5);
									if (lastDate.Date < salaryDate.Date && date.Date >= salaryDate.Date)
									{
										InitializeWorkerLoanPayments(workerModel.Worker,
											workerModel.InfoMonthes.First(m => m.Date.Year == date.Year && m.Date.Month == date.Month), salaryDate);
									}
								}
							}
						}

						lastDate = date;
					}

					timeManagementRepository.AddInfoMonths(infoMonths.ToArray());
					timeManagementRepository.AddInfoDates(infoDates.ToArray());

					utilRepository.EditParameter(ParameterType.LastEnteringDate, DateTime.Now);
				}
			}

			_isInitializeAbsentDatesProcessing = false;
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

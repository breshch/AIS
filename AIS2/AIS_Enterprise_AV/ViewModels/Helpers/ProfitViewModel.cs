﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_AV.Models;
using AIS_Enterprise_Data;
using AIS_Enterprise_Global.Helpers;
using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
	public class ProfitViewModel : ViewModelBase
	{
		public ProfitViewModel(int year, int month)
		{
			var minskAndOvertimeSalary = GetMinskAndOvertimeSalary(year, month);
			var realAndOvertime = GetRealAndOvertimeSalary(year, month);
			var transportProfit = GetTransportProfit(year, month);

			RealSalary = realAndOvertime.Item1;
			RealOvertime = realAndOvertime.Item2;
			MinskSalary = minskAndOvertimeSalary.Item1;
			MinskOvertime = minskAndOvertimeSalary.Item2;
			DifferenceSalary = MinskSalary - RealSalary;
			DifferenceOvertime = MinskOvertime - RealOvertime;
			TransportExpence = transportProfit.Item1;
			TransportCompensation = transportProfit.Item2;
			DifferenceTransport = TransportExpence - TransportCompensation;
		}


		public double MinskSalary { get; set; }
		public double MinskOvertime { get; set; }
		public double TransportExpence { get; set; }
		public double RealSalary { get; set; }
		public double RealOvertime { get; set; }
		public double TransportCompensation { get; set; }
		public double DifferenceSalary { get; set; }
		public double DifferenceOvertime { get; set; }
		public double DifferenceTransport { get; set; }


		private Tuple<double, double> GetRealAndOvertimeSalary(int year, int month)
		{
			using (var bc = new BusinessContext())
			{
				int countWorkDays = bc.GetCountWorkDaysInMonth(year, month);
				var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);
				double allSumms = 0;
				double allOverTimes = 0;

				var lastDate = DateTime.Now;
				if (!(lastDate.Year == year && lastDate.Month == month))
				{
					lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
				}

				var weekendsInMonth = bc.GetHolidays(year, month).ToList();
				var workers = bc.GetDirectoryWorkers(year, month, false);
				var currentPosts = bc.GetCurrentPosts(lastDate);
				var infoDatesWorkers = bc.GetInfoDates(year, month);
				var salaries = bc.GetDirectoryPostSalaries(year, month);

				foreach (var worker in workers)
				{
					int countSickDays = 0;
					double workerPanalty = 0;
					double totalVocations = 0;
					double totalSickDays = 0;

					var workerPostReportSalaries = new List<WorkerPostReportSalary>();

					var infoDates = infoDatesWorkers.Where(d => d.DirectoryWorkerId == worker.Id);

					foreach (var infoDate in infoDates)
					{
						var currentWorkerPost = currentPosts.First(p => p.DirectoryWorkerId == worker.Id &&
																		(p.ChangeDate.Date <= infoDate.Date.Date && p.FireDate == null ||
																		p.FireDate != null && p.FireDate.Value.Date >= infoDate.Date.Date &&
																		p.ChangeDate.Date <= infoDate.Date.Date));

						var postSalary = salaries.Where(s => s.DirectoryPostId == currentWorkerPost.DirectoryPost.Id)
							.OrderByDescending(s => s.Date)
							.First(s => new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1).Date >= s.Date.Date);

						double workerSalaryInHour = (double)((postSalary.AdminWorkerSalary) / countWorkDays / 8);

						var workerPostReportSalary = workerPostReportSalaries.FirstOrDefault(w => w.PostId == currentWorkerPost.Id);

						if (workerPostReportSalary == null)
						{
							workerPostReportSalary = new WorkerPostReportSalary
							{
								PostId = currentWorkerPost.Id,
								PostName = currentWorkerPost.DirectoryPost.Name,
								AdminWorkerSalary = postSalary.AdminWorkerSalary.Value,
								ChangePostDay =
									currentWorkerPost.ChangeDate.Date >= new DateTime(year, month, 1).Date ? currentWorkerPost.ChangeDate.Day : 1
							};

							workerPostReportSalaries.Add(workerPostReportSalary);
						}


						switch (infoDate.DescriptionDay)
						{
							case DescriptionDay.Б:
								if (countSickDays < 5)
								{
									countSickDays++;
									totalSickDays += workerSalaryInHour * 8;
								}
								break;
							case DescriptionDay.О:
								totalVocations += workerSalaryInHour * 8;
								break;
						}

						if (infoDate.InfoPanalty != null)
						{
							workerPanalty += infoDate.InfoPanalty.Summ;
						}

						if (infoDate.CountHours != null)
						{
							if (weekendsInMonth.Any(w => w.Date.Date == infoDate.Date.Date))
							{
								workerPostReportSalary.CountWorkOverTimeHours += infoDate.CountHours.Value;
							}
							else
							{
								if (infoDate.CountHours > 0 && infoDate.CountHours <= 8)
								{
									workerPostReportSalary.CountWorkHours += infoDate.CountHours.Value;
								}
								else if (infoDate.CountHours > 8)
								{
									workerPostReportSalary.CountWorkHours += 8;
									workerPostReportSalary.CountWorkOverTimeHours += infoDate.CountHours.Value - 8;
								}
							}
						}

						if (infoDate.InfoPanalty != null)
						{
							workerPanalty += infoDate.InfoPanalty.Summ;
						}
					}

					var infoMonth = bc.GetInfoMonth(worker.Id, year, month);

					double allSalary = 0;
					double allOverTimeSalary = 0;
					foreach (var workerPostReportSalary in workerPostReportSalaries)
					{
						allSalary += (workerPostReportSalary.AdminWorkerSalary / countWorkDays / 8) * workerPostReportSalary.CountWorkHours;
						allOverTimeSalary += ((workerPostReportSalary.AdminWorkerSalary / countWorkDays / 8) *
											  workerPostReportSalary.CountWorkOverTimeHours) * 2;
					}

					double summOfPayments = allSalary + allOverTimeSalary + infoMonth.Bonus + totalVocations + totalSickDays;
					double summOfHoldings = workerPanalty + infoMonth.BirthDays + infoMonth.Inventory;

					allSumms += (summOfPayments - summOfHoldings);
					allOverTimes += allOverTimeSalary;

				}

				return new Tuple<double, double>(Math.Round(allSumms, 2), Math.Round(allOverTimes, 2));
			}
		}

		private Tuple<double, double> GetMinskAndOvertimeSalary(int year, int month)
		{
			using (var bc = new BusinessContext())
			{
				//var lastDateInMonth = HelperMethods.GetLastDateInMonth(year, month);
				//var warehouseWorkers = bc.GetDirectoryWorkers(year, month, false).ToList();
				//var overTimes = bc.GetInfoOverTimes(year, month).ToList();
				//var weekEndsInMonth = bc.GetHolidays(year, month).ToList();
				//var workerSumms = new List<WorkerSummForReport>();
				//var currentRCs = bc.GetCurrentRCs(overTimes.Select(o => o.Id)).ToList();
				//int countWorkDayInMonth = bc.GetCountWorkDaysInMonth(year, month);

				//foreach (var overTime in overTimes)
				//{
				//	var overTimeRCs = currentRCs.Where(r => r.InfoOverTimeId == overTime.Id).ToList();
				//	int countRCs = overTimeRCs.Count();

				//	int currentPercentage = overTimeRCs.Sum(r => r.DirectoryRC.Percentage);
				//	for (int i = 0; i < countRCs; i++)
				//	{
				//		foreach (var worker in warehouseWorkers)
				//		{
				//			var workerSummForReport = workerSumms.FirstOrDefault(w => w.WorkerId == worker.Id);
				//			if (workerSummForReport == null)
				//			{
				//				workerSummForReport = new WorkerSummForReport { WorkerId = worker.Id };
				//				workerSumms.Add(workerSummForReport);
				//			}

				//			var infoDate = worker.InfoDates.FirstOrDefault(d => d.Date.Date == overTime.StartDate.Date);
				//			if (infoDate != null)
				//			{
				//				var overTimeHours = bc.IsOverTime(infoDate, weekEndsInMonth);
				//				if (overTimeHours != null)
				//				{
				//					double percentage;
				//					if (currentPercentage != 0)
				//					{
				//						percentage = overTimeHours.Value * 1.3 * overTimeRCs[i].DirectoryRC.Percentage / currentPercentage;
				//					}
				//					else
				//					{
				//						percentage = overTimeHours.Value * 1.3 / overTimeRCs.Count;
				//					}


				//					var workerRCSummForReport = workerSummForReport.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == overTimeRCs[i].DirectoryRC.Name);
				//					if (workerRCSummForReport == null)
				//					{
				//						workerRCSummForReport = new WorkerRCSummForReport { RCName = overTimeRCs[i].DirectoryRC.Name };
				//						workerSummForReport.WorkerRCSummForReports.Add(workerRCSummForReport);
				//					}

				//					var post = bc.GetCurrentPost(worker.Id, infoDate.Date).DirectoryPost;
				//					var postSalary = bc.GetDirectoryPostSalaryByDate(post.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

				//					double salaryInHour = postSalary.AdminWorkerSalary.Value / 8 / countWorkDayInMonth;
				//					workerRCSummForReport.Summ += percentage * 2 * salaryInHour;
				//				}
				//			}
				//		}
				//	}
				//}
				double totalTotalSalary = 0;

				//var currentMainWorkerPosts = bc.GetCurrentMainPosts(lastDateInMonth).ToList();
				double allOvertimes = 0;
				//foreach (var worker in warehouseWorkers)
				//{
				//	var currentWorkerPost = currentMainWorkerPosts.First(p => p.DirectoryWorkerId == worker.Id);
				//	var infoMonth = bc.GetInfoMonth(worker.Id, year, month);
				//	double salaryAV = 0;
				//	double salaryFenox = 0;
				//	var postSalary = bc.GetDirectoryPostSalaryByDate(currentWorkerPost.DirectoryPost.Id, new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1));

				//	if (!currentWorkerPost.IsTwoCompanies)
				//	{
				//		if (currentWorkerPost.DirectoryPost.DirectoryCompany.Name == "АВ")
				//		{
				//			salaryAV = postSalary.AdminWorkerSalary.Value;
				//		}
				//		else
				//		{
				//			salaryFenox = postSalary.AdminWorkerSalary.Value;
				//		}
				//	}
				//	else
				//	{
				//		salaryAV = postSalary.AdminWorkerSalary.Value - postSalary.UserWorkerHalfSalary.Value;
				//		salaryFenox = postSalary.UserWorkerHalfSalary.Value;
				//	}
				//	double totalOverTimeAV = 0;

				//	var workerSumm = workerSumms.FirstOrDefault(w => w.WorkerId == worker.Id);
				//	WorkerRCSummForReport rc = null;

				//	double rcValue = 0;
				//	if (workerSumm != null)
				//	{
				//		rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "КО-5");

				//		if (rc != null)
				//		{
				//			rcValue = rc.Summ;
				//		}
				//	}

				//	totalOverTimeAV += rcValue;

				//	rcValue = 0;
				//	if (workerSumm != null)
				//	{
				//		rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "ПАМ-16");

				//		if (rc != null)
				//		{
				//			rcValue = rc.Summ;
				//		}
				//	}

				//	totalOverTimeAV += rcValue;

				//	double cashAV = salaryAV - infoMonth.CardAV - infoMonth.PrepaymentBankTransaction;

				//	double totalOverTimeFenox = 0;
				//	rcValue = 0;
				//	if (workerSumm != null)
				//	{
				//		rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "МО-5");
				//		if (rc != null)
				//		{
				//			rcValue = rc.Summ;
				//		}
				//	}
				//	totalOverTimeFenox += rcValue;

				//	rcValue = 0;
				//	if (workerSumm != null)
				//	{
				//		rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "ПАМ-1");
				//		if (rc != null)
				//		{
				//			rcValue = rc.Summ;
				//		}
				//	}
				//	totalOverTimeFenox += rcValue;

				//	rcValue = 0;
				//	if (workerSumm != null)
				//	{
				//		rc = workerSumm.WorkerRCSummForReports.FirstOrDefault(w => w.RCName == "МО-2");
				//		if (rc != null)
				//		{
				//			rcValue = rc.Summ;
				//		}
				//	}
				//	totalOverTimeFenox += rcValue;

				//	double cashFenox = salaryFenox - infoMonth.CardFenox;
				//	totalTotalSalary += infoMonth.CardAV + infoMonth.PrepaymentBankTransaction + infoMonth.Compensation +
				//		infoMonth.VocationPayment + totalOverTimeAV + cashAV +
				//		infoMonth.CardFenox + totalOverTimeFenox + cashFenox;

				//	allOvertimes += totalOverTimeAV + totalOverTimeFenox;
				//}

				return new Tuple<double, double>(Math.Round(totalTotalSalary, 2), Math.Round(allOvertimes, 2));
			}
		}

		private Tuple<double, double> GetTransportProfit(int year, int month)
		{
			using (var bc = new BusinessContext())
			{
				var costs = bc.GetInfoCosts(year, month).ToArray();
				double sumExpence = costs
					.Where(c => c.DirectoryCostItem.Name == "Транспорт (5031)" &&
					            c.DirectoryTransportCompany.Name == "Логистикон" &&
					            c.DirectoryRC.Name != "ВСЕ" &&
					            c.DirectoryRC.Name != "КО-5" &&
					            !c.IsIncoming)
					.Sum(c => c.Summ);

				double sumIncoming = costs
					.Where(c => c.DirectoryCostItem.Name == "Транспорт (5031)" &&
					            c.DirectoryTransportCompany.Name == "Логистикон" &&
					            c.DirectoryRC.Name == "КО-5" &&
					            c.IsIncoming)
					.Sum(c => c.Summ);

				return new Tuple<double, double>(sumExpence, sumIncoming);
			}
		}
	}
}

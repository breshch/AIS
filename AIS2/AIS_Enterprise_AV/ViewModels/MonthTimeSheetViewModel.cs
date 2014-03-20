using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MonthTimeSheetViewModel : ViewModel
    {
        private bool _isLoad;

        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }

        public ObservableCollection<MonthTimeSheetWorker> MonthTimeSheetWorkers { get; set; }

        public MonthTimeSheetViewModel()
        {
            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;

            RefreshWorkers();
        }

        private void RefreshWorkers()
        {
            _isLoad = false;
            MonthTimeSheetWorkers = new ObservableCollection<MonthTimeSheetWorker>();

            int countWorkDaysInMonth = HelperCalendar.GetCountWorkDaysInMonth(SelectedYear, SelectedMonth);
            var firstDateInMonth = new DateTime(SelectedYear, SelectedMonth, 1);
            var lastDateInMonth = new DateTime(SelectedYear, SelectedMonth, DateTime.DaysInMonth(SelectedYear, SelectedMonth));

            var workers = BC.GetDirectoryWorkers(SelectedYear, SelectedMonth).ToList();

            foreach (var worker in workers)
            {
                var currentPosts = BC.GetCurrentPosts(worker.Id, SelectedYear, SelectedMonth);

                bool isFirst = false;

                double salary = 0;
                foreach (var currentPost in currentPosts)
                {
                    var monthTimeSheetWorker = MonthTimeSheetWorkers.FirstOrDefault(m => m.WorkerId == worker.Id && m.DirectoryPostId == currentPost.DirectoryPostId);

                    if (monthTimeSheetWorker == null)
                    {
                        monthTimeSheetWorker = new MonthTimeSheetWorker();

                        MonthTimeSheetWorkers.Add(monthTimeSheetWorker);

                        monthTimeSheetWorker.WorkerId = worker.Id;

                        if (!isFirst)
                        {
                            monthTimeSheetWorker.FullName = worker.FullName;
                            isFirst = true;
                        }

                        monthTimeSheetWorker.Date = firstDateInMonth;
                        monthTimeSheetWorker.DirectoryPostId = currentPost.DirectoryPostId;
                        monthTimeSheetWorker.PostChangeDate = currentPost.ChangeDate;
                        monthTimeSheetWorker.PostName = currentPost.DirectoryPost.Name;
                        monthTimeSheetWorker.SalaryInHour = currentPost.DirectoryPost.UserWorkerSalary / countWorkDaysInMonth / 8;
                        monthTimeSheetWorker.Hours = new ObservableCollection<HourWorker>();
                        for (int i = 0; i < 31; i++)
                        {
                            var hourWorker = new HourWorker();
                            hourWorker.OnChange += hourWorker_OnChange;
                            monthTimeSheetWorker.Hours.Add(hourWorker);
                        }

                        monthTimeSheetWorker.OverTime = 0;
                    }

                    int indexHour = 0;
                    for (DateTime date = firstDateInMonth; date <= lastDateInMonth && date <= DateTime.Now; date = date.AddDays(1))
                    {
                        if (date.Date >= currentPost.ChangeDate.Date && (currentPost.FireDate == null || currentPost.FireDate != null && currentPost.FireDate.Value.Date >= date.Date))
                        {
                            var day = worker.InfoDates.FirstOrDefault(d => d.Date.Date == date.Date);

                            if (day != null)
                            {
                                string hour = null;

                                switch (day.DescriptionDay)
                                {
                                    case DescriptionDay.Был:
                                        hour = day.CountHours.ToString();
                                        monthTimeSheetWorker.OverTime += day.CountHours.Value > 8 ? day.CountHours.Value - 8 : 0;

                                        salary += day.CountHours.Value <= 8 ? day.CountHours.Value * monthTimeSheetWorker.SalaryInHour : (8 + ((day.CountHours.Value - 8) * 2)) * monthTimeSheetWorker.SalaryInHour;
                                        break;
                                    case DescriptionDay.Б:
                                        if (monthTimeSheetWorker.SickDays < 5)
                                        {
                                            monthTimeSheetWorker.SickDays++;
                                            salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        }
                                        else
                                        {
                                            monthTimeSheetWorker.MissDays++;
                                        }
                                        break;
                                    case DescriptionDay.О:
                                        monthTimeSheetWorker.VocationDays++;
                                        salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        break;
                                    case DescriptionDay.ДО:
                                        break;
                                    case DescriptionDay.П:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    case DescriptionDay.С:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    default:
                                        break;
                                }

                                if (day.DescriptionDay != DescriptionDay.Был)
                                {
                                    hour = day.DescriptionDay.ToString();
                                }

                                monthTimeSheetWorker.Hours[indexHour].WorkerId = worker.Id;
                                monthTimeSheetWorker.Hours[indexHour].Date = date;
                                monthTimeSheetWorker.Hours[indexHour].Value = hour;
                            }
                        }

                        indexHour++;
                    }
                }

                var monthTimeSheetWorkerFinalSalary = MonthTimeSheetWorkers.First(m => m.WorkerId == worker.Id && m.FullName != null);

                var infoMonth = worker.InfoMonthes.FirstOrDefault(m => m.Date.Year == SelectedYear && m.Date.Month == SelectedMonth);
                if (infoMonth != null)
                {
                    monthTimeSheetWorkerFinalSalary.PrepaymentCash = infoMonth.PrepaymentCash;
                    monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction = infoMonth.PrepaymentBankTransaction;
                    monthTimeSheetWorkerFinalSalary.VocationPayment = infoMonth.VocationPayment;
                    monthTimeSheetWorkerFinalSalary.SalaryAV = infoMonth.SalaryAV;
                    monthTimeSheetWorkerFinalSalary.SalaryFenox = infoMonth.SalaryFenox;
                    monthTimeSheetWorkerFinalSalary.Panalty = infoMonth.Panalty;
                    monthTimeSheetWorkerFinalSalary.Inventory = infoMonth.Inventory;
                    monthTimeSheetWorkerFinalSalary.BirthDays = infoMonth.BirthDays;
                    monthTimeSheetWorkerFinalSalary.Bonus = infoMonth.Bonus;
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - monthTimeSheetWorkerFinalSalary.PrepaymentCash - monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction -
                       monthTimeSheetWorkerFinalSalary.VocationPayment - monthTimeSheetWorkerFinalSalary.SalaryAV - monthTimeSheetWorkerFinalSalary.SalaryFenox -
                       monthTimeSheetWorkerFinalSalary.Panalty - monthTimeSheetWorkerFinalSalary.Inventory - monthTimeSheetWorkerFinalSalary.BirthDays + monthTimeSheetWorkerFinalSalary.Bonus;
                }
            }

            _isLoad = true;
        }

        private void hourWorker_OnChange(int workerId)
        {
            if (_isLoad)
            {
                BC.RefreshContext();

                var firstDateInMonth = new DateTime(SelectedYear, SelectedMonth, 1);
                var lastDateInMonth = new DateTime(SelectedYear, SelectedMonth, DateTime.DaysInMonth(SelectedYear, SelectedMonth));

                var worker = BC.GetDirectoryWorker(workerId);

                var currentPosts = BC.GetCurrentPosts(worker.Id, SelectedYear, SelectedMonth);

                double salary = 0;
                foreach (var currentPost in currentPosts)
                {
                    var monthTimeSheetWorker = MonthTimeSheetWorkers.FirstOrDefault(m => m.WorkerId == worker.Id && m.DirectoryPostId == currentPost.DirectoryPostId);

                    monthTimeSheetWorker.OverTime = 0;
                    monthTimeSheetWorker.MissDays = 0;
                    monthTimeSheetWorker.SickDays = 0;
                    monthTimeSheetWorker.VocationDays = 0;

                    int indexHour = 0;
                    for (DateTime date = firstDateInMonth; date <= lastDateInMonth && date <= DateTime.Now; date = date.AddDays(1))
                    {
                        if (date.Date >= currentPost.ChangeDate.Date && (currentPost.FireDate == null || currentPost.FireDate != null && currentPost.FireDate.Value.Date >= date.Date))
                        {
                            var day = worker.InfoDates.FirstOrDefault(d => d.Date.Date == date.Date);

                            if (day != null)
                            {
                                string hour = null;

                                switch (day.DescriptionDay)
                                {
                                    case DescriptionDay.Был:
                                        hour = day.CountHours.ToString();
                                        monthTimeSheetWorker.OverTime += day.CountHours.Value > 8 ? day.CountHours.Value - 8 : 0;

                                        salary += day.CountHours.Value <= 8 ? day.CountHours.Value * monthTimeSheetWorker.SalaryInHour : (8 + ((day.CountHours.Value - 8) * 2)) * monthTimeSheetWorker.SalaryInHour;
                                        break;
                                    case DescriptionDay.Б:
                                        if (monthTimeSheetWorker.SickDays < 5)
                                        {
                                            monthTimeSheetWorker.SickDays++;
                                            salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        }
                                        else
                                        {
                                            monthTimeSheetWorker.MissDays++;
                                        }
                                        break;
                                    case DescriptionDay.О:
                                        monthTimeSheetWorker.VocationDays++;
                                        salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        break;
                                    case DescriptionDay.ДО:
                                        break;
                                    case DescriptionDay.П:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    case DescriptionDay.С:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        indexHour++;
                    }
                }

                var monthTimeSheetWorkerFinalSalary = MonthTimeSheetWorkers.First(m => m.WorkerId == worker.Id && m.FullName != null);

                var infoMonth = worker.InfoMonthes.FirstOrDefault(m => m.Date.Year == SelectedYear && m.Date.Month == SelectedMonth);
                if (infoMonth != null)
                {
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - monthTimeSheetWorkerFinalSalary.PrepaymentCash - monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction -
                       monthTimeSheetWorkerFinalSalary.VocationPayment - monthTimeSheetWorkerFinalSalary.SalaryAV - monthTimeSheetWorkerFinalSalary.SalaryFenox -
                       monthTimeSheetWorkerFinalSalary.Panalty - monthTimeSheetWorkerFinalSalary.Inventory - monthTimeSheetWorkerFinalSalary.BirthDays + monthTimeSheetWorkerFinalSalary.Bonus;
                }
            }
        }
    }
}

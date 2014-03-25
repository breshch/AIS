using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MonthTimeSheetViewModel : ViewModel
    {
        private bool _isLoad;

        private int _selectedYear;
        [StopNotify]
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();

                Monthes = new ObservableCollection<int>(BC.GetMonthes(_selectedYear));
                if (Monthes.Any())
                {
                    SelectedMonth = Monthes.Max();
                }
            }
        }

        private int _selectedMonth;
        [StopNotify]
        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();

                RefreshHeaderDays();
                RefreshWorkers();
            }
        }
        public ObservableCollection<int> Years { get; set; }
        public ObservableCollection<int> Monthes { get; set; }


        public ObservableCollection<MonthTimeSheetWorker> MonthTimeSheetWorkers { get; set; }

        public ObservableCollection<HeaderDayMonthTimeSheet> HeaderDayMonthTimeSheets { get; set; }

        public MonthTimeSheetViewModel()
        {
            MouseDoubleClickCommand = new RelayCommand(MouseDoubleClick);

            Years = new ObservableCollection<int>(BC.GetYears());
            if (Years.Any())
            {
                SelectedYear = Years.Max();
            }
        }

        private void RefreshHeaderDays()
        {
            if (HeaderDayMonthTimeSheets == null)
            {
                HeaderDayMonthTimeSheets = new ObservableCollection<HeaderDayMonthTimeSheet>();
            }
            else
            {
                HeaderDayMonthTimeSheets.Clear();
            }

            for (int i = 0; i < 31; i++)
            {
                HeaderDayMonthTimeSheets.Add(new HeaderDayMonthTimeSheet());
            }

            var currentDate = DateTime.Now;

            var firstDateInMonth = new DateTime(SelectedYear, SelectedMonth, 1);

            int lastDay;
            if (SelectedYear == currentDate.Year && SelectedMonth == currentDate.Month)
            {
                lastDay = currentDate.Day;
            }
            else
            {
                lastDay = DateTime.DaysInMonth(SelectedYear, SelectedMonth);
            }
            
            var lastDateInMonth = new DateTime(SelectedYear, SelectedMonth, lastDay);

            for (DateTime date = firstDateInMonth; date.Date <= lastDateInMonth.Date; date = date.AddDays(1))
            {
                HeaderDayMonthTimeSheets[date.Day - 1].Header = date.Day + Environment.NewLine + date.ToString("ddd", new CultureInfo(Properties.Settings.Default.Language));
                HeaderDayMonthTimeSheets[date.Day - 1].IsVisible = true;
            }
        }

        private void RefreshWorkers()
        {
            _isLoad = false;

            if (MonthTimeSheetWorkers == null)
            {
                MonthTimeSheetWorkers = new ObservableCollection<MonthTimeSheetWorker>();
            }
            else
            {
                MonthTimeSheetWorkers.Clear();
            }

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
                    for (DateTime date = firstDateInMonth; date.Date <= lastDateInMonth.Date && date.Date <= DateTime.Now.Date; date = date.AddDays(1))
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
                    monthTimeSheetWorkerFinalSalary.PrepaymentCash = infoMonth.PrepaymentCash.ToString();
                    monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction = infoMonth.PrepaymentBankTransaction.ToString();
                    monthTimeSheetWorkerFinalSalary.VocationPayment = infoMonth.VocationPayment.ToString();
                    monthTimeSheetWorkerFinalSalary.SalaryAV = infoMonth.SalaryAV.ToString();
                    monthTimeSheetWorkerFinalSalary.SalaryFenox = infoMonth.SalaryFenox.ToString();
                    monthTimeSheetWorkerFinalSalary.Panalty = infoMonth.Panalty.ToString();
                    monthTimeSheetWorkerFinalSalary.Inventory = infoMonth.Inventory.ToString();
                    monthTimeSheetWorkerFinalSalary.BirthDays = infoMonth.BirthDays;
                    monthTimeSheetWorkerFinalSalary.Bonus = infoMonth.Bonus.ToString();
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.VocationPayment) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryAV) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryFenox) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
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
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction) -
                      double.Parse(monthTimeSheetWorkerFinalSalary.VocationPayment) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryAV) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryFenox) -
                      double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
                }
            }
        }


        #region Commands

        public RelayCommand MouseDoubleClickCommand { get; set; }

        private void MouseDoubleClick(object parameter)
        {
            //var monthTimeSheetWorker = parameter as MonthTimeSheetWorker;

            //if (monthTimeSheetWorker.FullName != null)
            //{
            //    Debug.WriteLine(monthTimeSheetWorker.FullName);
            //}

            
        }

        #endregion

    }
}

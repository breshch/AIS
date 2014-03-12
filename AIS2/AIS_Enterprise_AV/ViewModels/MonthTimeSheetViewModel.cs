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
            MonthTimeSheetWorkers = new ObservableCollection<MonthTimeSheetWorker>();

            int countWorkDaysInMonth = HelperCalendar.GetCountWorkDaysInMonth(SelectedYear, SelectedMonth);
            var firstDateInMonth = new DateTime(SelectedYear, SelectedMonth, 1);
            var lastDateInMonth = new DateTime(SelectedYear, SelectedMonth, DateTime.DaysInMonth(SelectedYear, SelectedMonth));

            var workers = BC.GetDirectoryWorkers(SelectedYear, SelectedMonth).ToList();

            foreach (var worker in workers)
            {
                var currentPosts = BC.GetCurrentPosts(worker, SelectedYear, SelectedMonth);

                bool isFirst = false;
                foreach (var currentPost in currentPosts)
                {
                    var monthTimeSheetWorker = new MonthTimeSheetWorker();

                    if (!isFirst)
                    {
                        monthTimeSheetWorker.FullName = worker.FullName;
                        isFirst = true;
                    }

                    monthTimeSheetWorker.PostName = currentPost.DirectoryPost.Name;
                    monthTimeSheetWorker.SalaryInHour = currentPost.DirectoryPost.UserWorkerSalary / countWorkDaysInMonth / 8;



                    for (DateTime date = firstDateInMonth;  date <= lastDateInMonth; date = date.AddDays(1))
                    {
                        string hour = null;
                        if (date.Date >= currentPost.ChangeDate.Date && (currentPost.FireDate == null || currentPost.FireDate != null && currentPost.FireDate.Value.Date >= date.Date))
                        {
                            hour = 8.ToString();
                        }

                        monthTimeSheetWorker.Hours.Add(hour);
                    }

                    MonthTimeSheetWorkers.Add(monthTimeSheetWorker);
                }
            }
        }
    }
}

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

                        monthTimeSheetWorker.DirectoryPostId = currentPost.DirectoryPostId;
                        monthTimeSheetWorker.PostName = currentPost.DirectoryPost.Name;
                        monthTimeSheetWorker.SalaryInHour = currentPost.DirectoryPost.UserWorkerSalary / countWorkDaysInMonth / 8;
                        
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
                                        break;
                                    case DescriptionDay.Б:
                                        if (monthTimeSheetWorker.SickDays < 5)
                                        {
                                            monthTimeSheetWorker.SickDays++;
                                        }
                                        else
                                        {
                                            monthTimeSheetWorker.MissDays++;
                                        }
                                        break;
                                    case DescriptionDay.О:
                                        monthTimeSheetWorker.VocationDays++;
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

                                if (day.DescriptionDay == DescriptionDay.Был)
                                {
                                     
                                }
                                else
                                {
                                    hour = day.DescriptionDay.ToString();
                                }

                                monthTimeSheetWorker.Hours[indexHour] = hour;

                               
                            }
                        }

                        indexHour++;
                    }
                }
            }
        }
    }
}

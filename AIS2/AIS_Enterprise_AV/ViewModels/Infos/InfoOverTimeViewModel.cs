using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoOverTimeViewModel : ViewModel
    {
        #region Base

        private class DateProcessing
        {
            public DateTime Date { get; set; }
            public bool IsProcessed { get; set; }
        }

        private List<DateProcessing> _listDatesOfOverTime;

        public InfoOverTimeViewModel(List<DateTime> listDatesOfOverTime, DateTime startDate, DateTime endDate)
        {
            SaveOverTimeCommand = new RelayCommand(SaveOverTime);

            _listDatesOfOverTime = new List<DateProcessing>(listDatesOfOverTime.Select(d => new DateProcessing { Date = d, IsProcessed = false }));

            var allowedDates = BC.GetInfoOverTimeDates(startDate.Year, startDate.Month).ToList().Concat(listDatesOfOverTime);

            if (_listDatesOfOverTime.Any())
            {
                SelectedOverTimeDate = _listDatesOfOverTime.First().Date;
            }
            else
            {
                SelectedOverTimeDate = allowedDates.Last();
            }

            Calendar dummyCal = new Calendar();
            BlackoutDates = new CalendarBlackoutDatesCollection(dummyCal);

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (!allowedDates.Contains(date))
                {
                    CalendarDateRange r = new CalendarDateRange(date, date);
                    BlackoutDates.Add(r);
                }
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion


        #region Properties

        private DateTime _selectedOverTimeDate;
        [StopNotify]
        public DateTime SelectedOverTimeDate
        {
            get
            {
                return _selectedOverTimeDate;
            }
            set
            {
                _selectedOverTimeDate = value;
                OnPropertyChanged();

                var overTime = BC.GetInfoOverTime(_selectedOverTimeDate);

                if (overTime != null)
                {
                    OverTimeDescription = overTime.Description;
                }
                else
                {
                    OverTimeDescription = null;
                }
            }
        }

        private string _overTimeDescription;
        [StopNotify]
        public string OverTimeDescription
        {
            get
            {
                return _overTimeDescription;
            }
            set
            {
                _overTimeDescription = value;
                OnPropertyChanged();
            }
        }

        public CalendarBlackoutDatesCollection BlackoutDates { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion


        #region Commands

        public RelayCommand SaveOverTimeCommand { get; set; }

        private void SaveOverTime(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(OverTimeDescription))
            {
                if (!BC.IsInfoOverTimeDate(SelectedOverTimeDate))
                {
                    BC.AddInfoOverTime(SelectedOverTimeDate, OverTimeDescription);
                    _listDatesOfOverTime.First(o => o.Date.Date == SelectedOverTimeDate.Date).IsProcessed = true;
                }
                else
                {
                    BC.EditInfoOverTime(SelectedOverTimeDate, OverTimeDescription);
                }

                var dateProcess = _listDatesOfOverTime.FirstOrDefault(o => o.IsProcessed == false);

                if (dateProcess != null)
                {
                    SelectedOverTimeDate = dateProcess.Date;
                }
                else
                {
                    var window = (Window)parameter;

                    if (window != null)
                    {
                        window.Close();
                    }
                }
                
            }
            else
            {
                BC.RemoveInfoOverTime(SelectedOverTimeDate);
            }
        }

        #endregion
    }
}

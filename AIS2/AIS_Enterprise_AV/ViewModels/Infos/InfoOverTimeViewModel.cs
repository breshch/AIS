using AIS_Enterprise_AV.Models.Directories;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoOverTimeViewModel : ViewModelAV
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
            SaveOverTimeCommand = new RelayCommand(SaveOverTime, CanSaveOverTime);
            RemoveOverTimeCommand = new RelayCommand(RemoveOverTime, CanRemoveOverTime);

            RefreshDirectoryRCs();

            _listDatesOfOverTime = new List<DateProcessing>(listDatesOfOverTime.Select(d => new DateProcessing { Date = d, IsProcessed = false }));

            var allowedDates = BC.GetInfoOverTimeDates(startDate.Year, startDate.Month).ToList().Concat(listDatesOfOverTime);

            Calendar dummyCal = new Calendar();
            BlackoutDates = new CalendarBlackoutDatesCollection(dummyCal);

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (!allowedDates.Any(a => a.Date == date.Date))
                {
                    CalendarDateRange r = new CalendarDateRange(date, date);
                    BlackoutDates.Add(r);
                }
            }

            StartDate = startDate;
            EndDate = endDate;
            

            if (!BC.IsWeekend(SelectedOverTimeDate))
	        {
                SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, 17, 0, 0);
	        }
            else
            {
                SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, 8, 0, 0);
            }

            SelectedEndTime = SelectedStartTime;

            if (_listDatesOfOverTime.Any())
            {
                SelectedOverTimeDate = _listDatesOfOverTime.First().Date;
            }
            else
            {
                SelectedOverTimeDate = allowedDates.Last();
            }
        }

        private void RefreshDirectoryRCs()
        {
            DirectoryRCs = new ObservableCollection<DirectoryRC>(BC.GetDirectoryRCs());
        }

        private void ClearInputData()
        {
            SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, 17, 00, 00);
            SelectedEndTime = SelectedStartTime;

            foreach (var rc in DirectoryRCs)
            {
                rc.IsChecked = false;
            }

            OverTimeDescription = null;
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
                if (_selectedOverTimeDate == value)
                {
                    return;
                }

                _selectedOverTimeDate = value;
                OnPropertyChanged();

                ClearInputData();

                //BC.RefreshContext();
                var overTime = BC.GetInfoOverTime(_selectedOverTimeDate);

                if (overTime != null)
                {
                    SelectedStartTime = overTime.StartDate;
                    SelectedEndTime = overTime.EndDate;

                    foreach (var rc in overTime.DirectoryRCs.ToList())
                    {
                        DirectoryRCs.First(r => r.Id == rc.Id).IsChecked = true;
                    }

                    //foreach (var rc in DirectoryRCs)
                    //{
                    //    rc.IsChecked = false;

                    //    if (overTime.DirectoryRCs.Select(r => r.Name).Contains(rc.Name))
                    //    {
                    //        Debug.WriteLine(rc.Name);
                    //        rc.IsChecked = true;
                    //    }
                    //}

                    OverTimeDescription = overTime.Description;
                }
                else
                {
                    var infoDates = BC.GetInfoDates(_selectedOverTimeDate);

                    double hours = 0;

                    if (BC.IsWeekend(_selectedOverTimeDate))
                    {
                        hours = infoDates.Where(d => d.CountHours != null).Max(d => d.CountHours.Value);

                    }
                    else
                    {
                        hours = infoDates.Where(d => d.CountHours != null && d.CountHours.Value > 8).Max(d => d.CountHours.Value) - 8;
                    }

                    SelectedEndTime = SelectedStartTime.AddHours(hours);
                }
            }
        }

        public DateTime SelectedStartTime { get; set; }

        public DateTime SelectedEndTime { get; set; }

        [Required]
        [Display(Name="Переработка")]
        public string OverTimeDescription { get; set; }

        public CalendarBlackoutDatesCollection BlackoutDates { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ObservableCollection<DirectoryRC> DirectoryRCs { get; set; }

        #endregion


        #region Commands

        public RelayCommand SaveOverTimeCommand { get; set; }

        public RelayCommand RemoveOverTimeCommand { get; set; }

        private void SaveOverTime(object parameter)
        {
            if (HelperMethods.IsFisrtTimeMoreSecondTime(SelectedEndTime, SelectedStartTime))
            {
                if (DirectoryRCs.Any(r => r.IsChecked))
                {
                    if (!BC.IsInfoOverTimeDate(SelectedOverTimeDate))
                    {
                        BC.AddInfoOverTime(SelectedStartTime, SelectedEndTime, DirectoryRCs.Where(r => r.IsChecked).ToList(), OverTimeDescription);
                        _listDatesOfOverTime.First(o => o.Date.Date == SelectedOverTimeDate.Date).IsProcessed = true;
                    }
                    else
                    {
                        BC.EditInfoOverTime(SelectedStartTime, SelectedEndTime, DirectoryRCs.Where(r => r.IsChecked).ToList(), OverTimeDescription);
                    }

                    var dateProcess = _listDatesOfOverTime.FirstOrDefault(o => o.IsProcessed == false);

                    if (dateProcess != null)
                    {
                        SelectedOverTimeDate = dateProcess.Date;
                        RefreshDirectoryRCs();
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
                    MessageBox.Show("Выберите ЦО");
                }
            }
            else
            {
                MessageBox.Show("Время окончания меньше, чем время начала переработки");
            }
        }

        private void RemoveOverTime(object parameter)
        {
            BC.RemoveInfoOverTime(SelectedOverTimeDate);

            var dateProcess = _listDatesOfOverTime.FirstOrDefault(o => o.IsProcessed == false);

            if (dateProcess != null)
            {
                SelectedOverTimeDate = dateProcess.Date;
                RefreshDirectoryRCs();
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

        private bool CanSaveOverTime(object parameter)
        {
            return IsValidateAllProperties();
        }

        private bool CanRemoveOverTime(object parameter)
        {
            return true;//BC.IsInfoOverTimeDate(SelectedOverTimeDate);
        }
        #endregion
    }
}

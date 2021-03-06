﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoOverTimeViewModel : ViewModelGlobal
    {
        #region Base

        private const int START_TIME_WEEKEND = 8;
        private const int START_TIME_WORK_DAY = 17;  
        private class DateProcessing
        {
            public DateTime Date { get; set; }
            public bool IsProcessed { get; set; }
        }

        private List<DateProcessing> _listDatesOfOverTime;
		private readonly string[] _rcsFenoxNotEnabled = { "МО-2", "ПАМ-1" };

        public InfoOverTimeViewModel(List<DateTime> listDatesOfOverTime, DateTime startDate, DateTime endDate)
        {
            SaveOverTimeCommand = new RelayCommand(SaveOverTime, CanSaveOverTime);
            RemoveOverTimeCommand = new RelayCommand(RemoveOverTime, CanRemoveOverTime);

			RefreshDirectoryRCs(startDate.Year, startDate.Month);

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
                SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, START_TIME_WORK_DAY, 0, 0);
	        }
            else
            {
                SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, START_TIME_WEEKEND, 0, 0);
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

        private void RefreshDirectoryRCs(int year, int month)
        {
	        var rcs = BC.GetDirectoryRCsByPercentage(year, month);

	        foreach (var rc in rcs)
	        {
		        if (!_rcsFenoxNotEnabled.Contains(rc.Name))
		        {
			        rc.IsEnabled = true;
		        }
	        }

            DirectoryRCs = new ObservableCollection<DirectoryRC>(rcs);
        }

        private void ClearInputData()
        {
            SelectedStartTime = new DateTime(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month, SelectedOverTimeDate.Day, START_TIME_WORK_DAY, 00, 00);
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
        [NoMagic]
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
                RaisePropertyChanged();

                ClearInputData();

                var overTime = BC.GetInfoOverTime(_selectedOverTimeDate);

                if (overTime != null)
                {
                    SelectedStartTime = overTime.StartDate;
                    SelectedEndTime = overTime.EndDate;

                    DirectoryRCs.Clear();
					foreach (var rc in BC.GetDirectoryRCsByPercentage(SelectedStartTime.Year, SelectedStartTime.Month).ToList())
                    {
                        rc.IsChecked = overTime.CurrentRCs.ToList().Any(r => r.DirectoryRC.Name == rc.Name);
                        DirectoryRCs.Add(rc);
                    }

                    OverTimeDescription = overTime.Description;
                }
                else
                {
                    var infoDates = BC.GetInfoDates(_selectedOverTimeDate).ToList();

                    double hours = 0;

                    if (BC.IsWeekend(_selectedOverTimeDate))
                    {
                        hours = infoDates.Where(d => d.CountHours != null).Max(d => d.CountHours.Value);
                        SelectedStartTime = new DateTime(_selectedOverTimeDate.Year, _selectedOverTimeDate.Month, _selectedOverTimeDate.Day, START_TIME_WEEKEND, 0, 0);
                    }
                    else
                    {
                        hours = infoDates.Where(d => d.CountHours != null && d.CountHours.Value > 8).Max(d => d.CountHours.Value) - 8;
                        SelectedStartTime = new DateTime(_selectedOverTimeDate.Year, _selectedOverTimeDate.Month, _selectedOverTimeDate.Day, START_TIME_WORK_DAY, 0, 0);
                    }

                    SelectedEndTime = SelectedStartTime.AddHours(hours);
                }
            }
        }

        public DateTime SelectedStartTime { get; set; }

        public DateTime SelectedEndTime { get; set; }

        private string _overTimeDescription;

        [Required]
        [Display(Name="Переработка")]
        public string OverTimeDescription
        {
            get
            {
                return _overTimeDescription;
            }
            set
            {
                _overTimeDescription = value;
                RaisePropertyChanged();
            }
        }

        public CalendarBlackoutDatesCollection BlackoutDates { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private ObservableCollection<DirectoryRC> _directoryRCs;
        public ObservableCollection<DirectoryRC> DirectoryRCs
        {
            get
            {
                return _directoryRCs;
            }
            set
            {
                _directoryRCs = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Commands

        public RelayCommand SaveOverTimeCommand { get; set; }

        public RelayCommand RemoveOverTimeCommand { get; set; }

        private void SaveOverTime(object parameter)
        {
            if (HelperMethods.IsFisrtTimeMoreSecondTime(SelectedEndTime, SelectedStartTime))
            {
				var rcMo5 = DirectoryRCs.FirstOrDefault(x => x.Name == "МО-5");
				if (rcMo5 != null)
				{
					foreach (var rcFenox in _rcsFenoxNotEnabled)
					{
						var rc = DirectoryRCs.FirstOrDefault(x => x.Name == rcFenox);
						if (rc != null)
						{
							rc.IsChecked = rcMo5.IsChecked;
						}
					}
				}

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
						RefreshDirectoryRCs(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month);
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
					RefreshDirectoryRCs(StartDate.Year, StartDate.Month);
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
            var infoDates = BC.GetInfoDates(SelectedOverTimeDate);

            bool isOverTime = false;
            if (BC.IsWeekend(SelectedOverTimeDate))
            {
                isOverTime = infoDates.Any(d => d.CountHours != null);
            }
            else
            {
                isOverTime = infoDates.Any(d => d.CountHours != null && d.CountHours > 8);
            }

            if (!isOverTime)
            {
                BC.RemoveInfoOverTime(SelectedOverTimeDate);

                var dateProcess = _listDatesOfOverTime.FirstOrDefault(o => o.IsProcessed == false);

                if (dateProcess != null)
                {
                    SelectedOverTimeDate = dateProcess.Date;
					RefreshDirectoryRCs(SelectedOverTimeDate.Year, SelectedOverTimeDate.Month);
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
                MessageBox.Show("Нельзя удалить переработку, так как есть сотрудники с переработкой на эту дату");
            }
        }

        private bool CanSaveOverTime(object parameter)
        {
            return IsValidateAllProperties();
        }

        private bool CanRemoveOverTime(object parameter)
        {
            try
            {
                return BC.IsInfoOverTimeDate(SelectedOverTimeDate);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}

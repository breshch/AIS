using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public InfoOverTimeViewModel(List<DateTime> listDatesOfOverTime)
        {
            SaveOverTimeCommand = new RelayCommand(SaveOverTime);

            _listDatesOfOverTime = new List<DateProcessing>(listDatesOfOverTime.Select(d => new DateProcessing { Date = d, IsProcessed = false }));

            if (_listDatesOfOverTime.Any())
            {
                SelectedOverTimeDate = _listDatesOfOverTime.First().Date;
            }
            else
            {
                SelectedOverTimeDate = DateTime.Now;
            }
            
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

        #endregion


        #region Commands

        public RelayCommand SaveOverTimeCommand { get; set; }

        private void SaveOverTime(object parameter)
        {
            BC.AddInfoOverTime(SelectedOverTimeDate, OverTimeDescription);
            _listDatesOfOverTime.First(o => o.Date.Date == SelectedOverTimeDate.Date).IsProcessed = true;

            var dateProcess = _listDatesOfOverTime.FirstOrDefault(o => o.IsProcessed == false);

            if (dateProcess != null)
            {
                SelectedOverTimeDate = dateProcess.Date;
            }
        }

        #endregion
    }
}

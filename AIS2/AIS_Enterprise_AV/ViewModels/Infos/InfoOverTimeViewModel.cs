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

        public InfoOverTimeViewModel()
        {
            SaveOverTimeCommand = new RelayCommand(SaveOverTime);

            SelectedOverTimeDate = DateTime.Now;
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
        }

        #endregion
    }
}

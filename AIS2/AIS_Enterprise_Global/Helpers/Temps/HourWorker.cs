using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class HourWorker : ViewModel
    {
        public event Action<int> OnChange;

        [StopNotify]
        public int WorkerId { get; set; }

        [StopNotify]
        public DateTime Date { get; set; }

        private string _value;
        [StopNotify]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                string prevValue = _value;

                _value = value;
                OnPropertyChanged();

                BC.EditInfoDateHour(WorkerId, Date, _value);

                OnChange(WorkerId);
            }
        }
    }
}

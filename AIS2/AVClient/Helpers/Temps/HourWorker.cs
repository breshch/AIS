using System;
using System.Windows;
using AVClient.AVServiceReference;

namespace AVClient.Helpers.Temps
{
    public class HourWorker : ViewModelGlobal
    {
        public event Action<int> OnChange;

        [NoMagic]
        public int WorkerId { get; set; }

        [NoMagic]
        public DateTime Date { get; set; }

        private string _value;
        [NoMagic]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                value = value.ToUpper().Replace(".", ",");

                if (_value == value)
                {
                    return;
                }

                if (!Enum.IsDefined(typeof(DescriptionDay), value))
                {
                    double result;
                    if (!double.TryParse(value, out result))
                    {
                        MessageBox.Show("Введите общепринятые сокращения.");
                        return;
                    }

                    if (result <= 0 || result > 16)
                    {
                        MessageBox.Show("Введите только число, большее 0 и меньшее, либо равное 16.");
                        return;
                    }
                }

                string prevValue = _value;

                _value = value;
                RaisePropertyChanged();

                BC.EditInfoDateHour(WorkerId, Date, _value);

                OnChange(WorkerId);
            }
        }
    }
}

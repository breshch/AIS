using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos
{
    public class AddInfoSafeViewModel : ViewModelGlobal
    {

        #region Base
        private bool _isIncoming;
        public AddInfoSafeViewModel(string titleName, string buttonName, bool isIncoming)
        {
            Title = titleName;
            ButtonName = buttonName;
            _isIncoming = isIncoming;
            AddCommand = new RelayCommand(Add);
            SelectedDate = DateTime.Now;
            Currencies = new ObservableCollection<Currency>(Enum.GetNames(typeof(Currency)).Select(c => (Currency) Enum.Parse(typeof(Currency), c)));
            SelectedCurrency = Currencies.First();
        }

        #endregion

        #region Properties

        public string  Title { get; set; }
        public string  ButtonName { get; set; }
        public DateTime  SelectedDate { get; set; }
        public double SummCash { get; set; }
        public ObservableCollection<Currency> Currencies { get; set; }
        public Currency SelectedCurrency { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            BC.AddInfoSafeHand(SelectedDate, _isIncoming, SummCash, SelectedCurrency, CashType.Наличка, null);
            
            var window = parameter as Window;
            window.Close();    
        }

        #endregion
    }
}

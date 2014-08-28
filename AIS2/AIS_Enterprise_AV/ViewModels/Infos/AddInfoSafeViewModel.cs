
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
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
        }

        #endregion

        #region Properties

        public string  Title { get; set; }
        public string  ButtonName { get; set; }
        public DateTime  SelectedDate { get; set; }
        public double SummCash { get; set; }
        #endregion

        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            BC.AddInfoSafe(SelectedDate, _isIncoming, SummCash, CashType.Наличка, null);
            
            var window = parameter as Window;
            window.Close();    
        }

        #endregion
    }
}

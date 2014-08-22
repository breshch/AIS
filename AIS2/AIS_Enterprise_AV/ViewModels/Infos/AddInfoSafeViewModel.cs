
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

        public AddInfoSafeViewModel()
        {
            AddCommand = new RelayCommand(Add);
            SelectedDate = DateTime.Now;
        }

        #endregion

        #region Properties

        public DateTime  SelectedDate { get; set; }
        public double SummCash { get; set; }
        #endregion

        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            BC.AddInfoSafe(SelectedDate, false, SummCash, CashType.Наличка);
            
            var window = parameter as Window;
            window.Close();    
        }

        #endregion
    }
}

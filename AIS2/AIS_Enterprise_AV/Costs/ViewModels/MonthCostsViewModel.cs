using AIS_Enterprise_AV.Costs.Views;
using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.Costs.ViewModels
{
    public class MonthCostsViewModel : ViewModelGlobal
    {
        #region Base
        private bool _isNotTransportOnly;

        public MonthCostsViewModel()
        {
            _isNotTransportOnly = HelperMethods.IsPrivilege(BC, UserPrivileges.CostsVisibility_IsNotTransportOnly);

            Costs = new ObservableCollection<InfoCost>();

            Years = new ObservableCollection<int>(BC.GetInfoCostYears());
            if (Years.Any())
            {
                SelectedYear = Years.Last();
            }

            Cash = BC.GetInfoCash();

            ShowDayCostsCommand = new RelayCommand(ShowDayCosts);

            
            VisibilityCash = _isNotTransportOnly ? Visibility.Visible : Visibility.Collapsed;  
        }

        #endregion

        #region Properties
        public double Cash { get; set; }

        public ObservableCollection<int> Years { get; set; }

        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged();

                Monthes = new ObservableCollection<int>(BC.GetInfoCostMonthes(_selectedYear));
                if (Monthes.Any())
                {
                    SelectedMonth = Monthes.Last();
                }
            }
        }
        public ObservableCollection<int> Monthes { get; set; }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged();

                Costs.Clear();

                if (_isNotTransportOnly)
                {
                    foreach (var cost in BC.GetInfoCosts(SelectedYear, _selectedMonth))
                    {
                        Costs.Add(cost);
                    }
                }
                else
                {
                    foreach (var cost in BC.GetInfoCostsTransportExpenseOnly(SelectedYear, _selectedMonth))
                    {
                        Costs.Add(cost);
                    }
                }
            }
        }
        public double Summ { get; set; }

        public ObservableCollection<InfoCost> Costs { get; set; }

        public InfoCost SelectedCost { get; set; }

        public Visibility VisibilityCash { get; set; }


        #endregion

        #region Commands

        public RelayCommand ShowDayCostsCommand { get; set; }

        private void ShowDayCosts(object parameter)
        {
            if (SelectedCost != null)
            {
                var dayCostsView = new DayCostsView(SelectedCost.Date);
                dayCostsView.ShowDialog();
            }
            
        }

        #endregion

    }
}

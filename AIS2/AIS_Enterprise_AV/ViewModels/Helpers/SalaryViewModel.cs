using AIS_Enterprise_AV.Helpers;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class SalaryViewModel : ViewModelGlobal
    {
        #region Base
        
        public SalaryViewModel()
        {
            Years = new ObservableCollection<int>(BC.GetYears());
            if (Years.Any())
            {
                SelectedYear = Years.Last();
            }

            FormingSalaryCommand = new RelayCommand(FormingSalary);


        }

        
        #endregion


        #region Properties
        public ObservableCollection<int> Years { get; set; }
        public ObservableCollection<int> Monthes { get; set; }
        
        
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
                OnPropertyChanged();

                Monthes = new ObservableCollection<int>(BC.GetMonthes(_selectedYear));
                if (Monthes.Any())
                {
                    SelectedMonth = Monthes.Last();
                }
            }
        }
        public int SelectedMonth { get; set; }


        #endregion


        #region Commands

        public RelayCommand FormingSalaryCommand { get; set; }

        private void FormingSalary(object parameter)
        {
            FormingSalaryReport.CompletedReportMinsk(BC, SelectedYear, SelectedMonth);
            FormingSalaryReport.ComplitedReportSalaryWorkers(BC, SelectedYear, SelectedMonth);
        }


        #endregion
    }
}

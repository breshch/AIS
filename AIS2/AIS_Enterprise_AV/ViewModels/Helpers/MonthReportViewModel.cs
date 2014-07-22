using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class MonthReportViewModel : ViewModelGlobal
    {
        #region Base

        private Action<BusinessContext, int, int> _methodCreationReports;
        private GettingMonthes _methodGettingMonthes;

        public MonthReportViewModel(string title, Action<BusinessContext, int, int> methodCreationReports, GettingYears methodGettingYears, GettingMonthes methodGettingMonthes)
        {
            TitleName = title;
            _methodCreationReports = methodCreationReports;
            _methodGettingMonthes = methodGettingMonthes;

            Years = new ObservableCollection<int>(methodGettingYears(BC));
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
                RaisePropertyChanged();

                Monthes = new ObservableCollection<int>(_methodGettingMonthes(BC, SelectedYear));
                if (Monthes.Any())
                {
                    SelectedMonth = Monthes.Last();
                }
            }
        }
        public int SelectedMonth { get; set; }

        public string TitleName { get; set; }

        #endregion


        #region Commands

        public RelayCommand FormingSalaryCommand { get; set; }

        private void FormingSalary(object parameter)
        {
            _methodCreationReports.Invoke(BC, SelectedYear, SelectedMonth);
        }


        #endregion
    }
}

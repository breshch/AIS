using System;
using System.Collections.ObjectModel;
using System.Linq;
using AVClient.Helpers;

namespace AVClient.ViewModels.Helpers
{
    public class MonthReportViewModel : ViewModelGlobal
    {
        #region Base

        private Action<int, int> _methodCreationReports;
        private GettingMonthes _methodGettingMonthes;

        public MonthReportViewModel(string title, Action<int, int> methodCreationReports, GettingYears methodGettingYears, GettingMonthes methodGettingMonthes)
        {
            TitleName = title;
            _methodCreationReports = methodCreationReports;
            _methodGettingMonthes = methodGettingMonthes;

            Years = new ObservableCollection<int>(methodGettingYears());
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

                Monthes = new ObservableCollection<int>(_methodGettingMonthes(SelectedYear));
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
            _methodCreationReports.Invoke(SelectedYear, SelectedMonth);
        }


        #endregion
    }
}

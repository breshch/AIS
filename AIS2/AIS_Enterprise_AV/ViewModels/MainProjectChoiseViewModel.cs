using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.Infos.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainProjectChoiseViewModel : ViewModelGlobal
    {
        #region Base

        public MainProjectChoiseViewModel()
        {
            MonthTimeSheetCommand = new RelayCommand(MonthTimeSheet);
            RemainsCommand = new RelayCommand(Remains);
            ProcessingBookKeepingCommand = new RelayCommand(ProcessingBookKeeping);
            RemainsLoanCommand = new RelayCommand(RemainsLoan);
            
            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_MonthTimeSheetEnable.ToString()))
            {
                IsEnabledMonthTimeSheet = true;
            }

            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_DbFenoxEnable.ToString()))
            {
                IsEnabledDbFenox = true;
            }

            if (DirectoryUser.Privileges.Contains(UserPrivileges.MultyProject_DbFenoxEnable.ToString()))
            {
                IsEnabledDbFenox = true;
            }
        }

        #endregion


        #region Properties

        public bool IsEnabledMonthTimeSheet { get; set; }

        public bool IsEnabledDbFenox { get; set; }
        public bool IsEnabledProcessingBookKeeping { get; set; }
        #endregion


        #region Commands

        public RelayCommand MonthTimeSheetCommand { get; set; }
        public RelayCommand RemainsCommand { get; set; }
        public RelayCommand ProcessingBookKeepingCommand { get; set; }
        public RelayCommand RemainsLoanCommand { get; set; }


        private void MonthTimeSheet(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();

            HelperMethods.CloseWindow(parameter);
        }

        private void Remains(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new InfoRemainsViewModel(), new InfoRemainsView());

            HelperMethods.CloseWindow(parameter);
        }

        private void ProcessingBookKeeping(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new PercentageProcessingBookKeepingViewModel(), new PercentageProcessingBookKeepingView());

            HelperMethods.CloseWindow(parameter);
        }

        private void RemainsLoan(object parameter)
        {
            var window = parameter as Window;
            window.Visibility = Visibility.Hidden;

            HelperMethods.ShowView(new PickDateReportViewModel(), new PickDateReportView());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

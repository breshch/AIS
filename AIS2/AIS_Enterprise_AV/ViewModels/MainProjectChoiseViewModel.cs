using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.Infos.ViewModels;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIS_Enterprise_AV.ViewModels
{
    public class MainProjectChoiseViewModel : ViewModelGlobal
    {
        #region Base

        public MainProjectChoiseViewModel()
        {
            MonthTimeSheetCommand = new RelayCommand(MonthTimeSheet);
            CarPartsCommand = new RelayCommand(CarParts);
        }

        #endregion


        #region Properties
        


        #endregion

        #region Commands

        public RelayCommand MonthTimeSheetCommand { get; set; }
        public RelayCommand CarPartsCommand { get; set; }

        private void MonthTimeSheet(object parameter)
        {
            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();

            HelperMethods.CloseWindow(parameter);
        }

        private void CarParts(object parameter)
        {
            //var dialog = new OpenFileDialog();
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    string path = dialog.FileName;
            //    ConvertingCarPartsExcelToDB.ConvertImport(BC, path);
            //}

            //dialog = new OpenFileDialog();
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    string path = dialog.FileName;
            //    ConvertingCarPartsExcelToDB.ConvertRussian(BC, path);
            //}

            HelperMethods.ShowView(new AddEditContainersViewModel(), new AddEditContainersView());
            HelperMethods.CloseWindow(parameter);
        }
        #endregion
    }
}

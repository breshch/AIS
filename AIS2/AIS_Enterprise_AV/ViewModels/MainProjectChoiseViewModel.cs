using AIS_Enterprise_AV.Helpers.ExcelToDB;
using AIS_Enterprise_AV.Infos.ViewModels;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Infos;
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
            IncomingCommand = new RelayCommand(Incoming);
            OutcomingCommand = new RelayCommand(Outcoming);
        }

        #endregion


        #region Properties
        


        #endregion

        #region Commands

        public RelayCommand MonthTimeSheetCommand { get; set; }
        public RelayCommand IncomingCommand { get; set; }
        public RelayCommand OutcomingCommand { get; set; }

        private void MonthTimeSheet(object parameter)
        {
            var monthTimeSheetView = new MonthTimeSheetView();
            monthTimeSheetView.ShowDialog();

            HelperMethods.CloseWindow(parameter);
        }

        private void Incoming(object parameter)
        {
            HelperMethods.ShowView(new AddEditContainersViewModel<InfoInContainer, CurrentInContainerCarPart>("Приход"), new AddEditContainersView());
            HelperMethods.CloseWindow(parameter);
        }

        private void Outcoming(object parameter)
        {
            HelperMethods.ShowView(new AddEditContainersViewModel<InfoOutContainer, CurrentOutContainerCarPart>("Расход"), new AddEditContainersView());
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
        }

        #endregion
    }
}

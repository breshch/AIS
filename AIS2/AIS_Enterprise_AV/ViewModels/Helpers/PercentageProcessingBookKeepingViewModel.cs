using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIS_Enterprise_AV.Helpers.ConvertingExcel;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_AV.Helpers.ExcelToDB;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class PercentageProcessingBookKeepingViewModel : ViewModelGlobal
    {
        #region Base

        private int _prevPercentageRus;
        private int _prevPercentageImport;

        public PercentageProcessingBookKeepingViewModel()
        {
            CarPartPriceRusCommand = new RelayCommand(CarPartPriceRus);
            CarPartPriceImportCommand = new RelayCommand(CarPartPriceImport);
            LoadingFileCommand = new RelayCommand(LoadingFile);

            
            PercentageRus = BC.GetParameterValue<int>(ParameterType.PercentageRusBookKeeping);
            PercentageImport = BC.GetParameterValue<int>(ParameterType.PercentageImportBookKeeping);

            LastRusDate = BC.GetParameterValue<string>(ParameterType.LastRusDate);
            LastImportDate = BC.GetParameterValue<string>(ParameterType.LastImportDate);
            _prevPercentageRus = PercentageRus;
            _prevPercentageImport = PercentageImport;
        }

        #endregion


        #region Properties

        public int PercentageRus { get; set; }
        public int PercentageImport { get; set; }
        public string LastRusDate { get; set; }
        public string LastImportDate { get; set; }


        #endregion


        #region Commands

        public RelayCommand CarPartPriceRusCommand { get; set; }
        public RelayCommand CarPartPriceImportCommand { get; set; }
        public RelayCommand LoadingFileCommand { get; set; }



        private void CarPartPriceRus(object parameter)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                var priceDate = ConvertingCarPartsExcelToDB.ConvertPriceRus(BC, path);
                BC.EditParameter(ParameterType.LastRusDate, priceDate.ToShortDateString());
                LastRusDate = priceDate.ToShortDateString();
            }
        }

        private void CarPartPriceImport(object parameter)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                var priceDate = ConvertingCarPartsExcelToDB.ConvertPriceImport(BC, path);
                BC.EditParameter(ParameterType.LastImportDate, priceDate.ToShortDateString());
                LastImportDate = priceDate.ToShortDateString();
            }
        }


        private void LoadingFile(object parameter)
        {
            using (var openDialog = new OpenFileDialog())
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openDialog.FileName;

                    ProcessingInvoice.Procesing(BC, path, PercentageRus, PercentageImport);

                    if (_prevPercentageRus != PercentageRus)
                    {
                        BC.EditParameter(ParameterType.PercentageRusBookKeeping, PercentageRus.ToString());
                        _prevPercentageRus = PercentageRus;
                    }

                    if (_prevPercentageImport != PercentageImport)
                    {
                        BC.EditParameter(ParameterType.PercentageImportBookKeeping, PercentageImport.ToString());
                        _prevPercentageImport = PercentageImport;
                    }
                }
            }
        }

        #endregion
    }
}

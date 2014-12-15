using System;
using System.Collections.Generic;
using System.IO;
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
            LoadingFolderCommand = new RelayCommand(LoadingFolder);
            
            PercentageRus = BC.GetParameterValue<int>(ParameterType.PercentageRusBookKeeping);
            PercentageImport = BC.GetParameterValue<int>(ParameterType.PercentageImportBookKeeping);

            Currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
            SelectedCurrency = Currency.RUR;

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
        public List<Currency> Currencies  { get; set; }
        public Currency SelectedCurrency { get; set; }

        #endregion


        #region Commands

        public RelayCommand CarPartPriceRusCommand { get; set; }
        public RelayCommand CarPartPriceImportCommand { get; set; }
        public RelayCommand LoadingFileCommand { get; set; }
        public RelayCommand LoadingFolderCommand { get; set; }


        private void CarPartPriceRus(object parameter)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                var priceDate = ConvertingCarPartsExcelToDB.ConvertPriceRus(BC, path, SelectedCurrency);
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
                var priceDate = ConvertingCarPartsExcelToDB.ConvertPriceImport(BC, path, SelectedCurrency);
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

                    path = Reports.Helpers.ConvertXlsToXlsx(path);

                    var invoices = ProcessingInvoice.Procesing(BC, path, PercentageRus, PercentageImport);
                    ProcessingInvoice.ComplitedCompliteInvoice(path, PercentageRus, PercentageImport, invoices);

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

        private void LoadingFolder(object parameter)
        {
            using (var openDialog = new FolderBrowserDialog())
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string directoryPath = openDialog.SelectedPath;

                    foreach (var path in Directory.GetFiles(directoryPath))
                    {
                        var extension = Path.GetExtension(path);
                        if (extension == ".xls" || extension == ".xlsx")
                        {
                            string pathFile = path;
                            pathFile = Reports.Helpers.ConvertXlsToXlsx(path);

                            var invoices = ProcessingInvoice.Procesing(BC, pathFile, PercentageRus, PercentageImport);
                            ProcessingInvoice.ComplitedCompliteInvoice(pathFile, PercentageRus, PercentageImport, invoices);
                        }
                    }

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

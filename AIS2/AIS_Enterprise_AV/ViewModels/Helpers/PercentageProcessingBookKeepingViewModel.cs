using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIS_Enterprise_AV.Helpers.ConvertingExcel;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Helpers
{
    public class PercentageProcessingBookKeepingViewModel : ViewModelGlobal
    {
        #region Base

        private int _prevPercentageRus;
        private int _prevPercentageImport;

        public PercentageProcessingBookKeepingViewModel()
        {
            LoadingFileCommand = new RelayCommand(LoadingFile);

            PercentageRus = BC.GetParameterValue<int>("PercentageRusBookKeeping");
            PercentageImport = BC.GetParameterValue<int>("PercentageImportBookKeeping");

            _prevPercentageRus = PercentageRus;
            _prevPercentageImport = PercentageImport;
        }

        #endregion


        #region Properties

        public int PercentageRus { get; set; }
        public int PercentageImport { get; set; }

        #endregion


        #region Commands

        public RelayCommand LoadingFileCommand { get; set; }

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
                        BC.EditParameter("PercentageRusBookKeeping", PercentageRus.ToString());
                        _prevPercentageRus = PercentageRus;
                    }

                    if (_prevPercentageImport != PercentageImport)
                    {
                        BC.EditParameter("PercentageImportBookKeeping", PercentageImport.ToString());
                        _prevPercentageImport = PercentageImport;
                    }
                }
            }
        }

        #endregion
    }
}

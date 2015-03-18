using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;

namespace AIS_Enterprise_Global.ViewModels.Infos
{
    public class InfoPanaltyViewModel : ViewModelGlobal
    {
        #region Base
        private int _workerId;

        public InfoPanaltyViewModel(int workerId, DateTime date)
        {
            SaveInfoPanaltyCommand = new RelayCommand(SaveInfoPanalty);
            RemoveInfoPanaltyCommand = new RelayCommand(RemoveInfoPanalty);

            _workerId = workerId;
            InfoPanaltyWorkerFullName = BC.GetDirectoryWorker(workerId).FullName;
            SelectedInfoPanaltyDate = date;

            var infoPanalty = BC.GetInfoPanalty(workerId, date);
            if (infoPanalty != null)
            {
                InfoPanaltySumm = infoPanalty.Summ.ToString();
                InfoPanaltyDescription = infoPanalty.Description;
            }
        }

       
        #endregion


        #region Properties
        public string InfoPanaltyWorkerFullName { get; set; }

        public DateTime SelectedInfoPanaltyDate { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Штраф")]
        public string InfoPanaltySumm { get; set; }
        
        [Required]
        [Display(Name = "Описание штрафа")]
        public string InfoPanaltyDescription { get; set; }
       
        #endregion


        #region Commands

        public RelayCommand SaveInfoPanaltyCommand { get; set; }
        public RelayCommand RemoveInfoPanaltyCommand { get; set; }

        private void SaveInfoPanalty(object parameter)
        {
            double summ = double.Parse(InfoPanaltySumm);
            if (!BC.IsInfoPanalty(_workerId, SelectedInfoPanaltyDate))
            {
                BC.AddInfoPanalty(_workerId, SelectedInfoPanaltyDate, summ, InfoPanaltyDescription);
            }
            else
            {
                BC.EditInfoPanalty(_workerId, SelectedInfoPanaltyDate, summ, InfoPanaltyDescription);
            }

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        private void RemoveInfoPanalty(object parameter)
        {
            BC.RemoveInfoPanalty(_workerId, SelectedInfoPanaltyDate);

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

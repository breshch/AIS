using System;
using System.Linq;
using AIS_Enterprise_AV.ViewModels.Currents.Base;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Currents
{
    public class CurrentAddContainerViewModel : CurrentBaseContainerViewModel
    {
        #region Base

        public CurrentAddContainerViewModel(bool isIncoming)
            : base(isIncoming)
        {
            TitleContainerName = "Добавление контейнера";
            ButtonAddEditContainerName = "Добавить контейнер";
            DatePhysical = DateTime.Now;
            DateOrder = DateTime.Now;

            AddEditConteinerCommand = new RelayCommand(AddContainer, IsAnyCarParts);
        }

        #endregion

        #region Commands

        private void AddContainer(object parameter)
        {
            BC.AddInfoContainer(Name, Description, DatePhysical, DateOrder, _isIncoming, CurrentContainerCarParts.ToList());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

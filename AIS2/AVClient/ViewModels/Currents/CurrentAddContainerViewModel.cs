using System;
using System.Linq;
using AVClient.Helpers;
using AVClient.ViewModels.Currents.Base;

namespace AVClient.ViewModels.Currents
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
            BC.AddInfoContainer(Name, Description, DatePhysical, DateOrder, _isIncoming, CurrentContainerCarParts.ToArray());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

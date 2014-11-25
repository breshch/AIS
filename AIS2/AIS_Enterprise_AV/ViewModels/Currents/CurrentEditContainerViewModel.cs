using AIS_Enterprise_AV.ViewModels.Currents.Base;
using AIS_Enterprise_AV.ViewModels.Directories.Base;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Currents
{
    public class CurrentEditContainerViewModel : CurrentBaseContainerViewModel
    {
        #region Base

        private int _containerId;

        public CurrentEditContainerViewModel(int containerId, bool isIncoming) : base(isIncoming)
        {
            TitleContainerName = "Редактирование контейнера";
            ButtonAddEditContainerName = "Изменить контейнер";

            var container = BC.GetInfoContainer(containerId);

            Name = container.Name;
            Description = container.Description;
            DatePhysical = container.DatePhysical;
            DateOrder = container.DateOrder;
            CurrentContainerCarParts = new ObservableCollection<CurrentContainerCarPart>(container.CarParts);
            _containerId = container.Id;

            AddEditConteinerCommand = new RelayCommand(EditContainer, IsAnyCarParts);
        }

        #endregion

        #region Commands

        private void EditContainer(object parameter)
        {
            BC.EditInfoContainer(_containerId, Name, Description, DatePhysical, DateOrder, _isIncoming, CurrentContainerCarParts.ToList());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

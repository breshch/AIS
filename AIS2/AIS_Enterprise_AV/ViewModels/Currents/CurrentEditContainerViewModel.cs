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
    public class CurrentEditContainerViewModel<InfoContainer, CurrentContainerCarPart> : CurrentBaseContainerViewModel<InfoContainer, CurrentContainerCarPart>
        where InfoContainer : InfoBaseContainer, new()
        where CurrentContainerCarPart : CurrentBaseContainerCarPart, new()
    {
        #region Base

        private int _containerId;

        public CurrentEditContainerViewModel(int containerId) : base()
        {
            TitleContainerName = "Редактирование контейнера";
            ButtonAddEditContainerName = "Изменить контейнер";

            var container = BC.GetInfoContainer<InfoContainer>(containerId);

            Name = container.Name;
            Description = container.Description;
            Date = container.Date;
            CurrentContainerCarParts = new ObservableCollection<CurrentContainerCarPart>(container.CarParts.Cast<CurrentContainerCarPart>());
            _containerId = container.Id;

            AddEditConteinerCommand = new RelayCommand(EditContainer, IsAnyCarParts);
        }

        #endregion

        #region Commands

        private void EditContainer(object parameter)
        {
            BC.EditInfoContainer<InfoContainer, CurrentContainerCarPart>(_containerId, Name, Description, Date, CurrentContainerCarParts.ToList());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

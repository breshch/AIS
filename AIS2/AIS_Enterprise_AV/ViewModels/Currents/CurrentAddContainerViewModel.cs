using AIS_Enterprise_AV.ViewModels.Currents.Base;
using AIS_Enterprise_AV.ViewModels.Directories.Base;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Currents
{
    public class CurrentAddContainerViewModel<InfoContainer, CurrentContainerCarPart> : CurrentBaseContainerViewModel<InfoContainer, CurrentContainerCarPart>
        where InfoContainer : InfoBaseContainer, new()
        where CurrentContainerCarPart : CurrentBaseContainerCarPart
    {
        #region Base

        public CurrentAddContainerViewModel()
            : base()
        {
            TitleContainerName = "Добавление контейнера";
            ButtonAddEditContainerName = "Добавить контейнер";
            Date = DateTime.Now;

            AddEditConteinerCommand = new RelayCommand(AddContainer, IsAnyCarParts);
        }

        #endregion

        #region Commands

        private void AddContainer(object parameter)
        {
            BC.AddInfoContainer<InfoContainer, CurrentContainerCarPart>(Name, Description, Date, CurrentContainerCarParts.ToList());

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

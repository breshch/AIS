using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class EditContainerCarPartViewModel<InfoContainer, CurrentContainerCarPart> : BaseContainerCarPartViewModel<InfoContainer, CurrentContainerCarPart>
        where InfoContainer : InfoBaseContainer
        where CurrentContainerCarPart : CurrentBaseContainerCarPart
    {
        #region Base

        public CurrentContainerCarPart CurrentNewContainerCarPart;

        public EditContainerCarPartViewModel(CurrentContainerCarPart carPart)
            : base()
        {
            AddEditCarPartCommand = new RelayCommand(EditCarPart);
            AddEditCarPartTitle = "Редактирование автозапчасти";
            AddEditCarPartName = "Изменить автозапчасть";

            SelectedCarPart = CarParts.First(c => c.Id == carPart.DirectoryCarPartId);
            CountCarParts = carPart.CountCarParts;

            CurrentNewContainerCarPart = carPart;
        }

        #endregion

        #region Commands

        private void EditCarPart(object parameter)
        {
            CurrentNewContainerCarPart.DirectoryCarPart = SelectedCarPart;
            CurrentNewContainerCarPart.DirectoryCarPartId = SelectedCarPart.Id;
            CurrentNewContainerCarPart.CountCarParts = CountCarParts;

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

using AIS_Enterprise_AV.ViewModels.Directories;
using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_AV.Views.Directories;
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
    public class EditContainerCarPartViewModel : BaseContainerCarPartViewModel
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
            SelectedCarPartText = SelectedCarPart.FullCarPartName;
            CountCarParts = carPart.CountCarParts.ToString();

            CurrentNewContainerCarPart = carPart;
        }

        #endregion

        #region Commands

        private void EditCarPart(object parameter)
        {
            if (SelectedCarPartText != null && SelectedCarPart == null)
            {
                var view = new AddDirectoryCarPartVew();
                var viewModel = new AddDirectoryCarPartViewModel();
                view.DataContext = viewModel;

                view.ShowDialog();

                if (viewModel.NewDirectoryCarPart == null)
                {
                    return;
                }

                var newCarPart = viewModel.NewDirectoryCarPart;

                CurrentNewContainerCarPart.DirectoryCarPart = newCarPart;
                CurrentNewContainerCarPart.DirectoryCarPartId = newCarPart.Id;
                CurrentNewContainerCarPart.CountCarParts = int.Parse(CountCarParts);
            }
            else
            {
                CurrentNewContainerCarPart.DirectoryCarPart = SelectedCarPart;
                CurrentNewContainerCarPart.DirectoryCarPartId = SelectedCarPart.Id;
                CurrentNewContainerCarPart.CountCarParts = int.Parse(CountCarParts);
            }

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

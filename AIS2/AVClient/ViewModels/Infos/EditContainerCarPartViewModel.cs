using System.Linq;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.ViewModels.Directories;
using AVClient.ViewModels.Infos.Base;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Infos
{
    public class EditContainerCarPartViewModel : BaseContainerCarPartViewModel
    {
        #region Base

        public CurrentContainerCarPart CurrentNewContainerCarPart;

        public EditContainerCarPartViewModel(CurrentContainerCarPart carPart)
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

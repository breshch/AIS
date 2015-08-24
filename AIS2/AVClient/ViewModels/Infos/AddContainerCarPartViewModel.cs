using System;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.ViewModels.Directories;
using AVClient.ViewModels.Infos.Base;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Infos
{
    public class AddContainerCarPartViewModel : BaseContainerCarPartViewModel
    {
        #region Base

        public event Action<CurrentContainerCarPart> AddingCarPart = delegate { };

        private CurrentContainerCarPart _currentContainerCarPart;

        public AddContainerCarPartViewModel()
        {
            AddEditCarPartCommand = new RelayCommand(AddCarPart, IsFullData);
            AddEditCarPartTitle = "Добавление автозапчасти";
            AddEditCarPartName = "Добавить автозапчасть";
        }

        private void ClearForm()
        {
            SelectedCarPart = null;
            SelectedCarPartText = null;
            CountCarParts = null;
        }

        #endregion

        #region Commands

        private void AddCarPart(object parameter)
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

                _currentContainerCarPart = new CurrentContainerCarPart
                {
                    DirectoryCarPart = newCarPart,
                    DirectoryCarPartId = newCarPart.Id,
                    CountCarParts = int.Parse(CountCarParts)
                };
            }
            else
            {
                _currentContainerCarPart = new CurrentContainerCarPart
                {
                    DirectoryCarPart = SelectedCarPart,
                    DirectoryCarPartId = SelectedCarPart.Id,
                    CountCarParts = int.Parse(CountCarParts)
                };     
            }

            AddingCarPart(_currentContainerCarPart);

            ClearForm();
        }

        #endregion
    }
}

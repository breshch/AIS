using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Directories.Base
{
    public abstract class DirectoryContainerBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryContainerBaseViewModel()
        {
            AddCarPartCommnad = new RelayCommand(Add);
            EditCarPartCommnad = new RelayCommand(Edit);
            RemoveCarPartCommnad = new RelayCommand(Remove);
        }

        #endregion

        #region Properties

        public string TitleContainerName { get; set; }
        public DateTime Date { get; set; }

        public string Name { get; set; }
        public string  Description { get; set; }
        public ObservableCollection<CurrentContainerCarPart> CurrentContainerCarParts { get; set; }
        public CurrentContainerCarPart SelectedCurrentContainerCarPart { get; set; }

        public string  ButtonAddEditContainerName { get; set; }
        
        #endregion

        #region Commands

        public RelayCommand AddCarPartCommnad { get; set; }
        public RelayCommand EditCarPartCommnad { get; set; }
        public RelayCommand RemoveCarPartCommnad { get; set; }
        
        public RelayCommand AddConteinerCommand { get; set; }

        public void Add(object parameter)
        {
            var viewModel = new AddContainerCarPartViewModel();
            viewModel.AddingCarPart += viewModel_AddingCarPart;

            var view = new AddEditContainerCarPartView();

            view.DataContext = viewModel;
            view.Show();
        }

        private void viewModel_AddingCarPart(CurrentContainerCarPart currentContainerCarPart)
        {
            CurrentContainerCarParts.Add(currentContainerCarPart);     
        }

        public void Edit(object parameter)
        {

        }
        public void Remove(object parameter)
        {
            BC.RemoveCurrentContainerCarPart(SelectedCurrentContainerCarPart);
        }

        #endregion
    }
}

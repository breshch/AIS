using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Infos;
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

namespace AIS_Enterprise_AV.ViewModels.Currents.Base
{
    public abstract class CurrentBaseContainerViewModel<InfoContainer,CurrentContainerCarPart> : ViewModelGlobal
        where InfoContainer : InfoBaseContainer, new()
        where CurrentContainerCarPart : CurrentBaseContainerCarPart, new()
    {
        #region Base

        public CurrentBaseContainerViewModel()
        {
            AddCarPartCommnad = new RelayCommand(Add);
            EditCarPartCommnad = new RelayCommand(Edit, IsSelectedCarPart);
            RemoveCarPartCommnad = new RelayCommand(Remove, IsSelectedCarPart);

            CurrentContainerCarParts = new ObservableCollection<CurrentContainerCarPart>();
        }

        #endregion

        #region Properties

        public string TitleContainerName { get; set; }
        public DateTime Date { get; set; }

        public string Name { get; set; }
        public string  Description { get; set; }
        public ObservableCollection<CurrentContainerCarPart> CurrentContainerCarParts { get; set; }
        public CurrentContainerCarPart SelectedCurrentContainerCarPart { get; set; }

        public string ButtonAddEditContainerName { get; set; }
        
        #endregion

        #region Commands

        public RelayCommand AddCarPartCommnad { get; set; }
        public RelayCommand EditCarPartCommnad { get; set; }
        public RelayCommand RemoveCarPartCommnad { get; set; }
        
        public RelayCommand AddEditConteinerCommand { get; set; }

        private void Add(object parameter)
        {
            var viewModel = new AddContainerCarPartViewModel<InfoContainer, CurrentContainerCarPart>();
            viewModel.AddingCarPart += viewModel_AddingCarPart;

            var view = new AddEditContainerCarPartView();

            view.DataContext = viewModel;
            view.Show();
        }

        private void viewModel_AddingCarPart(CurrentContainerCarPart currentContainerCarPart)
        {
            CurrentContainerCarParts.Add(currentContainerCarPart);     
        }

        private void Edit(object parameter)
        {
            int index = CurrentContainerCarParts.IndexOf(SelectedCurrentContainerCarPart);

            var viewModel = new EditContainerCarPartViewModel<InfoContainer, CurrentContainerCarPart>(SelectedCurrentContainerCarPart);
            HelperMethods.ShowView(viewModel, new AddEditContainerCarPartView());

            var carPart = viewModel.CurrentNewContainerCarPart;
            CurrentContainerCarParts.RemoveAt(index);
            CurrentContainerCarParts.Insert(index, carPart);
        }
        private void Remove(object parameter)
        {
            CurrentContainerCarParts.Remove(SelectedCurrentContainerCarPart);
        }

        private bool IsSelectedCarPart(object parameter)
        {
            return SelectedCurrentContainerCarPart != null;
        }

        protected bool IsAnyCarParts(object parameter)
        {
            return CurrentContainerCarParts.Any();
        }

        #endregion
    }
}

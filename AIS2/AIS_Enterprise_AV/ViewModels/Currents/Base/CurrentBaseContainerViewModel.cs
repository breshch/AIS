using System;
using System.Collections.ObjectModel;
using System.Linq;
using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Currents.Base
{
    public abstract class CurrentBaseContainerViewModel : ViewModelGlobal
    {
        #region Base

        protected bool _isIncoming;

        public CurrentBaseContainerViewModel(bool isIncoming)
        {
            _isIncoming = isIncoming;
            DiscriptionTitle = isIncoming ? "Номер контейнера (SS)" : "Название клиента";
            DatePhysicalTitle = isIncoming ? "Дата прихода" : "Дата расхода";

            AddCarPartCommnad = new RelayCommand(Add);
            EditCarPartCommnad = new RelayCommand(Edit, IsSelectedCarPart);
            RemoveCarPartCommnad = new RelayCommand(Remove, IsSelectedCarPart);

            CurrentContainerCarParts = new ObservableCollection<CurrentContainerCarPart>();

        }

        #endregion

        #region Properties
        public string DiscriptionTitle { get; set; }
        public string DatePhysicalTitle { get; set; }

        public string TitleContainerName { get; set; }
        public DateTime DatePhysical { get; set; }
        public DateTime? DateOrder { get; set; }

        public string Name { get; set; }
        public string  Description { get; set; }
        public ObservableCollection<CurrentContainerCarPart> CurrentContainerCarParts { get; set; }
        public CurrentContainerCarPart SelectedCurrentContainerCarPart { get; set; }
		public ObservableCollection<ContainerCountCarParts> TotalCarPartsCount { get; set; }		

        public string ButtonAddEditContainerName { get; set; }
        
        #endregion

        #region Commands

        public RelayCommand AddCarPartCommnad { get; set; }
        public RelayCommand EditCarPartCommnad { get; set; }
        public RelayCommand RemoveCarPartCommnad { get; set; }
        
        public RelayCommand AddEditConteinerCommand { get; set; }

        private void Add(object parameter)
        {
            var viewModel = new AddContainerCarPartViewModel();
            viewModel.AddingCarPart += viewModel_AddingCarPart;

            var view = new AddEditContainerCarPartView();

            view.DataContext = viewModel;
            view.Show();
        }

	    private void RecalculateTotalCarPart()
	    {
			TotalCarPartsCount = new ObservableCollection<ContainerCountCarParts>
			{
				new ContainerCountCarParts
				{
					Text = "Итого",
					Count = CurrentContainerCarParts.Sum(p => p.CountCarParts)
				}
			};
	    }



        private void viewModel_AddingCarPart(CurrentContainerCarPart currentContainerCarPart)
        {
            CurrentContainerCarParts.Add(currentContainerCarPart);

			RecalculateTotalCarPart();
        }

        private void Edit(object parameter)
        {
            int index = CurrentContainerCarParts.IndexOf(SelectedCurrentContainerCarPart);

            var viewModel = new EditContainerCarPartViewModel(SelectedCurrentContainerCarPart);
            HelperMethods.ShowView(viewModel, new AddEditContainerCarPartView());

            var carPart = viewModel.CurrentNewContainerCarPart;
            CurrentContainerCarParts.RemoveAt(index);
            CurrentContainerCarParts.Insert(index, carPart);

			RecalculateTotalCarPart();
        }

        private void Remove(object parameter)
        {
            CurrentContainerCarParts.Remove(SelectedCurrentContainerCarPart);

			RecalculateTotalCarPart();
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

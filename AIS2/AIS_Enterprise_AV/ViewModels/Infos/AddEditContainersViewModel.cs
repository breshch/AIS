﻿using AIS_Enterprise_AV.ViewModels.Currents;
using AIS_Enterprise_AV.ViewModels.Directories;
using AIS_Enterprise_AV.Views.Currents;
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

namespace AIS_Enterprise_AV.Infos.ViewModels
{
    public class AddEditContainersViewModel<InfoContainer, CurrentContainerCarPart> : ViewModelGlobal 
        where InfoContainer : InfoBaseContainer , new()
        where CurrentContainerCarPart : CurrentBaseContainerCarPart
    {
        #region Base

        public AddEditContainersViewModel(string title)
        {
            Years = new ObservableCollection<int>(BC.GetContainerYears<InfoContainer>());
           
            if (Years.Any())
            {
                SelectedYear = Years.Last(); 
            }

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, IsSelectedContainer);
            RemoveCommand = new RelayCommand(Remove, IsSelectedContainer);

            AddEditContainersTitle = title;
        }

        private void RefreshContainers()
        {
            InfoContainers = new ObservableCollection<InfoContainer>(BC.GetContainers<InfoContainer>(SelectedYear, _selectedMonth));
        }

        #endregion

        #region Properties

        public string AddEditContainersTitle { get; set; }

        public ObservableCollection<int> Years { get; set; }
        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set 
            {
                _selectedYear = value;
                RaisePropertyChanged();

                if (Monthes == null)
                {
                    Monthes = new ObservableCollection<int>(BC.GetContainerMonthes<InfoContainer>(_selectedYear));
                }
                else
                {
                    Monthes.Clear();
                    foreach (var month in BC.GetContainerMonthes<InfoContainer>(_selectedYear))
                    {
                        Monthes.Add(month);
                    }
                }
                SelectedMonth = Monthes.Last();
            }
        }

        public ObservableCollection<int> Monthes { get; set; }
        private int _selectedMonth;
        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged();

                RefreshContainers();
            }
        }

        public ObservableCollection<InfoContainer> InfoContainers { get; set; }
        public InfoContainer SelectedInfoContainer { get; set; }
        #endregion

        #region Commands
        
        public RelayCommand AddCommand{ get; set; }
        public RelayCommand EditCommand{ get; set; }
        public RelayCommand RemoveCommand{ get; set; }

        private void Add(object parameter)
        {
            HelperMethods.ShowView(new CurrentAddContainerViewModel<InfoContainer, CurrentContainerCarPart>(), new AddEditCurrentContainerCarPartsView());

            RefreshContainers();
        }
        private void Edit(object parameter)
        {
            HelperMethods.ShowView(new CurrentEditContainerViewModel<InfoContainer, CurrentContainerCarPart>(SelectedInfoContainer.Id), new AddEditCurrentContainerCarPartsView());

            BC.RefreshContext();

            RefreshContainers();
        }
        private void Remove(object parameter)
        {
            BC.RemoveInfoContainer<InfoContainer>(SelectedInfoContainer);

            RefreshContainers();
        }

        private bool IsSelectedContainer(object parameter)
        {
            return SelectedInfoContainer != null;
        }

        #endregion
    }
}

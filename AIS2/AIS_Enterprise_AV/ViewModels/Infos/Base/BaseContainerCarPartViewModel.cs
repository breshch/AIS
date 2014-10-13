﻿using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos.Base
{
    public class BaseContainerCarPartViewModel : ViewModelGlobal
    {
        #region Base

        public BaseContainerCarPartViewModel()
        {
            CarParts = new ObservableCollection<DirectoryCarPart>(BC.GetDirectoryCarParts());
        }

        #endregion


        #region Properties

        public string AddEditCarPartTitle { get; set; }

        public ObservableCollection<DirectoryCarPart> CarParts { get; set; }

        private DirectoryCarPart _selectedCarPart;
        public DirectoryCarPart SelectedCarPart
        {
            get
            {
                return _selectedCarPart;
            }
            set
            {
                _selectedCarPart = value;
                RaisePropertyChanged();

                SelectedDescription = _selectedCarPart.Description;
            }
        }

        public string SelectedDescription { get; set; }

        public int CountCarParts { get; set; }

        public string AddEditCarPartName { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddEditCarPartCommand { get; set; }

        #endregion
    }
}

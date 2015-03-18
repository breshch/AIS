using System;
using System.Collections.ObjectModel;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoCarPartMovementViewModel : ViewModelGlobal
    {
        #region Base

        public InfoCarPartMovementViewModel()
        {
            DirectoryCarParts = new ObservableCollection<DirectoryCarPart>(BC.GetDirectoryCarParts());
            SelectedDateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectedDateTo = DateTime.Now;
            InfoCarPartMovements = new ObservableCollection<InfoCarPartMovement>();
        }

        private void RefreshMovements()
        {
            InfoCarPartMovements.Clear();
            foreach (var movement in BC.GetMovementsByDates(SelectedDirectoryCarPart, SelectedDateFrom, SelectedDateTo))
            {
                InfoCarPartMovements.Add(movement);
            }
        }
        #endregion

        #region Properties

        public ObservableCollection<DirectoryCarPart> DirectoryCarParts { get; set; }
        
        private DirectoryCarPart _selectedDirectoryCarPart;
        public DirectoryCarPart SelectedDirectoryCarPart
        {
            get 
            {
                return _selectedDirectoryCarPart;
            }
            set
            {
                _selectedDirectoryCarPart = value;
                RaisePropertyChanged();
                SelectedDescription = _selectedDirectoryCarPart.Description + " " + _selectedDirectoryCarPart.OriginalNumber;

                RefreshMovements();
            }
        }

        public string SelectedDescription { get; set; }

        private DateTime _selectedDateFrom;
        public DateTime SelectedDateFrom
        {
            get
            {
                return _selectedDateFrom;
            }
            set
            {
                _selectedDateFrom = value;
                RaisePropertyChanged();

                if (SelectedDirectoryCarPart != null)
                {
                    RefreshMovements();
                }
            }
        }

        private DateTime _selectedDateTo;
        public DateTime SelectedDateTo
        {
            get
            {
                return _selectedDateTo;
            }
            set
            {
                _selectedDateTo = value;
                RaisePropertyChanged();

                if (SelectedDirectoryCarPart != null)
                {
                    RefreshMovements();
                }
            }
        }

        public ObservableCollection<InfoCarPartMovement> InfoCarPartMovements { get; set; }
        public InfoCarPartMovement SelectedInfoCarPartMovement { get; set; }


        
        #endregion
    }
}


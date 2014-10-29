using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_AV.Infos.ViewModels;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoRemainsViewModel : ViewModelGlobal
    {
        #region Base

        public InfoRemainsViewModel()
        {
            SelectedDate = DateTime.Now;
            DirectoryCarParts = new ObservableCollection<DirectoryCarPart>(BC.GetDirectoryCarParts());
            InfoCarPartRemains = new ObservableCollection<InfoCarPartRemain>();

            IncomingCommand = new RelayCommand(Incoming);
            OutcomingCommand = new RelayCommand(Outcoming);
            MovementCommand = new RelayCommand(Movement);

        }
        
        #endregion

        #region Properties

        public ObservableCollection<DirectoryCarPart> DirectoryCarParts { get; set; }
        public string Description { get; set; }

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

                Description = _selectedDirectoryCarPart.Description;

                var infoCarPartRemain = new InfoCarPartRemain();

                var infoLastMonthDayRemain = BC.GetInfoLastMonthDayRemain(SelectedDate, _selectedDirectoryCarPart.Id);
                if (infoLastMonthDayRemain != null)
                {
                    infoCarPartRemain.LastMonthDayRemain = infoLastMonthDayRemain.Count;
                }

                infoCarPartRemain.Incomings = BC.GetInfoCarPartIncomingCountTillDate(SelectedDate, _selectedDirectoryCarPart.Id, true);

                infoCarPartRemain.Outcomings = BC.GetInfoCarPartIncomingCountTillDate(SelectedDate, _selectedDirectoryCarPart.Id, false);

                infoCarPartRemain.RemainToDate = infoCarPartRemain.LastMonthDayRemain + infoCarPartRemain.Incomings - infoCarPartRemain.Outcomings;
                
                InfoCarPartRemains.Clear();
                InfoCarPartRemains.Add(infoCarPartRemain);
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<InfoCarPartRemain> InfoCarPartRemains { get; set; }
        public InfoCarPartRemain SelectedInfoCarPartRemain { get; set; }


        #endregion

        #region Commands

        public RelayCommand IncomingCommand { get; set; }

        private void Incoming(object parameter)
        {
            HelperMethods.ShowView(new AddEditContainersViewModel(true), new AddEditContainersView());
            HelperMethods.CloseWindow(parameter);

        }

        public RelayCommand OutcomingCommand { get; set; }

        private void Outcoming(object parameter)
        {
            HelperMethods.ShowView(new AddEditContainersViewModel(false), new AddEditContainersView());
            HelperMethods.CloseWindow(parameter);

        }
        public RelayCommand MovementCommand { get; set; }

        private void Movement(object parameter)
        {
            HelperMethods.ShowView(new InfoCarPartMovementViewModel(), new InfoCarPartMovementView());
            HelperMethods.CloseWindow(parameter);

        }
        #endregion
    }
}

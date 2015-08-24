using System;
using System.Collections.ObjectModel;
using System.Linq;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Helpers.Temps;
using AVClient.Views.Infos;

namespace AVClient.ViewModels.Infos
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

        public DirectoryCarPart _selectedDirectoryCarPart;
        public DirectoryCarPart SelectedDirectoryCarPart
        {
            get { return _selectedDirectoryCarPart; }
            set
            {
                _selectedDirectoryCarPart = value;

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

        private string _selectedDirectoryCarPartText;
        public string SelectedDirectoryCarPartText
        {
            get
            {
                return _selectedDirectoryCarPartText;
            }
            set
            {
                _selectedDirectoryCarPartText = value;
                RaisePropertyChanged();

                var carPart = DirectoryCarParts.FirstOrDefault(c => c.FullCarPartName.ToLower() == _selectedDirectoryCarPartText.ToLower());

                if (carPart != null)
                {
                    SelectedDirectoryCarPart = carPart;
                }
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

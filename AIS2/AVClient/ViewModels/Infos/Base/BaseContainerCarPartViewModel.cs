using System.Collections.ObjectModel;
using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Infos.Base
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

        public string SelectedCarPartText { get; set; }

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

                if (_selectedCarPart == null)
                {
                    SelectedDescription = null;    
                }
                else
                {
                    SelectedDescription = _selectedCarPart.Description + " " + _selectedCarPart.OriginalNumber;
                }
            }
        }

        public string SelectedDescription { get; set; }

        public string CountCarParts { get; set; }

        public string AddEditCarPartName { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddEditCarPartCommand { get; set; }

        protected bool IsFullData(object parameter)
        {
            int count;
            return SelectedCarPartText != null && int.TryParse(CountCarParts, out count) && count > 0;
        }

        #endregion
    }
}

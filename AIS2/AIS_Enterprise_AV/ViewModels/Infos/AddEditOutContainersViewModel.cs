using AIS_Enterprise_AV.ViewModels.Directories;
using AIS_Enterprise_AV.Views.Currents;
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
    public class AddEditOutContainersViewModel : ViewModelGlobal
    {
        #region Base

        public AddEditOutContainersViewModel()
        {
            Years = new ObservableCollection<int>(BC.GetOutContainerYears());
           
            if (Years.Any())
            {
                SelectedYear = Years.Last(); 
            }

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, IsSelectedContainer);
            RemoveCommand = new RelayCommand(Remove, IsSelectedContainer);

            AddEditContainersTitle = "Расход";
        }

        private void RefreshContainers()
        {
            InfoContainers = new ObservableCollection<InfoOutContainer>(BC.GetOutContainers(SelectedYear, _selectedMonth));
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
                    Monthes = new ObservableCollection<int>(BC.GetOutContainerMonthes(_selectedYear));
                }
                else
                {
                    Monthes.Clear();
                    foreach (var month in BC.GetOutContainerMonthes(_selectedYear))
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

        public ObservableCollection<InfoOutContainer> InfoContainers { get; set; }
        public InfoOutContainer SelectedInfoContainer { get; set; }
        #endregion

        #region Commands
        
        public RelayCommand AddCommand{ get; set; }
        public RelayCommand EditCommand{ get; set; }
        public RelayCommand RemoveCommand{ get; set; }

        private void Add(object parameter)
        {
            HelperMethods.ShowView(new DirectoryAddContainerViewModel(), new AddEditCurrentContainerCarPartsView());

            RefreshContainers();
        }
        private void Edit(object parameter)
        {
            HelperMethods.ShowView(new DirectoryEditContainerViewModel(SelectedInfoContainer.Id), new AddEditCurrentContainerCarPartsView());

            BC.RefreshContext();

            RefreshContainers();
        }
        private void Remove(object parameter)
        {
            BC.RemoveInfoOutContainer(SelectedInfoContainer);

            RefreshContainers();
        }

        private bool IsSelectedContainer(object parameter)
        {
            return SelectedInfoContainer != null;
        }

        #endregion
    }
}

using AIS_Enterprise_AV.ViewModels.Directories;
using AIS_Enterprise_AV.Views.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Infos.ViewModels
{
    public class AddEditContainersViewModel : ViewModelGlobal
    {
        #region Base

        public AddEditContainersViewModel()
        {
            Years = new ObservableCollection<int>(BC.GetContainerYears());
           
            if (Years.Any())
            {
                SelectedYear = Years.Last(); 
            }


            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            RemoveCommand = new RelayCommand(Remove);
        }

        #endregion

        #region Properties

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
                    Monthes = new ObservableCollection<int>(BC.GetContainerMonthes(_selectedYear));
                }
                else
                {
                    Monthes.Clear();
                    foreach (var month in BC.GetContainerMonthes(_selectedYear))
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

                DirecoryContainers = new ObservableCollection<DirectoryContainer>(BC.GetContainers(SelectedYear, _selectedMonth));
            }
        }

        public ObservableCollection<DirectoryContainer> DirecoryContainers { get; set; }
        public DirectoryContainer SelectedDirectoryContainer { get; set; }
        #endregion

        #region Commands
        
        public RelayCommand AddCommand{ get; set; }
        public RelayCommand EditCommand{ get; set; }
        public RelayCommand RemoveCommand{ get; set; }

        private void Add(object parameter)
        {
            HelperMethods.ShowView(new DirectoryAddContainerViewModel(), new AddEditCurrentContainerCarPartsView());
        }
        private void Edit(object parameter)
        {

        }
        private void Remove(object parameter)
        {
            BC.RemoveDirectoryContainer(SelectedDirectoryContainer);
        }

        #endregion
    }
}

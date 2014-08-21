using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Infos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class InfoSafeViewModel : ViewModelGlobal
    {
        #region Base

        public InfoSafeViewModel()
        {
            SelectedInfoSafeDate = DateTime.Now;

            RefreshInfoSafes();

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, IsSelectedInfoCost);
            RemoveCommand = new RelayCommand(Remove, IsSelectedInfoCost);
            ShowPaymentsCommand = new RelayCommand(ShowPayments, IsSelectedInfoCost);
        }

        private void RefreshInfoSafes()
        {
            InfoSafes = new ObservableCollection<InfoSafe>(BC.GetInfoSafes(SelectedInfoSafeDate));
            TotalSummLoans = BC.GetLoans();

        }

        #endregion

        #region Properties

        public ObservableCollection<InfoSafe> InfoSafes { get; set; }
        private DateTime _selectedInfoSafeDate;
        public DateTime SelectedInfoSafeDate 
        {
            get
            {
                return _selectedInfoSafeDate;
            }
            set 
            {
                _selectedInfoSafeDate = value;
                RaisePropertyChanged();

                TotalSummSelectedDate = BC.GetInfoCash(SelectedInfoSafeDate);
                TotalSummEndingMonth = BC.GetInfoCash(SelectedInfoSafeDate.Year, SelectedInfoSafeDate.Month);


            }
        
        }
        public InfoSafe SelectedInfoSafe { get; set; }
        public double TotalSummSelectedDate { get; set; }
        public double TotalSummEndingMonth { get; set; }
        public double TotalSummLoans { get; set; }
        #endregion

        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ShowPaymentsCommand { get; set; }
        private void Add(object parameter)
        {
            HelperMethods.ShowView(new AddInfoSafeViewModel(), new AddEditInfoSafeView());

            RefreshInfoSafes();

        }

        private void Edit(object parameter)
        {
            HelperMethods.ShowView(new EditInfoSafeViewModel(SelectedInfoSafe), new AddEditInfoSafeView());

            BC.RefreshContext();

            RefreshInfoSafes();
        }

        private void Remove(object parameter)
        {
            BC.RemoveInfoSafe(SelectedInfoSafe);

            RefreshInfoSafes();
        }

        private bool IsSelectedInfoCost(object parameter)
        {
            return SelectedInfoSafe != null;
        }


        private void ShowPayments(object parameter)
        {
            HelperMethods.ShowView(new InfoPaymentsViewModel(SelectedInfoSafe.Id), new InfoPaymentsView());
        }
        #endregion
    }
}

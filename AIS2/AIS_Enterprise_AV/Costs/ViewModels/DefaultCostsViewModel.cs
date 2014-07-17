using AIS_Enterprise_AV.Costs.Views;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.Costs.ViewModels
{
    public class DefaultCostsViewModel : ViewModelGlobal
    {
        #region Base
        public DefaultCostsViewModel()
        {
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, IsSelectedDefaultCost);
            RemoveCommand = new RelayCommand(Remove, IsSelectedDefaultCost);

            DefaultCosts = new ObservableCollection<DefaultCost>();

            RefreshDefaultCosts();
        }

        private void RefreshDefaultCosts()
        {
            DefaultCosts.Clear();
            foreach (var defaultCost in BC.GetDefaultCosts())
            {
                DefaultCosts.Add(defaultCost);
            }
        }
        
        #endregion

        #region Properties

        public ObservableCollection<DefaultCost> DefaultCosts { get; set; }
        public DefaultCost SelectedDefaultCost { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }


        private void Add(object parameter)
        {
            HelperMethods.ShowView(new AddDefaultCostViewModel(), new AddEditDefaultCostView());
            RefreshDefaultCosts();
        }

        private void Edit(object parameter)
        {
            HelperMethods.ShowView(new EditDefaultCostViewModel(SelectedDefaultCost), new AddEditDefaultCostView());

            BC.RefreshContext();
            RefreshDefaultCosts();
        }

        private void Remove(object parameter)
        {
            BC.RemoveDefaultCost(SelectedDefaultCost);
            RefreshDefaultCosts();
        }

        private bool IsSelectedDefaultCost(object parameter)
        {
            return SelectedDefaultCost != null;
        }

        #endregion

    }
}

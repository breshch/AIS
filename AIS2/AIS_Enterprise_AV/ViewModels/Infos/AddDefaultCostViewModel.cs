using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddDefaultCostViewModel : BaseDefaultCostViewModel 
    {
        #region Base

        public AddDefaultCostViewModel() : base()
        {
            SelectedDirectoryRC = DirectoryRCs.First(r => r.Name == "ВСЕ");

            AddEditDefaultCostCommand = new RelayCommand(Add, IsValidate);

            AddEditButtonName = "Добавить";
        }

        #endregion

        #region Commands

        private void Add(object parameter)
        {
            BC.AddDefaultCost(SelectedDirectoryCostItem, SelectedDirectoryRC ,SelectedDirectoryNote, SummOfPayment, DayOfPayment);

            var window = parameter as Window;
            window.Close();
        }

        #endregion
    }
}

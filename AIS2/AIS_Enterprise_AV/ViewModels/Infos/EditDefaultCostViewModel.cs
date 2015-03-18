using System.Linq;
using System.Windows;
using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Data.Helpers;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class EditDefaultCostViewModel : BaseDefaultCostViewModel
    {
        #region Base
        private int _defaultCostId; 
        public EditDefaultCostViewModel(DefaultCost defaultCost)
        {
            _defaultCostId = defaultCost.Id;
            
            SelectedDirectoryCostItem = DirectoryCostItems.First(c => c.Name == defaultCost.DirectoryCostItem.Name);
            SelectedDirectoryRC = DirectoryRCs.First(r => r.Name == defaultCost.DirectoryRC.Name);
            SelectedDirectoryNote = DirectoryNotes.First(n => n.Description == defaultCost.DirectoryNote.Description);
            SummOfPayment = defaultCost.SummOfPayment;
            DayOfPayment = defaultCost.DayOfPayment;

            AddEditDefaultCostCommand = new RelayCommand(Edit, IsValidate);

            AddEditButtonName = "Изменить";
        }


        #endregion

        #region Command

        private void Edit(object parameter)
        {
            BC.EditDefaultCost(_defaultCostId, SelectedDirectoryCostItem, SelectedDirectoryRC, SelectedDirectoryNote, SummOfPayment, DayOfPayment);

            var window = parameter as Window;
            window.Close();
        }
        
        #endregion
    }
}

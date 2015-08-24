using System.Linq;
using System.Windows;
using AVClient.Helpers;
using AVClient.ViewModels.Infos.Base;

namespace AVClient.ViewModels.Infos
{
    public class AddDefaultCostViewModel : BaseDefaultCostViewModel 
    {
        #region Base

        public AddDefaultCostViewModel()
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

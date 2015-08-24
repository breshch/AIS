using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Helpers.Attributes;

namespace AVClient.ViewModels.Infos.Base
{
    public abstract class BaseDefaultCostViewModel : ViewModelGlobal
    {
        #region Base

        public BaseDefaultCostViewModel()
        {
            DirectoryCostItems = new ObservableCollection<DirectoryCostItem>(BC.GetDirectoryCostItems());
            DirectoryRCs = new ObservableCollection<DirectoryRC>(BC.GetDirectoryRCs());
            DirectoryNotes = new ObservableCollection<DirectoryNote>(BC.GetDirectoryNotes());
        }

        #endregion

        #region Properties

        public ObservableCollection<DirectoryCostItem> DirectoryCostItems { get; set; }

        [RequireSelected]
        [Display(Name = "Статья затрат")]
        public DirectoryCostItem SelectedDirectoryCostItem { get; set; }
       
        public ObservableCollection<DirectoryRC> DirectoryRCs { get; set; }

        [RequireSelected]
        [Display(Name = "ЦО")]
        public DirectoryRC SelectedDirectoryRC { get; set; }

        public ObservableCollection<DirectoryNote> DirectoryNotes { get; set; }

        [RequireSelected]
        [Display(Name = "Примечание")]
        public DirectoryNote SelectedDirectoryNote { get; set; }

        [Required]
        [Display(Name = "Сумма")]
        [DoubleValue(MinValue = 0)]
        public double SummOfPayment { get; set; }

        [Required]
        [Display(Name = "День списания")]
        [DoubleValue(MinValue = 1, MaxValue = 31)]
        public int DayOfPayment { get; set; }

        public string AddEditButtonName { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddEditDefaultCostCommand { get; set; }

        protected bool IsValidate(object parameter)
        {
            return IsValidateAllProperties();
        }

        #endregion
    }
}

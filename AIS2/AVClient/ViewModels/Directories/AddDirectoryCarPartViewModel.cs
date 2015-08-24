using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Directories
{
    public class AddDirectoryCarPartViewModel : ViewModelGlobal
    {
        #region Base

        public AddDirectoryCarPartViewModel()
        {
                AddCommand = new RelayCommand(Add);
        }

        #endregion
        
        
        #region Properties

        public string Article { get; set; }
        public string Mark { get; set; }
        public DirectoryCarPart NewDirectoryCarPart { get; set; }

        #endregion
        
        
        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            NewDirectoryCarPart = new DirectoryCarPart
            {
                Article = Article,
                Mark = Mark
            };
           
            HelperMethods.CloseWindow(parameter);
            
        }
        #endregion
    }
}

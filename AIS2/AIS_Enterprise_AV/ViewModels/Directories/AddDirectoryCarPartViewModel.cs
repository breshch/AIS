using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Directories
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
	        bool isImport = Mark == null;
	        NewDirectoryCarPart = BC.AddDirectoryCarPart(Article, Mark, null, null, null, null, null, null, null, isImport);
           
            HelperMethods.CloseWindow(parameter);
            
        }
        #endregion
    }
}

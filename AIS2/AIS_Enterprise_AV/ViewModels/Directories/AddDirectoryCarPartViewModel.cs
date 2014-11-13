using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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
            NewDirectoryCarPart = new DirectoryCarPart
            {
                Article = Article,
                Mark = Mark,
                Note = new CarPartNote(),
                FactoryAndCross = new CarPartFactoryAndCross()
            };
           
            HelperMethods.CloseWindow(parameter);
            
        }
        #endregion
    }
}

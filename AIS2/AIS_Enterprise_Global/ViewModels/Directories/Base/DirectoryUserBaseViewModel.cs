using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels.Directories.Base
{
    public abstract class DirectoryUserBaseViewModel : ViewModelGlobal
    {
        #region Base
        public DirectoryUserBaseViewModel()
        {
            DirectoryUserStatuses = new ObservableCollection<DirectoryUserStatus>(BC.GetDirectoryUserStatuses());
        } 
        #endregion


        #region Properties

        public string DirectoryUserName { get; set; }
        public ObservableCollection<DirectoryUserStatus> DirectoryUserStatuses {get;set;}
        public DirectoryUserStatus SelectedDirectoryUserStatus {get;set;}

        #endregion
    }
}

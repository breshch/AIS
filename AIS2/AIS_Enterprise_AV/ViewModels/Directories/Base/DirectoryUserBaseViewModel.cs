using System.Collections.ObjectModel;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

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

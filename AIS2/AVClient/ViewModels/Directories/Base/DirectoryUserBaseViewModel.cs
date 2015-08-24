using System.Collections.ObjectModel;
using AVClient.AVServiceReference;
using AVClient.Helpers;

namespace AVClient.ViewModels.Directories.Base
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

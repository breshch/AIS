using System.Linq;
using System.Windows;
using AVClient.Helpers;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryAddPostViewModel : DirectoryPostBaseViewModel
    {
        # region Base
        public DirectoryAddPostViewModel()
        {
            AddEditPostCommand = new RelayCommand(Add);

            AddEditPostTitle = "Добавление должности";
            AddEditPostName = "Добавить должность";
        }

        #endregion 

        #region Commands

        private void Add(object parameter)
        {
            BC.AddDirectoryPost(DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, DirectoryPostSalaries.ToArray());

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

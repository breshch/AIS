using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryEditPostViewModel : DirectoryPostBaseViewModel
    {
        # region Base

        private int _postId;
        public DirectoryEditPostViewModel(DirectoryPost directoryPost)
        {
            DirectoryPostName = directoryPost.Name;

			//TODO Refactor
            //SelectedDirectoryTypeOfPost = DirectoryTypeOfPosts.First(p => p.Name == directoryPost.TypeOfPost);
            SelectedDirectoryCompany = DirectoryCompanies.First(c => c.Name == directoryPost.DirectoryCompany.Name);
            _postId = directoryPost.Id;

            DirectoryPostSalaries = new ObservableCollection<DirectoryPostSalary>(BC.GetDirectoryPostSalaries(directoryPost.Id));

            AddEditPostCommand = new RelayCommand(Edit);

            AddEditPostTitle = "Редактирование должности";
            AddEditPostName = "Изменить должность";
        }

        #endregion 

        #region Commands

        private void Edit(object parameter)
        {
            BC.EditDirectoryPost(_postId, DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, DirectoryPostSalaries.ToArray());

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

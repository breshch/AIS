using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryEditPostViewModel : DirectoryPostBaseViewModel
    {
        # region Base

        private int _postId;
        public DirectoryEditPostViewModel(DirectoryPost directoryPost) : base()
        {
            DirectoryPostName = directoryPost.Name;
            SelectedDirectoryTypeOfPost = DirectoryTypeOfPosts.First(p => p.Name == directoryPost.DirectoryTypeOfPost.Name);
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
            BC.EditDirectoryPost(_postId, DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, DirectoryPostSalaries.ToList());

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

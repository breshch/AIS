using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            SelectedDirectoryPostDate = directoryPost.Date;
            DirectoryPostUserWorkerSalary = directoryPost.UserWorkerSalary.ToString();
            DirectoryPostAdminWorkerSalary = directoryPost.AdminWorkerSalary.ToString();
            DirectoryPostUserWorkerHalfSalary = directoryPost.UserWorkerHalfSalary.ToString();
            _postId = directoryPost.Id;

            EditCommand = new RelayCommand(Edit);
        }

        #endregion 

        #region Commands

        public RelayCommand EditCommand { get; set; }

        private void Edit(object parameter)
        {
            BC.EditDirectoryPost(_postId, DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, SelectedDirectoryPostDate, DirectoryPostUserWorkerSalary, DirectoryPostAdminWorkerSalary,
                DirectoryPostUserWorkerHalfSalary);

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryAddPostViewModel : DirectoryPostBaseViewModel
    {
        # region Base
        public DirectoryAddPostViewModel() : base()
        {
            SelectedDirectoryPostDate = DateTime.Now;

            AddCommand = new RelayCommand(Add);
        }

        #endregion 

        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            BC.AddDirectoryPost(DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, SelectedDirectoryPostDate, DirectoryPostUserWorkerSalary,DirectoryPostAdminWorkerSalary,
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

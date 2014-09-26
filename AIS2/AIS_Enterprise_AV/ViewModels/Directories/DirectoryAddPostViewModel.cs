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
            AddEditPostCommand = new RelayCommand(Add);

            AddEditPostTitle = "Добавление должности";
            AddEditPostName = "Добавить должность";
        }

        #endregion 

        #region Commands

        private void Add(object parameter)
        {
            BC.AddDirectoryPost(DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, DirectoryPostSalaries.ToList());

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        #endregion
    }
}

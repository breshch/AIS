using AIS_Enterprise_AV.ViewModels.Directories.Base;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_AV.ViewModels.Directories
{
    public class DirectoryAddPostSalaryViewModel : DirectoryPostSalaryBaseViewModel
    {
        #region Base
        public DirectoryAddPostSalaryViewModel() : base()
        {
            AddEditPostSalaryTitle = "Добавление окладов";
            AddEditPostSalaryName = "Добавить";

            SelectedDirectoryPostDate = DateTime.Now;

        } 
        #endregion
    }
}

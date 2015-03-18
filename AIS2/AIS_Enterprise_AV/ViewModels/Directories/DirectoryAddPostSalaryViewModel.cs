using System;
using AIS_Enterprise_AV.ViewModels.Directories.Base;

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

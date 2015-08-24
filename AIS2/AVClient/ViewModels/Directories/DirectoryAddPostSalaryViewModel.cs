using System;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryAddPostSalaryViewModel : DirectoryPostSalaryBaseViewModel
    {
        #region Base
        public DirectoryAddPostSalaryViewModel()
        {
            AddEditPostSalaryTitle = "Добавление окладов";
            AddEditPostSalaryName = "Добавить";

            SelectedDirectoryPostDate = DateTime.Now;

        } 
        #endregion
    }
}

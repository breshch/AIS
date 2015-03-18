using AIS_Enterprise_AV.ViewModels.Directories.Base;
using AIS_Enterprise_Data.Directories;

namespace AIS_Enterprise_AV.ViewModels.Directories
{
    public class DirectoryEditPostSalaryViewModel : DirectoryPostSalaryBaseViewModel
    {
        #region Base
        public DirectoryEditPostSalaryViewModel(DirectoryPostSalary postSalary) : base()
        {
            AddEditPostSalaryTitle = "Редактирование окладов";
            AddEditPostSalaryName = "Изменить";

            SelectedDirectoryPostDate = postSalary.Date;
            DirectoryPostUserWorkerSalary = postSalary.UserWorkerSalary.ToString();
            DirectoryPostAdminWorkerSalary = postSalary.AdminWorkerSalary.ToString();
            DirectoryPostUserWorkerHalfSalary = postSalary.UserWorkerHalfSalary.ToString();
        } 
        #endregion
    }
}

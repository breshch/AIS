using AVClient.AVServiceReference;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryEditPostSalaryViewModel : DirectoryPostSalaryBaseViewModel
    {
        #region Base
        public DirectoryEditPostSalaryViewModel(DirectoryPostSalary postSalary)
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

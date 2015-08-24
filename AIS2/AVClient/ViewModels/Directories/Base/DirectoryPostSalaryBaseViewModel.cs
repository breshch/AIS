using System;
using System.ComponentModel.DataAnnotations;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Helpers.Attributes;

namespace AVClient.ViewModels.Directories.Base
{
    public class DirectoryPostSalaryBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostSalaryBaseViewModel()
        {
            AddEditCommand = new RelayCommand(AddEdit);
        }


        #endregion

        #region Properties

        public string  AddEditPostSalaryTitle { get; set; }

        public DateTime SelectedDirectoryPostDate { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Оклад")]
        public string DirectoryPostUserWorkerSalary { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Админ Оклад")]
        public string DirectoryPostAdminWorkerSalary { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Совместительство")]
        public string DirectoryPostUserWorkerHalfSalary { get; set; }

        public string  AddEditPostSalaryName { get; set; }

        public DirectoryPostSalary DirectoryPostSalary { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddEditCommand { get; set; }

        private void AddEdit(object parameter)
        {
            DirectoryPostSalary = new DirectoryPostSalary
            {
                Date = SelectedDirectoryPostDate,
                UserWorkerSalary = double.Parse(DirectoryPostUserWorkerSalary),
                AdminWorkerSalary = double.Parse(DirectoryPostAdminWorkerSalary),
                UserWorkerHalfSalary = double.Parse(DirectoryPostUserWorkerHalfSalary)
            };

            HelperMethods.CloseWindow(parameter);
        }

        #endregion
    }
}

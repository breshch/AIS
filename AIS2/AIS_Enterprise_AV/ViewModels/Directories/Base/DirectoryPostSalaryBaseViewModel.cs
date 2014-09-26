using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Directories.Base
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

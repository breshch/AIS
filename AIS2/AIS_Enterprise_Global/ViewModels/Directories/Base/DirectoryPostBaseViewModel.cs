using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels.Directories.Base
{
    public class DirectoryPostBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostBaseViewModel()
            : base()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(BC.GetDirectoryTypeOfPosts());
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());
        }

        #endregion


        #region Properties

        [Required]
        [Display(Name = "Должность")]
        public string DirectoryPostName { get; set; }

        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }

        [RequireSelected]
        [Display(Name = "Вид должности")]
        public DirectoryTypeOfPost SelectedDirectoryTypeOfPost { get; set; }

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        [RequireSelected]
        [Display(Name = "Компания")]
        public DirectoryCompany SelectedDirectoryCompany { get; set; }

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

        #endregion
    }
}

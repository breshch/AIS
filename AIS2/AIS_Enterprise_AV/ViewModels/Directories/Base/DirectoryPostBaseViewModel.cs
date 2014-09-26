using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_AV.ViewModels.Directories;
using AIS_Enterprise_AV.Views.Directories;

namespace AIS_Enterprise_Global.ViewModels.Directories.Base
{
    public abstract class DirectoryPostBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostBaseViewModel()
            : base()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(BC.GetDirectoryTypeOfPosts());
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());

            AddPostSalaryCommand = new RelayCommand(AddPostSalary);
            EditPostSalaryCommand = new RelayCommand(EditPostSalary);
            RemovePostSalaryCommand = new RelayCommand(RemovePostSalary);

            DirectoryPostSalaries = new ObservableCollection<DirectoryPostSalary>();
        }


        #endregion


        #region Properties

        public string AddEditPostTitle { get; set; }

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

        public ObservableCollection<DirectoryPostSalary> DirectoryPostSalaries { get; set; }
        public DirectoryPostSalary SelectedDirectoryPostSalary { get; set; }

        public string AddEditPostName { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddPostSalaryCommand { get; set; }
        public RelayCommand EditPostSalaryCommand { get; set; }
        public RelayCommand RemovePostSalaryCommand { get; set; }
        public RelayCommand AddEditPostCommand { get; set; }

        private void AddPostSalary(object parameter)
        {
            var viewModel = new DirectoryAddPostSalaryViewModel();
            HelperMethods.ShowView(viewModel, new AddEditPostSalaryView());

            if (viewModel.DirectoryPostSalary != null)
            {
                var postSalary = viewModel.DirectoryPostSalary;
                DirectoryPostSalaries.Add(postSalary);
            }
        }

        private void EditPostSalary(object parameter)
        {
            int index = DirectoryPostSalaries.IndexOf(SelectedDirectoryPostSalary);

            var viewModel = new DirectoryEditPostSalaryViewModel(SelectedDirectoryPostSalary);
            HelperMethods.ShowView(viewModel, new AddEditPostSalaryView());

            if (viewModel.DirectoryPostSalary != null)
            {
                var postSalary = viewModel.DirectoryPostSalary;
                DirectoryPostSalaries.RemoveAt(index);
                DirectoryPostSalaries.Insert(index, postSalary);
            }
        }

        private void RemovePostSalary(object parameter)
        {
            DirectoryPostSalaries.Remove(SelectedDirectoryPostSalary);
        }

        #endregion
    }
}

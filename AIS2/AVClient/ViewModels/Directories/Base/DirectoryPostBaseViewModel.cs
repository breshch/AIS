using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Helpers.Attributes;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Directories.Base
{
    public abstract class DirectoryPostBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostBaseViewModel()
        {
            DirectoryTypeOfPosts = new ObservableCollection<TypeOfPost>(BC.GetDirectoryTypeOfPosts());
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

        public ObservableCollection<TypeOfPost> DirectoryTypeOfPosts { get; set; }

        [RequireSelected]
        [Display(Name = "Вид должности")]
        public TypeOfPost SelectedDirectoryTypeOfPost { get; set; }

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

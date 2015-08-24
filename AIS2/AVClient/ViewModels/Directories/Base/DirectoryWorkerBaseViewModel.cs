using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Views.Currents;
using Microsoft.Win32;
using Shared.Enums;
using BitmapImage = System.Windows.Media.Imaging.BitmapImage;
using Gender = AVClient.AVServiceReference.Gender;

namespace AVClient.ViewModels.Directories.Base
{
    public abstract class DirectoryWorkerBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryWorkerBaseViewModel()
        {
            IsAdminSalary = HelperMethods.IsPrivilege(UserPrivileges.Salary_AdminSalary);
            IsDeadSpiritVisibility = HelperMethods.IsPrivilege(UserPrivileges.WorkersVisibility_DeadSpirit);

            AddCompanyAndPostCommand = new RelayCommand(AddCompanyAndPost);
            EditCompanyAndPostCommand = new RelayCommand(EditCompanyAndPost, IsSelectedCompanyAndPost);
            RemoveCompanyAndPostCommand = new RelayCommand(RemoveCompanyAndPost, IsSelectedCompanyAndPost);
            AddPhotoCommand = new RelayCommand(AddPhoto);
            RemovePhotoCommand = new RelayCommand(RemovePhoto);

            SelectedIndexCurrentCompanyAndPost = -1;
        }

        protected void ClearInputData()
        {
            DirectoryWorkerLastName = null;
            DirectoryWorkerFirstName = null;
            DirectoryWorkerMidName = null;
            DirectoryWorkerGender = Gender.Male;
            SelectedDirectoryWorkerBirthDay = DateTime.Now;
            DirectoryWorkerAddress = null;
            DirectoryWorkerCellPhone = null;
            DirectoryWorkerHomePhone = null;
            SelectedDirectoryWorkerStartDate = DateTime.Now;
            CurrentCompaniesAndPosts.Clear();
            IsDeadSpirit = false;
        }

        #endregion


        #region Properties

        [Required]
        [Display(Name = "Фамилия")]
        public string DirectoryWorkerLastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string DirectoryWorkerFirstName { get; set; }

        public string DirectoryWorkerMidName { get; set; }

        public Gender DirectoryWorkerGender { get; set; }

        public DateTime SelectedDirectoryWorkerBirthDay { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string DirectoryWorkerAddress { get; set; }

        [Required]
        [Display(Name = "Мобильный телефон")]
        public string DirectoryWorkerCellPhone { get; set; }

        public string DirectoryWorkerHomePhone { get; set; }

        public DateTime SelectedDirectoryWorkerStartDate { get; set; }

        public BitmapImage Photo { get; set; }

        public string AddPhotoName { get; set; }

        public ObservableCollection<CurrentCompanyAndPost> CurrentCompaniesAndPosts { get; set; }

        public CurrentCompanyAndPost SelectedCurrentCompanyAndPost { get; set; }
        public int SelectedIndexCurrentCompanyAndPost { get; set; }

        public bool IsAdminSalary { get; set; }
        public bool IsDeadSpirit { get; set; }
        public bool IsDeadSpiritVisibility { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCompanyAndPostCommand { get; set; }
        public RelayCommand EditCompanyAndPostCommand { get; set; }
        public RelayCommand RemoveCompanyAndPostCommand { get; set; }

        public RelayCommand AddPhotoCommand { get; set; }
        public RelayCommand RemovePhotoCommand { get; set; }

        private void AddCompanyAndPost(object parameter)
        {
            var currentWorkerCompanyAndPostViewModel = new CurrentCompanyAndPostViewModel(SelectedDirectoryWorkerStartDate, DateTime.Now);
            var currentWorkerCompanyAndPostView = new CurrentCompanyAndPostView();

            currentWorkerCompanyAndPostView.DataContext = currentWorkerCompanyAndPostViewModel;
            currentWorkerCompanyAndPostView.ShowDialog();

            var currentCompanyAndPost = currentWorkerCompanyAndPostViewModel.CurrentCompanyAndPost;

            if (currentCompanyAndPost != null)
            {
                var postSalary = BC.GetDirectoryPostSalaryByDate(currentCompanyAndPost.DirectoryPost.Id, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

                currentCompanyAndPost.Salary = IsAdminSalary ? postSalary.AdminWorkerSalary.Value : postSalary.UserWorkerSalary;

                if (CurrentCompaniesAndPosts.Any())
                {
                    var prevPost = CurrentCompaniesAndPosts.OrderByDescending(s => s.PostChangeDate).First(s => currentCompanyAndPost.PostChangeDate.Date >= s.PostChangeDate.Date);
                    prevPost.PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);

                    int index = CurrentCompaniesAndPosts.ToList().IndexOf(prevPost);
                    if (index != (CurrentCompaniesAndPosts.Count - 1))
                    {
                        currentCompanyAndPost.PostFireDate = CurrentCompaniesAndPosts[index + 1].PostChangeDate.AddDays(-1);
                    }
                }

                CurrentCompaniesAndPosts.Add(currentCompanyAndPost);
                CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>(CurrentCompaniesAndPosts.OrderBy(p => p.PostChangeDate));
            }
        }

        private void EditCompanyAndPost(object parameter)
        {
            int prevIndex = SelectedIndexCurrentCompanyAndPost;
            var currentWorkerCompanyAndPostViewModel = new CurrentCompanyAndPostViewModel(SelectedCurrentCompanyAndPost, SelectedDirectoryWorkerStartDate, DateTime.Now);
            var currentWorkerCompanyAndPostView = new CurrentCompanyAndPostView();

            currentWorkerCompanyAndPostView.DataContext = currentWorkerCompanyAndPostViewModel;
            currentWorkerCompanyAndPostView.ShowDialog();

            var currentCompanyAndPost = currentWorkerCompanyAndPostViewModel.CurrentCompanyAndPost;

            if (currentCompanyAndPost != null)
            {
                var postSalary = BC.GetDirectoryPostSalaryByDate(currentCompanyAndPost.DirectoryPost.Id, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

                currentCompanyAndPost.Salary = IsAdminSalary ? postSalary.AdminWorkerSalary.Value : postSalary.UserWorkerSalary;

                if (CurrentCompaniesAndPosts.Any())
                {
                    var prevPost = CurrentCompaniesAndPosts[prevIndex != 0 ? prevIndex - 1 : 0];
                    prevPost.PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);

                    if (prevIndex != (CurrentCompaniesAndPosts.Count - 1))
                    {
                        currentCompanyAndPost.PostFireDate = CurrentCompaniesAndPosts[prevIndex + 1].PostChangeDate.AddDays(-1);
                    }
                }

                CurrentCompaniesAndPosts.Remove(SelectedCurrentCompanyAndPost);
                CurrentCompaniesAndPosts.Insert(prevIndex, currentCompanyAndPost);
            }
        }

        private void RemoveCompanyAndPost(object parameter)
        {
            int index = CurrentCompaniesAndPosts.IndexOf(SelectedCurrentCompanyAndPost);
            var fireDate = index < CurrentCompaniesAndPosts.Count - 1 ? CurrentCompaniesAndPosts[index + 1].PostChangeDate.AddDays(-1) : default(DateTime?);

            CurrentCompaniesAndPosts.Remove(SelectedCurrentCompanyAndPost);

            CurrentCompaniesAndPosts[index - 1].PostFireDate = fireDate;
        }

        private bool IsSelectedCompanyAndPost(object parameter)
        {
            return SelectedCurrentCompanyAndPost != null;
        }

        private void AddPhoto(object parameter)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                Photo = new BitmapImage(new Uri(filename));
                AddPhotoName = "Изменить фото";
            }

        }

        private void RemovePhoto(object parameter)
        {
            Photo = null;
            AddPhotoName = "Добавить фото";
        }

        #endregion
    }
}

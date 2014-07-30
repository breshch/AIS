using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Views.Currents;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AIS_Enterprise_Global.ViewModels
{
    public abstract class DirectoryWorkerBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryWorkerBaseViewModel()
            : base()
        {
            IsAdminSalary = HelperMethods.IsPrivilege(BC, UserPrivileges.Salary_AdminSalary);
            IsDeadSpiritVisibility = HelperMethods.IsPrivilege(BC, UserPrivileges.WorkersVisibility_DeadSpirit);

            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>();

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
                currentCompanyAndPost.Salary = IsAdminSalary ? currentCompanyAndPost.DirectoryPost.AdminWorkerSalary.Value : currentCompanyAndPost.DirectoryPost.UserWorkerSalary;

                if (CurrentCompaniesAndPosts.Any())
                {
                    CurrentCompaniesAndPosts.OrderBy(p => p.PostChangeDate).Last().PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);
                }

                CurrentCompaniesAndPosts.Add(currentCompanyAndPost);
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
                currentCompanyAndPost.Salary = IsAdminSalary ? currentCompanyAndPost.DirectoryPost.AdminWorkerSalary.Value : currentCompanyAndPost.DirectoryPost.UserWorkerSalary;

                if (CurrentCompaniesAndPosts.Any())
                {
                    //CurrentCompaniesAndPosts.OrderBy(p => p.PostChangeDate).Last().PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);
                }

                CurrentCompaniesAndPosts.Remove(SelectedCurrentCompanyAndPost);
                CurrentCompaniesAndPosts.Insert(prevIndex, currentCompanyAndPost);
            }
        }

        private void RemoveCompanyAndPost(object parameter)
        {
            CurrentCompaniesAndPosts.Remove(SelectedCurrentCompanyAndPost);
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

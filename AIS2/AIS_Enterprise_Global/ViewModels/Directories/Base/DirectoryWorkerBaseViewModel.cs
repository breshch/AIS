using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Views.Currents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.ViewModels
{
    public abstract class DirectoryWorkerBaseViewModel : ViewModel
    {
        #region Base

        public DirectoryWorkerBaseViewModel()
            : base()
        {
            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>();

            AddCompanyAndPostCommand = new RelayCommand(AddCompanyAndPost);
            RemoveCompanyAndPostCommand = new RelayCommand(RemoveCompanyAndPost, CanRemovingCompanyAndPost);
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

        public ObservableCollection<CurrentCompanyAndPost> CurrentCompaniesAndPosts { get; set; }

        public CurrentCompanyAndPost SelectedCurrentCompanyAndPost { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCompanyAndPostCommand { get; set; }
        public RelayCommand RemoveCompanyAndPostCommand { get; set; }

        private void AddCompanyAndPost(object parameter)
        {
            var currentWorkerCompanyAndPostViewModel = new CurrentCompanyAndPostViewModel();
            var currentWorkerCompanyAndPostView = new CurrentCompanyAndPostView();

            currentWorkerCompanyAndPostView.DataContext = currentWorkerCompanyAndPostViewModel;
            currentWorkerCompanyAndPostView.ShowDialog();

            var currentCompanyAndPost = currentWorkerCompanyAndPostViewModel.CurrentCompanyAndPost;

            if (currentCompanyAndPost != null)
            {
                if (CurrentCompaniesAndPosts.Any())
                {
                    CurrentCompaniesAndPosts.Last().PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);
                }

                CurrentCompaniesAndPosts.Add(currentCompanyAndPost);
            }
        }

        private void RemoveCompanyAndPost(object parameter)
        {
            CurrentCompaniesAndPosts.Remove(SelectedCurrentCompanyAndPost);
        }

        private bool CanRemovingCompanyAndPost(object parameter)
        {
            return SelectedCurrentCompanyAndPost != null;
        }

        #endregion
    }
}

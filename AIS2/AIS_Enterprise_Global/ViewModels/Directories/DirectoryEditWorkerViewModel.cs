using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Currents;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.Views.Currents;
using AIS_Enterprise_Global.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryEditWorkerViewModel : ViewModel
    {
        #region Base

        private DirectoryWorker _selectedDirectoryWorker;

        public DirectoryEditWorkerViewModel(int workerId) : base()
        {
            _selectedDirectoryWorker = BC.GetDirectoryWorker(workerId);
            FillInputData();

            AddCompanyAndPostCommand = new RelayCommand(AddCompanyAndPost);
            RemoveCompanyAndPostCommand = new RelayCommand(RemoveCompanyAndPost,CanRemovingCompanyAndPost);
            EditWorkerCommand = new RelayCommand(EditWorker, CanEditingWorker);
            FireWorkerCommand = new RelayCommand(FireWorker, CanEditingWorker);
        }

        private void ClearInputData()
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

        private void FillInputData()
        {
            DirectoryWorkerLastName = _selectedDirectoryWorker.LastName;
            DirectoryWorkerFirstName = _selectedDirectoryWorker.FirstName;
            DirectoryWorkerMidName = _selectedDirectoryWorker.MidName;
            DirectoryWorkerGender = _selectedDirectoryWorker.Gender;
            SelectedDirectoryWorkerBirthDay = _selectedDirectoryWorker.BirthDay;
            DirectoryWorkerAddress = _selectedDirectoryWorker.Address;
            DirectoryWorkerCellPhone = _selectedDirectoryWorker.CellPhone;
            DirectoryWorkerHomePhone = _selectedDirectoryWorker.HomePhone;
            SelectedDirectoryWorkerStartDate = _selectedDirectoryWorker.StartDate;
            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>(_selectedDirectoryWorker.CurrentCompaniesAndPosts.
                Select(c => new CurrentCompanyAndPost { DirectoryPost = c.DirectoryPost, PostChangeDate = c.ChangeDate}));
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

        [StopNotify]
        public DateTime? SelectedDirectoryWorkerFireDate { get; set; }

        public ObservableCollection<CurrentCompanyAndPost> CurrentCompaniesAndPosts { get; set; }

        public CurrentCompanyAndPost SelectedCurrentCompanyAndPost { get; set; }

        [StopNotify]
        public bool IsNotFireDate
        {
            get 
            {
                return _selectedDirectoryWorker.FireDate == null;
            }
        }

        #endregion

  
        #region Commands

        public RelayCommand AddCompanyAndPostCommand { get; set; }
        public RelayCommand RemoveCompanyAndPostCommand { get; set; }
        public RelayCommand EditWorkerCommand { get; set; }
        public RelayCommand FireWorkerCommand { get; set; }

        private void AddCompanyAndPost(object parameter)
        {
            var currentWorkerCompanyAndPostViewModel = new CurrentCompanyAndPostViewModel();
            var currentWorkerCompanyAndPostView = new CurrentCompanyAndPostView();

            currentWorkerCompanyAndPostView.DataContext = currentWorkerCompanyAndPostViewModel;
            currentWorkerCompanyAndPostView.ShowDialog();

            var currentCompanyAndPost = currentWorkerCompanyAndPostViewModel.CurrentCompanyAndPost;

            if (currentCompanyAndPost != null)
            {
                CurrentCompaniesAndPosts.Last().PostFireDate = currentCompanyAndPost.PostChangeDate.AddDays(-1);

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

        private void EditWorker(object parameter)
        {
            BC.EditDirectoryWorker(_selectedDirectoryWorker.Id, DirectoryWorkerLastName, DirectoryWorkerFirstName, DirectoryWorkerMidName, DirectoryWorkerGender, SelectedDirectoryWorkerBirthDay, DirectoryWorkerAddress,
                DirectoryWorkerHomePhone, DirectoryWorkerCellPhone, SelectedDirectoryWorkerStartDate, SelectedDirectoryWorkerFireDate, CurrentCompaniesAndPosts);

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        private bool CanEditingWorker(object parameter)
        {
            return IsValidateAllProperties() && CurrentCompaniesAndPosts.Any();
        }

        private void FireWorker(object parameter)
        {
            var directoryWorkerFireDateViewModel = new DirectoryWorkerFireDateViewModel();
            var directoryWorkerFireDateView = new DirectoryWorkerFireDateView();

            directoryWorkerFireDateView.DataContext = directoryWorkerFireDateViewModel;
            directoryWorkerFireDateView.ShowDialog();

            SelectedDirectoryWorkerFireDate = directoryWorkerFireDateViewModel.SelectedDirectoryWorkerFireDate;

            if (SelectedDirectoryWorkerFireDate != null)
            {
                EditWorker(parameter);
            }
        }

        #endregion
    }
}

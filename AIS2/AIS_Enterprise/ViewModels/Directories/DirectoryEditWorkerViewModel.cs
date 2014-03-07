using AIS_Enterprise.Helpers;
using AIS_Enterprise.Helpers.Temps;
using AIS_Enterprise.Models;
using AIS_Enterprise.Models.Currents;
using AIS_Enterprise.Models.Directories;
using AIS_Enterprise.ViewModels.Directories;
using AIS_Enterprise.Views.Currents;
using AIS_Enterprise.Views.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise.ViewModels
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
            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>(_selectedDirectoryWorker.CurrentCompaniesAndPosts.Select(c => new CurrentCompanyAndPost { DirectoryPost = c.DirectoryPost, PostChangeDate = c.ChangeDate}));
        }
        
        #endregion


        #region DirectoryWorkerLastName

        private string _directoryWorkerLastName;

        [Required]
        [Display(Name = "Фамилия")]
        public string DirectoryWorkerLastName
        {
            get
            {
                return _directoryWorkerLastName; 
            }
            set
            {
                _directoryWorkerLastName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerFirstName

        private string _directoryWorkerFirstName;

        [Required]
        [Display(Name = "Имя")]
        public string DirectoryWorkerFirstName
        {
            get
            {
                return _directoryWorkerFirstName;
            }
            set
            {
                _directoryWorkerFirstName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerMidName

        private string _directoryWorkerMidName;

        [Required]
        [Display(Name = "Отчество")]
        public string DirectoryWorkerMidName
        {
            get
            {
                return _directoryWorkerMidName;
            }
            set
            {
                _directoryWorkerMidName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerGender

        private Gender _directoryWorkerGender;
        public Gender DirectoryWorkerGender
        {
            get 
            {
                return _directoryWorkerGender;
            }
            set
            {
                _directoryWorkerGender = value;
            }
        }
        
        #endregion


        #region DirectoryWorkerBirthDate

        private DateTime _selectedDirectoryWorkerBirthDay;
        public DateTime SelectedDirectoryWorkerBirthDay
        {
            get
            {
                return _selectedDirectoryWorkerBirthDay;
            }
            set
            {
                _selectedDirectoryWorkerBirthDay = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerAddress

        private string _directoryWorkerAddress;

        [Required]
        [Display(Name = "Адрес")]
        public string DirectoryWorkerAddress
        {
            get
            {
                return _directoryWorkerAddress;
            }
            set
            {
                _directoryWorkerAddress = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerCellPhone

        private string _directoryWorkerCellPhone;

        [Required]
        [Display(Name = "Мобильный телефон")]
        public string DirectoryWorkerCellPhone
        {
            get
            {
                return _directoryWorkerCellPhone;
            }
            set
            {
                _directoryWorkerCellPhone = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerHomePhone

        private string _directoryWorkerHomePhone;
        public string DirectoryWorkerHomePhone
        {
            get
            {
                return _directoryWorkerHomePhone;
            }
            set
            {
                _directoryWorkerHomePhone = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region DirectoryWorkerStartDate

        private DateTime _selectedDirectoryWorkerStartDate;
        public DateTime SelectedDirectoryWorkerStartDate
        {
            get
            {
                return _selectedDirectoryWorkerStartDate;
            }
            set
            {
                _selectedDirectoryWorkerStartDate = value;
                OnPropertyChanged();
            }
        }
        #endregion 

        #region SelectedDirectoryWorkerFireDate

        public DateTime? SelectedDirectoryWorkerFireDate { get; set; }

        #endregion


        #region CurrentCompaniesAndPosts

        private ObservableCollection<CurrentCompanyAndPost> _currentCompaniesAndPosts;
        public ObservableCollection<CurrentCompanyAndPost> CurrentCompaniesAndPosts
        {
            get 
            {
                return _currentCompaniesAndPosts;
            }
            set 
            {
                _currentCompaniesAndPosts = value;
                OnPropertyChanged();
            }
        }

        private CurrentCompanyAndPost _selectedCurrentCompanyAndPost;
        public CurrentCompanyAndPost SelectedCurrentCompanyAndPost
        {
            get
            {
                return _selectedCurrentCompanyAndPost;
            }
            set
            {
                _selectedCurrentCompanyAndPost = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region IsFireDate

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
            var directoryWorkerSetFireDateViewModel = new DirectoryWorkerSetFireDateViewModel();
            var directoryWorkerSetFireDateView = new DirectoryWorkerSetFireDateView();

            directoryWorkerSetFireDateView.DataContext = directoryWorkerSetFireDateViewModel;
            directoryWorkerSetFireDateView.ShowDialog();

            SelectedDirectoryWorkerFireDate = directoryWorkerSetFireDateViewModel.SelectedDirectoryWorkerFireDate;

            if (SelectedDirectoryWorkerFireDate != null)
            {
                Debug.WriteLine("huy");
                EditWorker(parameter);
            }
        }

        #endregion
    }
}

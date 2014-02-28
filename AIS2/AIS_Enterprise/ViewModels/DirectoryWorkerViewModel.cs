using AIS_Enterprise.Helpers;
using AIS_Enterprise.Models;
using AIS_Enterprise.Models.Currents;
using AIS_Enterprise.Views.Currents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.ViewModels
{
    public class DirectoryWorkerViewModel : ViewModel
    {
        #region Base

        private BusinessContext _bc = new BusinessContext();

        public DirectoryWorkerViewModel()
        {
            DirectoryWorkerGender = Gender.Female;
            CurrentCompaniesAndPosts = new ObservableCollection<CurrentPost>();
            AddCompanyAndPostCommand = new RelayCommand(AddCompanyAndPost);
            RemoveCompanyAndPostCommand = new RelayCommand(RemoveCompanyAndPost,CanRemovingCompanyAndPost);
        }

        private void RefreshPostsAndCompanies()
        {
            
        }

        private void ClearInputData()
        {
            DirectoryWorkerLastName = null;
            DirectoryWorkerFirstName = null;
            DirectoryWorkerMidName = null;
            DirectoryWorkerGender = Gender.Male;
            SelectedDirectoryWorkerBirthDate = DateTime.Now;
            DirectoryWorkerAddress = null;
            DirectoryWorkerMobilePhone = null;
            DirectoryWorkerHomePhone = null;
            SelectedDirectoryWorkerStartDate = DateTime.Now;
        }
        
        #endregion


        #region DirectoryWorkerLastName

        private string _directoryWorkerLastName;
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
                OnPropertyChanged("ValidateDirectoryWorkerLastName");
            }
        }

        public string ValidateDirectoryWorkerLastName
        {
            get 
            {
                return Validations.ValidateText(DirectoryWorkerLastName, "Фамилия", 32);
            }
        }

        #endregion


        #region DirectoryWorkerFirstName

        private string _directoryWorkerFirstName;
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
                OnPropertyChanged("ValidateDirectoryWorkerFirstName");
            }
        }

        public string ValidateDirectoryWorkerFirstName
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerFirstName, "Имя", 32);
            }
        }

        #endregion


        #region DirectoryWorkerMidName

        private string _directoryWorkerMidName;
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
                OnPropertyChanged("ValidateDirectoryWorkerMidName");
            }
        }

        public string ValidateDirectoryWorkerMidName
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerMidName, "Отчество", 32);
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
                Debug.WriteLine(value);
                _directoryWorkerGender = value;
            }
        }
        
        #endregion


        #region DirectoryWorkerBirthDate

        private DateTime _selectedDirectoryWorkerBirthDate;
        public DateTime SelectedDirectoryWorkerBirthDate
        {
            get
            {
                return _selectedDirectoryWorkerBirthDate;
            }
            set
            {
                _selectedDirectoryWorkerBirthDate = value;
                OnPropertyChanged();
            }
        }


        #endregion


        #region DirectoryWorkerAddress

        private string _directoryWorkerAddress;
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
                OnPropertyChanged("ValidateDirectoryWorkerAddress");
            }
        }

        public string ValidateDirectoryWorkerAddress
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerAddress, "Адрес", 256);
            }
        }

        #endregion


        #region DirectoryWorkerMobilePhone

        private string _directoryWorkerMobilePhone;
        public string DirectoryWorkerMobilePhone
        {
            get
            {
                return _directoryWorkerMobilePhone;
            }
            set
            {
                _directoryWorkerMobilePhone = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerMobilePhone");
            }
        }

        public string ValidateDirectoryWorkerMobilePhone
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerMobilePhone, "Мобильный телефон", 16);
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
                OnPropertyChanged("ValidateDirectoryWorkerHomePhone");
            }
        }

        public string ValidateDirectoryWorkerHomePhone
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerHomePhone, "Домашний телефон", 16);
            }
        }

        #endregion


        #region DirectoryWorkerSartDate

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


        #region CurrentCompaniesAndPosts

        private ObservableCollection<CurrentPost> _currentCompaniesAndPosts;
        public ObservableCollection<CurrentPost> CurrentCompaniesAndPosts
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

        #endregion


        #region Commands

        public RelayCommand AddCompanyAndPostCommand { get; set; }
        public RelayCommand RemoveCompanyAndPostCommand { get; set; }

        private void AddCompanyAndPost(object parameter)
        {
            var currentWorkerCompanyAndPostViewModel = new CurrentWorkerCompanyAndPostViewModel();
            var currentWorkerCompanyAndPostView = new CurrentWorkerCompanyAndPostView();

            currentWorkerCompanyAndPostView.DataContext = currentWorkerCompanyAndPostViewModel;
            currentWorkerCompanyAndPostView.ShowDialog();

            var c = currentWorkerCompanyAndPostViewModel.CurrentCompanyAndPost;
        }

        private void RemoveCompanyAndPost(object parameter)
        {

        }

        private bool CanRemovingCompanyAndPost(object parameter)
        {
            return true;
        }

        #endregion


        #region Validation

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "DirectoryWorkerLastName":
                    return ValidateDirectoryWorkerLastName;
                
                case "DirectoryWorkerFirstName":
                    return ValidateDirectoryWorkerFirstName;
               
                case "DirectoryWorkerMidName":
                    return ValidateDirectoryWorkerMidName;
               
                case "DirectoryWorkerAddress":
                    return ValidateDirectoryWorkerAddress;
                
                case "DirectoryWorkerMobilePhone":
                    return ValidateDirectoryWorkerMobilePhone;
               
                case "DirectoryWorkerHomePhone":
                    return ValidateDirectoryWorkerHomePhone;
                
                //case "DirectoryWorkerMidName":
                //    return ValidateDirectoryWorkerMidName;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}

using AIS_Enterprise.Helpers;
using AIS_Enterprise.Models;
using AIS_Enterprise.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AIS_Enterprise.ViewModels
{
    public class DirectoryPostViewModel : ViewModel
    {
        private BusinessContext _bc = new BusinessContext();

        public DirectoryPostViewModel()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(_bc.GetDirectoryTypeOfPosts());
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(_bc.GetDirectoryCompanies());
            SelectedDirectoryPostDate = DateTime.Now;

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemove);
            ViewCloseCommand = new RelayCommand(ViewClose);

            RefreshDirectoryPosts();
        }

        private void RefreshDirectoryPosts()
        {
            DirectoryPosts = new ObservableCollection<DirectoryPost>(_bc.GetDirectoryPosts());
        }


        #region DirectoryPosts

        private ObservableCollection<DirectoryPost> _directoryPosts;
        public ObservableCollection<DirectoryPost> DirectoryPosts
        {
            get
            {
                return _directoryPosts;
            }
            set
            {
                _directoryPosts = value;
                OnPropertyChanged();
            }
        }
        
        private DirectoryPost _selectedDirectoryPost;
        public DirectoryPost SelectedDirectoryPost
        {
            get
            {
                return _selectedDirectoryPost;
            }
            set
            {
                _selectedDirectoryPost = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DirectoryPostName

        private string _directoryPostName;

        public string DirectoryPostName
        {
            get
            {
                return _directoryPostName;
            }
            set
            {
                _directoryPostName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryPostName");
            }
        }

        public string ValidateDirectoryPostName
        {
            get
            {
                return Validations.ValidateText(DirectoryPostName, "Должность", 32);
            }
        }

        #endregion

        #region DirectoryTypeOfPost

        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }

        private DirectoryTypeOfPost _selectedDirectoryTypeOfPost;
        public DirectoryTypeOfPost SelectedDirectoryTypeOfPost
        {
            get
            {
                return _selectedDirectoryTypeOfPost;
            }
            set
            {
                _selectedDirectoryTypeOfPost = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateSelectedDirectoryTypeOfPost");
            }
        }

        public string ValidateSelectedDirectoryTypeOfPost
        {
            get
            {
                return Validations.ValidateObject(SelectedDirectoryTypeOfPost, "Тип должности");
            }
        }

        #endregion

        #region DirectoryCompanyName

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;
        public DirectoryCompany SelectedDirectoryCompany
        {
            get
            {
                return _selectedDirectoryCompany;
            }
            set
            {
                _selectedDirectoryCompany = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateSelectedDirectoryCompany");
            }
        }

        public string ValidateSelectedDirectoryCompany
        {
            get
            {
                return Validations.ValidateObject(SelectedDirectoryCompany, "Компания");
            }
        }

        #endregion

        #region DirectoryPostDate

        private DateTime _selectedDirectoryPostDate;
        public DateTime SelectedDirectoryPostDate
        {
            get
            {
                return _selectedDirectoryPostDate;
            }
            set
            {
                _selectedDirectoryPostDate = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region DirectoryPostUserWorkerSalary

        private string _directoryPostUserWorkerSalary;
        public string DirectoryPostUserWorkerSalary
        {
            get 
            {
                return _directoryPostUserWorkerSalary;
            }
            set
            {
                _directoryPostUserWorkerSalary = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryPostUserWorkerSalary");
            }
        }

        public string ValidateDirectoryPostUserWorkerSalary
        {
            get 
            {
                return Validations.ValidateDoubleMoreAndEqualZero(DirectoryPostUserWorkerSalary, "Оклад");
            }
        }


        #endregion

        #region DirectoryPostUserWorkerHalfSalary

        private string _directoryPostUserWorkerHalfSalary;
        public string DirectoryPostUserWorkerHalfSalary
        {
            get
            {
                return _directoryPostUserWorkerHalfSalary;
            }
            set
            {
                _directoryPostUserWorkerHalfSalary = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryPostUserWorkerHalfSalary");
            }
        }

        public string ValidateDirectoryPostUserWorkerHalfSalary
        {
            get
            {
                return Validations.ValidateDoubleMoreAndEqualZero(DirectoryPostUserWorkerHalfSalary, "Совместительство");
            }
        }


        #endregion

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ViewCloseCommand { get; set; }

        private void ClearInputData()
        {
            DirectoryPostName = null;
            SelectedDirectoryTypeOfPost = null;
            SelectedDirectoryCompany = null;
            SelectedDirectoryPostDate = DateTime.Now;
            DirectoryPostUserWorkerSalary = null;
            DirectoryPostUserWorkerHalfSalary = null;
        }

        private void Add(object parameter)
        {
            _bc.AddDirectoryPost(DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, SelectedDirectoryPostDate, DirectoryPostUserWorkerSalary, DirectoryPostUserWorkerHalfSalary);
            
            RefreshDirectoryPosts();

            ClearInputData();
        }

        private bool CanAdding(object parameter)
        {
            foreach (var propertyName in ValidatedAddingProperties)
            {
                if (OnValidate(propertyName) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private void Remove(object parameter)
        {
            _bc.RemoveDirectoryPost(SelectedDirectoryPost);
            RefreshDirectoryPosts();
        }

        private bool CanRemove(object parameter)
        {
            return SelectedDirectoryPost != null;
        }

        public void ViewClose(object parameter)
        {
            _bc.Dispose();
        }

        private string[] ValidatedAddingProperties =
        {
            "DirectoryPostName", 
            "SelectedDirectoryTypeOfPost",
            "SelectedDirectoryCompany",
            "DirectoryPostUserWorkerSalary",
            "DirectoryPostUserWorkerHalfSalary"
        };

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "DirectoryPostName":
                    return ValidateDirectoryPostName;

                case "SelectedDirectoryTypeOfPost":
                    return ValidateSelectedDirectoryTypeOfPost;

                case "SelectedDirectoryCompany":
                    return ValidateSelectedDirectoryCompany;

                case "DirectoryPostUserWorkerSalary":
                    return ValidateDirectoryPostUserWorkerSalary;

                case "DirectoryPostUserWorkerHalfSalary":
                    return ValidateDirectoryPostUserWorkerHalfSalary;

                default:
                    throw new InvalidOperationException();
            }
        }

    }
}

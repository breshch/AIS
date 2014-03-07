using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AIS_Enterprise_Global.Helpers.Attributes;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryPostViewModel : ViewModel
    {
        #region Base

        public DirectoryPostViewModel() : base()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(BC.GetDirectoryTypeOfPosts());
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());
            SelectedDirectoryPostDate = DateTime.Now;

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemove);

            RefreshDirectoryPosts();
        }

        private void RefreshDirectoryPosts()
        {
            DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts());
        }

        private void ClearInputData()
        {
            DirectoryPostName = null;
            SelectedDirectoryTypeOfPost = null;
            SelectedDirectoryCompany = null;
            SelectedDirectoryPostDate = DateTime.Now;
            DirectoryPostUserWorkerSalary = null;
            DirectoryPostUserWorkerHalfSalary = null;
        }

        #endregion


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

        [Required]
        [Display(Name = "Должность")]
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
            }
        }

        #endregion


        #region DirectoryTypeOfPost

        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }

        private DirectoryTypeOfPost _selectedDirectoryTypeOfPost;

        [RequireSelected]
        [Display(Name = "Вид должности")]
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
            }
        }

        #endregion


        #region DirectoryCompanyName

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;

        [RequireSelected]
        [Display(Name = "Компания")]
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

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Оклад")]
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
            }
        }

        #endregion


        #region DirectoryPostUserWorkerHalfSalary

        private string _directoryPostUserWorkerHalfSalary;

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Совместительство")]
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
            }
        }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private void Add(object parameter)
        {
            BC.AddDirectoryPost(DirectoryPostName, SelectedDirectoryTypeOfPost, SelectedDirectoryCompany, SelectedDirectoryPostDate, DirectoryPostUserWorkerSalary, DirectoryPostUserWorkerHalfSalary);
            
            RefreshDirectoryPosts();

            ClearInputData();
        }

        private bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        private void Remove(object parameter)
        {
            BC.RemoveDirectoryPost(SelectedDirectoryPost);
            RefreshDirectoryPosts();
        }

        private bool CanRemove(object parameter)
        {
            return SelectedDirectoryPost != null;
        }

        #endregion
    }
}

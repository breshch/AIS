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
    public class DirectoryTypeOfPostViewModel : ViewModel
    {
        #region Base

        private BusinessContext _bc = new BusinessContext();

        public DirectoryTypeOfPostViewModel()
        {
            RefreshDirectoryTypeOfPosts();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        private void RefreshDirectoryTypeOfPosts()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(_bc.GetDirectoryTypeOfPosts());
        }

        private void ClearInputData()
        {
            DirectoryTypeOfPostName = null;
        }

        #endregion


        #region DirectoryTypeOfPosts

        private ObservableCollection<DirectoryTypeOfPost> _directoryTypeOfPosts;
        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts
        {
            get
            {
                return _directoryTypeOfPosts;
            }
            set
            {
                _directoryTypeOfPosts = value;
                OnPropertyChanged();
            }
        }

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
            }
        }

        #endregion


        #region DirectoryTypeOfPostName

        private string _directoryTypeOfPostName;

        [Required]
        [Display(Name = "Тип должности")]
        public string DirectoryTypeOfPostName
        {
            get
            {
                return _directoryTypeOfPostName;
            }
            set
            {
                _directoryTypeOfPostName = value;
                OnPropertyChanged();
                //OnPropertyChanged("ValidateDirectoryTypeOfPostName");
            }
        }

        //public string ValidateDirectoryTypeOfPostName
        //{
        //    get
        //    {
        //        return Validations.ValidateText(DirectoryTypeOfPostName, "Тип должности", 32);
        //    }
        //}

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ViewCloseCommand { get; set; }

        public void Add(object parameter)
        {
            _bc.AddDirectoryTypeOfPost(DirectoryTypeOfPostName);

            RefreshDirectoryTypeOfPosts();

            ClearInputData();
        }

        public bool CanAdding(object parameter)
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

        public void Remove(object parameter)
        {
            _bc.RemoveDirectoryTypeOfPost(SelectedDirectoryTypeOfPost);

            RefreshDirectoryTypeOfPosts();
            
            if (DirectoryTypeOfPosts.Any())
            {
                SelectedDirectoryTypeOfPost = DirectoryTypeOfPosts.Last();
            }
        }

        public bool CanRemoving(object parameter)
        {
            return SelectedDirectoryTypeOfPost != null;
        }

        public void ViewClose(object parameter)
        {
            _bc.Dispose();
        }

        #endregion


        #region Validation

        private string[] ValidatedAddingProperties =
        {
            "DirectoryTypeOfPostName"
        };

        //protected override string OnValidate(string propertyName)
        //{
        //    switch (propertyName)
        //    {
        //        case "DirectoryTypeOfPostName":
        //            return ValidateDirectoryTypeOfPostName;
        //        default:
        //            throw new InvalidOperationException();
        //    }
        //}

        #endregion
    }
}

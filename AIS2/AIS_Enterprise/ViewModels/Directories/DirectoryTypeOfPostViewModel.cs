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

        public DirectoryTypeOfPostViewModel() :  base()
        {
            RefreshDirectoryTypeOfPosts();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
        }

        private void RefreshDirectoryTypeOfPosts()
        {
            DirectoryTypeOfPosts = new ObservableCollection<DirectoryTypeOfPost>(BC.GetDirectoryTypeOfPosts());
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
        [Display(Name = "Вид должности")]
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
            }
        }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        
        public void Add(object parameter)
        {
            BC.AddDirectoryTypeOfPost(DirectoryTypeOfPostName);

            RefreshDirectoryTypeOfPosts();

            ClearInputData();
        }

        public bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        public void Remove(object parameter)
        {
            BC.RemoveDirectoryTypeOfPost(SelectedDirectoryTypeOfPost);

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

        #endregion
    }
}

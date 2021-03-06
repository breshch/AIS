﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryTypeOfPostViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryTypeOfPostViewModel()
            : base()
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


        #region Properties

        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }

        public DirectoryTypeOfPost SelectedDirectoryTypeOfPost { get; set; }

        [Required]
        [Display(Name = "Вид должности")]
        public string DirectoryTypeOfPostName { get; set; }

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
            BC.RemoveDirectoryTypeOfPost(SelectedDirectoryTypeOfPost.Id);

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

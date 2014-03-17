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
    public class DirectoryTypeOfPostViewModel : ViewModel
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

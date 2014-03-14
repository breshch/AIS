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


        #region Properties

        public ObservableCollection<DirectoryPost> DirectoryPosts { get; set; }

        public DirectoryPost SelectedDirectoryPost { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string DirectoryPostName { get; set; }

        public ObservableCollection<DirectoryTypeOfPost> DirectoryTypeOfPosts { get; set; }

        [RequireSelected]
        [Display(Name = "Вид должности")]
        public DirectoryTypeOfPost SelectedDirectoryTypeOfPost { get; set; }

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        [RequireSelected]
        [Display(Name = "Компания")]
        public DirectoryCompany SelectedDirectoryCompany { get; set; }

        public DateTime SelectedDirectoryPostDate { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Оклад")]
        public string DirectoryPostUserWorkerSalary { get; set; }

        [Required]
        [DoubleValue(MinValue = 0)]
        [Display(Name = "Совместительство")]
        public string DirectoryPostUserWorkerHalfSalary { get; set; }

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

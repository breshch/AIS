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


        #region Properties

        private BusinessContext _bc = new BusinessContext();

        private string _directoryTypeOfPostName;

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

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ViewCloseCommand { get; set; }


        public void Add(object parameter)
        {
            _bc.AddDirectoryTypeOfPost(DirectoryTypeOfPostName);

            RefreshDirectoryTypeOfPosts();

            DirectoryTypeOfPostName = null;
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

        private string ValidateDirectoryTypeOfPostName()
        {
            if (string.IsNullOrWhiteSpace(DirectoryTypeOfPostName))
            {
                return "Не заполнено поле \"Тип должности\".";
            }

            if (DirectoryTypeOfPostName.Length > 32)
            {
                return "Длина поля \"Тип должности\" должна быть не больше 32 символов.";
            }

            return null;
        }

        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "DirectoryTypeOfPostName")
            {
                return ValidateDirectoryTypeOfPostName();
            }

            return base.OnValidate(propertyName);
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

        public bool CanRemoving(object parameter)
        {
            return SelectedDirectoryTypeOfPost != null;
        }

        #endregion
    }
}

﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;

namespace AIS_Enterprise_Global.ViewModels
{
    public class CurrentTemporaryPostViewModel : ViewModelGlobal
    {
        #region Base

        private CurrentPost _mainPost;

        public CurrentTemporaryPostViewModel(int workerId, DateTime endDate)
            : base()
        {
            _mainPost = BC.GetMainPost(workerId, endDate);
            AddCommand = new RelayCommand(Add, CanAdding);
            var lastPost = BC.GetCurrentPost(workerId, endDate);

            DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts(lastPost.DirectoryPost.DirectoryCompany));

                if (lastPost.IsTemporaryPost)
                {
                    
                    SelectedDirectoryPost = DirectoryPosts.First(p => p.Name == _mainPost.DirectoryPost.Name);
                }

            SelectedPostChangeDate = endDate;
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryPost> DirectoryPosts { get; set; }

        [RequireSelected]
        [Display(Name = "Должность")]
        public DirectoryPost SelectedDirectoryPost { get; set; }

        public DateTime SelectedPostChangeDate { get; set; }

        public CurrentCompanyAndPost CurrentCompanyAndPost { get; set; }


        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            CurrentCompanyAndPost = new CurrentCompanyAndPost
            {
                DirectoryPost = SelectedDirectoryPost,
                PostChangeDate = SelectedPostChangeDate,
                IsTemporaryPost = _mainPost.DirectoryPostId != SelectedDirectoryPost.Id
            };

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        private bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        #endregion
    }
}

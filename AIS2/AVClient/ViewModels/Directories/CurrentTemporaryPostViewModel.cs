using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Helpers.Attributes;

namespace AVClient.ViewModels.Directories
{
    public class CurrentTemporaryPostViewModel : ViewModelGlobal
    {
        #region Base

        private CurrentPost _mainPost;

        public CurrentTemporaryPostViewModel(int workerId, DateTime endDate)
        {
            _mainPost = BC.GetMainPost(workerId, endDate);
            AddCommand = new RelayCommand(Add, CanAdding);
            var lastPost = BC.GetCurrentPost(workerId, endDate);

            DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPostsByCompany(lastPost.DirectoryPost.DirectoryCompany));

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

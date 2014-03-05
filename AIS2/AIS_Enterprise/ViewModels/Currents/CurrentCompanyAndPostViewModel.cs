using AIS_Enterprise.Helpers;
using AIS_Enterprise.Helpers.Temps;
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
using System.Windows;

namespace AIS_Enterprise.ViewModels
{
    public class CurrentCompanyAndPostViewModel : ViewModel
    {
        #region Base

        public CurrentCompanyAndPostViewModel()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());
            
            AddCommand = new RelayCommand(Add, CanAdding);

            SelectedPostChangeDate = DateTime.Now;
        }

        #endregion


        #region DirectoryCompanies

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;

        [Required]
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

                DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts(SelectedDirectoryCompany));
            }
        }

        #endregion


        #region DirectoryPost

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

        [Required]
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


        #region SelectedPostChangeDate

        private DateTime _selectedPostChangeDate;
        public DateTime SelectedPostChangeDate
        {
            get 
            {
                return _selectedPostChangeDate;
            }
            set
            {
                _selectedPostChangeDate = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region CurrentCompanyAndPost

        public CurrentCompanyAndPost CurrentCompanyAndPost { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCommand{ get; set; }
        
        private void Add(object parameter)
        {
            CurrentCompanyAndPost = new CurrentCompanyAndPost
            {
                DirectoryPost = SelectedDirectoryPost,
                PostChangeDate = SelectedPostChangeDate
            };

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        private bool CanAdding(object parameter)
        {
            return true;
        }

        #endregion
    }
}

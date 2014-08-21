using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_Global.ViewModels
{
    public class CurrentCompanyAndPostViewModel : ViewModelGlobal
    {
        #region Base

        private bool _isEditCurrentCompanyAndPost = false;
        private CurrentCompanyAndPost _currentCompanyAndPost;

        public CurrentCompanyAndPostViewModel(DateTime startDate, DateTime endDate)
            : base()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());

            AddCommand = new RelayCommand(Add, CanAdding);

            StartDate = startDate;
            EndDate = endDate;

            SelectedPostChangeDate = endDate;
        }

        public CurrentCompanyAndPostViewModel(CurrentCompanyAndPost currentCompanyAndPost, DateTime startDate, DateTime endDate)
            : this(startDate, endDate) 
        {
            _currentCompanyAndPost = currentCompanyAndPost;
            _isEditCurrentCompanyAndPost = true;

            SelectedDirectoryCompany = DirectoryCompanies.First(c => c.Name == currentCompanyAndPost.DirectoryPost.DirectoryCompany.Name);
            IsTwoCompanies = currentCompanyAndPost.IsTwoCompanies;
            SelectedPostChangeDate = currentCompanyAndPost.PostChangeDate;
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;

        [NoMagic]
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
                RaisePropertyChanged();

                DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts(SelectedDirectoryCompany));

                if (_isEditCurrentCompanyAndPost)
                {
                    SelectedDirectoryPost = DirectoryPosts.First(p => p.Name == _currentCompanyAndPost.DirectoryPost.Name);
                    _isEditCurrentCompanyAndPost = false;
                }
            }
        }

        public ObservableCollection<DirectoryPost> DirectoryPosts { get; set; }

        [RequireSelected]
        [Display(Name = "Должность")]
        public DirectoryPost SelectedDirectoryPost { get; set; }

        public DateTime SelectedPostChangeDate { get; set; }

        public CurrentCompanyAndPost CurrentCompanyAndPost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsTwoCompanies { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }

        private void Add(object parameter)
        {
            CurrentCompanyAndPost = new CurrentCompanyAndPost
            {
                DirectoryPost = SelectedDirectoryPost,
                PostChangeDate = SelectedPostChangeDate,
                IsTwoCompanies = IsTwoCompanies
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

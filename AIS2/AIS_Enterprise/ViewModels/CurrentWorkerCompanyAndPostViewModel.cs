using AIS_Enterprise.Helpers;
using AIS_Enterprise.Helpers.Temps;
using AIS_Enterprise.Models;
using AIS_Enterprise.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise.ViewModels
{
    public class CurrentWorkerCompanyAndPostViewModel : ViewModel
    {
        #region Base

        private BusinessContext _bc = new BusinessContext();

        public CurrentWorkerCompanyAndPostViewModel()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(_bc.GetDirectoryCompanies());
            
            AddCommand = new RelayCommand(Add, CanAdding);

            SelectedPostChangeDate = DateTime.Now;
        }

        #endregion

        #region DirectoryCompany

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;
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
                OnPropertyChanged("ValidateSelectedDirectoryCompany");

                DirectoryPosts = new ObservableCollection<DirectoryPost>(_bc.GetDirectoryPosts(SelectedDirectoryCompany));
            }
        }

        public string ValidateSelectedDirectoryCompany
        {
            get
            {
                return Validations.ValidateObject(SelectedDirectoryCompany, "Компания");
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
                OnPropertyChanged("ValidateSelectedDirectoryPost");
            }
        }

        public string ValidateSelectedDirectoryPost
        {
            get
            {
                return Validations.ValidateObject(SelectedDirectoryPost, "Должность");
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


        public CurrentCompanyAndPost CurrentCompanyAndPost { get; set; }

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


        #region Validation

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "SelectedDirectoryCompany":
                    return ValidateSelectedDirectoryCompany;
                case "SelectedDirectoryPost":
                    return ValidateSelectedDirectoryPost;

                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion


    }
}

using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Helpers.Temps;
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
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels
{
    public class CurrentCompanyAndPostViewModel : ViewModelGlobal
    {
        #region Base

        public CurrentCompanyAndPostViewModel(DateTime startDate, DateTime endDate)
            : base()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());

            AddCommand = new RelayCommand(Add, CanAdding);

            StartDate = startDate;
            EndDate = endDate;

            SelectedPostChangeDate = endDate;
        }


        #endregion


        #region Properties

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        private DirectoryCompany _selectedDirectoryCompany;

        [StopNotify]
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
                OnPropertyChanged();

                DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts(SelectedDirectoryCompany));
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

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }

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
            return IsValidateAllProperties();
        }

        #endregion
    }
}

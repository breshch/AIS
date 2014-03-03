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
    public class DirectoryCompanyViewModel : ViewModel
    {
        #region Base

        public DirectoryCompanyViewModel()
        {
            RefreshDirectoryCompanies();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        private void RefreshDirectoryCompanies()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(BC.GetDirectoryCompanies());
        }

        private void ClearInputData()
        {
            DirectoryCompanyName = null;
        }

        #endregion


        #region DirectoryCompanies

        private ObservableCollection<DirectoryCompany> _directoryCompanies;
        public ObservableCollection<DirectoryCompany> DirectoryCompanies
        {
            get
            {
                return _directoryCompanies;
            }
            set
            {
                _directoryCompanies = value;
                OnPropertyChanged();
            }
        }

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
            }
        }

        #endregion


        #region DirectoryCompanyName

        private string _directoryCompanyName;

        [Required]
        [Display(Name = "Название компании")]
        public string DirectoryCompanyName
        {
            get
            {
                return _directoryCompanyName;
            }
            set
            {
                _directoryCompanyName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public void Add(object parameter)
        {
            BC.AddDirectoryCompany(DirectoryCompanyName);

            RefreshDirectoryCompanies();

            ClearInputData();
        }

        public bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        public void Remove(object parameter)
        {
            BC.RemoveDirectoryCompany(SelectedDirectoryCompany);

            RefreshDirectoryCompanies();
            
            if (DirectoryCompanies.Any())
            {
                SelectedDirectoryCompany = DirectoryCompanies.Last();
            }
        }

        public bool CanRemoving(object parameter)
        {
            return SelectedDirectoryCompany != null;
        }

        #endregion
    }
}

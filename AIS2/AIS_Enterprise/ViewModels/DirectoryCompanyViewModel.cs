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
        public DirectoryCompanyViewModel()
        {
            RefreshDirectoryCompanies();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        private void RefreshDirectoryCompanies()
        {
            DirectoryCompanies = new ObservableCollection<DirectoryCompany>(_bc.GetDirectoryCompanies());
        }


        #region Properties

        private BusinessContext _bc = new BusinessContext();

        private string _directoryCompanyName;

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
                OnPropertyChanged("ValidateDirectoryCompanyName");
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

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ViewCloseCommand { get; set; }

        private void ClearInputData()
        {
            DirectoryCompanyName = null;
        }

        public void Add(object parameter)
        {
            _bc.AddDirectoryCompany(DirectoryCompanyName);

            RefreshDirectoryCompanies();

            ClearInputData();
        }

        public void Remove(object parameter)
        {
            _bc.RemoveDirectoryCompany(SelectedDirectoryCompany);

            RefreshDirectoryCompanies();
            
            if (DirectoryCompanies.Any())
            {
                SelectedDirectoryCompany = DirectoryCompanies.Last();
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
            "DirectoryCompanyName", 
        };

        public string ValidateDirectoryCompanyName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DirectoryCompanyName))
                {
                    return "Не заполнено поле \"Название компании\"";
                }

                if (DirectoryCompanyName.Length > 64)
                {
                    return "Длина поля \"Название компании\" должна быть не больше 64 символов";
                }

                return null;
            }
        }

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "DirectoryCompanyName":
                    return ValidateDirectoryCompanyName;
                default:
                    throw new InvalidOperationException();
            }
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
            return SelectedDirectoryCompany != null;
        }

        #endregion
    }
}

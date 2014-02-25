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
    public class DirectoryTypeOfCompanyViewModel : ViewModel
    {
        public DirectoryTypeOfCompanyViewModel()
        {
            RefreshDirectoryTypeOfCompanies();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        private void RefreshDirectoryTypeOfCompanies()
        {
            DirectoryTypeOfCompanies = new ObservableCollection<DirectoryTypeOfCompany>(_bc.GetDirectoryTypeOfCompanies());
        }


        #region Properties

        private BusinessContext _bc = new BusinessContext();

        private string _directoryTypeOfCompanyName;

        public string DirectoryTypeOfCompanyName
        {
            get
            {
                return _directoryTypeOfCompanyName;
            }
            set
            {
                _directoryTypeOfCompanyName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryTypeOfCompanyName");
            }
        }

        private DirectoryTypeOfCompany _selectedDirectoryTypeOfCompany;

        public DirectoryTypeOfCompany SelectedDirectoryTypeOfCompany
        {
            get
            {
                return _selectedDirectoryTypeOfCompany;
            }
            set
            {
                _selectedDirectoryTypeOfCompany = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DirectoryTypeOfCompany> _directoryTypeOfCompanies;
        public ObservableCollection<DirectoryTypeOfCompany> DirectoryTypeOfCompanies
        { 
            get
            {
                return _directoryTypeOfCompanies;
            }
            set
            {
                _directoryTypeOfCompanies = value;
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
            _bc.AddDirectoryTypeOfCompany(DirectoryTypeOfCompanyName);

            RefreshDirectoryTypeOfCompanies();

            DirectoryTypeOfCompanyName = null;
        }

        public void Remove(object parameter)
        {
            _bc.RemoveDirectoryTypeOfCompany(SelectedDirectoryTypeOfCompany);

            RefreshDirectoryTypeOfCompanies();
            
            if (DirectoryTypeOfCompanies.Any())
            {
                SelectedDirectoryTypeOfCompany = DirectoryTypeOfCompanies.Last();
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
            "DirectoryTypeOfCompanyName"
        };

        public string ValidateDirectoryTypeOfCompanyName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DirectoryTypeOfCompanyName))
                {
                    return "Не заполнено поле \"Вид деятельности компании\".";
                }

                if (DirectoryTypeOfCompanyName.Length > 32)
                {
                    return "Длина поля \"Вид деятельности компании\" должна быть не больше 32 символов.";
                }

                return null;
            }
        }

        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "DirectoryTypeOfCompanyName")
            {
                return ValidateDirectoryTypeOfCompanyName;
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
            return SelectedDirectoryTypeOfCompany != null;
        }

        #endregion
    }
}

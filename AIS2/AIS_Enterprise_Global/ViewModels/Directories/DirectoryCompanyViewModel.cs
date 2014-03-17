using AIS_Enterprise_Global.Helpers;
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
using System.Data.Entity;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryCompanyViewModel : ViewModel
    {
        #region Base

        public DirectoryCompanyViewModel() : base()
        {
            RefreshDirectoryCompanies();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
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


        #region Properties

        public ObservableCollection<DirectoryCompany> DirectoryCompanies { get; set; }

        public DirectoryCompany SelectedDirectoryCompany { get; set; }

        [Required]
        [Display(Name = "Название компании")]
        public string DirectoryCompanyName { get; set; }

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
            BC.RemoveDirectoryCompany(SelectedDirectoryCompany.Id);

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

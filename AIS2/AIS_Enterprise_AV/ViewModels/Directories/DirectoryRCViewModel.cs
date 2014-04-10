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
using AIS_Enterprise_AV.Models.Directories;

namespace AIS_Enterprise_AV.ViewModels
{
    public class DirectoryRCViewModel : ViewModelAV
    {
        #region Base

        public DirectoryRCViewModel() : base()
        {
            RefreshDirectoryRCs();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
        }

        private void RefreshDirectoryRCs()
        {
            DirectoryRCs = new ObservableCollection<DirectoryRC>(BC.GetDirectoryRCs().ToList());
        }

        private void ClearInputData()
        {
            DirectoryRCName = null;
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryRC> DirectoryRCs { get; set; }

        public DirectoryRC SelectedDirectoryRC { get; set; }

        [Required]
        [Display(Name = "Название ЦО")]
        public string DirectoryRCName { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public void Add(object parameter)
        {
            BC.AddDirectoryRC(DirectoryRCName);

            RefreshDirectoryRCs();

            ClearInputData();
        }

        public bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        public void Remove(object parameter)
        {
            BC.RemoveDirectoryRC(SelectedDirectoryRC.Id);

            RefreshDirectoryRCs();
            
            if (DirectoryRCs.Any())
            {
                SelectedDirectoryRC = DirectoryRCs.Last();
            }
        }

        public bool CanRemoving(object parameter)
        {
            return SelectedDirectoryRC != null;
        }

        #endregion
    }
}
